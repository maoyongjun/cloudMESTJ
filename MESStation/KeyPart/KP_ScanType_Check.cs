using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESStation.BaseClass;
using MESDataObject.Module;
using MESDataObject;
using Newtonsoft.Json.Linq;
using System.Reflection;
using MESDBHelper;

namespace MESStation.KeyPart
{
    public class KP_ScanType_Check
    {
        public static void CheckTEST(SN_KP config,LogicObject.SN sn,Row_R_SN_KP scan,List<Row_R_SN_KP> scans , MesAPIBase API ,OleExec sfcdb)
        {
            
        }

        public static void LastScanRuleChecker(SN_KP config, LogicObject.SN sn, Row_R_SN_KP scan, List<Row_R_SN_KP> scans, MesAPIBase API, OleExec sfcdb)
        {            
            T_R_SN_KP t_r_sn_kp = new T_R_SN_KP(sfcdb, DB_TYPE_ENUM.Oracle);
            List<R_SN_KP> kpList = t_r_sn_kp.GetKPRecordBySnID(sn.ID, sfcdb);
            R_SN_KP lastScan = kpList.Find(k => k.SCANSEQ == (scan.SCANSEQ - 1));
            string scanValue = scan.VALUE.Substring(4, scan.VALUE.Length - 4);
            string lastScanVlaue = lastScan.VALUE.Substring(0, scan.VALUE.Length - 4);
            if (lastScan != null)
            {
                if (scanValue != lastScanVlaue)
                {
                    throw new Exception("this value "+ scan.VALUE +"is inconsistent with the last one");
                }
            }
        }

        public static void SystemSNScanRuleChecker(SN_KP config, LogicObject.SN sn, Row_R_SN_KP scan, List<Row_R_SN_KP> scans, MesAPIBase API, OleExec sfcdb)
        {
            LogicObject.SN kpsn = new LogicObject.SN(scan.VALUE, sfcdb, DB_TYPE_ENUM.Oracle);
            if (kpsn.CompletedFlag != "1")
            {
                throw new Exception($@"{kpsn.SerialNo} not finish !");
            }
            if (kpsn.ShippedFlag == "1")
            {
                throw new Exception($@"{kpsn.SerialNo} has being Shipped!");
            }
            if (kpsn.baseSN.SKUNO != scan.PARTNO)
            {
                throw new Exception($@"{kpsn.SerialNo} is {kpsn.SkuNo} config is {scan.PARTNO}");
            }
            kpsn.baseSN.SHIPPED_FLAG = "1";
            kpsn.baseSN.SHIPDATE = DateTime.Now;
            kpsn.baseSN.EDIT_TIME = DateTime.Now;
            kpsn.baseSN.EDIT_EMP = API.LoginUser.EMP_NO;
            sfcdb.ORM.Updateable<R_SN>(kpsn.baseSN).Where(t => t.ID == kpsn.baseSN.ID).ExecuteCommand();



        }
    }
}
