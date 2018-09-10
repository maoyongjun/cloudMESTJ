using MESStation.Interface.SAPRFC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESInterface.TJ
{
    public class DownSO : taskBase
    {
        public delegate void AddDataGridDelegate(string dgvname, DataTable dt);
        public AddDataGridDelegate addDataGridDelegate;

        public override void init()
        {
            try
            {
                Output.UI = new DownSO_UI(this);
                Console.Out.WriteLine("init");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public override void Start()
        {
            try
            {
                ZRFC_GET_SALES_HEADER2 zrfc_GET_SALES_HEADER2 = new ZRFC_GET_SALES_HEADER2("TJ");

                zrfc_GET_SALES_HEADER2.SetValues("0200274854", "GHUE");//NHGZ,WDN1//WDN1,WSL3
                zrfc_GET_SALES_HEADER2.CallRFC();

                DataTable result = zrfc_GET_SALES_HEADER2.GetTableValue("SOMASTER");
                if (this.addDataGridDelegate != null)
                {
                    this.addDataGridDelegate("SOHeaderData", result);
                }

                ZRFC_GET_SALES_DETAIL zrfc_GET_SALES_DETAIL = new ZRFC_GET_SALES_DETAIL("TJ");

                zrfc_GET_SALES_DETAIL.SetValues("0200274854", "GHUE");//NHGZ,WDN1//WDN1,WSL3
                zrfc_GET_SALES_DETAIL.CallRFC();

                DataTable detailResult = zrfc_GET_SALES_DETAIL.GetTableValue("SOSKU");
                if (this.addDataGridDelegate != null)
                {
                    this.addDataGridDelegate("SODetailData", detailResult);
                }
                Console.Out.WriteLine("action");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
