using MESDataObject;
using MESDataObject.Module;
using MESDBHelper;
using MESStation.BaseClass;
using MESStation.Interface.SAPRFC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface
{
    class Download_skuno_TJ : MesAPIBase
    {
        protected APIInfo FDownload = new APIInfo()
        {
            FunctionName = "Download_skuno",
            Description = "Download skuno For TjL5",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "SKUNO", InputType = "STRING", DefaultValue = "SKUNO"},
                new APIInputInfo() {InputName = "PLANT", InputType = "STRING", DefaultValue = "PLANT"}
            },
            Permissions = new List<MESPermission>()//不需要任何權限
        };

        public Download_skuno_TJ()
        {
            this.Apis.Add(FDownload.FunctionName, FDownload);
        }



        /// <summary>
        /// DonwLoad WO From SAP
        /// </summary>
        /// <param name="requestValue"></param>
        /// <param name="Data"></param>
        /// <param name="StationReturn"></param>
        public void Download_skuno(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec Sfcdb = this.DBPools["SFCDB"].Borrow();
            string WO = Data["WO"].ToString();
            string PLANT = Data["PLANT"].ToString();
            Sfcdb.BeginTrain();
            //Download(WO, PLANT);
            //DownloadDetail(WO, PLANT);
            Sfcdb.CommitTrain();
            StationReturn.Status = StationReturnStatusValue.Pass;
            StationReturn.Message = MESReturnMessage.GetMESReturnMessage("MES00000102");

        }


        public void Download(string SKUNO, string PLANT, OleExec Sfcdb) {
            ZRFC_GET_PROD_MASTER zrfc_GET_PROD_MASTER = new ZRFC_GET_PROD_MASTER();
            zrfc_GET_PROD_MASTER.SetValues(SKUNO, PLANT);
            zrfc_GET_PROD_MASTER.CallRFC();
            DataTable skuno_table = zrfc_GET_PROD_MASTER.GetTableValue("PROD_MASTER");
            T_C_SKU t_c_sku = new T_C_SKU(Sfcdb, DB_TYPE_ENUM.Oracle);

            Row_C_SKU row_C_SKU = (Row_C_SKU)t_c_sku.NewRow();
            if (skuno_table.Rows.Count > 0)
            {

                row_C_SKU.ID = t_c_sku.GetNewID(BU, Sfcdb);

                row_C_SKU.BU = this.BU;
                row_C_SKU.SKUNO = skuno_table.Rows[0]["MATNR"].ToString();
                row_C_SKU.VERSION = skuno_table.Rows[0]["REVLV"].ToString();
                row_C_SKU.SKU_NAME = skuno_table.Rows[0]["BISMT"].ToString();
                row_C_SKU.C_SERIES_ID = "";
                row_C_SKU.CUST_PARTNO = skuno_table.Rows[0]["BISMT"].ToString();
                row_C_SKU.CUST_SKU_CODE = "";
                row_C_SKU.SN_RULE = "";
                row_C_SKU.PANEL_RULE = "";
                row_C_SKU.DESCRIPTION = skuno_table.Rows[0]["MAKTX"].ToString();
                row_C_SKU.LAST_EDIT_USER = this.LoginUser.EMP_NO;
                row_C_SKU.LAST_EDIT_TIME =DateTime.Now;
                row_C_SKU.SKU_TYPE ="";
                row_C_SKU.AQLTYPE = "";

                String skuno = row_C_SKU.SKUNO;
                string sql = $@"DELETE FROM C_SKU WHERE SKUNO='{skuno}'";
                Sfcdb.ExecSQL(sql);
                sql = row_C_SKU.GetInsertString(DB_TYPE_ENUM.Oracle);
                Sfcdb.ExecSQL(sql);

            }
        }

    }
}
