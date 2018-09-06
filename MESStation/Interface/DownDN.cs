using MESDataObject;
using MESDataObject.Module;
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
    class DownDN : MesAPIBase
    {
        protected APIInfo FDownload = new APIInfo()
        {
            FunctionName = "Download_DN",
            Description = "Download DN For TjL5",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "DN", InputType = "STRING", DefaultValue = "DN"},
                new APIInputInfo() {InputName = "PLANT", InputType = "STRING", DefaultValue = "PLANT"}
            },
            Permissions = new List<MESPermission>()//不需要任何權限
        };

        public DownDN()
        {
            this.Apis.Add(FDownload.FunctionName, FDownload);
        }

        public void Download_DN(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec Sfcdb = this.DBPools["SFCDB"].Borrow();
            string DN = Data["DN"].ToString();
            string PLANT = Data["PLANT"].ToString();
            Sfcdb.BeginTrain();

            try
            {
                Download(Sfcdb,DN, PLANT);
                DownloadDetail(Sfcdb,DN, PLANT);
                Sfcdb.CommitTrain();
            }
            catch (Exception e)
            {
                Sfcdb.RollbackTrain();
                throw e;
            }
            StationReturn.Status = StationReturnStatusValue.Pass;
            StationReturn.Message = MESReturnMessage.GetMESReturnMessage("MSGCODE20180906102708");

        }

        public void DownloadDetail(OleExec sfcdb,string DN, string Plant)
        {
            
            DataTable RFC_Table = new DataTable();


            Dictionary<string, string> DicPara = new Dictionary<string, string>();

            sfcdb = this.DBPools["SFCDB"].Borrow();


            ZRFC_GET_SHIP_DETAIL zrfc_GET_SHIP_DETAIL = new ZRFC_GET_SHIP_DETAIL();
            zrfc_GET_SHIP_DETAIL.SetValues(DN, Plant);//NHGZ,WDN1//WDN1,WSL3
            zrfc_GET_SHIP_DETAIL.CallRFC();

            DataTable result = zrfc_GET_SHIP_DETAIL.GetTableValue("SHIPORDERDETAIL");


            T_R_ZRFC_GET_SHIP_DETAIL dn_detail = new T_R_ZRFC_GET_SHIP_DETAIL(sfcdb, DB_TYPE_ENUM.Oracle);

            Row_R_ZRFC_GET_SHIP_DETAIL dn_row_detail = (Row_R_ZRFC_GET_SHIP_DETAIL)dn_detail.NewRow();
            
            if (result.Rows.Count > 0)
            {
                string sql = $@"DELETE FROM R_ZRFC_GET_SHIP_DETAIL WHERE VBELN = '{DN}'";
                sfcdb.ExecSQL(sql);
                foreach (DataRow R_dn_detail in result.Rows)
                    {
                        dn_row_detail.ID = dn_detail.GetNewID(BU, sfcdb);
                        dn_row_detail.VGBEL = R_dn_detail["VGBEL"].ToString();
                        dn_row_detail.VBELN = R_dn_detail["VBELN"].ToString();
                        dn_row_detail.MATNR = R_dn_detail["MATNR"].ToString();
                        dn_row_detail.POSNR = R_dn_detail["POSNR"].ToString();
                        dn_row_detail.ARKTX = R_dn_detail["ARKTX"].ToString();
                        dn_row_detail.KDMAT = R_dn_detail["KDMAT"].ToString();
                        dn_row_detail.LFIMG = R_dn_detail["LFIMG"].ToString();
                        dn_row_detail.BSTKD = R_dn_detail["BSTKD"].ToString();
                        dn_row_detail.POSEX = R_dn_detail["POSEX"].ToString();
                        dn_row_detail.NTGEW = R_dn_detail["NTGEW"].ToString();
                        dn_row_detail.IHREZ = R_dn_detail["IHREZ"].ToString();
                        dn_row_detail.BSTKD_E = R_dn_detail["BSTKD_E"].ToString();
                        dn_row_detail.POSEX_E = R_dn_detail["POSEX_E"].ToString();
                        dn_row_detail.IHREZ_E = R_dn_detail["IHREZ_E"].ToString();
                        dn_row_detail.POSNR2 = R_dn_detail["POSNR2"].ToString();
                        dn_row_detail.POSNR3 = R_dn_detail["POSNR3"].ToString();
                        dn_row_detail.REVISION = R_dn_detail["REVISION"].ToString();
                        dn_row_detail.WERKS_D = R_dn_detail["WERKS_D"].ToString();
                        dn_row_detail.LGORT_D = R_dn_detail["LGORT_D"].ToString();
                        dn_row_detail.LPRIO = R_dn_detail["LPRIO"].ToString();
                        dn_row_detail.PROFL = R_dn_detail["PROFL"].ToString();
                        dn_row_detail.VTEXT = R_dn_detail["VTEXT"].ToString();
                        dn_row_detail.KUNNR = R_dn_detail["KUNNR"].ToString();
                        dn_row_detail.VGPOS = R_dn_detail["VGPOS"].ToString();
                        dn_row_detail.VSBED = R_dn_detail["VSBED"].ToString();
                        dn_row_detail.ERDAT = R_dn_detail["ERDAT"].ToString();
                        dn_row_detail.WERKS = R_dn_detail["WERKS"].ToString();
                        dn_row_detail.WADAT = R_dn_detail["WADAT"].ToString();
                        dn_row_detail.LFIMG_PACK = R_dn_detail["LFIMG_PACK"].ToString();
                        dn_row_detail.PALLET_NO = R_dn_detail["PALLET_NO"].ToString();
                        dn_row_detail.BOX_NO = R_dn_detail["BOX_NO"].ToString();
                        dn_row_detail.NET_PER_BOX = R_dn_detail["NET_PER_BOX"].ToString();
                        dn_row_detail.GROSS_PER_BOX = R_dn_detail["GROSS_PER_BOX"].ToString();
                        dn_row_detail.NET_WEIGHT = R_dn_detail["NET_WEIGHT"].ToString();
                        dn_row_detail.GROSS_WEIGHT = R_dn_detail["GROSS_WEIGHT"].ToString();
                        dn_row_detail.UNIT_WEI = R_dn_detail["UNIT_WEI"].ToString();
                        dn_row_detail.LENGTH = R_dn_detail["LENGTH"].ToString();
                        dn_row_detail.WIDTH = R_dn_detail["WIDTH"].ToString();
                        dn_row_detail.HEIGHT = R_dn_detail["HEIGHT"].ToString();
                        dn_row_detail.UNIT_DIM = R_dn_detail["UNIT_DIM"].ToString();
                        dn_row_detail.BOX_LENGTH = R_dn_detail["BOX_LENGTH"].ToString();
                        dn_row_detail.BOX_WIDTH = R_dn_detail["BOX_WIDTH"].ToString();
                        dn_row_detail.BOX_HEIGHT = R_dn_detail["BOX_HEIGHT"].ToString();
                        dn_row_detail.UNIT_DIM_BOX = R_dn_detail["UNIT_DIM_BOX"].ToString();
                        dn_row_detail.P_WEIGHT_P = R_dn_detail["P_WEIGHT_P"].ToString();
                        dn_row_detail.EX_WEIGHT_P = R_dn_detail["EX_WEIGHT_P"].ToString();
                        dn_row_detail.ISO_COUN_ST = R_dn_detail["ISO_COUN_ST"].ToString();
                        dn_row_detail.ISO_COUN_PO = R_dn_detail["ISO_COUN_PO"].ToString();
                        dn_row_detail.MATNR_WEIGHT = R_dn_detail["MATNR_WEIGHT"].ToString();
                        dn_row_detail.PALLET_WEIGHT = R_dn_detail["PALLET_WEIGHT"].ToString();
                        dn_row_detail.BOX_WEIGHT = R_dn_detail["BOX_WEIGHT"].ToString();
                        dn_row_detail.SHIP_TO = R_dn_detail["SHIP_TO"].ToString();
                        dn_row_detail.SHIP_TO_DES = R_dn_detail["SHIP_TO_DES"].ToString();
                        dn_row_detail.BOX_COUNT = R_dn_detail["BOX_COUNT"].ToString();
                        dn_row_detail.PALLET_COUNT = R_dn_detail["PALLET_COUNT"].ToString();

                        sql = dn_row_detail.GetInsertString(DB_TYPE_ENUM.Oracle);
                        sfcdb.ExecSQL(sql);
                    }





                }
                
        }

        public void Download(OleExec sfcdb, string DN, string Plant)
        {
            DataTable RFC_Table = new DataTable();
            Dictionary<string, string> DicPara = new Dictionary<string, string>();
            ZRFC_GET_SHIP_HEADER zrfc_GET_SHIP_HEADER = new ZRFC_GET_SHIP_HEADER();
            zrfc_GET_SHIP_HEADER.SetValues(DN, Plant);
            zrfc_GET_SHIP_HEADER.CallRFC();

            DataTable result = zrfc_GET_SHIP_HEADER.GetTableValue("SHIPORDERHEADER");

            T_R_ZRFC_GET_SHIP_HEADER dn_header = new T_R_ZRFC_GET_SHIP_HEADER(sfcdb, DB_TYPE_ENUM.Oracle);

            Row_R_ZRFC_GET_SHIP_HEADER dn_row_header = (Row_R_ZRFC_GET_SHIP_HEADER)dn_header.NewRow();

            if (result.Rows.Count > 0)
            {
                string sql = $@"DELETE FROM R_ZRFC_GET_SHIP_HEADER WHERE VBELN = '{DN}'";
                sfcdb.ExecSQL(sql);
                dn_row_header.ID = dn_header.GetNewID(BU, sfcdb);
                dn_row_header.VGBEL = result.Rows[0]["VGBEL"].ToString();
                dn_row_header.VBELN = result.Rows[0]["VBELN"].ToString();
                dn_row_header.ERDAT = result.Rows[0]["ERDAT"].ToString();
                dn_row_header.WADAT_IST = result.Rows[0]["WADAT_IST"].ToString();
                dn_row_header.KUNAG = result.Rows[0]["KUNAG"].ToString();
                dn_row_header.NAME1SALE = result.Rows[0]["NAME1SALE"].ToString();
                dn_row_header.KUNNR = result.Rows[0]["KUNNR"].ToString();
                dn_row_header.NAME1SHIP = result.Rows[0]["NAME1SHIP"].ToString();
                dn_row_header.STRAS = result.Rows[0]["STRAS"].ToString();
                dn_row_header.ORT01 = result.Rows[0]["ORT01"].ToString();
                dn_row_header.REGIO = result.Rows[0]["REGIO"].ToString();
                dn_row_header.PSTLZ = result.Rows[0]["PSTLZ"].ToString();
                dn_row_header.TELF1 = result.Rows[0]["TELF1"].ToString();
                dn_row_header.PSTYV = result.Rows[0]["PSTYV"].ToString();
                dn_row_header.BSTKD = result.Rows[0]["BSTKD"].ToString();
                dn_row_header.BOLNR = result.Rows[0]["BOLNR"].ToString();
                dn_row_header.LIFNR = result.Rows[0]["LIFNR"].ToString();
                dn_row_header.WERKS = result.Rows[0]["WERKS"].ToString();
                dn_row_header.ERZET = result.Rows[0]["ERZET"].ToString();
                dn_row_header.AEDAT = result.Rows[0]["AEDAT"].ToString();
                dn_row_header.UTIME = result.Rows[0]["UTIME"].ToString();
                dn_row_header.INVOICE = result.Rows[0]["INVOICE"].ToString();
                dn_row_header.VSBED = result.Rows[0]["VSBED"].ToString();
                dn_row_header.ROUTE = result.Rows[0]["ROUTE"].ToString();
                dn_row_header.CUSTPONO = result.Rows[0]["CUSTPONO"].ToString();
                dn_row_header.LFART = result.Rows[0]["LFART"].ToString();
                dn_row_header.NAMEC = result.Rows[0]["NAMEC"].ToString();
                dn_row_header.TELFC = result.Rows[0]["TELFC"].ToString();
                dn_row_header.TDLNR = result.Rows[0]["TDLNR"].ToString();
                dn_row_header.SCACD = result.Rows[0]["SCACD"].ToString();
                dn_row_header.WADAT = result.Rows[0]["WADAT"].ToString();
                dn_row_header.KUNWE = result.Rows[0]["KUNWE"].ToString();
                dn_row_header.HEADREMARK = result.Rows[0]["HEADREMARK"].ToString();
                dn_row_header.PERSON = result.Rows[0]["PERSON"].ToString();
                dn_row_header.TEL_NUMBER = result.Rows[0]["TEL_NUMBER"].ToString();
                dn_row_header.TEL_EXTENS = result.Rows[0]["TEL_EXTENS"].ToString();
                dn_row_header.FAX_NUMBER = result.Rows[0]["FAX_NUMBER"].ToString();
                dn_row_header.FAX_EXTENS = result.Rows[0]["FAX_EXTENS"].ToString();
                dn_row_header.REMARK = result.Rows[0]["REMARK"].ToString();
                dn_row_header.SH_NAME = result.Rows[0]["SH_NAME"].ToString();
                dn_row_header.SH_CITY1 = result.Rows[0]["SH_CITY1"].ToString();
                dn_row_header.SH_POST = result.Rows[0]["SH_POST"].ToString();
                dn_row_header.SH_STREET = result.Rows[0]["SH_STREET"].ToString();
                dn_row_header.SH_STREET2 = result.Rows[0]["SH_STREET2"].ToString();
                dn_row_header.SH_VAT = result.Rows[0]["SH_VAT"].ToString();
                dn_row_header.SH_COUNTRY = result.Rows[0]["SH_COUNTRY"].ToString();
                dn_row_header.PLAN_DATE = result.Rows[0]["PLAN_DATE"].ToString();
                dn_row_header.POST_FLAG = result.Rows[0]["POST_FLAG"].ToString();
                dn_row_header.LPRIO = result.Rows[0]["LPRIO"].ToString();
                dn_row_header.LDDAT = result.Rows[0]["LDDAT"].ToString();
                dn_row_header.INCO1 = result.Rows[0]["INCO1"].ToString();
                dn_row_header.SHIPREMARK = result.Rows[0]["SHIPREMARK"].ToString();
                dn_row_header.VKORG = result.Rows[0]["VKORG"].ToString();
                dn_row_header.VTEXT = result.Rows[0]["VTEXT"].ToString();
                dn_row_header.ENAME = result.Rows[0]["ENAME"].ToString();
                dn_row_header.BEROT = result.Rows[0]["BEROT"].ToString();
                dn_row_header.DN1 = result.Rows[0]["DN1"].ToString();
                dn_row_header.SHIPTYPE = result.Rows[0]["SHIPTYPE"].ToString();
                dn_row_header.INVOICE_NO = result.Rows[0]["INVOICE_NO"].ToString();
                dn_row_header.ZSHIPTO = result.Rows[0]["ZSHIPTO"].ToString();
                sql = dn_row_header.GetInsertString(DB_TYPE_ENUM.Oracle);
                sfcdb.ExecSQL(sql);


            }



        }
    }
}
