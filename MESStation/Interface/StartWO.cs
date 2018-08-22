using MESDataObject;
using MESDataObject.Module;
using MESDBHelper;
using MESStation.BaseClass;
using MESStation.LogicObject;
using MESStation.SNMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface
{
    class StartWO : MesAPIBase
    {
        protected APIInfo info = new APIInfo()
        {
            FunctionName = "startWO",
            Description = "start WO For TjL5",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "WO", InputType = "STRING", DefaultValue = "WO"}
            },
            Permissions = new List<MESPermission>()//不需要任何權限
        };

        protected APIInfo unstart = new APIInfo()
        {
            FunctionName = "unstartWO",
            Description = "unstartWO For TjL5",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "WO", InputType = "STRING", DefaultValue = "WO"}
            },
            Permissions = new List<MESPermission>()//不需要任何權限
        };

        public StartWO()
        {
            this.Apis.Add(info.FunctionName, info);
            this.Apis.Add(unstart.FunctionName, unstart);
        }
        public void unstartWO(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn) {
            OleExec Sfcdb = this.DBPools["SFCDB"].Borrow();
            string WO = Data["WO"].ToString();
            T_R_SN t_r_sn = new T_R_SN(Sfcdb, this.DBTYPE);
            t_r_sn.deleteSNByWO(WO, Sfcdb);
            StationReturn.Status = StationReturnStatusValue.Pass;
        }

        public void startWO(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec Sfcdb = this.DBPools["SFCDB"].Borrow();
            string WO = Data["WO"].ToString();
            T_R_SN t_r_sn = new T_R_SN(Sfcdb, this.DBTYPE);
            string id = t_r_sn.findOneSNByWO(WO, Sfcdb);
            if (!string.IsNullOrEmpty(id)) {
                StationReturn.Status = StationReturnStatusValue.Fail;
                StationReturn.Message = "工单已展开,不能重复展开";
                return;
            }
            T_R_WO_HEADER_TJ t_R_WO_HEADER_TJ = new T_R_WO_HEADER_TJ(Sfcdb, this.DBTYPE);
            Row_R_WO_HEADER_TJ row_R_WO_HEADER = t_R_WO_HEADER_TJ.GetWo(WO, Sfcdb);
            T_C_SKU table_sku = new T_C_SKU(Sfcdb, this.DBTYPE);
            string user = this.LoginUser.EMP_NO; 
            MESDataObject.Module.SkuObject SkuObject = table_sku.GetSkuBySkuno(row_R_WO_HEADER.MATNR, Sfcdb);
            float qty = float.Parse(row_R_WO_HEADER.GAMNG);
            qty = 5;
            //获取路由id
            T_R_SKU_ROUTE t_R_SKU_ROUTE = new T_R_SKU_ROUTE(Sfcdb, this.DBTYPE);
            string routeid = t_R_SKU_ROUTE.getRouteIdBySkuName(row_R_WO_HEADER.MATNR, Sfcdb);
            //获取keypartlistid
            T_C_KP_LIST t_C_KP_LIST = new T_C_KP_LIST(Sfcdb, this.DBTYPE);
            List<string> keypartlistids =  t_C_KP_LIST.GetListIDBySkuno(row_R_WO_HEADER.MATNR,Sfcdb );
  
            if (keypartlistids.Count == 0) {
                StationReturn.Status = StationReturnStatusValue.Fail;
                StationReturn.Message = "未配置keypartlistid";
                return;
            }
            //生成工单基础表
            T_R_WO_BASE t_R_WO_BASE = new T_R_WO_BASE(Sfcdb, this.DBTYPE);
            t_R_WO_BASE.deleteWOByWo(WO, Sfcdb);
            t_R_WO_BASE.addWOByWOHeader(BU, user, routeid, row_R_WO_HEADER, Sfcdb);

            WorkOrder objWorkorder = new WorkOrder();
            objWorkorder.Init(WO, Sfcdb);
            objWorkorder.WorkorderNo = WO;
            objWorkorder.KP_LIST_ID = keypartlistids[0].ToString();
            objWorkorder.SkuNO = row_R_WO_HEADER.MATNR;

            //生成SN
            for (int i = 0; i < qty; i++)
            {
                String nextSN = SNmaker.GetNextSN(SkuObject.SnRule, Sfcdb, WO);
                Console.Out.WriteLine(nextSN);
                t_r_sn.addStartSNRecords(BU, user,WO, routeid, row_R_WO_HEADER, nextSN, Sfcdb);
            }
            //生成keypartlistid
           
            List<R_SN> r_sns = t_r_sn.GETSN(WO,Sfcdb);
            for (int i = 0; i < r_sns.Count; i++)
            {
                T_C_KP_LIST c_kp_list = new T_C_KP_LIST(Sfcdb, this.DBTYPE);
                if (objWorkorder.KP_LIST_ID != "" && c_kp_list.KpIDIsExist(objWorkorder.KP_LIST_ID, Sfcdb))
                {
                    SN snObject = new SN();
                    MESStationBase Station = new MESStationBase();
                    Station.BU = this.LoginUser.BU;
                    Station.LoginUser = this.LoginUser;
                    Station.SFCDB = Sfcdb;
                    snObject.InsertR_SN_KP(objWorkorder, r_sns[i], Sfcdb, Station, this.DBTYPE);
                }
            }
       
           
            StationReturn.Data = qty;
            StationReturn.Status = StationReturnStatusValue.Pass;
            StationReturn.Message = MESReturnMessage.GetMESReturnMessage("MSGCODE20180801141046");

        }

    }
}
