using MESDataObject.Module;
using MESDBHelper;
using MESReport.Common;
using MESStation.BaseClass;
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

            int packs = (int)(workorder.WORKORDER_QTY / qty)+1;
            string ruleName = c_PACKING.SN_RULE;
            for (int i = 0; i < packs; i++) {
                string nextpackno = SNmaker.GetNextSN(ruleName,Sfcdb);
                System.Console.WriteLine(nextpackno);
            }

            StationReturn.Status = StationReturnStatusValue.Pass;
        }
    }
}
