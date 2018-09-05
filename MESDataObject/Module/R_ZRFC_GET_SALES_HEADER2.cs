using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_ZRFC_GET_SALES_HEADER2 : DataObjectTable
    {
        public T_R_ZRFC_GET_SALES_HEADER2(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_ZRFC_GET_SALES_HEADER2(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_ZRFC_GET_SALES_HEADER2);
            TableName = "R_ZRFC_GET_SALES_HEADER2".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
    }
    public class Row_R_ZRFC_GET_SALES_HEADER2 : DataObjectBase
    {
        public Row_R_ZRFC_GET_SALES_HEADER2(DataObjectInfo info) : base(info)
        {

        }
        public R_ZRFC_GET_SALES_HEADER2 GetDataObject()
        {
            R_ZRFC_GET_SALES_HEADER2 DataObject = new R_ZRFC_GET_SALES_HEADER2();
            DataObject.NTGEW = this.NTGEW;
            DataObject.POSEX = this.POSEX;
            DataObject.ERDAT = this.ERDAT;
            DataObject.LPRIO = this.LPRIO;
            DataObject.STELFX = this.STELFX;
            DataObject.STELF1 = this.STELF1;
            DataObject.SLAND1 = this.SLAND1;
            DataObject.SPSTLZ = this.SPSTLZ;
            DataObject.SREGIO = this.SREGIO;
            DataObject.SORT01 = this.SORT01;
            DataObject.SADRNR = this.SADRNR;
            DataObject.SSTRAS = this.SSTRAS;
            DataObject.SNAME2 = this.SNAME2;
            DataObject.SNAME1 = this.SNAME1;
            DataObject.BSTKD = this.BSTKD;
            DataObject.AEDAT = this.AEDAT;
            DataObject.AUART = this.AUART;
            DataObject.KUNAG = this.KUNAG;
            DataObject.KUNNR = this.KUNNR;
            DataObject.SVBELN = this.SVBELN;
            DataObject.WERKS = this.WERKS;
            DataObject.VBELN = this.VBELN;
            DataObject.ID = this.ID;
            DataObject.BEZEI = this.BEZEI;
            DataObject.AUGRU = this.AUGRU;
            DataObject.BSTDK = this.BSTDK;
            DataObject.TEXTZ012 = this.TEXTZ012;
            DataObject.TEXTZ011 = this.TEXTZ011;
            DataObject.TEXT0018 = this.TEXT0018;
            DataObject.TEXT0017 = this.TEXT0017;
            DataObject.TEXT0016 = this.TEXT0016;
            DataObject.TEXT0015 = this.TEXT0015;
            DataObject.TEXT0014 = this.TEXT0014;
            DataObject.TEXT0013 = this.TEXT0013;
            DataObject.TEXT0012 = this.TEXT0012;
            DataObject.TEXT0011 = this.TEXT0011;
            DataObject.TEXT0010 = this.TEXT0010;
            DataObject.TEXT0005 = this.TEXT0005;
            DataObject.TEXT0004 = this.TEXT0004;
            DataObject.TEXT0003 = this.TEXT0003;
            DataObject.TEXT0002 = this.TEXT0002;
            DataObject.TEXT0001 = this.TEXT0001;
            DataObject.RSTELFX = this.RSTELFX;
            DataObject.RSTELF1 = this.RSTELF1;
            DataObject.RSLAND1 = this.RSLAND1;
            DataObject.RSPSTLZ = this.RSPSTLZ;
            DataObject.RSREGIO = this.RSREGIO;
            DataObject.RSORT01 = this.RSORT01;
            DataObject.RSTRAS = this.RSTRAS;
            DataObject.RSNAME2 = this.RSNAME2;
            DataObject.RSNAME1 = this.RSNAME1;
            DataObject.SUBMI = this.SUBMI;
            DataObject.CHANGE_FLAG = this.CHANGE_FLAG;
            DataObject.CITY2 = this.CITY2;
            DataObject.VSNMR_V = this.VSNMR_V;
            DataObject.PRSDT = this.PRSDT;
            DataObject.VSBED = this.VSBED;
            DataObject.SNAME4 = this.SNAME4;
            DataObject.SSTRAS4 = this.SSTRAS4;
            DataObject.SSTRAS3 = this.SSTRAS3;
            DataObject.SSTRAS2 = this.SSTRAS2;
            DataObject.SNAME3 = this.SNAME3;
            DataObject.ERZET = this.ERZET;
            DataObject.UTIME = this.UTIME;
            DataObject.VDATU = this.VDATU;
            DataObject.STATUS3 = this.STATUS3;
            DataObject.LIFSK = this.LIFSK;
            DataObject.STATUS1 = this.STATUS1;
            DataObject.POSNR2 = this.POSNR2;
            DataObject.IHREZ_E = this.IHREZ_E;
            DataObject.POSEX_E = this.POSEX_E;
            DataObject.BSTKD_E = this.BSTKD_E;
            DataObject.IHREZ = this.IHREZ;
            return DataObject;
        }
        public string NTGEW
        {
            get
            {
                return (string)this["NTGEW"];
            }
            set
            {
                this["NTGEW"] = value;
            }
        }
        public string POSEX
        {
            get
            {
                return (string)this["POSEX"];
            }
            set
            {
                this["POSEX"] = value;
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
        public string STELFX
        {
            get
            {
                return (string)this["STELFX"];
            }
            set
            {
                this["STELFX"] = value;
            }
        }
        public string STELF1
        {
            get
            {
                return (string)this["STELF1"];
            }
            set
            {
                this["STELF1"] = value;
            }
        }
        public string SLAND1
        {
            get
            {
                return (string)this["SLAND1"];
            }
            set
            {
                this["SLAND1"] = value;
            }
        }
        public string SPSTLZ
        {
            get
            {
                return (string)this["SPSTLZ"];
            }
            set
            {
                this["SPSTLZ"] = value;
            }
        }
        public string SREGIO
        {
            get
            {
                return (string)this["SREGIO"];
            }
            set
            {
                this["SREGIO"] = value;
            }
        }
        public string SORT01
        {
            get
            {
                return (string)this["SORT01"];
            }
            set
            {
                this["SORT01"] = value;
            }
        }
        public string SADRNR
        {
            get
            {
                return (string)this["SADRNR"];
            }
            set
            {
                this["SADRNR"] = value;
            }
        }
        public string SSTRAS
        {
            get
            {
                return (string)this["SSTRAS"];
            }
            set
            {
                this["SSTRAS"] = value;
            }
        }
        public string SNAME2
        {
            get
            {
                return (string)this["SNAME2"];
            }
            set
            {
                this["SNAME2"] = value;
            }
        }
        public string SNAME1
        {
            get
            {
                return (string)this["SNAME1"];
            }
            set
            {
                this["SNAME1"] = value;
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
        public string AUART
        {
            get
            {
                return (string)this["AUART"];
            }
            set
            {
                this["AUART"] = value;
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
        public string SVBELN
        {
            get
            {
                return (string)this["SVBELN"];
            }
            set
            {
                this["SVBELN"] = value;
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
        public string BEZEI
        {
            get
            {
                return (string)this["BEZEI"];
            }
            set
            {
                this["BEZEI"] = value;
            }
        }
        public string AUGRU
        {
            get
            {
                return (string)this["AUGRU"];
            }
            set
            {
                this["AUGRU"] = value;
            }
        }
        public string BSTDK
        {
            get
            {
                return (string)this["BSTDK"];
            }
            set
            {
                this["BSTDK"] = value;
            }
        }
        public string TEXTZ012
        {
            get
            {
                return (string)this["TEXTZ012"];
            }
            set
            {
                this["TEXTZ012"] = value;
            }
        }
        public string TEXTZ011
        {
            get
            {
                return (string)this["TEXTZ011"];
            }
            set
            {
                this["TEXTZ011"] = value;
            }
        }
        public string TEXT0018
        {
            get
            {
                return (string)this["TEXT0018"];
            }
            set
            {
                this["TEXT0018"] = value;
            }
        }
        public string TEXT0017
        {
            get
            {
                return (string)this["TEXT0017"];
            }
            set
            {
                this["TEXT0017"] = value;
            }
        }
        public string TEXT0016
        {
            get
            {
                return (string)this["TEXT0016"];
            }
            set
            {
                this["TEXT0016"] = value;
            }
        }
        public string TEXT0015
        {
            get
            {
                return (string)this["TEXT0015"];
            }
            set
            {
                this["TEXT0015"] = value;
            }
        }
        public string TEXT0014
        {
            get
            {
                return (string)this["TEXT0014"];
            }
            set
            {
                this["TEXT0014"] = value;
            }
        }
        public string TEXT0013
        {
            get
            {
                return (string)this["TEXT0013"];
            }
            set
            {
                this["TEXT0013"] = value;
            }
        }
        public string TEXT0012
        {
            get
            {
                return (string)this["TEXT0012"];
            }
            set
            {
                this["TEXT0012"] = value;
            }
        }
        public string TEXT0011
        {
            get
            {
                return (string)this["TEXT0011"];
            }
            set
            {
                this["TEXT0011"] = value;
            }
        }
        public string TEXT0010
        {
            get
            {
                return (string)this["TEXT0010"];
            }
            set
            {
                this["TEXT0010"] = value;
            }
        }
        public string TEXT0005
        {
            get
            {
                return (string)this["TEXT0005"];
            }
            set
            {
                this["TEXT0005"] = value;
            }
        }
        public string TEXT0004
        {
            get
            {
                return (string)this["TEXT0004"];
            }
            set
            {
                this["TEXT0004"] = value;
            }
        }
        public string TEXT0003
        {
            get
            {
                return (string)this["TEXT0003"];
            }
            set
            {
                this["TEXT0003"] = value;
            }
        }
        public string TEXT0002
        {
            get
            {
                return (string)this["TEXT0002"];
            }
            set
            {
                this["TEXT0002"] = value;
            }
        }
        public string TEXT0001
        {
            get
            {
                return (string)this["TEXT0001"];
            }
            set
            {
                this["TEXT0001"] = value;
            }
        }
        public string RSTELFX
        {
            get
            {
                return (string)this["RSTELFX"];
            }
            set
            {
                this["RSTELFX"] = value;
            }
        }
        public string RSTELF1
        {
            get
            {
                return (string)this["RSTELF1"];
            }
            set
            {
                this["RSTELF1"] = value;
            }
        }
        public string RSLAND1
        {
            get
            {
                return (string)this["RSLAND1"];
            }
            set
            {
                this["RSLAND1"] = value;
            }
        }
        public string RSPSTLZ
        {
            get
            {
                return (string)this["RSPSTLZ"];
            }
            set
            {
                this["RSPSTLZ"] = value;
            }
        }
        public string RSREGIO
        {
            get
            {
                return (string)this["RSREGIO"];
            }
            set
            {
                this["RSREGIO"] = value;
            }
        }
        public string RSORT01
        {
            get
            {
                return (string)this["RSORT01"];
            }
            set
            {
                this["RSORT01"] = value;
            }
        }
        public string RSTRAS
        {
            get
            {
                return (string)this["RSTRAS"];
            }
            set
            {
                this["RSTRAS"] = value;
            }
        }
        public string RSNAME2
        {
            get
            {
                return (string)this["RSNAME2"];
            }
            set
            {
                this["RSNAME2"] = value;
            }
        }
        public string RSNAME1
        {
            get
            {
                return (string)this["RSNAME1"];
            }
            set
            {
                this["RSNAME1"] = value;
            }
        }
        public string SUBMI
        {
            get
            {
                return (string)this["SUBMI"];
            }
            set
            {
                this["SUBMI"] = value;
            }
        }
        public string CHANGE_FLAG
        {
            get
            {
                return (string)this["CHANGE_FLAG"];
            }
            set
            {
                this["CHANGE_FLAG"] = value;
            }
        }
        public string CITY2
        {
            get
            {
                return (string)this["CITY2"];
            }
            set
            {
                this["CITY2"] = value;
            }
        }
        public string VSNMR_V
        {
            get
            {
                return (string)this["VSNMR_V"];
            }
            set
            {
                this["VSNMR_V"] = value;
            }
        }
        public string PRSDT
        {
            get
            {
                return (string)this["PRSDT"];
            }
            set
            {
                this["PRSDT"] = value;
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
        public string SNAME4
        {
            get
            {
                return (string)this["SNAME4"];
            }
            set
            {
                this["SNAME4"] = value;
            }
        }
        public string SSTRAS4
        {
            get
            {
                return (string)this["SSTRAS4"];
            }
            set
            {
                this["SSTRAS4"] = value;
            }
        }
        public string SSTRAS3
        {
            get
            {
                return (string)this["SSTRAS3"];
            }
            set
            {
                this["SSTRAS3"] = value;
            }
        }
        public string SSTRAS2
        {
            get
            {
                return (string)this["SSTRAS2"];
            }
            set
            {
                this["SSTRAS2"] = value;
            }
        }
        public string SNAME3
        {
            get
            {
                return (string)this["SNAME3"];
            }
            set
            {
                this["SNAME3"] = value;
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
        public string VDATU
        {
            get
            {
                return (string)this["VDATU"];
            }
            set
            {
                this["VDATU"] = value;
            }
        }
        public string STATUS3
        {
            get
            {
                return (string)this["STATUS3"];
            }
            set
            {
                this["STATUS3"] = value;
            }
        }
        public string LIFSK
        {
            get
            {
                return (string)this["LIFSK"];
            }
            set
            {
                this["LIFSK"] = value;
            }
        }
        public string STATUS1
        {
            get
            {
                return (string)this["STATUS1"];
            }
            set
            {
                this["STATUS1"] = value;
            }
        }
        public string POSNR2
        {
            get
            {
                return (string)this["POSNR2"];
            }
            set
            {
                this["POSNR2"] = value;
            }
        }
        public string IHREZ_E
        {
            get
            {
                return (string)this["IHREZ_E"];
            }
            set
            {
                this["IHREZ_E"] = value;
            }
        }
        public string POSEX_E
        {
            get
            {
                return (string)this["POSEX_E"];
            }
            set
            {
                this["POSEX_E"] = value;
            }
        }
        public string BSTKD_E
        {
            get
            {
                return (string)this["BSTKD_E"];
            }
            set
            {
                this["BSTKD_E"] = value;
            }
        }
        public string IHREZ
        {
            get
            {
                return (string)this["IHREZ"];
            }
            set
            {
                this["IHREZ"] = value;
            }
        }
    }
    public class R_ZRFC_GET_SALES_HEADER2
    {
        public string NTGEW;
        public string POSEX;
        public string ERDAT;
        public string LPRIO;
        public string STELFX;
        public string STELF1;
        public string SLAND1;
        public string SPSTLZ;
        public string SREGIO;
        public string SORT01;
        public string SADRNR;
        public string SSTRAS;
        public string SNAME2;
        public string SNAME1;
        public string BSTKD;
        public string AEDAT;
        public string AUART;
        public string KUNAG;
        public string KUNNR;
        public string SVBELN;
        public string WERKS;
        public string VBELN;
        public string ID;
        public string BEZEI;
        public string AUGRU;
        public string BSTDK;
        public string TEXTZ012;
        public string TEXTZ011;
        public string TEXT0018;
        public string TEXT0017;
        public string TEXT0016;
        public string TEXT0015;
        public string TEXT0014;
        public string TEXT0013;
        public string TEXT0012;
        public string TEXT0011;
        public string TEXT0010;
        public string TEXT0005;
        public string TEXT0004;
        public string TEXT0003;
        public string TEXT0002;
        public string TEXT0001;
        public string RSTELFX;
        public string RSTELF1;
        public string RSLAND1;
        public string RSPSTLZ;
        public string RSREGIO;
        public string RSORT01;
        public string RSTRAS;
        public string RSNAME2;
        public string RSNAME1;
        public string SUBMI;
        public string CHANGE_FLAG;
        public string CITY2;
        public string VSNMR_V;
        public string PRSDT;
        public string VSBED;
        public string SNAME4;
        public string SSTRAS4;
        public string SSTRAS3;
        public string SSTRAS2;
        public string SNAME3;
        public string ERZET;
        public string UTIME;
        public string VDATU;
        public string STATUS3;
        public string LIFSK;
        public string STATUS1;
        public string POSNR2;
        public string IHREZ_E;
        public string POSEX_E;
        public string BSTKD_E;
        public string IHREZ;
    }
}