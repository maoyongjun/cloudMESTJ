using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface.SAPRFC
{
    class ZRFC_GET_SHIP_HEADER : SAP_RFC_BASE
    {
        public ZRFC_GET_SHIP_HEADER() : base()
        {
            SetRFC_NAME("ZRFC_GET_SHIP_HEADER");
        }
        public void SetValues(string DN, string PLANT)
        {
            ClearValues();

            SetValue("vbeln0", DN);
            SetValue("plant0", PLANT);


        }
    }
}
