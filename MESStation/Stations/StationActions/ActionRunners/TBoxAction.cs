using MESDataObject;
using MESDataObject.Module;
using MESPubLab.MESStation;
using MESPubLab.MESStation.MESReturnView.Station;
using MESStation.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Stations.StationActions.ActionRunners
{
    class TBoxAction
    {
        public static void SNPackAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            string ErrMessage = string.Empty;
            //sn对象
            //箱号对象
            if (Paras.Count != 2)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000051", new string[] { "2", Paras.Count.ToString() });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession SnSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SnSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }
            SN sn= (SN)SnSession.Value;

            MESStationSession PackSession = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (PackSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }
            R_PACKING pack = (R_PACKING)PackSession.Value;
            T_R_SN_PACKING r_sn_packing = new T_R_SN_PACKING(Station.SFCDB,Station.DBType);
            Row_R_SN_PACKING row_sn_packing = (Row_R_SN_PACKING) r_sn_packing.NewRow();
            row_sn_packing.ID = r_sn_packing.GetNewID(Station.BU,Station.SFCDB);
            row_sn_packing.PACK_ID = pack.ID;
            row_sn_packing.SN_ID = sn.ID;
            row_sn_packing.EDIT_EMP ="test";
            row_sn_packing.EDIT_TIME = r_sn_packing.GetDBDateTime(Station.SFCDB);
            Station.SFCDB.ExecSQL(row_sn_packing.GetInsertString(Station.DBType));
            Station.AddMessage("MES00000054", new string[] { pack.PACK_NO.ToString() }, StationMessageState.Pass); //回饋消息到前台
        }
    }
}
