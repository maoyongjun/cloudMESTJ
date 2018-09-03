using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDataObject;
using System.Data;
using MESDataObject.Module;
using MESPubLab.MESStation;
using MESStation.LogicObject;
using MESPubLab.MESStation.MESReturnView.Station;
using MESDBHelper;

namespace MESStation.Stations.StationActions.ActionRunners
{
    public class PassAction
    {
        /// <summary>
        /// 處理SN狀態/記錄過站記錄/統計良率 for TCQS
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void TCQSPassStationAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            SN SnObject = null;
            T_R_SN TRSN = new T_R_SN(Station.SFCDB, Station.DBType);
            T_R_TCQS_YIELD_RATE_DETAIL TRTCQS = new T_R_TCQS_YIELD_RATE_DETAIL(Station.SFCDB, Station.DBType);
            R_TCQS_YIELD_RATE_DETAIL RTCQS = new R_TCQS_YIELD_RATE_DETAIL();
            string ErrMessage = string.Empty;
            string DeviceName = string.Empty;
            //獲取DB時間,所有數據更新使用同一時間
            DateTime DT = Station.GetDBDateTime();

            if (Paras.Count != 4)
            {
                //參數不正確：配置表中参数不够，应该为 {0} 个，实际只有 {1} 个！
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000051", new string[] { "3", Paras.Count.ToString() });
                throw new MESReturnMessage(ErrMessage);
            }

            //獲取到 SN 對象
            MESStationSession SNSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SNSession == null)
            {
                //无法获取到 {0} 的数据，请检查工站配置！
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }
            SnObject = (SN)SNSession.Value;

            //STATUS,方便過站處理/寫良率和UPH使用
            MESStationSession StatusSession = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (StatusSession == null)
            {
                //如果沒有，則創建一個該工站的StatusSession,且SN狀態默認為該Action中設定的狀態Value = Paras[1].VALUE
                StatusSession = new MESStationSession() { MESDataType = Paras[1].SESSION_TYPE, InputValue = Input.Value.ToString(), Value = Paras[1].VALUE, SessionKey = Paras[1].SESSION_KEY, ResetInput = Input };
                Station.StationSession.Add(StatusSession);
                //如果該工站沒有設定默認狀態，則默認為PASS
                if (StatusSession.Value == null ||
                    (StatusSession.Value != null && StatusSession.Value.ToString() == ""))
                {
                    StatusSession.Value = "PASS";
                }
            }

            //Device:站點名稱
            MESStationSession DeviceSession = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (DeviceSession != null)
            {
                DeviceName = DeviceSession.Value.ToString();
            }
            else //如果站點名稱不存在,則默認為工站名稱
            {
                DeviceName = Station.StationName;
            }

            //TCQSRecord:TCQS良率統計記錄
            MESStationSession TCQSSession= Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (TCQSSession == null)
            {
                //无法获取到 {0} 的数据，请检查工站配置！
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[3].SESSION_TYPE + Paras[3].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }
            else
            {
                RTCQS = (R_TCQS_YIELD_RATE_DETAIL)TCQSSession.Value;
            }

            //如果過站,則按MES原邏輯處理
            if (RTCQS.TOTAL_FRESH_BUILD_QTY > 0 || RTCQS.TOTAL_REWORK_BUILD_QTY > 0)
            {
                TRSN.PassStation(SnObject.SerialNo, Station.Line, Station.StationName, DeviceName, Station.BU, StatusSession.Value.ToString(), Station.LoginUser.EMP_NO, Station.SFCDB);
            }
            //所有測試記錄，都要統計在TCQS的三個表中(TCQS_Yield/Tmp_ATEData/R_SN_Detail)
            ErrMessage = TRTCQS.RecordTCQSYieldRate(RTCQS, SnObject.SerialNo, Station.BU, Station.SFCDB);
            Station.AddMessage("MES00000150", new string[] { SnObject.SerialNo, "TCQS Yield Rate" }, StationMessageState.Pass);
        }

    }
}
