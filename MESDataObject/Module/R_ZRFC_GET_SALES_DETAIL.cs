using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_ZRFC_GET_SALES_DETAIL : DataObjectTable
    {
        public T_R_ZRFC_GET_SALES_DETAIL(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_ZRFC_GET_SALES_DETAIL(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_ZRFC_GET_SALES_DETAIL);
            TableName = "R_ZRFC_GET_SALES_DETAIL".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
    }
    public class Row_R_ZRFC_GET_SALES_DETAIL : DataObjectBase
    {
        public Row_R_ZRFC_GET_SALES_DETAIL(DataObjectInfo info) : base(info)
        {

        }
        public R_ZRFC_GET_SALES_DETAIL GetDataObject()
        {
            R_ZRFC_GET_SALES_DETAIL DataObject = new R_ZRFC_GET_SALES_DETAIL();
            DataObject.POSNR = this.POSNR;
            DataObject.MATNR = this.MATNR;
            DataObject.VBELN = this.VBELN;
            DataObject.ID = this.ID;
            DataObject.EDATU = this.EDATU;
            DataObject.ROUTE = this.ROUTE;
            DataObject.EMPST = this.EMPST;
            DataObject.MVGR1 = this.MVGR1;
            DataObject.WLSNR = this.WLSNR;
            DataObject.ODDNR = this.ODDNR;
            DataObject.NETPR = this.NETPR;
            DataObject.WAERK = this.WAERK;
            DataObject.CUSPO = this.CUSPO;
            DataObject.PROFL = this.PROFL;
            DataObject.PSTYV = this.PSTYV;
            DataObject.BSTDK_E = this.BSTDK_E;
            DataObject.LPRIO = this.LPRIO;
            DataObject.ABGRU = this.ABGRU;
            DataObject.POSNR2 = this.POSNR2;
            DataObject.IHREZ_E = this.IHREZ_E;
            DataObject.POSEX_E = this.POSEX_E;
            DataObject.BSTKD_E = this.BSTKD_E;
            DataObject.IHREZ = this.IHREZ;
            DataObject.NTGEW = this.NTGEW;
            DataObject.POSEX = this.POSEX;
            DataObject.AEDAT = this.AEDAT;
            DataObject.ZTERM = this.ZTERM;
            DataObject.LGORT = this.LGORT;
            DataObject.WERKS = this.WERKS;
            DataObject.BRGEW = this.BRGEW;
            DataObject.BSTKD = this.BSTKD;
            DataObject.KWMENG = this.KWMENG;
            DataObject.VSTEL = this.VSTEL;
            DataObject.KDMAT = this.KDMAT;
            DataObject.ARKTX = this.ARKTX;
            return DataObject;
        }
        public string POSNR
        {
            get
            {
                return (string)this["POSNR"];
            }
            set
            {
                this["POSNR"] = value;
            }
        }
        public string MATNR
        {
            get
            {
                return (string)this["MATNR"];
            }
            set
            {
                this["MATNR"] = value;
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
        public string EDATU
        {
            get
            {
                return (string)this["EDATU"];
            }
            set
            {
                this["EDATU"] = value;
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
        public string EMPST
        {
            get
            {
                return (string)this["EMPST"];
            }
            set
            {
                this["EMPST"] = value;
            }
        }
        public string MVGR1
        {
            get
            {
                return (string)this["MVGR1"];
            }
            set
            {
                this["MVGR1"] = value;
            }
        }
        public string WLSNR
        {
            get
            {
                return (string)this["WLSNR"];
            }
            set
            {
                this["WLSNR"] = value;
            }
        }
        public string ODDNR
        {
            get
            {
                return (string)this["ODDNR"];
            }
            set
            {
                this["ODDNR"] = value;
            }
        }
        public string NETPR
        {
            get
            {
                return (string)this["NETPR"];
            }
            set
            {
                this["NETPR"] = value;
            }
        }
        public string WAERK
        {
            get
            {
                return (string)this["WAERK"];
            }
            set
            {
                this["WAERK"] = value;
            }
        }
        public string CUSPO
        {
            get
            {
                return (string)this["CUSPO"];
            }
            set
            {
                this["CUSPO"] = value;
            }
        }
        public string PROFL
        {
            get
            {
                return (string)this["PROFL"];
            }
            set
            {
                this["PROFL"] = value;
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
        public string BSTDK_E
        {
            get
            {
                return (string)this["BSTDK_E"];
            }
            set
            {
                this["BSTDK_E"] = value;
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
        public string ABGRU
        {
            get
            {
                return (string)this["ABGRU"];
            }
            set
            {
                this["ABGRU"] = value;
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
        public string ZTERM
        {
            get
            {
                return (string)this["ZTERM"];
            }
            set
            {
                this["ZTERM"] = value;
            }
        }
        public string LGORT
        {
            get
            {
                return (string)this["LGORT"];
            }
            set
            {
                this["LGORT"] = value;
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
        public string BRGEW
        {
            get
            {
                return (string)this["BRGEW"];
            }
            set
            {
                this["BRGEW"] = value;
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
        public string KWMENG
        {
            get
            {
                return (string)this["KWMENG"];
            }
            set
            {
                this["KWMENG"] = value;
            }
        }
        public string VSTEL
        {
            get
            {
                return (string)this["VSTEL"];
            }
            set
            {
                this["VSTEL"] = value;
            }
        }
        public string KDMAT
        {
            get
            {
                return (string)this["KDMAT"];
            }
            set
            {
                this["KDMAT"] = value;
            }
        }
        public string ARKTX
        {
            get
            {
                return (string)this["ARKTX"];
            }
            set
            {
                this["ARKTX"] = value;
            }
        }
    }
    public class R_ZRFC_GET_SALES_DETAIL
    {
        public string POSNR;
        public string MATNR;
        public string VBELN;
        public string ID;
        public string EDATU;
        public string ROUTE;
        public string EMPST;
        public string MVGR1;
        public string WLSNR;
        public string ODDNR;
        public string NETPR;
        public string WAERK;
        public string CUSPO;
        public string PROFL;
        public string PSTYV;
        public string BSTDK_E;
        public string LPRIO;
        public string ABGRU;
        public string POSNR2;
        public string IHREZ_E;
        public string POSEX_E;
        public string BSTKD_E;
        public string IHREZ;
        public string NTGEW;
        public string POSEX;
        public string AEDAT;
        public string ZTERM;
        public string LGORT;
        public string WERKS;
        public string BRGEW;
        public string BSTKD;
        public string KWMENG;
        public string VSTEL;
        public string KDMAT;
        public string ARKTX;
    }
}