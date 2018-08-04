﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MESReport.Test
{
    public class MESAPIListReport:ReportBase
    {
        public override void Run()
        {
            
            Assembly assenbly = Assembly.Load("MESStation");
            //Type tagType = typeof(BaseClass.MesAPIBase);
            Type[] t = assenbly.GetTypes();
            Type tagType = assenbly.GetType("MESStation.BaseClass.MesAPIBase");
            var method = tagType.GetMethod("GetApiListTable");
            var res = method.Invoke(null, new object[] { });
            DataTable dt = (DataTable)res;
            ReportTable retTab = new ReportTable();
            retTab.LoadData(dt, null);
            retTab.Tittle = "";
            Outputs.Add(retTab);

        }

    }
}
