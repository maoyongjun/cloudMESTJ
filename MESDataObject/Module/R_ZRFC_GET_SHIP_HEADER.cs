using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_ZRFC_GET_SHIP_HEADER : DataObjectTable
    {
        public T_R_ZRFC_GET_SHIP_HEADER(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_ZRFC_GET_SHIP_HEADER(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_ZRFC_GET_SHIP_HEADER);
            TableName = "R_ZRFC_GET_SHIP_HEADER".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
    }
    public class Row_R_ZRFC_GET_SHIP_HEADER : DataObjectBase
    {
        public Row_R_ZRFC_GET_SHIP_HEADER(DataObjectInfo info) : base(info)
        {

        }
        public R_ZRFC_GET_SHIP_HEADER GetDataObject()
        {
            R_ZRFC_GET_SHIP_HEADER DataObject = new R_ZRFC_GET_SHIP_HEADER();
            DataObject.VTEXT = this.VTEXT;
            DataObject.VKORG = this.VKORG;
            DataObject.SHIPREMARK = this.SHIPREMARK;
            DataObject.INCO1 = this.INCO1;
            DataObject.LDDAT = this.LDDAT;
            DataObject.LPRIO = this.LPRIO;
            DataObject.POST_FLAG = this.POST_FLAG;
            DataObject.PLAN_DATE = this.PLAN_DATE;
            DataObject.SH_COUNTRY = this.SH_COUNTRY;
            DataObject.SH_VAT = this.SH_VAT;
            DataObject.SH_STREET2 = this.SH_STREET2;
            DataObject.SH_STREET = this.SH_STREET;
            DataObject.SH_POST = this.SH_POST;
            DataObject.SH_CITY1 = this.SH_CITY1;
            DataObject.SH_NAME = this.SH_NAME;
            DataObject.REMARK = this.REMARK;
            DataObject.FAX_EXTENS = this.FAX_EXTENS;
            DataObject.FAX_NUMBER = this.FAX_NUMBER;
            DataObject.TEL_EXTENS = this.TEL_EXTENS;
            DataObject.TEL_NUMBER = this.TEL_NUMBER;
            DataObject.PERSON = this.PERSON;
            DataObject.HEADREMARK = this.HEADREMARK;
            DataObject.KUNWE = this.KUNWE;
            DataObject.WADAT = this.WADAT;
            DataObject.SCACD = this.SCACD;
            DataObject.TDLNR = this.TDLNR;
            DataObject.TELFC = this.TELFC;
            DataObject.NAMEC = this.NAMEC;
            DataObject.LFART = this.LFART;
            DataObject.CUSTPONO = this.CUSTPONO;
            DataObject.ROUTE = this.ROUTE;
            DataObject.VSBED = this.VSBED;
            DataObject.INVOICE = this.INVOICE;
            DataObject.UTIME = this.UTIME;
            DataObject.AEDAT = this.AEDAT;
            DataObject.ERZET = this.ERZET;
            DataObject.WERKS = this.WERKS;
            DataObject.LIFNR = this.LIFNR;
            DataObject.BOLNR = this.BOLNR;
            DataObject.BSTKD = this.BSTKD;
            DataObject.PSTYV = this.PSTYV;
            DataObject.TELF1 = this.TELF1;
            DataObject.PSTLZ = this.PSTLZ;
            DataObject.REGIO = this.REGIO;
            DataObject.ORT01 = this.ORT01;
            DataObject.STRAS = this.STRAS;
            DataObject.NAME1SHIP = this.NAME1SHIP;
            DataObject.KUNNR = this.KUNNR;
            DataObject.NAME1SALE = this.NAME1SALE;
            DataObject.KUNAG = this.KUNAG;
            DataObject.WADAT_IST = this.WADAT_IST;
            DataObject.ERDAT = this.ERDAT;
            DataObject.VBELN = this.VBELN;
            DataObject.VGBEL = this.VGBEL;
            DataObject.ID = this.ID;
            DataObject.ZSHIPTO = this.ZSHIPTO;
            DataObject.INVOICE_NO = this.INVOICE_NO;
            DataObject.SHIPTYPE = this.SHIPTYPE;
            DataObject.DN1 = this.DN1;
            DataObject.BEROT = this.BEROT;
            DataObject.ENAME = this.ENAME;
            return DataObject;
        }
        public string VTEXT
        {
            get
            {
                return (string)this["VTEXT"];
            }
            set
            {
                this["VTEXT"] = value;
            }
        }
        public string VKORG
        {
            get
            {
                return (string)this["VKORG"];
            }
            set
            {
                this["VKORG"] = value;
            }
        }
        public string SHIPREMARK
        {
            get
            {
                return (string)this["SHIPREMARK"];
            }
            set
            {
                this["SHIPREMARK"] = value;
            }
        }
        public string INCO1
        {
            get
            {
                return (string)this["INCO1"];
            }
            set
            {
                this["INCO1"] = value;
            }
        }
        public string LDDAT
        {
            get
            {
                return (string)this["LDDAT"];
            }
            set
            {
                this["LDDAT"] = value;
            }
        }
        public string LPRIO
        {
            get
            {
                return (string)this["LPRIO"];
            }
            set
            {
                this["LPRIO"] = value;
            }
        }
        public string POST_FLAG
        {
            get
            {
                return (string)this["POST_FLAG"];
            }
            set
            {
                this["POST_FLAG"] = value;
            }
        }
        public string PLAN_DATE
        {
            get
            {
                return (string)this["PLAN_DATE"];
            }
            set
            {
                this["PLAN_DATE"] = value;
            }
        }
        public string SH_COUNTRY
        {
            get
            {
                return (string)this["SH_COUNTRY"];
            }
            set
            {
                this["SH_COUNTRY"] = value;
            }
        }
        public string SH_VAT
        {
            get
            {
                return (string)this["SH_VAT"];
            }
            set
            {
                this["SH_VAT"] = value;
            }
        }
        public string SH_STREET2
        {
            get
            {
                return (string)this["SH_STREET2"];
            }
            set
            {
                this["SH_STREET2"] = value;
            }
        }
        public string SH_STREET
        {
            get
            {
                return (string)this["SH_STREET"];
            }
            set
            {
                this["SH_STREET"] = value;
            }
        }
        public string SH_POST
        {
            get
            {
                return (string)this["SH_POST"];
            }
            set
            {
                this["SH_POST"] = value;
            }
        }
        public string SH_CITY1
        {
            get
            {
                return (string)this["SH_CITY1"];
            }
            set
            {
                this["SH_CITY1"] = value;
            }
        }
        public string SH_NAME
        {
            get
            {
                return (string)this["SH_NAME"];
            }
            set
            {
                this["SH_NAME"] = value;
            }
        }
        public string REMARK
        {
            get
            {
                return (string)this["REMARK"];
            }
            set
            {
                this["REMARK"] = value;
            }
        }
        public string FAX_EXTENS
        {
            get
            {
                return (string)this["FAX_EXTENS"];
            }
            set
            {
                this["FAX_EXTENS"] = value;
            }
        }
        public string FAX_NUMBER
        {
            get
            {
                return (string)this["FAX_NUMBER"];
            }
            set
            {
                this["FAX_NUMBER"] = value;
            }
        }
        public string TEL_EXTENS
        {
            get
            {
                return (string)this["TEL_EXTENS"];
            }
            set
            {
                this["TEL_EXTENS"] = value;
            }
        }
        public string TEL_NUMBER
        {
            get
            {
                return (string)this["TEL_NUMBER"];
            }
            set
            {
                this["TEL_NUMBER"] = value;
            }
        }
        public string PERSON
        {
            get
            {
                return (string)this["PERSON"];
            }
            set
            {
                this["PERSON"] = value;
            }
        }
        public string HEADREMARK
        {
            get
            {
                return (string)this["HEADREMARK"];
            }
            set
            {
                this["HEADREMARK"] = value;
            }
        }
        public string KUNWE
        {
            get
            {
                return (string)this["KUNWE"];
            }
            set
            {
                this["KUNWE"] = value;
            }
        }
        public string WADAT
        {
            get
            {
                return (string)this["WADAT"];
            }
            set
            {
                this["WADAT"] = value;
            }
        }
        public string SCACD
        {
            get
            {
                return (string)this["SCACD"];
            }
            set
            {
                this["SCACD"] = value;
            }
        }
        public string TDLNR
        {
            get
            {
                return (string)this["TDLNR"];
            }
            set
            {
                this["TDLNR"] = value;
            }
        }
        public string TELFC
        {
            get
            {
                return (string)this["TELFC"];
            }
            set
            {
                this["TELFC"] = value;
            }
        }
        public string NAMEC
        {
            get
            {
                return (string)this["NAMEC"];
            }
            set
            {
                this["NAMEC"] = value;
            }
        }
        public string LFART
        {
            get
            {
                return (string)this["LFART"];
            }
            set
            {
                this["LFART"] = value;
            }
        }
        public string CUSTPONO
        {
            get
            {
                return (string)this["CUSTPONO"];
            }
            set
            {
                this["CUSTPONO"] = value;
            }
        }
        public string ROUTE
        {
            get
            {
                return (string)this["ROUTE"];
            }
            set
            {
                this["ROUTE"] = value;
            }
        }
        public string VSBED
        {
            get
            {
                return (string)this["VSBED"];
            }
            set
            {
                this["VSBED"] = value;
            }
        }
        public string INVOICE
        {
            get
            {
                return (string)this["INVOICE"];
            }
            set
            {
                this["INVOICE"] = value;
            }
        }
        public string UTIME
        {
            get
            {
                return (string)this["UTIME"];
            }
            set
            {
                this["UTIME"] = value;
            }
        }
        public string AEDAT
        {
            get
            {
                return (string)this["AEDAT"];
            }
            set
            {
                this["AEDAT"] = value;
            }
        }
        public string ERZET
        {
            get
            {
                return (string)this["ERZET"];
            }
            set
            {
                this["ERZET"] = value;
            }
        }
        public string WERKS
        {
            get
            {
                return (string)this["WERKS"];
            }
            set
            {
                this["WERKS"] = value;
            }
        }
        public string LIFNR
        {
            get
            {
                return (string)this["LIFNR"];
            }
            set
            {
                this["LIFNR"] = value;
            }
        }
        public string BOLNR
        {
            get
            {
                return (string)this["BOLNR"];
            }
            set
            {
                this["BOLNR"] = value;
            }
        }
        public string BSTKD
        {
            get
            {
                return (string)this["BSTKD"];
            }
            set
            {
                this["BSTKD"] = value;
            }
        }
        public string PSTYV
        {
            get
            {
                return (string)this["PSTYV"];
            }
            set
            {
                this["PSTYV"] = value;
            }
        }
        public string TELF1
        {
            get
            {
                return (string)this["TELF1"];
            }
            set
            {
                this["TELF1"] = value;
            }
        }
        public string PSTLZ
        {
            get
            {
                return (string)this["PSTLZ"];
            }
            set
            {
                this["PSTLZ"] = value;
            }
        }
        public string REGIO
        {
            get
            {
                return (string)this["REGIO"];
            }
            set
            {
                this["REGIO"] = value;
            }
        }
        public string ORT01
        {
            get
            {
                return (string)this["ORT01"];
            }
            set
            {
                this["ORT01"] = value;
            }
        }
        public string STRAS
        {
            get
            {
                return (string)this["STRAS"];
            }
            set
            {
                this["STRAS"] = value;
            }
        }
        public string NAME1SHIP
        {
            get
            {
                return (string)this["NAME1SHIP"];
            }
            set
            {
                this["NAME1SHIP"] = value;
            }
        }
        public string KUNNR
        {
            get
            {
                return (string)this["KUNNR"];
            }
            set
            {
                this["KUNNR"] = value;
            }
        }
        public string NAME1SALE
        {
            get
            {
                return (string)this["NAME1SALE"];
            }
            set
            {
                this["NAME1SALE"] = value;
            }
        }
        public string KUNAG
        {
            get
            {
                return (string)this["KUNAG"];
            }
            set
            {
                this["KUNAG"] = value;
            }
        }
        public string WADAT_IST
        {
            get
            {
                return (string)this["WADAT_IST"];
            }
            set
            {
                this["WADAT_IST"] = value;
            }
        }
        public string ERDAT
        {
            get
            {
                return (string)this["ERDAT"];
            }
            set
            {
                this["ERDAT"] = value;
            }
        }
        public string VBELN
        {
            get
            {
                return (string)this["VBELN"];
            }
            set
            {
                this["VBELN"] = value;
            }
        }
        public string VGBEL
        {
            get
            {
                return (string)this["VGBEL"];
            }
            set
            {
                this["VGBEL"] = value;
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
        public string ZSHIPTO
        {
            get
            {
                return (string)this["ZSHIPTO"];
            }
            set
            {
                this["ZSHIPTO"] = value;
            }
        }
        public string INVOICE_NO
        {
            get
            {
                return (string)this["INVOICE_NO"];
            }
            set
            {
                this["INVOICE_NO"] = value;
            }
        }
        public string SHIPTYPE
        {
            get
            {
                return (string)this["SHIPTYPE"];
            }
            set
            {
                this["SHIPTYPE"] = value;
            }
        }
        public string DN1
        {
            get
            {
                return (string)this["DN1"];
            }
            set
            {
                this["DN1"] = value;
            }
        }
        public string BEROT
        {
            get
            {
                return (string)this["BEROT"];
            }
            set
            {
                this["BEROT"] = value;
            }
        }
        public string ENAME
        {
            get
            {
                return (string)this["ENAME"];
            }
            set
            {
                this["ENAME"] = value;
            }
        }
    }
    public class R_ZRFC_GET_SHIP_HEADER
    {
        public string VTEXT;
        public string VKORG;
        public string SHIPREMARK;
        public string INCO1;
        public string LDDAT;
        public string LPRIO;
        public string POST_FLAG;
        public string PLAN_DATE;
        public string SH_COUNTRY;
        public string SH_VAT;
        public string SH_STREET2;
        public string SH_STREET;
        public string SH_POST;
        public string SH_CITY1;
        public string SH_NAME;
        public string REMARK;
        public string FAX_EXTENS;
        public string FAX_NUMBER;
        public string TEL_EXTENS;
        public string TEL_NUMBER;
        public string PERSON;
        public string HEADREMARK;
        public string KUNWE;
        public string WADAT;
        public string SCACD;
        public string TDLNR;
        public string TELFC;
        public string NAMEC;
        public string LFART;
        public string CUSTPONO;
        public string ROUTE;
        public string VSBED;
        public string INVOICE;
        public string UTIME;
        public string AEDAT;
        public string ERZET;
        public string WERKS;
        public string LIFNR;
        public string BOLNR;
        public string BSTKD;
        public string PSTYV;
        public string TELF1;
        public string PSTLZ;
        public string REGIO;
        public string ORT01;
        public string STRAS;
        public string NAME1SHIP;
        public string KUNNR;
        public string NAME1SALE;
        public string KUNAG;
        public string WADAT_IST;
        public string ERDAT;
        public string VBELN;
        public string VGBEL;
        public string ID;
        public string ZSHIPTO;
        public string INVOICE_NO;
        public string SHIPTYPE;
        public string DN1;
        public string BEROT;
        public string ENAME;
    }
}