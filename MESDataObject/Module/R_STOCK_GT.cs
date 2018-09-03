using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_STOCK_GT : DataObjectTable
    {
        public T_R_STOCK_GT(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_STOCK_GT(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_STOCK_GT);
            TableName = "R_STOCK_GT".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }

        public bool WOIsExistAndNotGT(string wo, OleExec sfcdb)
        {
            string sql = $@"select * from R_STOCK_GT where workorderno='{wo}' and sap_flag='0' and backflush_time is null ";
            DataTable dt = sfcdb.ExecSelect(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 獲取未拋賬的工單
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="confirmed_flag"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public R_STOCK_GT GetNotGTbjByWO(string wo, string confirmed_flag, OleExec sfcdb)
        {
            string sql = $@"select * from R_STOCK_GT where workorderno='{wo}' and sap_flag='0' and confirmed_flag='{confirmed_flag}' and backflush_time is null ";
            DataTable dt = sfcdb.ExecSelect(sql).Tables[0];            
            if (dt.Rows.Count > 0)
            {
                Row_R_STOCK_GT rowObj = (Row_R_STOCK_GT)this.NewRow();
                rowObj.loadData(dt.Rows[0]);
                return rowObj.GetDataObject();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 獲取未拋賬的工單list
        /// </summary>
        /// <param name="confirmed_flag"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public List<R_STOCK_GT> GetNotGTListByConfirmedFlag(string confirmed_flag,OleExec sfcdb)
        {
            string sql = $@"select * from R_STOCK_GT where 1=1 and sap_flag='0' and confirmed_flag='{confirmed_flag}' ";
            List<R_STOCK_GT> GTList = new List<R_STOCK_GT>();
            DataTable dt = sfcdb.ExecSelect(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {               
                foreach (DataRow row in dt.Rows)
                {
                    Row_R_STOCK_GT rowObj = (Row_R_STOCK_GT)this.NewRow();
                    rowObj.loadData(row);
                    GTList.Add(rowObj.GetDataObject());
                }
            }            
            return GTList;
        }
    }
    public class Row_R_STOCK_GT : DataObjectBase
    {
        public Row_R_STOCK_GT(DataObjectInfo info) : base(info)
        {

        }
        public R_STOCK_GT GetDataObject()
        {
            R_STOCK_GT DataObject = new R_STOCK_GT();
            DataObject.SAP_STATION_CODE = this.SAP_STATION_CODE;
            DataObject.BACKFLUSH_TIME = this.BACKFLUSH_TIME;
            DataObject.EDIT_TIME = this.EDIT_TIME;
            DataObject.EDIT_EMP = this.EDIT_EMP;
            DataObject.SAP_MESSAGE = this.SAP_MESSAGE;
            DataObject.SAP_FLAG = this.SAP_FLAG;
            DataObject.CONFIRMED_FLAG = this.CONFIRMED_FLAG;
            DataObject.TO_STORAGE = this.TO_STORAGE;
            DataObject.FROM_STORAGE = this.FROM_STORAGE;
            DataObject.TOTAL_QTY = this.TOTAL_QTY;
            DataObject.WORKORDERNO = this.WORKORDERNO;
            DataObject.SKUNO = this.SKUNO;
            DataObject.ID = this.ID;            
            return DataObject;
        }
        public string SAP_STATION_CODE
        {
            get
            {
                return (string)this["SAP_STATION_CODE"];
            }
            set
            {
                this["SAP_STATION_CODE"] = value;
            }
        }
        public DateTime? BACKFLUSH_TIME
        {
            get
            {
                return (DateTime?)this["BACKFLUSH_TIME"];
            }
            set
            {
                this["BACKFLUSH_TIME"] = value;
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
        public string SAP_MESSAGE
        {
            get
            {
                return (string)this["SAP_MESSAGE"];
            }
            set
            {
                this["SAP_MESSAGE"] = value;
            }
        }
        public string SAP_FLAG
        {
            get
            {
                return (string)this["SAP_FLAG"];
            }
            set
            {
                this["SAP_FLAG"] = value;
            }
        }
        public string CONFIRMED_FLAG
        {
            get
            {
                return (string)this["CONFIRMED_FLAG"];
            }
            set
            {
                this["CONFIRMED_FLAG"] = value;
            }
        }
        public string TO_STORAGE
        {
            get
            {
                return (string)this["TO_STORAGE"];
            }
            set
            {
                this["TO_STORAGE"] = value;
            }
        }
        public string FROM_STORAGE
        {
            get
            {
                return (string)this["FROM_STORAGE"];
            }
            set
            {
                this["FROM_STORAGE"] = value;
            }
        }
        public double? TOTAL_QTY
        {
            get
            {
                return (double?)this["TOTAL_QTY"];
            }
            set
            {
                this["TOTAL_QTY"] = value;
            }
        }
        public string WORKORDERNO
        {
            get
            {
                return (string)this["WORKORDERNO"];
            }
            set
            {
                this["WORKORDERNO"] = value;
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
    public class R_STOCK_GT
    {
        public string SAP_STATION_CODE;
        public DateTime? BACKFLUSH_TIME;
        public DateTime? EDIT_TIME;
        public string EDIT_EMP;
        public string SAP_MESSAGE;
        public string SAP_FLAG;
        public string CONFIRMED_FLAG;
        public string TO_STORAGE;
        public string FROM_STORAGE;
        public double? TOTAL_QTY;
        public string WORKORDERNO;
        public string SKUNO;
        public string ID;        
    }
}