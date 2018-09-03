using MESDataObject;
using MESDBHelper;
using MESPubLab.MESStation;
using MESStation.Interface.SAPRFC;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDataObject.Module;
using System.Data;

namespace MESStation.Interface
{
    public class JobStockGT : MesAPIBase
    {   
        protected APIInfo FDoJobStockGT = new APIInfo()
        {
            FunctionName = "DoJobStockGT",
            Description = "JobStock工站轉倉拋賬 ",
            Parameters = new List<APIInputInfo>()
            {
            },
            Permissions = new List<MESPermission>() { }
        };

        public JobStockGT() {
            this.Apis.Add(FDoJobStockGT.FunctionName, FDoJobStockGT);
        }
        

        public void DoJobStockGT(JObject requestValue, JObject Data, MESStationReturn StationReturn)
        {
            OleExec SFCDB = DBPools["SFCDB"].Borrow();
            try
            {                
                ZRFC_SFC_NSG_0020 zrfc_sfc_nsg_0020 = new ZRFC_SFC_NSG_0020(this.BU);
                string lockIp = "";               
                string postDate;
                string o_flag = "";
                string o_flag1 = "";
                string o_flag2 = "";
                string o_message = "";
                string o_message1 = "";
                string o_message2 = "";
                bool IsRuning = false;
                Row_R_STOCK_GT rowStockGT;
                DataTable table = new DataTable();
                SFCDB.ThrowSqlExeception = true;
                T_R_SYNC_LOCK t_r_sync_lock = new T_R_SYNC_LOCK(SFCDB,DB_TYPE_ENUM.Oracle);
                T_R_STOCK_GT t_r_stock_gt = new T_R_STOCK_GT(SFCDB, DB_TYPE_ENUM.Oracle);
                T_R_STOCK t_r_stock = new T_R_STOCK(SFCDB,DB_TYPE_ENUM.Oracle);

                if (Interface.IsMonthly(SFCDB, DB_TYPE_ENUM.Oracle))
                {
                    //月結不給拋賬
                    //throw new Exception("This time is monthly,can't BackFlush");
                    throw new Exception(MESReturnMessage.GetMESReturnMessage("MSGCODE20180803152122", new string[] { }));
                }

                IsRuning = t_r_sync_lock.IsLock("JOBSTOCKGT", SFCDB, DB_TYPE_ENUM.Oracle, out lockIp);
                if (IsRuning)
                {
                    //throw new Exception("JOBSTOCKGT interface is running on " + lockIp + ",Please try again later");
                    throw new Exception(MESReturnMessage.GetMESReturnMessage("MSGCODE20180803152222", new string[] { lockIp })); 
                }
                t_r_sync_lock.SYNC_Lock(BU, this.IP, "JOBSTOCKGT", "JOBSTOCKGT", this.LoginUser.EMP_NO, SFCDB, DB_TYPE_ENUM.Oracle);

                List<R_STOCK_GT> GTList = t_r_stock_gt.GetNotGTListByConfirmedFlag("0",SFCDB);
               
                postDate = Interface.GetPostDate(SFCDB);

                if (GTList != null && GTList.Count > 0)
                {
                    foreach (R_STOCK_GT r_stock_gt in GTList)
                    {
                        rowStockGT = null;
                        zrfc_sfc_nsg_0020.SetValue("I_AUFNR", r_stock_gt.WORKORDERNO);
                        zrfc_sfc_nsg_0020.SetValue("I_BUDAT", postDate);
                        zrfc_sfc_nsg_0020.SetValue("I_FLAG", r_stock_gt.CONFIRMED_FLAG);
                        zrfc_sfc_nsg_0020.SetValue("I_LGORT_TO", r_stock_gt.TO_STORAGE);
                        zrfc_sfc_nsg_0020.SetValue("I_LMNGA", r_stock_gt.TOTAL_QTY.ToString());
                        zrfc_sfc_nsg_0020.SetValue("I_STATION", r_stock_gt.SAP_STATION_CODE);                        
                        try
                        {
                            // zrfc_sfc_nsg_0020 中包含三個動作101，521，轉倉，flag，flag1，flag2 一次對應這三個動作
                            // flag，flag1，flag2 這幾個flag 0表示OK，1表示false                           
                            zrfc_sfc_nsg_0020.CallRFC();
                            o_flag = zrfc_sfc_nsg_0020.GetValue("O_FLAG");
                            o_flag1 = zrfc_sfc_nsg_0020.GetValue("O_FLAG1");
                            o_flag2 = zrfc_sfc_nsg_0020.GetValue("O_FLAG2");
                            o_message = zrfc_sfc_nsg_0020.GetValue("O_MESSAGE");
                            o_message1 = zrfc_sfc_nsg_0020.GetValue("O_MESSAGE1");
                            o_message2 = zrfc_sfc_nsg_0020.GetValue("O_MESSAGE2");

                            rowStockGT = (Row_R_STOCK_GT)t_r_stock_gt.GetObjByID(r_stock_gt.ID, SFCDB);
                            rowStockGT.SAP_MESSAGE = "101:" + o_message + ";521:" + o_message1 + ";311" + o_message2;
                            rowStockGT.BACKFLUSH_TIME = GetDBDateTime();
                            if (zrfc_sfc_nsg_0020.GetValue("O_FLAG") == "0")
                            {
                                rowStockGT.SAP_FLAG = "1";
                                t_r_stock.UpdateSapFlagByGTID(rowStockGT.ID, rowStockGT.SAP_FLAG,SFCDB);
                            }
                            else
                            {
                                rowStockGT.SAP_FLAG = "2";
                            }
                            SFCDB.ExecSQL(rowStockGT.GetUpdateString(DB_TYPE_ENUM.Oracle));
                        }
                        catch (Exception ex)
                        {
                            Interface.WriteIntoMESLog(SFCDB, BU, "MESStation", "MESStation.Interface.JobStockGT", "DoJobStockGT", r_stock_gt.WORKORDERNO + ";" + this.IP + ";" + ex.ToString(), "", this.LoginUser.EMP_NO);
                            r_stock_gt.SAP_FLAG = "2";
                        }
                    }
                }
                t_r_sync_lock.SYNC_UnLock(BU, this.IP, "JOBSTOCKGT", "JOBSTOCKGT", this.LoginUser.EMP_NO, SFCDB, DB_TYPE_ENUM.Oracle);
                if (SFCDB != null)
                {
                    DBPools["SFCDB"].Return(SFCDB);
                }
                StationReturn.Status = StationReturnStatusValue.Pass;
            }
            catch (Exception exception)
            {
                if (SFCDB != null)
                {
                    DBPools["SFCDB"].Return(SFCDB);
                }
                StationReturn.Status = StationReturnStatusValue.Fail;
                throw exception;
            }
        }
    }
}
