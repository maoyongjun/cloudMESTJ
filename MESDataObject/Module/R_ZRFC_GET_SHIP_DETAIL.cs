using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;

namespace MESDataObject.Module
{
    public class T_R_ZRFC_GET_SHIP_DETAIL : DataObjectTable
    {
        public T_R_ZRFC_GET_SHIP_DETAIL(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_ZRFC_GET_SHIP_DETAIL(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_ZRFC_GET_SHIP_DETAIL);
            TableName = "R_ZRFC_GET_SHIP_DETAIL".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
    }
    public class Row_R_ZRFC_GET_SHIP_DETAIL : DataObjectBase
    {
        public Row_R_ZRFC_GET_SHIP_DETAIL(DataObjectInfo info) : base(info)
        {

        }
        public R_ZRFC_GET_SHIP_DETAIL GetDataObject()
        {
            R_ZRFC_GET_SHIP_DETAIL DataObject = new R_ZRFC_GET_SHIP_DETAIL();
            DataObject.PALLET_COUNT = this.PALLET_COUNT;
            DataObject.BOX_COUNT = this.BOX_COUNT;
            DataObject.SHIP_TO_DES = this.SHIP_TO_DES;
            DataObject.SHIP_TO = this.SHIP_TO;
            DataObject.BOX_WEIGHT = this.BOX_WEIGHT;
            DataObject.PALLET_WEIGHT = this.PALLET_WEIGHT;
            DataObject.MATNR_WEIGHT = this.MATNR_WEIGHT;
            DataObject.ISO_COUN_PO = this.ISO_COUN_PO;
            DataObject.ISO_COUN_ST = this.ISO_COUN_ST;
            DataObject.EX_WEIGHT_P = this.EX_WEIGHT_P;
            DataObject.P_WEIGHT_P = this.P_WEIGHT_P;
            DataObject.UNIT_DIM_BOX = this.UNIT_DIM_BOX;
            DataObject.BOX_HEIGHT = this.BOX_HEIGHT;
            DataObject.BOX_WIDTH = this.BOX_WIDTH;
            DataObject.BOX_LENGTH = this.BOX_LENGTH;
            DataObject.UNIT_DIM = this.UNIT_DIM;
            DataObject.HEIGHT = this.HEIGHT;
            DataObject.WIDTH = this.WIDTH;
            DataObject.LENGTH = this.LENGTH;
            DataObject.UNIT_WEI = this.UNIT_WEI;
            DataObject.GROSS_WEIGHT = this.GROSS_WEIGHT;
            DataObject.NET_WEIGHT = this.NET_WEIGHT;
            DataObject.GROSS_PER_BOX = this.GROSS_PER_BOX;
            DataObject.NET_PER_BOX = this.NET_PER_BOX;
            DataObject.BOX_NO = this.BOX_NO;
            DataObject.PALLET_NO = this.PALLET_NO;
            DataObject.LFIMG_PACK = this.LFIMG_PACK;
            DataObject.WADAT = this.WADAT;
            DataObject.WERKS = this.WERKS;
            DataObject.ERDAT = this.ERDAT;
            DataObject.VSBED = this.VSBED;
            DataObject.VGPOS = this.VGPOS;
            DataObject.KUNNR = this.KUNNR;
            DataObject.VTEXT = this.VTEXT;
            DataObject.PROFL = this.PROFL;
            DataObject.LPRIO = this.LPRIO;
            DataObject.LGORT_D = this.LGORT_D;
            DataObject.WERKS_D = this.WERKS_D;
            DataObject.REVISION = this.REVISION;
            DataObject.POSNR3 = this.POSNR3;
            DataObject.POSNR2 = this.POSNR2;
            DataObject.IHREZ_E = this.IHREZ_E;
            DataObject.POSEX_E = this.POSEX_E;
            DataObject.BSTKD_E = this.BSTKD_E;
            DataObject.IHREZ = this.IHREZ;
            DataObject.NTGEW = this.NTGEW;
            DataObject.POSEX = this.POSEX;
            DataObject.BSTKD = this.BSTKD;
            DataObject.LFIMG = this.LFIMG;
            DataObject.KDMAT = this.KDMAT;
            DataObject.ARKTX = this.ARKTX;
            DataObject.POSNR = this.POSNR;
            DataObject.MATNR = this.MATNR;
            DataObject.VBELN = this.VBELN;
            DataObject.VGBEL = this.VGBEL;
            DataObject.ID = this.ID;
            return DataObject;
        }
        public string PALLET_COUNT
        {
            get
            {
                return (string)this["PALLET_COUNT"];
            }
            set
            {
                this["PALLET_COUNT"] = value;
            }
        }
        public string BOX_COUNT
        {
            get
            {
                return (string)this["BOX_COUNT"];
            }
            set
            {
                this["BOX_COUNT"] = value;
            }
        }
        public string SHIP_TO_DES
        {
            get
            {
                return (string)this["SHIP_TO_DES"];
            }
            set
            {
                this["SHIP_TO_DES"] = value;
            }
        }
        public string SHIP_TO
        {
            get
            {
                return (string)this["SHIP_TO"];
            }
            set
            {
                this["SHIP_TO"] = value;
            }
        }
        public string BOX_WEIGHT
        {
            get
            {
                return (string)this["BOX_WEIGHT"];
            }
            set
            {
                this["BOX_WEIGHT"] = value;
            }
        }
        public string PALLET_WEIGHT
        {
            get
            {
                return (string)this["PALLET_WEIGHT"];
            }
            set
            {
                this["PALLET_WEIGHT"] = value;
            }
        }
        public string MATNR_WEIGHT
        {
            get
            {
                return (string)this["MATNR_WEIGHT"];
            }
            set
            {
                this["MATNR_WEIGHT"] = value;
            }
        }
        public string ISO_COUN_PO
        {
            get
            {
                return (string)this["ISO_COUN_PO"];
            }
            set
            {
                this["ISO_COUN_PO"] = value;
            }
        }
        public string ISO_COUN_ST
        {
            get
            {
                return (string)this["ISO_COUN_ST"];
            }
            set
            {
                this["ISO_COUN_ST"] = value;
            }
        }
        public string EX_WEIGHT_P
        {
            get
            {
                return (string)this["EX_WEIGHT_P"];
            }
            set
            {
                this["EX_WEIGHT_P"] = value;
            }
        }
        public string P_WEIGHT_P
        {
            get
            {
                return (string)this["P_WEIGHT_P"];
            }
            set
            {
                this["P_WEIGHT_P"] = value;
            }
        }
        public string UNIT_DIM_BOX
        {
            get
            {
                return (string)this["UNIT_DIM_BOX"];
            }
            set
            {
                this["UNIT_DIM_BOX"] = value;
            }
        }
        public string BOX_HEIGHT
        {
            get
            {
                return (string)this["BOX_HEIGHT"];
            }
            set
            {
                this["BOX_HEIGHT"] = value;
            }
        }
        public string BOX_WIDTH
        {
            get
            {
                return (string)this["BOX_WIDTH"];
            }
            set
            {
                this["BOX_WIDTH"] = value;
            }
        }
        public string BOX_LENGTH
        {
            get
            {
                return (string)this["BOX_LENGTH"];
            }
            set
            {
                this["BOX_LENGTH"] = value;
            }
        }
        public string UNIT_DIM
        {
            get
            {
                return (string)this["UNIT_DIM"];
            }
            set
            {
                this["UNIT_DIM"] = value;
            }
        }
        public string HEIGHT
        {
            get
            {
                return (string)this["HEIGHT"];
            }
            set
            {
                this["HEIGHT"] = value;
            }
        }
        public string WIDTH
        {
            get
            {
                return (string)this["WIDTH"];
            }
            set
            {
                this["WIDTH"] = value;
            }
        }
        public string LENGTH
        {
            get
            {
                return (string)this["LENGTH"];
            }
            set
            {
                this["LENGTH"] = value;
            }
        }
        public string UNIT_WEI
        {
            get
            {
                return (string)this["UNIT_WEI"];
            }
            set
            {
                this["UNIT_WEI"] = value;
            }
        }
        public string GROSS_WEIGHT
        {
            get
            {
                return (string)this["GROSS_WEIGHT"];
            }
            set
            {
                this["GROSS_WEIGHT"] = value;
            }
        }
        public string NET_WEIGHT
        {
            get
            {
                return (string)this["NET_WEIGHT"];
            }
            set
            {
                this["NET_WEIGHT"] = value;
            }
        }
        public string GROSS_PER_BOX
        {
            get
            {
                return (string)this["GROSS_PER_BOX"];
            }
            set
            {
                this["GROSS_PER_BOX"] = value;
            }
        }
        public string NET_PER_BOX
        {
            get
            {
                return (string)this["NET_PER_BOX"];
            }
            set
            {
                this["NET_PER_BOX"] = value;
            }
        }
        public string BOX_NO
        {
            get
            {
                return (string)this["BOX_NO"];
            }
            set
            {
                this["BOX_NO"] = value;
            }
        }
        public string PALLET_NO
        {
            get
            {
                return (string)this["PALLET_NO"];
            }
            set
            {
                this["PALLET_NO"] = value;
            }
        }
        public string LFIMG_PACK
        {
            get
            {
                return (string)this["LFIMG_PACK"];
            }
            set
            {
                this["LFIMG_PACK"] = value;
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
        public string VGPOS
        {
            get
            {
                return (string)this["VGPOS"];
            }
            set
            {
                this["VGPOS"] = value;
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
        public string LGORT_D
        {
            get
            {
                return (string)this["LGORT_D"];
            }
            set
            {
                this["LGORT_D"] = value;
            }
        }
        public string WERKS_D
        {
            get
            {
                return (string)this["WERKS_D"];
            }
            set
            {
                this["WERKS_D"] = value;
            }
        }
        public string REVISION
        {
            get
            {
                return (string)this["REVISION"];
            }
            set
            {
                this["REVISION"] = value;
            }
        }
        public string POSNR3
        {
            get
            {
                return (string)this["POSNR3"];
            }
            set
            {
                this["POSNR3"] = value;
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
        public string LFIMG
        {
            get
            {
                return (string)this["LFIMG"];
            }
            set
            {
                this["LFIMG"] = value;
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
    }
    public class R_ZRFC_GET_SHIP_DETAIL
    {
        public string PALLET_COUNT;
        public string BOX_COUNT;
        public string SHIP_TO_DES;
        public string SHIP_TO;
        public string BOX_WEIGHT;
        public string PALLET_WEIGHT;
        public string MATNR_WEIGHT;
        public string ISO_COUN_PO;
        public string ISO_COUN_ST;
        public string EX_WEIGHT_P;
        public string P_WEIGHT_P;
        public string UNIT_DIM_BOX;
        public string BOX_HEIGHT;
        public string BOX_WIDTH;
        public string BOX_LENGTH;
        public string UNIT_DIM;
        public string HEIGHT;
        public string WIDTH;
        public string LENGTH;
        public string UNIT_WEI;
        public string GROSS_WEIGHT;
        public string NET_WEIGHT;
        public string GROSS_PER_BOX;
        public string NET_PER_BOX;
        public string BOX_NO;
        public string PALLET_NO;
        public string LFIMG_PACK;
        public string WADAT;
        public string WERKS;
        public string ERDAT;
        public string VSBED;
        public string VGPOS;
        public string KUNNR;
        public string VTEXT;
        public string PROFL;
        public string LPRIO;
        public string LGORT_D;
        public string WERKS_D;
        public string REVISION;
        public string POSNR3;
        public string POSNR2;
        public string IHREZ_E;
        public string POSEX_E;
        public string BSTKD_E;
        public string IHREZ;
        public string NTGEW;
        public string POSEX;
        public string BSTKD;
        public string LFIMG;
        public string KDMAT;
        public string ARKTX;
        public string POSNR;
        public string MATNR;
        public string VBELN;
        public string VGBEL;
        public string ID;
    }
}