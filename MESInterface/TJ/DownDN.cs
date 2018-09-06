using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESInterface.TJ
{
    class DownDN : taskBase
    {
        public override void init()
        {
            try
            {
                Output.UI = new DownDN_UI();
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
                Console.Out.WriteLine("action");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
