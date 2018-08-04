using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MESDBHelper;
namespace MESReport.BaseReport
{
    public class OBAByLotReport : ReportBase
    {
        ReportInput LotNo = new ReportInput { Name = "LotNo", InputType = "TXT", Value = "", Enable = true, SendChangeEvent = false, ValueForUse = null };
        public OBAByLotReport()
        {
            Inputs.Add(LotNo);
        }
        public override void Run()
        {
            DataTable dt = null;
            string lot_no = LotNo.Value.ToString();
            OleExec sfcdb = DBPools["SFCDB"].Borrow();
            DataRow linkDataRow = null;
            if (lot_no == "" || LotNo.Value == null)
            {
                ReportAlart alart = new ReportAlart("lot_no Can not be null");
                Outputs.Add(alart);
                return;
            }
            DataTable linkTable = new DataTable();
            try {
                string strSql = $@"select rld.sn,
                                       rld.workorderno,
                                       rpk.pack_no carton,
                                       rlp.packno pallet,
                                       decode(rld.status,'','未抽檢',1,'PASS',2,'FAIL') status,
                                       rls.lot_no,
                                       rls.edit_emp,
                                       rld.edit_time
                                  from r_lot_status rls,
                                       r_lot_pack   rlp,
                                       r_lot_detail rld,
                                       r_sn         rsn,
                                       r_sn_packing rspk,
                                       r_packing    rpk
                                 where rls.id = rld.lot_id
                                   and rlp.lotno = rls.lot_no
                                   and rld.sn = rsn.sn
                                   and rsn.id = rspk.sn_id
                                   and rspk.pack_id = rpk.id
                                   and rls.lot_no = '{lot_no}' order by rld.edit_time";
                dt = sfcdb.RunSelect(strSql).Tables[0];
                if (sfcdb != null)
                {
                    DBPools["SFCDB"].Return(sfcdb);
                }
                if (dt.Rows.Count == 0)
                {
                    ReportAlart alart = new ReportAlart("No Data!");
                    Outputs.Add(alart);
                    return;
                }

                linkTable.Columns.Add("sn");
                linkTable.Columns.Add("skuno");
                linkTable.Columns.Add("workorderno");
                linkTable.Columns.Add("carton");
                linkTable.Columns.Add("pallet");
                linkTable.Columns.Add("status");
                linkTable.Columns.Add("lotno");
                linkTable.Columns.Add("edit_emp");
                linkTable.Columns.Add("edit_time");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    linkDataRow = linkTable.NewRow();
                    //跳轉的頁面鏈接
                    linkDataRow["sn"] = "Link#/FunctionPage/Report/Report.html?ClassName=MESReport.BaseReport.SNReport&RunFlag=1&SN=" + dt.Rows[i]["SN"].ToString();
                    linkDataRow["skuno"] = "";
                    linkDataRow["workorderno"] = "";
                    linkDataRow["carton"] = "";
                    linkDataRow["pallet"] = "";
                    linkDataRow["status"] = "";
                    linkDataRow["lotno"] = "";
                    linkDataRow["edit_emp"] = "";
                    linkDataRow["edit_time"] = "";
                    linkTable.Rows.Add(linkDataRow);

                }
                ReportTable reportTable = new ReportTable();
                reportTable.LoadData(dt, linkTable);
                reportTable.Tittle = "OBASAMPLE DETAIL";
                Outputs.Add(reportTable);
            } catch (Exception ex) {
                if (sfcdb != null)
                {
                    DBPools["SFCDB"].Return(sfcdb);
                }
                ReportAlart alart = new ReportAlart(ex.Message);
                Outputs.Add(alart);
                return;
            }
        }
    }
}
