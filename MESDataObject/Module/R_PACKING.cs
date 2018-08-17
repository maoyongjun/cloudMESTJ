using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_PACKING : DataObjectTable
    {
        public T_R_PACKING(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_PACKING(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_PACKING);
            TableName = "R_PACKING".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }

        public Row_R_PACKING GetRPackingByPackNo(OleExec DB,string PackNo)
        {

            PackNo = "P18061500001";
            string strSql = $@" SELECT * FROM R_PACKING where PACK_NO='{PackNo}' ";
            DataSet ds = DB.ExecSelect(strSql);
            Row_R_PACKING r = (Row_R_PACKING)this.NewRow();
            r.loadData(ds.Tables[0].Rows[0]);
            return r;
        }

        public List<R_PACKING> GetListPackByParentPackId(string parentPackId,OleExec DB)
        {
            List<R_PACKING> packingList = new List<R_PACKING>();
            Row_R_PACKING rowPacking;
            string strSql = $@"select * from r_packing where parent_pack_id='{parentPackId}'";
            DataSet ds = DB.ExecSelect(strSql);
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                rowPacking = (Row_R_PACKING)this.NewRow();
                rowPacking.loadData(row);
                packingList.Add(rowPacking.GetDataObject());
            }
            return packingList;
        }
        public List<R_PACKING> GetListPackByPackno(string packno, OleExec DB)
        {
            List<R_PACKING> packingList = new List<R_PACKING>();
            Row_R_PACKING rowPacking;
            string strSql = $@"select * from r_packing where PACK_NO='{packno}'";
            DataSet ds = DB.ExecSelect(strSql);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                rowPacking = (Row_R_PACKING)this.NewRow();
                rowPacking.loadData(row);
                packingList.Add(rowPacking.GetDataObject());
            }
            return packingList;
        }

        public bool CheckCloseByPackno(string packNo,OleExec db)
        {
            string sql = $@" SELECT * FROM R_PACKING where closed_flag='1' and PACK_NO='{packNo}' ";
            DataSet ds = db.ExecSelect(sql);
            if(ds.Tables[0].Rows.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 檢查卡通是否在對應棧板內
        /// </summary>
        /// <param name="packNo"></param>
        /// <param name="parnetPackID"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public bool CheckPackNoExistByParentPackID(string packNo,string parnetPackID,OleExec sfcdb)
        {
            string sql = $@" SELECT * FROM R_PACKING where parent_pack_id='{parnetPackID}' and PACK_NO='{packNo}' ";
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

        public string UpdateParentPackIDByPackNo(string packNo, string parnetPackID,string emp, OleExec sfcdb)
        {
            string sql = $@" update r_packing set parent_pack_id='{parnetPackID}',edit_time=sysdate,edit_emp='{emp}' where pack_no='{packNo}'";
            return sfcdb.ExecSQL(sql);
        }

        public string UpdateParentPackIDBySN(string sn,string parentPackID,OleExec sfcdb)
        {
            string sql = $@" update r_packing set parent_pack_id='' where id in (select n.pack_id from r_sn_packing n,r_sn m where n.sn_id=m.id and m.sn='{sn}' and m.valid_flag='1')";
            return sfcdb.ExecSQL(sql);
        }

        public R_PACKING GetPackingObjectBySN(string sn, OleExec sfcdb)
        {
            string sql = $@"select * from r_packing  where id in (select n.pack_id from r_sn_packing n,r_sn m where n.sn_id=m.id and m.sn='{sn}' and m.valid_flag='1')";
            DataSet ds = sfcdb.ExecSelect(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Row_R_PACKING rowPacking = (Row_R_PACKING)this.NewRow();
                rowPacking.loadData(ds.Tables[0].Rows[0]);
                return rowPacking.GetDataObject();
            }
            else
            {
                return null;
            }
        }

        public string UpdateCloseFlagByPackID(string packID,string closedFlag,OleExec sfcdb)
        {
            Row_R_PACKING rowPacking = (Row_R_PACKING)this.GetObjByID(packID, sfcdb);
            rowPacking.CLOSED_FLAG = closedFlag;
            return sfcdb.ExecSQL(rowPacking.GetUpdateString(DB_TYPE_ENUM.Oracle));
        }

        public string UpdateQtyByID(string packId,bool isAdd,double qty,string emp,OleExec sfcdb)
        {
            Row_R_PACKING rowPacking = (Row_R_PACKING)this.GetObjByID(packId, sfcdb);
            if(isAdd)
            {
                rowPacking.QTY = rowPacking.QTY + qty;
            }
            else
            {
                rowPacking.QTY = rowPacking.QTY - qty;
            }
            rowPacking.EDIT_TIME = this.GetDBDateTime(sfcdb);
            rowPacking.EDIT_EMP = emp;
            return sfcdb.ExecSQL(rowPacking.GetUpdateString(DB_TYPE_ENUM.Oracle));
        }

        public bool PackNoIsExist(string packNo, OleExec sfcdb)
        {
            string sql = $@" SELECT * FROM R_PACKING where PACK_NO='{packNo}' ";
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

        public List<string> GetPakcingSNList(string parentPakcId,OleExec sfcdb)
        {
            string sql=$@"select c.sn from r_packing a,r_sn_packing b,r_sn c where a.parent_pack_id='{parentPakcId}' and a.id = b.pack_id and b.sn_id = c.id";
            List<string> snList = new List<string>();            
            DataSet ds = sfcdb.ExecSelect(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    snList.Add(ds.Tables[0].Rows[i]["SN"].ToString());
                }
            }
            else
            {
                snList = null;
            }
            return snList;
        }
    }
    public class Row_R_PACKING : DataObjectBase
    {
        public Row_R_PACKING(DataObjectInfo info) : base(info)
        {

        }
        public R_PACKING GetDataObject()
        {
            R_PACKING DataObject = new R_PACKING();
            DataObject.IP = this.IP;
            DataObject.STATION = this.STATION;
            DataObject.LINE = this.LINE;
            DataObject.EDIT_EMP = this.EDIT_EMP;
            DataObject.EDIT_TIME = this.EDIT_TIME;
            DataObject.CREATE_TIME = this.CREATE_TIME;
            DataObject.CLOSED_FLAG = this.CLOSED_FLAG;
            DataObject.QTY = this.QTY;
            DataObject.MAX_QTY = this.MAX_QTY;
            DataObject.SKUNO = this.SKUNO;
            DataObject.PARENT_PACK_ID = this.PARENT_PACK_ID;
            DataObject.PACK_TYPE = this.PACK_TYPE;
            DataObject.PACK_NO = this.PACK_NO;
            DataObject.ID = this.ID;
            return DataObject;
        }
        public string IP
        {
            get
            {
                return (string)this["IP"];
            }
            set
            {
                this["IP"] = value;
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
        public string LINE
        {
            get
            {
                return (string)this["LINE"];
            }
            set
            {
                this["LINE"] = value;
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
        public DateTime? CREATE_TIME
        {
            get
            {
                return (DateTime?)this["CREATE_TIME"];
            }
            set
            {
                this["CREATE_TIME"] = value;
            }
        }
        public string CLOSED_FLAG
        {
            get
            {
                return (string)this["CLOSED_FLAG"];
            }
            set
            {
                this["CLOSED_FLAG"] = value;
            }
        }
        public double? QTY
        {
            get
            {
                return (double?)this["QTY"];
            }
            set
            {
                this["QTY"] = value;
            }
        }
        public double? MAX_QTY
        {
            get
            {
                return (double?)this["MAX_QTY"];
            }
            set
            {
                this["MAX_QTY"] = value;
            }
        }
        public string SKUNO
        {
            get
            {
                return (string)this["SKUNO"];
            }
            set
            {
                this["SKUNO"] = value;
            }
        }
        public string PARENT_PACK_ID
        {
            get
            {
                return (string)this["PARENT_PACK_ID"];
            }
            set
            {
                this["PARENT_PACK_ID"] = value;
            }
        }
        public string PACK_TYPE
        {
            get
            {
                return (string)this["PACK_TYPE"];
            }
            set
            {
                this["PACK_TYPE"] = value;
            }
        }
        public string PACK_NO
        {
            get
            {
                return (string)this["PACK_NO"];
            }
            set
            {
                this["PACK_NO"] = value;
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
    public class R_PACKING
    {
        public string IP{get;set;}
        public string STATION{get;set;}
        public string LINE{get;set;}
        public string EDIT_EMP{get;set;}
        public DateTime? EDIT_TIME{get;set;}
        public DateTime? CREATE_TIME{get;set;}
        public string CLOSED_FLAG{get;set;}
        public double? QTY{get;set;}
        public double? MAX_QTY{get;set;}
        public string SKUNO{get;set;}
        public string PARENT_PACK_ID{get;set;}
        public string PACK_TYPE{get;set;}
        public string PACK_NO{get;set;}
        public string ID{get;set;}
    }
}