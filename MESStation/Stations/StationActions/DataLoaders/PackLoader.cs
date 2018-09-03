using MESDataObject.Module;
using MESDBHelper;
using MESPubLab.MESStation;
using MESStation.LogicObject;
using MESPubLab.MESStation.MESReturnView.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDataObject;

namespace MESStation.Stations.StationActions.DataLoaders
{
    class PackLoader
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void InputPackNoDataloader(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count == 0)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }

            MESStationSession PackNoSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            
            if (PackNoSession != null)
                Station.StationSession.Remove(PackNoSession);
            else
            {
                PackNoSession = new MESStationSession() { MESDataType = Paras[0].SESSION_TYPE, InputValue = Input.Value.ToString(), SessionKey = Paras[0].SESSION_KEY, ResetInput = Input };
                PackNoSession.InputValue = Input.Value.ToString();
                Station.StationSession.Add(PackNoSession);
            }

        }

        /// <summary>
        /// 移棧板或卡通，從輸入加載包裝信息
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void CartonOrPalletDataloader(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 6)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }           
            MESStationSession sessionLocation = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);           

            if (sessionLocation == null)               
            {
                sessionLocation = new MESStationSession() { MESDataType = Paras[0].SESSION_TYPE,SessionKey = Paras[0].SESSION_KEY, ResetInput = Input };  
                Station.StationSession.Add(sessionLocation);
            }

            MESStationSession sessionSku = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionSku == null)
            {
                sessionSku = new MESStationSession() { MESDataType = Paras[1].SESSION_TYPE,SessionKey = Paras[1].SESSION_KEY, ResetInput = Input };               
                Station.StationSession.Add(sessionSku);
            }

            MESStationSession sessionVer = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionVer == null)
            {
                sessionVer = new MESStationSession() { MESDataType = Paras[2].SESSION_TYPE, SessionKey = Paras[2].SESSION_KEY, ResetInput = Input };        
                Station.StationSession.Add(sessionVer);
            }

            MESStationSession sessionType = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (sessionType == null)
            {
                sessionType = new MESStationSession() { MESDataType = Paras[3].SESSION_TYPE, SessionKey = Paras[3].SESSION_KEY, ResetInput = Input };
                Station.StationSession.Add(sessionType);
            }

            MESStationSession sessionCount = Station.StationSession.Find(t => t.MESDataType == Paras[4].SESSION_TYPE && t.SessionKey == Paras[4].SESSION_KEY);
            if (sessionCount == null)
            {
                sessionCount = new MESStationSession() { MESDataType = Paras[4].SESSION_TYPE, SessionKey = Paras[4].SESSION_KEY, ResetInput = Input };
                Station.StationSession.Add(sessionCount);
            }

            MESStationSession sessionListItem = Station.StationSession.Find(t => t.MESDataType == Paras[5].SESSION_TYPE && t.SessionKey == Paras[5].SESSION_KEY);
            if (sessionListItem == null)
            {
                sessionListItem = new MESStationSession() { MESDataType = Paras[5].SESSION_TYPE, SessionKey = Paras[5].SESSION_KEY, ResetInput = Input };
                Station.StationSession.Add(sessionListItem);
            }

            try
            {
                string inputValue = Input.Value.ToString();               
                if (string.IsNullOrEmpty(inputValue))
                {
                    throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000007", new string[] { inputValue }));
                }
                LogicObject.Packing packObject = new LogicObject.Packing();
                packObject.DataLoad(inputValue,Station.BU, Station.SFCDB, Station.DBType);
                sessionLocation.Value = packObject;
                sessionSku.Value = packObject.Skuno;
                sessionType.Value = packObject.SkunoVer;
                sessionVer.Value = packObject.SkunoVer;
                sessionCount.Value = packObject.PackList.Count;
                sessionListItem.Value = packObject.PackList;
                Station.AddMessage("MES00000029", new string[] { "Location", inputValue }, StationMessageState.Pass);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
