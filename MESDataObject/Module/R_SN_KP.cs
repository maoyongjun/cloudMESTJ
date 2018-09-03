using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_SN_KP : DataObjectTable
    {
        public T_R_SN_KP(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_SN_KP(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_SN_KP);
            TableName = "R_SN_KP".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
        public List<R_SN_KP> GetKPRecordBySnIDStation(string SNID, string Station, OleExec SFCDB)
        {
            string strSql = $@"select * from r_sn_kp r where r_sn_id='{SNID}' and station='{Station}'order by r.itemseq,r.scanseq,r.detailseq  ";
            List<R_SN_KP> LR = new List<R_SN_KP>();
            DataSet res = SFCDB.RunSelect(strSql);
            for (int i = 0; i < res.Tables[0].Rows.Count; i++)
            {
                Row_R_SN_KP R = new Row_R_SN_KP(this.DataInfo);
                R.loadData(res.Tables[0].Rows[i]);
                LR.Add(R.GetDataObject());
            }
            return LR;
        }

        public List<R_SN_KP> GetKPRecordBySnID(string SNID, OleExec SFCDB)
        {
            string strSql = $@"select * from r_sn_kp r where r_sn_id='{SNID}' order by r.itemseq,r.scanseq,r.detailseq  ";
            List<R_SN_KP> LR = new List<R_SN_KP>();
            DataSet res = SFCDB.RunSelect(strSql);
            for (int i = 0; i < res.Tables[0].Rows.Count; i++)
            {
                Row_R_SN_KP R = new Row_R_SN_KP(this.DataInfo);
                R.loadData(res.Tables[0].Rows[i]);
                LR.Add(R.GetDataObject());
            }
            return LR;
        }
        public string UpdateSNBySnId(string snId,string sn,string user, OleExec SFCDB)
        {
            string sql = $@" update r_sn_kp set sn='{sn}',edit_time=sysdate,edit_emp='{user}' where r_sn_id='{snId}' ";
            return SFCDB.ExecSQL(sql);
        }
        public int  RetrunUpdateKPSNBySnId(string snId,string stationName,string user,OleExec DB) {
            string sql = $@" update r_sn_kp set VALUE ='',edit_time=sysdate,edit_emp='{user}'  where r_sn_id='{snId}' and station ='{stationName}'";
            int res = DB.ExecuteNonQuery(sql, CommandType.Text, null);
            return res;
        }

        public bool CheckLinkBySNID(string snID, OleExec sfcdb)
        {
            string sql = $@"select * from r_sn_kp where r_sn_id='{snID}' and value is not null";
            DataSet ds = sfcdb.RunSelect(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckLinkByValue(string value, OleExec sfcdb)
        {
            string sql = $@"select * from r_sn_kp where value ='{value}'  AND SCANTYPE IN ('SystemSN')";
            DataSet ds = sfcdb.RunSelect(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int DeleteBySNID(string snID, OleExec sfcdb)
        {
            string sql = $@" delete r_sn_kp where r_sn_id='{snID}' ";
            int res = sfcdb.ExecuteNonQuery(sql, CommandType.Text, null);
            return res;
        }

        public bool KpIsLinkBySN(string snID, string kp, OleExec sfcdb)
        {
            string sql = $@"select * from r_sn_kp where r_sn_id='{snID}' and value ='{kp}'";
            DataSet ds = sfcdb.RunSelect(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ReplaceSnKP(string newSn, string oldSn, OleExec sfcdb)
        {
            int result = 0;
            string sql = string.Empty;
            if (this.DBType == DB_TYPE_ENUM.Oracle)
            {
                sql = $@"UPDATE R_SN_KEYPART_DETAIL R SET R.SN='{newSn}' WHERE R.SN='{oldSn}'";
                result = sfcdb.ExecSqlNoReturn(sql, null);
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }

            return result;

        }
    }
    public class Row_R_SN_KP : DataObjectBase
    {
        public Row_R_SN_KP(DataObjectInfo info) : base(info)
        {
            
        }

        
        public R_SN_KP GetDataObject()
        {
            R_SN_KP DataObject = new R_SN_KP();
            DataObject.ID = this.ID;
            DataObject.R_SN_ID = this.R_SN_ID;
            DataObject.SN = this.SN;
            DataObject.VALUE = this.VALUE;
            DataObject.PARTNO = this.PARTNO;
            DataObject.KP_NAME = this.KP_NAME;
            DataObject.MPN = this.MPN;
            DataObject.SCANTYPE = this.SCANTYPE;
            DataObject.ITEMSEQ = this.ITEMSEQ;
            DataObject.SCANSEQ = this.SCANSEQ;
            DataObject.DETAILSEQ = this.DETAILSEQ;
            DataObject.STATION = this.STATION;
            DataObject.REGEX = this.REGEX;
            DataObject.VALID_FLAG = this.VALID_FLAG;
            DataObject.EXKEY1 = this.EXKEY1;
            DataObject.EXVALUE1 = this.EXVALUE1;
            DataObject.EXKEY2 = this.EXKEY2;
            DataObject.EXVALUE2 = this.EXVALUE2;
            DataObject.EDIT_TIME = this.EDIT_TIME;
            DataObject.EDIT_EMP = this.EDIT_EMP;
            return DataObject;
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
        public string R_SN_ID
        {
            get
            {
                return (string)this["R_SN_ID"];
            }
            set
            {
                this["R_SN_ID"] = value;
            }
        }
        public string SN
        {
            get
            {
                return (string)this["SN"];
            }
            set
            {
                this["SN"] = value;
            }
        }
        public string VALUE
        {
            get
            {
                return (string)this["VALUE"];
            }
            set
            {
                this["VALUE"] = value;
            }
        }
        public string PARTNO
        {
            get
            {
                return (string)this["PARTNO"];
            }
            set
            {
                this["PARTNO"] = value;
            }
        }
        public string KP_NAME
        {
            get
            {
                return (string)this["KP_NAME"];
            }
            set
            {
                this["KP_NAME"] = value;
            }
        }
        public string MPN
        {
            get
            {
                return (string)this["MPN"];
            }
            set
            {
                this["MPN"] = value;
            }
        }
        public string SCANTYPE
        {
            get
            {
                return (string)this["SCANTYPE"];
            }
            set
            {
                this["SCANTYPE"] = value;
            }
        }
        public double? ITEMSEQ
        {
            get
            {
                return (double?)this["ITEMSEQ"];
            }
            set
            {
                this["ITEMSEQ"] = value;
            }
        }
        public double? SCANSEQ
        {
            get
            {
                return (double?)this["SCANSEQ"];
            }
            set
            {
                this["SCANSEQ"] = value;
            }
        }
        public double? DETAILSEQ
        {
            get
            {
                return (double?)this["DETAILSEQ"];
            }
            set
            {
                this["DETAILSEQ"] = value;
            }
        }
        public string STATION
        {
            get
            {
                return (string)this["STATION"];
            }
            set
            {
                this["STATION"] = value;
            }
        }
        public string REGEX
        {
            get
            {
                return (string)this["REGEX"];
            }
            set
            {
                this["REGEX"] = value;
            }
        }
        public double? VALID_FLAG
        {
            get
            {
                return (double?)this["VALID_FLAG"];
            }
            set
            {
                this["VALID_FLAG"] = value;
            }
        }
        public string EXKEY1
        {
            get
            {
                return (string)this["EXKEY1"];
            }
            set
            {
                this["EXKEY1"] = value;
            }
        }
        public string EXVALUE1
        {
            get
            {
                return (string)this["EXVALUE1"];
            }
            set
            {
                this["EXVALUE1"] = value;
            }
        }
        public string EXKEY2
        {
            get
            {
                return (string)this["EXKEY2"];
            }
            set
            {
                this["EXKEY2"] = value;
            }
        }
        public string EXVALUE2
        {
            get
            {
                return (string)this["EXVALUE2"];
            }
            set
            {
                this["EXVALUE2"] = value;
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
    }
    public class R_SN_KP
    {
        public string ID{get;set;}
        public string R_SN_ID{get;set;}
        public string SN{get;set;}
        public string VALUE{get;set;}
        public string PARTNO{get;set;}
        public string KP_NAME{get;set;}
        public string MPN{get;set;}
        public string SCANTYPE{get;set;}
        public double? ITEMSEQ{get;set;}
        public double? SCANSEQ{get;set;}
        public double? DETAILSEQ{get;set;}
        public string STATION{get;set;}
        public string REGEX{get;set;}
        public double? VALID_FLAG{get;set;}
        public string EXKEY1{get;set;}
        public string EXVALUE1{get;set;}
        public string EXKEY2{get;set;}
        public string EXVALUE2{get;set;}
        public DateTime? EDIT_TIME{get;set;}
        public string EDIT_EMP{get;set;}
    }
}