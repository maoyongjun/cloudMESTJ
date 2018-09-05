using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface.SAPRFC
{
    class ZRFC_GET_SHIP_DETAIL : SAP_RFC_BASE
    {
        public ZRFC_GET_SHIP_DETAIL() : base()
        {
            SetRFC_NAME("ZRFC_GET_SHIP_DETAIL");
        }
        public void SetValues(string DN, string PLANT)
        {
            ClearValues();

            SetValue("VBELN0", DN);
            SetValue("PLANT0", PLANT);


        }
    }
}
