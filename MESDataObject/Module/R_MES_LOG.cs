using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_MES_LOG : DataObjectTable
    {
        public T_R_MES_LOG(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_MES_LOG(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_MES_LOG);
            TableName = "R_MES_LOG".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
    }
    public class Row_R_MES_LOG : DataObjectBase
    {
        public Row_R_MES_LOG(DataObjectInfo info) : base(info)
        {

        }
        public R_MES_LOG GetDataObject()
        {
            R_MES_LOG DataObject = new R_MES_LOG();
            DataObject.FUNCTION_NAME = this.FUNCTION_NAME;
            DataObject.CLASS_NAME = this.CLASS_NAME;
            DataObject.PROGRAM_NAME = this.PROGRAM_NAME;
            DataObject.ID = this.ID;
            DataObject.DATA3 = this.DATA3;
            DataObject.DATA2 = this.DATA2;
            DataObject.DATA1 = this.DATA1;
            DataObject.EDIT_TIME = this.EDIT_TIME;
            DataObject.EDIT_EMP = this.EDIT_EMP;
            DataObject.LOG_SQL = this.LOG_SQL;
            DataObject.LOG_MESSAGE = this.LOG_MESSAGE;
            DataObject.MAILFLAG = this.MAILFLAG;
            return DataObject;
        }
        public string FUNCTION_NAME
        {
            get
            {
                return (string)this["FUNCTION_NAME"];
            }
            set
            {
                this["FUNCTION_NAME"] = value;
            }
        }
        public string CLASS_NAME
        {
            get
            {
                return (string)this["CLASS_NAME"];
            }
            set
            {
                this["CLASS_NAME"] = value;
            }
        }
        public string PROGRAM_NAME
        {
            get
            {
                return (string)this["PROGRAM_NAME"];
            }
            set
            {
                this["PROGRAM_NAME"] = value;
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
        public string DATA3
        {
            get
            {
                return (string)this["DATA3"];
            }
            set
            {
                this["DATA3"] = value;
            }
        }
        public string DATA2
        {
            get
            {
                return (string)this["DATA2"];
            }
            set
            {
                this["DATA2"] = value;
            }
        }
        public string DATA1
        {
            get
            {
                return (string)this["DATA1"];
            }
            set
            {
                this["DATA1"] = value;
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
        public string LOG_SQL
        {
            get
            {
                return (string)this["LOG_SQL"];
            }
            set
            {
                this["LOG_SQL"] = value;
            }
        }
        public string LOG_MESSAGE
        {
            get
            {
                return (string)this["LOG_MESSAGE"];
            }
            set
            {
                this["LOG_MESSAGE"] = value;
            }
        }
        public string MAILFLAG
        {
            get
            {
                return (string)this["MAILFLAG"];
            }
            set
            {
                this["MAILFLAG"] = value;
            }
        }
    }
    public class R_MES_LOG
    {
        public string FUNCTION_NAME{ get; set; }
        public string CLASS_NAME{ get; set; }
        public string PROGRAM_NAME{ get; set; }
        public string ID{ get; set; }
        public string DATA3{ get; set; }
        public string DATA2{ get; set; }
        public string DATA1{ get; set; }
        public DateTime? EDIT_TIME{ get; set; }
        public string EDIT_EMP{ get; set; }
        public string LOG_SQL{ get; set; }
        public string LOG_MESSAGE{ get; set; }
        public string MAILFLAG { get; set; }
    }
}