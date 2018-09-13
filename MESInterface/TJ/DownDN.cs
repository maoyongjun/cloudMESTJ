using MESStation.Interface.SAPRFC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESInterface.TJ
{
   public class DownDN : taskBase
    {
        public delegate void AddDataGridDelegate(string dgvname, DataTable dt);
        public AddDataGridDelegate addDataGridDelegate;

        public override void init()
        {
            try
            {
                Output.UI = new DownDN_UI(this);
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
                ZRFC_GET_SHIP_HEADER zrfc_GET_SHIP_HEADER = new ZRFC_GET_SHIP_HEADER("TJ");

                //zrfc_GET_SHIP_HEADER.SetValues("3500581409", "GHUE");//NHGZ,WDN1//WDN1,WSL3
                zrfc_GET_SHIP_HEADER.SetLastTime("09/11/2018", "07:00:00");
                zrfc_GET_SHIP_HEADER.CallRFC();

                DataTable result = zrfc_GET_SHIP_HEADER.GetTableValue("SHIPORDERHEADER");
                if (this.addDataGridDelegate != null)
                {
                    this.addDataGridDelegate("dnHeader", result);
                }

                ZRFC_GET_SHIP_DETAIL zrfc_GET_SHIP_DETAIL = new ZRFC_GET_SHIP_DETAIL("TJ");

                zrfc_GET_SHIP_DETAIL.SetValues("3500581409", "GHUE");//NHGZ,WDN1//WDN1,WSL3
                zrfc_GET_SHIP_DETAIL.CallRFC();

               DataTable detailResult = zrfc_GET_SHIP_DETAIL.GetTableValue("SHIPORDERDETAIL");
                if (this.addDataGridDelegate != null)
                {
                    this.addDataGridDelegate("DNDetail", detailResult);
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
