using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_SHIP_DETAIL : DataObjectTable
    {
        public T_R_SHIP_DETAIL(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_SHIP_DETAIL(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_SHIP_DETAIL);
            TableName = "R_SHIP_DETAIL".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
    }
    public class Row_R_SHIP_DETAIL : DataObjectBase
    {
        public Row_R_SHIP_DETAIL(DataObjectInfo info) : base(info)
        {

        }
        public R_SHIP_DETAIL GetDataObject()
        {
            R_SHIP_DETAIL DataObject = new R_SHIP_DETAIL();
            DataObject.CREATEBY = this.CREATEBY;
            DataObject.SHIPDATE = this.SHIPDATE;
            DataObject.DN_LINE = this.DN_LINE;
            DataObject.DN_NO = this.DN_NO;
            DataObject.SKUNO = this.SKUNO;
            DataObject.SN = this.SN;
            DataObject.ID = this.ID;
            return DataObject;
        }
        public string CREATEBY
        {
            get
            {
                return (string)this["CREATEBY"];
            }
            set
            {
                this["CREATEBY"] = value;
            }
        }
        public DateTime? SHIPDATE
        {
            get
            {
                return (DateTime?)this["SHIPDATE"];
            }
            set
            {
                this["SHIPDATE"] = value;
            }
        }
        public string DN_LINE
        {
            get
            {
                return (string)this["DN_LINE"];
            }
            set
            {
                this["DN_LINE"] = value;
            }
        }
        public string DN_NO
        {
            get
            {
                return (string)this["DN_NO"];
            }
            set
            {
                this["DN_NO"] = value;
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
    public class R_SHIP_DETAIL
    {
        public string CREATEBY  { get; set; } 
        public DateTime? SHIPDATE{ get; set; }
        public string DN_LINE{ get; set; }
        public string DN_NO{ get; set; }
        public string SKUNO{ get; set; }
        public string SN{ get; set; }
        public string ID{ get; set; }
    }
}