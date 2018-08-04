using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_SN_PACKING : DataObjectTable
    {
        public T_R_SN_PACKING(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_SN_PACKING(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_SN_PACKING);
            TableName = "R_SN_PACKING".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }

        public List<Row_R_SN_PACKING> GetPackItem(string PackID, OleExec DB)
        {
            List<Row_R_SN_PACKING> ret = new List<Row_R_SN_PACKING>();
            string strSql = $@"select * from R_SN_PACKING where PACK_ID = '{PackID}' ";
            DataSet res = DB.RunSelect(strSql);
            
            for (int i = 0; i < res.Tables[0].Rows.Count; i++)
            {
                Row_R_SN_PACKING R = (Row_R_SN_PACKING)NewRow();
                R.loadData(res.Tables[0].Rows[i]);
                ret.Add(R);
            }
            return ret;
        }

        public Row_R_SN_PACKING GetDataBySNID(string SN_ID, OleExec DB)
        {
            string strSql = $@"select * from R_SN_PACKING where SN_ID = '{SN_ID}' ";
            DataSet res = DB.RunSelect(strSql);
            if (res.Tables[0].Rows.Count > 0)
            {
                Row_R_SN_PACKING ret = (Row_R_SN_PACKING)NewRow();
                ret.loadData(res.Tables[0].Rows[0]);
                return ret;
            }
            return null;
        }

        /// <summary>
        /// 檢查包裝內所有SN的當前工站是否一致:包裝為空或狀態不一致返回False
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="Station"></param>
        /// <returns></returns>
        public bool CheckPackSnStatus(OleExec DB,string Station,string PackNo)
        {
            string strSql = $@" select count(1) lotnum from (                             
                                select count(1) as lotnum1 from r_sn a,r_packing b,r_sn_packing c,r_packing d where a.id=c.sn_id and c.pack_id=b.id and b.parent_pack_id=d.id 
                                and d.pack_no='{PackNo}' and a.next_station='{Station}') a,  
                                (select count(1) as lotnum2 from r_sn a,r_packing b,r_sn_packing c,r_packing d where a.id=c.sn_id and c.pack_id=b.id and b.parent_pack_id=d.id 
                                and d.pack_no='{PackNo}' ) b  where a.lotnum1=b.lotnum2 and a.lotnum1<>0";
            int tolnum = Convert.ToInt32(DB.ExecSelectOneValue(strSql).ToString());
            if (tolnum == 1)
                return true;
            return false;
        }

        /// <summary>
        /// 檢查SN是否在對應卡通內
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="packID"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public bool CheckSNExistByPackID(string snID, string packID, OleExec sfcdb)
        {
            string sql = $@" select * from r_sn_packing where pack_id='{packID}' and sn_id='snID' ";
            DataSet ds = sfcdb.ExecSelect(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string UpdatePackIDBySnID(string snID, string parnetPackID,string emp, OleExec sfcdb)
        {
            string sql = $@" update r_sn_packing set pack_id='{parnetPackID}',edit_emp='{emp}',edit_time=sysdate where sn_id='{snID}' ";
            return sfcdb.ExecSQL(sql);
        }
    }
    public class Row_R_SN_PACKING : DataObjectBase
    {
        public Row_R_SN_PACKING(DataObjectInfo info) : base(info)
        {

        }
        public R_SN_PACKING GetDataObject()
        {
            R_SN_PACKING DataObject = new R_SN_PACKING();
            DataObject.EDIT_EMP = this.EDIT_EMP;
            DataObject.EDIT_TIME = this.EDIT_TIME;
            DataObject.PACK_ID = this.PACK_ID;
            DataObject.SN_ID = this.SN_ID;
            DataObject.ID = this.ID;
            return DataObject;
        }
        public string EDIT_EMP
        {
            get
            {
                return (string)this["EDIT_EMP"];
            }
            set
            {
                this["EDIT_EMP"] = value;
            }
        }
        public DateTime? EDIT_TIME
        {
            get
            {
                return (DateTime?)this["EDIT_TIME"];
            }
            set
            {
                this["EDIT_TIME"] = value;
            }
        }
        public string PACK_ID
        {
            get
            {
                return (string)this["PACK_ID"];
            }
            set
            {
                this["PACK_ID"] = value;
            }
        }
        public string SN_ID
        {
            get
            {
                return (string)this["SN_ID"];
            }
            set
            {
                this["SN_ID"] = value;
            }
        }
        public string ID
        {
            get
            {
                return (string)this["ID"];
            }
            set
            {
                this["ID"] = value;
            }
        }
    }
    public class R_SN_PACKING
    {
        public string EDIT_EMP{get;set;}
        public DateTime? EDIT_TIME{get;set;}
        public string PACK_ID{get;set;}
        public string SN_ID{get;set;}
        public string ID{get;set;}
    }
}