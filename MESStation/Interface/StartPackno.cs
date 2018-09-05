using MESDataObject.Module;
using MESDBHelper;
using MESPubLab.MESStation;
using MESReport.Common;
using MESStation.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface
{
    class StartPackno: MesAPIBase
    {
        protected APIInfo info = new APIInfo()
        {
            FunctionName = "startPackno",
            Description = "start Packno For TjL5",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "wo", InputType = "STRING", DefaultValue = "wo"},
                new APIInputInfo() {InputName = "packType", InputType = "STRING", DefaultValue = "packType"}
            },
            Permissions = new List<MESPermission>()//不需要任何權限
        };
        public StartPackno()
        {
            this.Apis.Add(info.FunctionName, info);
        }

        public void startPackno(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            //通过工单的数量和包规来生成箱号

            OleExec Sfcdb = this.DBPools["SFCDB"].Borrow();
            string packType = Data["packType"].ToString();
            string wo = Data["wo"].ToString();
            WorkOrder workorder = new WorkOrder();
            workorder.Init(wo,Sfcdb);

            T_C_PACKING t_C_PACKING = new T_C_PACKING(Sfcdb,DBTYPE);
            C_PACKING c_PACKING = t_C_PACKING.GetPackingBySkuAndType(workorder.SkuNO, packType,Sfcdb);
            double? qty = c_PACKING.MAX_QTY;

            int packs = (int)(workorder.WORKORDER_QTY / qty);
            string ruleName = c_PACKING.SN_RULE;
            T_R_PACKING t_r_packing = new T_R_PACKING(Sfcdb, DBTYPE);

            int remainQty = 0;
            if (packs * qty < workorder.WORKORDER_QTY)
            {
                remainQty = (int)(workorder.WORKORDER_QTY - packs * qty);
                packs = packs + 1;
            }
            Sfcdb.BeginTrain();
            try
            {
                for (int i = 0; i < packs; i++)
                {
                    string nextpackno = SNmaker.GetNextSN(ruleName, Sfcdb);
                    System.Console.WriteLine(nextpackno);
                    Row_R_PACKING row_r_packing = (Row_R_PACKING)t_r_packing.NewRow();

                    row_r_packing.IP = this.IP;
                    row_r_packing.STATION = "STARTPARTNO";
                    row_r_packing.LINE = "LINE0";
                    row_r_packing.EDIT_EMP = this.LoginUser.EMP_NO;
                    row_r_packing.EDIT_TIME = DateTime.Now;
                    row_r_packing.CREATE_TIME = DateTime.Now;
                    row_r_packing.CLOSED_FLAG = "0";
                    row_r_packing.QTY = 0;
                    if (packs == i && remainQty > 0)
                    {
                        row_r_packing.MAX_QTY = remainQty;
                    }
                    else
                    {
                        row_r_packing.MAX_QTY = qty;
                    }
                    row_r_packing.SKUNO = workorder.SkuNO;
                    row_r_packing.PARENT_PACK_ID = "";
                    row_r_packing.PACK_TYPE = packType;
                    row_r_packing.PACK_NO = nextpackno;
                    row_r_packing.ID = t_r_packing.GetNewID(this.BU, Sfcdb);

                    Sfcdb.ExecSQL(row_r_packing.GetInsertString(DBTYPE));

                }
            }
            catch (Exception e){
                Sfcdb.RollbackTrain();
                throw e;
            }
            Sfcdb.CommitTrain();

            StationReturn.Status = StationReturnStatusValue.Pass;
        }
    }
}
