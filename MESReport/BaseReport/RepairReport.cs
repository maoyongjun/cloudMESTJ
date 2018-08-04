using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;
using MESDataObject;
using MESDataObject.Module;

namespace MESReport.BaseReport
{
    //維修報表

    public class RepairReport : ReportBase
    {

        ReportInput StartTime = new ReportInput() { Name = "StartTime", InputType = "DateTime", Value = "2017/02/01 12:00:00", Enable = true, SendChangeEvent = false, ValueForUse = null };
        ReportInput EndTime = new ReportInput() { Name = "EndTime", InputType = "DateTime", Value = "2018/02/12 12:00:00", Enable = true, SendChangeEvent = false, ValueForUse = null };
        ReportInput SN = new ReportInput() { Name = "SN", InputType = "TXT", Value = "ALL", Enable = true, SendChangeEvent = false, ValueForUse = null };
        ReportInput SkuNo = new ReportInput() { Name = "SkuNo", InputType = "Select", Value = "ALL", Enable = true, SendChangeEvent = false, ValueForUse = new string[] { "ALL" } };

        public RepairReport()
        {
            Inputs.Add(StartTime);
            Inputs.Add(EndTime);
            Inputs.Add(SN);
            Inputs.Add(SkuNo);
        }

        public override void Init()
        {
            try
            {
                StartTime.Value = DateTime.Now.AddDays(-1);
                EndTime.Value = DateTime.Now;
                OleExec SFCDB = DBPools["SFCDB"].Borrow();
                InitSkuno(SFCDB);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public override void Run()
        {
            DateTime stime = Convert.ToDateTime(StartTime.Value);
            DateTime etime = Convert.ToDateTime(EndTime.Value);
            string svalue = stime.ToString("yyyy/MM/dd HH:mm:ss");
            string evalue = etime.ToString("yyyy/MM/dd HH:mm:ss");
            OleExec SFCDB = DBPools["SFCDB"].Borrow();
            try
            {
                //string runSql = $@"SELECT a.sn, a.fail_line, b.EDIT_TIME repair_date, b.fail_code,  B.FAIL_LOCATION
                //                FROM R_REPAIR_MAIN a, R_REPAIR_FAILCODE b, R_REPAIR_ACTION c
                //                WHERE a.id = b.repair_main_id(+) AND B.ID = C.REPAIR_FAILCODE_ID(+)
                //               AND a.CREATE_TIME BETWEEN TO_DATE ('{svalue}','YYYY/MM/DD HH24:MI:SS')
                //              AND TO_DATE ('{evalue}', 'YYYY/MM/DD HH24:MI:SS') ";
                string runSql = $@"select rrm.sn,rrm.workorderno,rrm.skuno,rrm.fail_station,rrf.fail_code,
                                  rra.action_code,cec.chinese_description,rra.fail_location,rra.kp_no,
                                  rra.date_code,rra.lot_code,rra.process,rra.repair_emp,rra.repair_time
                                  from r_repair_main rrm,r_repair_failcode rrf,
                                  c_error_code cec,r_repair_action rra
                                  where  rrm.sn = rrf.sn and cec.error_code =rrf.fail_code(+)
                                  and rrm.CREATE_TIME BETWEEN TO_DATE ('{svalue}','YYYY/MM/DD HH24:MI:SS') 
                                  AND TO_DATE ('{evalue}', 'YYYY/MM/DD HH24:MI:SS') and rrf.sn = rra.sn ";
                if (SN.Value.ToString() != "ALL"&& SN.Value.ToString() != string.Empty)
                {
                    runSql += $@" and rrm.SN = '{ SN.Value.ToString()}'";
                }
                if (SkuNo.Value.ToString() != "ALL"&& SkuNo.Value.ToString() != string.Empty)
                {
                    runSql += $@" and rrm.skuno = '{SkuNo.Value.ToString()}'";
                }
                runSql += $@" order by rrm.sn ,rrm.workorderno,rra.repair_time";
                RunSqls.Add(runSql);
                DataSet res = SFCDB.RunSelect(runSql);
                ReportTable retTab = new ReportTable();

                retTab.LoadData(res.Tables[0], null);
                retTab.Tittle = "Repair Report";
                Outputs.Add(retTab);

                DBPools["SFCDB"].Return(SFCDB);
            }
            catch (Exception ee)
            {
                DBPools["SFCDB"].Return(SFCDB);
                throw ee;
            }

        }

        public void InitSkuno(OleExec db)
        {
            List<string> skuno = new List<string>();
            DataTable dt = new DataTable();
            T_C_SKU sku = new T_C_SKU(db, DB_TYPE_ENUM.Oracle);
            dt = sku.GetALLSkuno(db);
            skuno.Add("ALL");
            foreach (DataRow dr in dt.Rows)
            {
                skuno.Add(dr["SKUNO"].ToString());

            }
            SkuNo.ValueForUse = skuno;
        }
    }
}
