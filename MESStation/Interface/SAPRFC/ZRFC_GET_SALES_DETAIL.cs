using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface.SAPRFC
{
    class ZRFC_GET_SALES_DETAIL : SAP_RFC_BASE
    {
        public ZRFC_GET_SALES_DETAIL() : base()
        {
            SetRFC_NAME("ZRFC_GET_SALES_DETAIL");
        }
        public void SetValues(string SO, string PLANT)
        {
            ClearValues();

            SetValue("ORDERNUM", SO);
            SetValue("PLANT", PLANT);


        }
    }
}
