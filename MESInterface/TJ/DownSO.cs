using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESInterface.TJ
{
    class DownSO : taskBase
    {
        public override void init()
        {
            try
            {
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
