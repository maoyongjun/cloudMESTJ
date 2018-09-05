using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface.SAPRFC
{
    class ZP2SF01 : SAP_RFC_BASE
    {
        public ZP2SF01() : base()
        {
            SetRFC_NAME("ZP2SF01");
        }
        public void SetValues(string TPO, string PLANT)
        {
            ClearValues();

            SetValue("TKNUM0", TPO);
            SetValue("PLANT0", PLANT);


        }
    }
}
