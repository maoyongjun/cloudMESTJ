using MESDataObject;
using MESDataObject.Module;
using MESDBHelper;
using MESStation.BaseClass;
using MESStation.LogicObject;
using MESStation.MESReturnView.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Stations.StationActions.DataCheckers
{
    public class CheckRepairFail
    {
        /// <summary>
        /// 維修輸入SN Fail狀態檢查
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void SNRepairFailChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            OleExec sfcdb = Station.SFCDB;
            //input test
            /*string inputValue = Input.Value.ToString();
            if (string.IsNullOrEmpty(inputValue))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000006", new string[] { "SN輸入值" }));
            }
            SN sn = new SN(inputValue, sfcdb, DB_TYPE_ENUM.Oracle);*/

            MESStationSession SN_Session = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE
                           && t.SessionKey == Paras[0].SESSION_KEY);
            if (SN_Session == null)
            {
                foreach (R_Station_Output output in Station.StationOutputs)
                {
                    Station.StationSession.Find(s => s.MESDataType == output.SESSION_TYPE && s.SessionKey == output.SESSION_KEY).Value = "";
                }
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000045", new string[] { "SN" }));
            }
            SN sn = (SN) SN_Session.Value;
            
            if (sn.RepairFailedFlag == "0")
            {
                foreach (R_Station_Output output in Station.StationOutputs)
                {
                    Station.StationSession.Find(s => s.MESDataType == output.SESSION_TYPE && s.SessionKey == output.SESSION_KEY).Value = "";
                }
                //正常品
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000078", new string[] { sn.SerialNo }));
            }
            List<R_REPAIR_MAIN> repairMains = new T_R_REPAIR_MAIN(sfcdb, DB_TYPE_ENUM.Oracle).GetRepairMainBySN(sfcdb, sn.SerialNo);
            if (repairMains == null || repairMains.Count == 0)
            {
                foreach (R_Station_Output output in Station.StationOutputs)
                {
                    Station.StationSession.Find(s => s.MESDataType == output.SESSION_TYPE && s.SessionKey == output.SESSION_KEY).Value = "";
                }
                //無維修主檔信息
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000079", new string[] { "SN", sn.SerialNo }));
            }
            R_REPAIR_MAIN rm = repairMains.Find(r => r.CLOSED_FLAG == "0");
            if (rm == null)
            {
                foreach (R_Station_Output output in Station.StationOutputs)
                {
                    Station.StationSession.Find(s => s.MESDataType == output.SESSION_TYPE && s.SessionKey == output.SESSION_KEY).Value = "";
                }
                //無維修主檔信息
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000079", new string[] { "SN", sn.SerialNo }));
            }
            //foreach (R_REPAIR_MAIN rm in repairMains)
            //{
            //    //存在closed_flag=0
            //    if (rm.CLOSED_FLAG != "0")
            //    {
            //        foreach (R_Station_Output output in Station.StationOutputs)
            //        {
            //            Station.StationSession.Find(s => s.MESDataType == output.SESSION_TYPE && s.SessionKey == output.SESSION_KEY).Value = "";
            //        }
            //        throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000097", new string[] {"SN", rm.SN }));
            //    }
            //}
            Station.AddMessage("MES00000046", new string[] { "OK" }, StationMessageState.Pass);
        }


        /// <summary>
        /// 維修輸入SN Fail次數管控
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void SNRepairCountChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            //string inputValue = Input.Value.ToString();

            if (Paras.Count != 2)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession SN_Session = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SN_Session == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000045", new string[] { "SN" }));
            }
            SN sn = (SN) SN_Session.Value;
            string skuno = null;
            //Paras: SESSION_TYPE='SKU'  SESSION_KEY='1'  VALUE='0,1'
            switch (Paras[1].VALUE)
            {
                case "0":
                    skuno = sn.SkuNo;
                    break;
                default:
                    skuno = "ALL";
                    break;
            }

            OleExec sfcdb = Station.SFCDB;
            C_REPAIR_DAY repairDay = new T_C_REPAIR_DAY(sfcdb, DB_TYPE_ENUM.Oracle).GetDetailBySkuno(sfcdb, skuno);
            if (repairDay != null)
            {
                //repair_count
                if (repairDay.REPAIR_COUNT == 3)
                {
                    Station.AddMessage("MES00000087", new string[] { repairDay.REPAIR_COUNT.ToString(), "請注意" }, StationMessageState.Message);
                }
                if (repairDay.REPAIR_COUNT > 3)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000087", new string[] { repairDay.REPAIR_COUNT.ToString(), "已鎖定" }));
                }
            }
        }

        public static void RepairPCBASNChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 2)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession SNSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SNSession == null || SNSession.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000045", new string[] { "PCBASNSession" }));
            }
            MESStationSession PCBASNSession = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (PCBASNSession == null || PCBASNSession.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000045", new string[] { "PCBASNSession" }));
            }
            try
            {
                LogicObject.SN snObject = (LogicObject.SN)SNSession.Value;
                if (snObject.SerialNo != PCBASNSession.Value.ToString() && snObject.BoxSN != PCBASNSession.Value.ToString())
                {
                    T_R_SN_KP t_sn_kp = new T_R_SN_KP(Station.SFCDB, Station.DBType);
                    if (!t_sn_kp.KpIsLinkBySN(snObject.ID, PCBASNSession.Value.ToString(), Station.SFCDB))
                    {
                        throw new Exception(MESReturnMessage.GetMESReturnMessage("MSGCODE20180616081316", new string[] { PCBASNSession.Value.ToString(), snObject.SerialNo }));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// REPAIR_CHECK_IN狀態檢查
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void RepairInStatusChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 1)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }            
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE }));
            }
            SN snObject = (SN)sessionSN.Value;
            T_R_REPAIR_TRANSFER t_r_repair = new T_R_REPAIR_TRANSFER(Station.SFCDB, Station.DBType);
            if (t_r_repair.SNIsRepairIn(snObject.SerialNo,Station.SFCDB))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180619154230", new string[] { snObject.SerialNo })); 
            }
        }
        
        /// <summary>
        /// REPAIR_CHECK_IN權限檢查
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void RepairInEmpChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 2)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            string type = Paras[0].VALUE.ToString().ToUpper();
            if (type == "")
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE }));
            }
            MESStationSession sessionEmp = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionEmp == null || sessionEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE }));
            }
            //Vertiv SE&RE黃克喜要求只有指定人員才能掃入和接收REPAIR_CHECK_IN 
            T_c_user t_c_uer = new T_c_user(Station.SFCDB, Station.DBType);
            Row_c_user rowUser = t_c_uer.getC_Userbyempno(sessionEmp.Value.ToString(), Station.SFCDB, Station.DBType);           
            T_C_CONTROL t_c_control = new T_C_CONTROL(Station.SFCDB, Station.DBType);
            string[] inEmp = t_c_control.GetControlByName("REPAIR_CHECK_IN_SEND", Station.SFCDB).CONTROL_VALUE.Split(',');
            string[] receiveEmp = t_c_control.GetControlByName("REPAIR_CHECK_IN_RECEIVE", Station.SFCDB).CONTROL_VALUE.Split(',');
            List<string> inEmpList = new List<string>(inEmp);
            List<string> receiveList = new List<string>(receiveEmp);
            if (rowUser == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180620163103", new string[] { sessionEmp.Value.ToString() }));
            }
            if (type == "SEND")
            {
                if (inEmpList.Find(s => s == rowUser.EMP_NO) == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180619154648", new string[] { rowUser.EMP_NO }));
                }
            }
            else if(type == "RECEIVE")
            {
                if (receiveList.Find(s => s == rowUser.EMP_NO) == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180619154947", new string[] { rowUser.EMP_NO }));
                }
            }
            else
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180607163531", new string[] { "Input" })); 
            }
        }

        /// <summary>
        /// REPAIR_CHECK_OUT狀態檢查
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void RepairOutStatusChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 1)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }           
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE }));
            }
            SN snObject = (SN)sessionSN.Value;
            T_R_REPAIR_TRANSFER t_r_repair = new T_R_REPAIR_TRANSFER(Station.SFCDB, Station.DBType);
            if (!t_r_repair.SNIsRepairIn(snObject.SerialNo,Station.SFCDB))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180619154342", new string[] { snObject.SerialNo })); 
            }

            T_R_REPAIR_MAIN t_r_repair_main = new T_R_REPAIR_MAIN(Station.SFCDB, Station.DBType);
            if(!t_r_repair_main.SNIsRepaired(snObject.SerialNo,Station.SFCDB))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000071", new string[] { snObject.SerialNo })); 
            }
        }
        /// <summary>
        /// REPAIR_CHECK_OUT權限檢查
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void RepairOutEmpChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 2)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            string type = Paras[0].VALUE.ToString().ToUpper();
            if (type == "")
            { 
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE }));
            }
            MESStationSession sessionEmp = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionEmp == null || sessionEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE }));
            }
            //Vertiv SE&RE黃克喜要求只有指定人員才能掃入和接收REPAIR_CHECK_IN 
            T_c_user t_c_uer = new T_c_user(Station.SFCDB, Station.DBType);
            Row_c_user rowUser = t_c_uer.getC_Userbyempno(sessionEmp.Value.ToString(), Station.SFCDB, Station.DBType);
            T_C_CONTROL t_c_control = new T_C_CONTROL(Station.SFCDB, Station.DBType);
            string[] inEmp = t_c_control.GetControlByName("REPAIR_CHECK_OUT_SEND", Station.SFCDB).CONTROL_VALUE.Split(',');
            string[] receiveEmp = t_c_control.GetControlByName("REPAIR_CHECK_OUT_RECEIVE", Station.SFCDB).CONTROL_VALUE.Split(',');
            List<string> inEmpList = new List<string>(inEmp);
            List<string> receiveList = new List<string>(receiveEmp);
            if (rowUser == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180620163103", new string[] { sessionEmp.Value.ToString() }));
            }
            if (type == "SEND")
            {
                if (inEmpList.Find(s => s == rowUser.EMP_NO) == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180619155133", new string[] { rowUser.EMP_NO }));
                }
            }
            else if (type == "RECEIVE")
            {
                if (receiveList.Find(s => s == rowUser.EMP_NO) == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180619154947", new string[] { rowUser.EMP_NO }));
                }
            }
            else
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180607163531", new string[] { "Input" }));
            }
        }

        /// <summary>
        /// 檢查SN是否有掃REPAIR_CHECK_IN
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void RepairInChecker(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 1)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE }));
            }
            SN snObject = (SN)sessionSN.Value;
            T_R_REPAIR_TRANSFER t_r_repair = new T_R_REPAIR_TRANSFER(Station.SFCDB, Station.DBType);
            if (!t_r_repair.SNIsRepairIn(snObject.SerialNo, Station.SFCDB))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180619154342", new string[] { snObject.SerialNo }));
            }
        }
    }
}
