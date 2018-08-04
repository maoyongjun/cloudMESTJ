using MESDataObject;
using MESDataObject.Module;
using MESDBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.LogicObject
{
    public class Packing
    {
        //包裝類型
        public string PackID { get; set; }
        public string PackNo { get; set; }        
        public string PackType { get; set; }
        public string ParentPackID { get; set; }
        public string Skuno { get; set; }
        public string SkunoVer { get; set; }
        public double? MaxQty { get; set; }
        public double? Qty { get; set; }
        public string ClosedFlag { get; set; }
        public DateTime? CreatTime { get; set; }
        public DateTime? EditTime { get; set; }
        public string EditEmp { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string IP { get; set; }
        public List<string> PackList { get; set; }

        public Packing()
        {
        }

        public void DataLoad(string packNo,string bu, OleExec sfcdb,DB_TYPE_ENUM DBType)
        {
            List<string> itemList = new List<string>();
            T_C_SKU t_c_sku = new T_C_SKU(sfcdb, DBType);
            T_R_PACKING t_r_packing = new T_R_PACKING(sfcdb, DBType);
            T_R_SN_PACKING t_r_sn_packing = new T_R_SN_PACKING(sfcdb, DBType);
            T_R_SN t_r_sn = new T_R_SN(sfcdb, DBType);
            T_C_PACKING t_c_packing = new T_C_PACKING(sfcdb, DBType);
            R_PACKING packing = new R_PACKING();
            C_SKU sku = new C_SKU();
            Packing packObject = new Packing();
            C_PACKING c_packing = new C_PACKING();
            packing = t_r_packing.GetRPackingByPackNo(sfcdb, packNo).GetDataObject();
            sku = t_c_sku.GetSku(packing.SKUNO, sfcdb, DBType).GetDataObject();
            if (packing.PACK_TYPE == LogicObject.PackType.PALLET.ToString().ToUpper())
            {
                c_packing = t_c_packing.GetPackingBySkuAndType(sku.SKUNO, LogicObject.PackType.CARTON.ToString().ToUpper(), sfcdb);                
                if (c_packing.MAX_QTY==1 && bu.ToUpper().Equals("VERTIV"))
                {
                    //VERTIV 當卡通包規為1時，調棧板顯示卡通內的SN
                    itemList = t_r_packing.GetPakcingSNList(packing.ID, sfcdb);
                }
                else
                {
                    List<R_PACKING> packingList = t_r_packing.GetListPackByParentPackId(packing.ID, sfcdb);
                    foreach (R_PACKING pack in packingList)
                    {
                        itemList.Add(pack.PACK_NO);
                    }
                }
            }
            else if (packing.PACK_TYPE == LogicObject.PackType.CARTON.ToString().ToUpper())
            {
                List<Row_R_SN_PACKING> snPackingList = t_r_sn_packing.GetPackItem(packing.ID, sfcdb);
                foreach (Row_R_SN_PACKING sn in snPackingList)
                {
                    itemList.Add(t_r_sn.GetById(sn.SN_ID, sfcdb).SN);
                }
            }
            this.PackID = packing.ID;
            this.PackNo = packing.PACK_NO;
            this.PackType = packing.PACK_TYPE;
            this.ParentPackID = packing.PARENT_PACK_ID;
            this.Skuno = packing.SKUNO;
            this.SkunoVer = sku.VERSION;
            this.MaxQty = packing.MAX_QTY;
            this.Qty = packing.QTY;
            this.ClosedFlag = packing.CLOSED_FLAG;
            this.CreatTime = packing.CREATE_TIME;
            this.EditTime = packing.EDIT_TIME;
            this.EditEmp = packing.EDIT_EMP;
            this.Line = packing.LINE;
            this.Station = packing.STATION;
            this.IP = packing.IP;
            this.PackList = itemList;
        }
    }

    public enum PackType
    {
        PALLET = 0,
        CARTON = 1
    }
}
