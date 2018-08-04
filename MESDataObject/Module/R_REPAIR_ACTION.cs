using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_r_repair_action : DataObjectTable
    {
        public T_r_repair_action(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_r_repair_action(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_r_repair_action);
            TableName = "r_repair_action".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }

        public DataTable SelectRepairActionBySN(string sn, OleExec DB, DB_TYPE_ENUM DBType)
        {
            string strSql = $@"select re.sn,re.action_code,re.section_id,re.process,item.item_name,items.items_son,re.reason_code,re.description,re.fail_location,re.fail_code,
                                re.keypart_sn,re.new_keypart_sn,re.kp_no,re.tr_sn,re.mfr_code,re.mfr_name,re.date_code,re.lot_code,re.new_kp_no,
                                re.new_tr_sn,re.new_mfr_code,re.new_mfr_name,re.new_date_code,re.new_lot_code,re.repair_emp,re.edit_time,re.edit_emp
                                 from r_repair_action re,c_repair_items item,c_repair_items_son items  where 1=1 and re.items_id=item.id and re.items_son_id=items.id and sn='{sn}' ";
            DataTable res = DB.ExecSelect(strSql).Tables[0];
            return res;

        }
    }
    public class Row_r_repair_action : DataObjectBase
    {
        public Row_r_repair_action(DataObjectInfo info) : base(info)
        {

        }
        public r_repair_action GetDataObject()
        {
            r_repair_action DataObject = new r_repair_action();
            DataObject.ID = this.ID;
            DataObject.REPAIR_FAILCODE_ID = this.REPAIR_FAILCODE_ID;
            DataObject.SN = this.SN;
            DataObject.ACTION_CODE = this.ACTION_CODE;
            DataObject.SECTION_ID = this.SECTION_ID;
            DataObject.PROCESS = this.PROCESS;
            DataObject.ITEMS_ID = this.ITEMS_ID;
            DataObject.ITEMS_SON_ID = this.ITEMS_SON_ID;
            DataObject.REASON_CODE = this.REASON_CODE;
            DataObject.DESCRIPTION = this.DESCRIPTION;
            DataObject.FAIL_LOCATION = this.FAIL_LOCATION;
            DataObject.FAIL_CODE = this.FAIL_CODE;
            DataObject.KEYPART_SN = this.KEYPART_SN;
            DataObject.NEW_KEYPART_SN = this.NEW_KEYPART_SN;
            DataObject.KP_NO = this.KP_NO;
            DataObject.TR_SN = this.TR_SN;
            DataObject.MFR_CODE = this.MFR_CODE;
            DataObject.MFR_NAME = this.MFR_NAME;
            DataObject.DATE_CODE = this.DATE_CODE;
            DataObject.LOT_CODE = this.LOT_CODE;
            DataObject.NEW_KP_NO = this.NEW_KP_NO;
            DataObject.NEW_TR_SN = this.NEW_TR_SN;
            DataObject.NEW_MFR_CODE = this.NEW_MFR_CODE;
            DataObject.NEW_MFR_NAME = this.NEW_MFR_NAME;
            DataObject.NEW_DATE_CODE = this.NEW_DATE_CODE;
            DataObject.NEW_LOT_CODE = this.NEW_LOT_CODE;
            DataObject.REPAIR_EMP = this.REPAIR_EMP;
            DataObject.REPAIR_TIME = this.REPAIR_TIME;
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
        public string REPAIR_FAILCODE_ID
        {
            get

            {
                return (string)this["REPAIR_FAILCODE_ID"];
            }
            set
            {
                this["REPAIR_FAILCODE_ID"] = value;
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
        public string ACTION_CODE
        {
            get

            {
                return (string)this["ACTION_CODE"];
            }
            set
            {
                this["ACTION_CODE"] = value;
            }
        }
        public string SECTION_ID
        {
            get

            {
                return (string)this["SECTION_ID"];
            }
            set
            {
                this["SECTION_ID"] = value;
            }
        }
        public string PROCESS
        {
            get

            {
                return (string)this["PROCESS"];
            }
            set
            {
                this["PROCESS"] = value;
            }
        }
        public string ITEMS_ID
        {
            get

            {
                return (string)this["ITEMS_ID"];
            }
            set
            {
                this["ITEMS_ID"] = value;
            }
        }
        public string ITEMS_SON_ID
        {
            get

            {
                return (string)this["ITEMS_SON_ID"];
            }
            set
            {
                this["ITEMS_SON_ID"] = value;
            }
        }
        public string REASON_CODE
        {
            get

            {
                return (string)this["REASON_CODE"];
            }
            set
            {
                this["REASON_CODE"] = value;
            }
        }
        public string DESCRIPTION
        {
            get

            {
                return (string)this["DESCRIPTION"];
            }
            set
            {
                this["DESCRIPTION"] = value;
            }
        }
        public string FAIL_LOCATION
        {
            get

            {
                return (string)this["FAIL_LOCATION"];
            }
            set
            {
                this["FAIL_LOCATION"] = value;
            }
        }
        public string FAIL_CODE
        {
            get

            {
                return (string)this["FAIL_CODE"];
            }
            set
            {
                this["FAIL_CODE"] = value;
            }
        }
        public string KEYPART_SN
        {
            get

            {
                return (string)this["KEYPART_SN"];
            }
            set
            {
                this["KEYPART_SN"] = value;
            }
        }
        public string NEW_KEYPART_SN
        {
            get

            {
                return (string)this["NEW_KEYPART_SN"];
            }
            set
            {
                this["NEW_KEYPART_SN"] = value;
            }
        }
        public string KP_NO
        {
            get

            {
                return (string)this["KP_NO"];
            }
            set
            {
                this["KP_NO"] = value;
            }
        }
        public string TR_SN
        {
            get

            {
                return (string)this["TR_SN"];
            }
            set
            {
                this["TR_SN"] = value;
            }
        }
        public string MFR_CODE
        {
            get

            {
                return (string)this["MFR_CODE"];
            }
            set
            {
                this["MFR_CODE"] = value;
            }
        }
        public string MFR_NAME
        {
            get

            {
                return (string)this["MFR_NAME"];
            }
            set
            {
                this["MFR_NAME"] = value;
            }
        }
        public string DATE_CODE
        {
            get

            {
                return (string)this["DATE_CODE"];
            }
            set
            {
                this["DATE_CODE"] = value;
            }
        }
        public string LOT_CODE
        {
            get

            {
                return (string)this["LOT_CODE"];
            }
            set
            {
                this["LOT_CODE"] = value;
            }
        }
        public string NEW_KP_NO
        {
            get

            {
                return (string)this["NEW_KP_NO"];
            }
            set
            {
                this["NEW_KP_NO"] = value;
            }
        }
        public string NEW_TR_SN
        {
            get

            {
                return (string)this["NEW_TR_SN"];
            }
            set
            {
                this["NEW_TR_SN"] = value;
            }
        }
        public string NEW_MFR_CODE
        {
            get

            {
                return (string)this["NEW_MFR_CODE"];
            }
            set
            {
                this["NEW_MFR_CODE"] = value;
            }
        }
        public string NEW_MFR_NAME
        {
            get
            {
                return (string)this["NEW_MFR_NAME"];
            }
            set
            {
                this["NEW_MFR_NAME"] = value;
            }
        }
        public string NEW_DATE_CODE
        {
            get

            {
                return (string)this["NEW_DATE_CODE"];
            }
            set
            {
                this["NEW_DATE_CODE"] = value;
            }
        }
        public string NEW_LOT_CODE
        {
            get

            {
                return (string)this["NEW_LOT_CODE"];
            }
            set
            {
                this["NEW_LOT_CODE"] = value;
            }
        }
        public string REPAIR_EMP
        {
            get

            {
                return (string)this["REPAIR_EMP"];
            }
            set
            {
                this["REPAIR_EMP"] = value;
            }
        }
        public DateTime? REPAIR_TIME
        {
            get

            {
                return (DateTime?)this["REPAIR_TIME"];
            }
            set
            {
                this["REPAIR_TIME"] = value;
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
    public class r_repair_action
    {
        public string ID{get;set;}
        public string REPAIR_FAILCODE_ID{get;set;}
        public string SN{get;set;}
        public string ACTION_CODE{get;set;}
        public string SECTION_ID{get;set;}
        public string PROCESS{get;set;}
        public string ITEMS_ID{get;set;}
        public string ITEMS_SON_ID{get;set;}
        public string REASON_CODE{get;set;}
        public string DESCRIPTION{get;set;}
        public string FAIL_LOCATION{get;set;}
        public string FAIL_CODE{get;set;}
        public string KEYPART_SN{get;set;}
        public string NEW_KEYPART_SN{get;set;}
        public string KP_NO{get;set;}
        public string TR_SN{get;set;}
        public string MFR_CODE{get;set;}
        public string MFR_NAME{get;set;}
        public string DATE_CODE{get;set;}
        public string LOT_CODE{get;set;}
        public string NEW_KP_NO{get;set;}
        public string NEW_TR_SN{get;set;}
        public string NEW_MFR_CODE{get;set;}
        public string NEW_MFR_NAME{get;set;}
        public string NEW_DATE_CODE{get;set;}
        public string NEW_LOT_CODE{get;set;}
        public string REPAIR_EMP{get;set;}
        public DateTime? REPAIR_TIME{get;set;}
        public DateTime? EDIT_TIME{get;set;}
        public string EDIT_EMP{get;set;}
    }
}