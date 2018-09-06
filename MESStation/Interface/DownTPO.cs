using MESDataObject;
using MESDBHelper;
using MESPubLab.MESStation;
using MESStation.Interface.SAPRFC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface
{
    class DownTPO : MesAPIBase
    {
        protected APIInfo FDownload = new APIInfo()
        {
            FunctionName = "Download_TPO",
            Description = "Download TPO For TjL5",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "TPO", InputType = "STRING", DefaultValue = "TPO"},
                new APIInputInfo() {InputName = "PLANT", InputType = "STRING", DefaultValue = "PLANT"}
            },
            Permissions = new List<MESPermission>()//不需要任何權限
        };

        public DownTPO()
        {
            this.Apis.Add(FDownload.FunctionName, FDownload);
        }

        public void Download_TPO(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec Sfcdb = this.DBPools["SFCDB"].Borrow();
            Sfcdb.BeginTrain();
            try
            {
                string TPO = Data["TPO"].ToString();
                string PLANT = Data["PLANT"].ToString();
                Download(Sfcdb, TPO, PLANT);
                Sfcdb.CommitTrain();
            }
            catch (Exception e)
            {
                Sfcdb.RollbackTrain();
                throw e;
            }
            StationReturn.Status = StationReturnStatusValue.Pass;
            StationReturn.Message = MESReturnMessage.GetMESReturnMessage("MES00000102");

        }

        public void Download(OleExec sfcdb,string TPO, string Plant)
        {

            DataTable RFC_Table = new DataTable();
            Dictionary<string, string> DicPara = new Dictionary<string, string>();
            ZP2SF01 zp2SF01 = new ZP2SF01();
            zp2SF01.SetValues(TPO, Plant);
            zp2SF01.CallRFC();

            DataTable result = zp2SF01.GetTableValue("TRANS_ORDER");

            //T_R_ZRFC_GET_SALES_HEADER2 so_header = new T_R_ZRFC_GET_SALES_HEADER2(sfcdb, DB_TYPE_ENUM.Oracle);

            //Row_R_ZRFC_GET_SALES_HEADER2 so_row_header = (Row_R_ZRFC_GET_SALES_HEADER2)so_header.NewRow();

            if (result.Rows.Count > 0)
            {
                //so_row_header.VBELN = result.Rows[0]["VBELN"].ToString();
                //so_row_header.WERKS = result.Rows[0]["WERKS"].ToString();
                //so_row_header.SVBELN = result.Rows[0]["SVBELN"].ToString();
                //so_row_header.KUNNR = result.Rows[0]["KUNNR"].ToString();
                //so_row_header.KUNAG = result.Rows[0]["KUNAG"].ToString();
                //so_row_header.AUART = result.Rows[0]["AUART"].ToString();
                //so_row_header.AEDAT = result.Rows[0]["AEDAT"].ToString();
                //so_row_header.BSTKD = result.Rows[0]["BSTKD"].ToString();
                //so_row_header.SNAME1 = result.Rows[0]["SNAME1"].ToString();
                //so_row_header.SNAME2 = result.Rows[0]["SNAME2"].ToString();
                //so_row_header.SSTRAS = result.Rows[0]["SSTRAS"].ToString();
                //so_row_header.SADRNR = result.Rows[0]["SADRNR"].ToString();
                //so_row_header.SORT01 = result.Rows[0]["SORT01"].ToString();
                //so_row_header.SREGIO = result.Rows[0]["SREGIO"].ToString();
                //so_row_header.SPSTLZ = result.Rows[0]["SPSTLZ"].ToString();
                //so_row_header.SLAND1 = result.Rows[0]["SLAND1"].ToString();
                //so_row_header.STELF1 = result.Rows[0]["STELF1"].ToString();
                //so_row_header.STELFX = result.Rows[0]["STELFX"].ToString();
                //so_row_header.LPRIO = result.Rows[0]["LPRIO"].ToString();
                //so_row_header.ERDAT = result.Rows[0]["ERDAT"].ToString();
                //so_row_header.POSEX = result.Rows[0]["POSEX"].ToString();
                //so_row_header.NTGEW = result.Rows[0]["NTGEW"].ToString();
                //so_row_header.IHREZ = result.Rows[0]["IHREZ"].ToString();
                //so_row_header.BSTKD_E = result.Rows[0]["BSTKD_E"].ToString();
                //so_row_header.POSEX_E = result.Rows[0]["POSEX_E"].ToString();
                //so_row_header.IHREZ_E = result.Rows[0]["IHREZ_E"].ToString();
                //so_row_header.POSNR2 = result.Rows[0]["POSNR2"].ToString();
                //so_row_header.STATUS1 = result.Rows[0]["STATUS1"].ToString();
                //so_row_header.LIFSK = result.Rows[0]["LIFSK"].ToString();
                //so_row_header.STATUS3 = result.Rows[0]["STATUS3"].ToString();
                //so_row_header.VDATU = result.Rows[0]["VDATU"].ToString();
                //so_row_header.UTIME = result.Rows[0]["UTIME"].ToString();
                //so_row_header.ERZET = result.Rows[0]["ERZET"].ToString();
                //so_row_header.SNAME3 = result.Rows[0]["SNAME3"].ToString();
                //so_row_header.SSTRAS2 = result.Rows[0]["SSTRAS2"].ToString();
                //so_row_header.SSTRAS3 = result.Rows[0]["SSTRAS3"].ToString();
                //so_row_header.SSTRAS4 = result.Rows[0]["SSTRAS4"].ToString();
                //so_row_header.SNAME4 = result.Rows[0]["SNAME4"].ToString();
                //so_row_header.VSBED = result.Rows[0]["VSBED"].ToString();
                //so_row_header.PRSDT = result.Rows[0]["PRSDT"].ToString();
                //so_row_header.VSNMR_V = result.Rows[0]["VSNMR_V"].ToString();
                //so_row_header.CITY2 = result.Rows[0]["CITY2"].ToString();
                //so_row_header.CHANGE_FLAG = result.Rows[0]["CHANGE_FLAG"].ToString();
                //so_row_header.SUBMI = result.Rows[0]["SUBMI"].ToString();
                //so_row_header.RSNAME1 = result.Rows[0]["RSNAME1"].ToString();
                //so_row_header.RSNAME2 = result.Rows[0]["RSNAME2"].ToString();
                //so_row_header.RSTRAS = result.Rows[0]["RSTRAS"].ToString();
                //so_row_header.RSORT01 = result.Rows[0]["RSORT01"].ToString();
                //so_row_header.RSREGIO = result.Rows[0]["RSREGIO"].ToString();
                //so_row_header.RSPSTLZ = result.Rows[0]["RSPSTLZ"].ToString();
                //so_row_header.RSLAND1 = result.Rows[0]["RSLAND1"].ToString();
                //so_row_header.RSTELF1 = result.Rows[0]["RSTELF1"].ToString();
                //so_row_header.RSTELFX = result.Rows[0]["RSTELFX"].ToString();
                //so_row_header.TEXT0001 = result.Rows[0]["TEXT0001"].ToString();
                //so_row_header.TEXT0002 = result.Rows[0]["TEXT0002"].ToString();
                //so_row_header.TEXT0003 = result.Rows[0]["TEXT0003"].ToString();
                //so_row_header.TEXT0004 = result.Rows[0]["TEXT0004"].ToString();
                //so_row_header.TEXT0005 = result.Rows[0]["TEXT0005"].ToString();
                //so_row_header.TEXT0010 = result.Rows[0]["TEXT0010"].ToString();
                //so_row_header.TEXT0011 = result.Rows[0]["TEXT0011"].ToString();
                //so_row_header.TEXT0012 = result.Rows[0]["TEXT0012"].ToString();
                //so_row_header.TEXT0013 = result.Rows[0]["TEXT0013"].ToString();
                //so_row_header.TEXT0014 = result.Rows[0]["TEXT0014"].ToString();
                //so_row_header.TEXT0015 = result.Rows[0]["TEXT0015"].ToString();
                //so_row_header.TEXT0016 = result.Rows[0]["TEXT0016"].ToString();
                //so_row_header.TEXT0017 = result.Rows[0]["TEXT0017"].ToString();
                //so_row_header.TEXT0018 = result.Rows[0]["TEXT0018"].ToString();
                //so_row_header.TEXTZ011 = result.Rows[0]["TEXTZ011"].ToString();
                //so_row_header.TEXTZ012 = result.Rows[0]["TEXTZ012"].ToString();
                //so_row_header.BSTDK = result.Rows[0]["BSTDK"].ToString();
                //so_row_header.AUGRU = result.Rows[0]["AUGRU"].ToString();
                //so_row_header.BEZEI = result.Rows[0]["BEZEI"].ToString();


            }



        }
    }
}
