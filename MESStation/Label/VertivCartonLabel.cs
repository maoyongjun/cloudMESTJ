using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDBHelper;
using MESDataObject.Module;
using MESDataObject;
using MESPubLab.MESStation.Label;

namespace MESStation.Label
{
    public class VertivCartonLabel:LabelBase
    {
        LabelInputValue I_SN = new LabelInputValue() { Name = "SN", Type = "STRING", Value = "", StationSessionType = "SN", StationSessionKey = "1" };
        LabelInputValue I_STATION = new LabelInputValue() { Name = "STATION", Type = "STRING", Value = "", StationSessionType = "STATION", StationSessionKey = "1" };

        LabelOutput O_GPN = new LabelOutput() { Name = "GPN", Type = LableOutPutTypeEnum.String, Description = "", Value = "" };
        LabelOutput O_GSN = new LabelOutput() { Name = "GSN", Type = LableOutPutTypeEnum.String, Description = "", Value = "" };
        LabelOutput O_PRINTDATE = new LabelOutput() { Name = "PRINTDATE", Type = LableOutPutTypeEnum.String, Description = "", Value = "" };
        LabelOutput O_VER = new LabelOutput() { Name = "VER", Type = LableOutPutTypeEnum.String, Description = "", Value = "" };
        LabelOutput O_TEST = new LabelOutput() { Name = "TEST", Type = LableOutPutTypeEnum.String, Description = "", Value = "" };
        public VertivCartonLabel()
        {
            Inputs.Add(I_SN);
            Inputs.Add(I_STATION);
            Outputs.Add(O_GPN);
            Outputs.Add(O_GSN);
            Outputs.Add(O_PRINTDATE);
            Outputs.Add(O_VER);
            Outputs.Add(O_TEST);
        }

        T_R_SN t_r_sn;
        T_R_SN_KP t_r_sn_kp;
        T_C_SKU_VER_MAPPING t_c_sku_ver_mapping;

        public override void MakeLabel(OleExec DB)
        {
            //base.MakeLabel(DB);
            t_r_sn = new T_R_SN(DB, MESDataObject.DB_TYPE_ENUM.Oracle);
            C_SKU_Label labelName = null;
            LogicObject.SN snObj;
            R_SN r_sn;
            if (I_SN.Value is string)
            {
                r_sn = t_r_sn.LoadSN(I_SN.Value.ToString(), DB);
            }
            else if (typeof(LogicObject.SN) == I_SN.Value.GetType())
            {
                snObj = (LogicObject.SN)I_SN.Value;
                r_sn = t_r_sn.LoadSN(snObj.SerialNo, DB);
            }
            else if (typeof(R_SN) == I_SN.Value.GetType())
            {
                r_sn = (R_SN)I_SN.Value;
            }
            else
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180607163531", new string[] { I_SN.Value.ToString() }));
            }
            labelName = DB.ORM.Queryable<C_SKU_Label>().Where(l => l.SKUNO == r_sn.SKUNO && l.STATION == I_STATION.Value.ToString()).ToList().FirstOrDefault();
            if (labelName.LABELNAME == "2000E3_CARTON")
            {
                Get2000E3CartonValue(r_sn, DB);
            }
        }

        private void Get2000E3CartonValue(R_SN r_sn, OleExec DB)
        {
            t_r_sn_kp = new T_R_SN_KP(DB, MESDataObject.DB_TYPE_ENUM.Oracle);
            t_c_sku_ver_mapping = new T_C_SKU_VER_MAPPING(DB, DB_TYPE_ENUM.Oracle);            
           
            R_SN kpSN;           
            R_SN_STATION_DETAIL snStationDetail = null;
            C_SKU_VER_MAPPING verMapping = null;
            R_WO_BASE r_wo_base = null;
            List<R_SN_KP> KPList = new List<R_SN_KP>();
            List<R_SN_KP> printKPList = new List<R_SN_KP>();
            R_SN_KP printKP = null;
            R_SN_KP GPNKP;
            R_SN_KP GSNKP;
            C_SKU_DETAIL skuDetail;
            if (r_sn != null)
            {
                r_wo_base = DB.ORM.Queryable<R_WO_BASE>().Where(wo => wo.WORKORDERNO == r_sn.WORKORDERNO).ToList().FirstOrDefault();
                KPList = t_r_sn_kp.GetKPRecordBySnID(r_sn.ID, DB);

                skuDetail = DB.ORM.Queryable<C_SKU_DETAIL>().Where(d => d.SKUNO == r_wo_base.SKUNO && d.STATION_NAME == I_STATION.Value.ToString()
                            && d.CATEGORY == "PRINT" && d.CATEGORY_NAME == "KEYPART").ToList().FirstOrDefault();
                if (skuDetail != null)
                { 
                    //打印keypart SN 對應的keypart 信息
                    printKP = KPList.Find(k => k.PARTNO == skuDetail.VALUE);
                }
                
                if (printKP != null)
                {
                    kpSN = t_r_sn.LoadSN(printKP.VALUE, DB);
                    printKPList = t_r_sn_kp.GetKPRecordBySnID(kpSN.ID, DB);

                    GPNKP = printKPList.Find(k => k.SCANTYPE == "GPN");
                    GSNKP = printKPList.Find(k => k.SCANTYPE == "GSN");
                    if (GPNKP != null)
                    {
                        O_GPN.Value = GPNKP.VALUE;
                    }
                    if (GSNKP != null)
                    {
                        O_GSN.Value = GSNKP.VALUE;
                    }
                }

                snStationDetail = DB.ORM.Queryable<R_SN_STATION_DETAIL>().Where(s => s.R_SN_ID == r_sn.ID && s.STATION_NAME == I_STATION.Value.ToString()).ToList().FirstOrDefault();               
                DateTime dateTime = (DateTime)snStationDetail.EDIT_TIME;                
                O_PRINTDATE.Value = dateTime.ToString("MM/dd/yyyy");

                verMapping = t_c_sku_ver_mapping.GetMappingBySkuAndVersion(r_wo_base.SKUNO, r_wo_base.SKU_VER, DB);
                if (verMapping != null)
                {
                    O_VER.Value = verMapping.CUSTOMER_VERSION;
                }
                else
                {
                    O_VER.Value = r_wo_base.SKU_VER;
                }
            }
        }
    }
}
