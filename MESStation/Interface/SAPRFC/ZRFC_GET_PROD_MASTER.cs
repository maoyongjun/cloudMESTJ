using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Interface.SAPRFC
{
    class ZRFC_GET_PROD_MASTER : SAP_RFC_BASE
    {
        public ZRFC_GET_PROD_MASTER() : base()
        {
            SetRFC_NAME("ZRFC_GET_PROD_MASTER");
        }
        public void SetValues(string SKUNO, string FACTORY)
        {
            ClearValues();

            SetValue("Partno", SKUNO);
            SetValue("PLANT", FACTORY);


        }
    }
}
