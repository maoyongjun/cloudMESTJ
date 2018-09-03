using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDataObject;
using MESDataObject.Module;
using MESPubLab.MESStation;
using MESStation.LogicObject;
using MESPubLab.MESStation.MESReturnView.Station;
using MESDBHelper;
using System.Data;
//using System.Transactions;

namespace MESStation.Stations.StationActions.ActionRunners
{
    public class RepairActions
    {
        //產品維修CheckIn Action
        public static void SNInRepairAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 3)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession sessionSendEmp = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSendEmp == null || sessionSendEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE }));
            }
            MESStationSession sessionReceiveEmp = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionReceiveEmp == null || sessionReceiveEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE }));
            }
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE }));
            }

            SN snObject = (SN)sessionSN.Value;
            T_R_REPAIR_MAIN rRepairMain = new T_R_REPAIR_MAIN(Station.SFCDB, Station.DBType);
            List<R_REPAIR_MAIN> RepariMainList = rRepairMain.GetRepairMainBySN(Station.SFCDB, snObject.SerialNo);
            R_REPAIR_MAIN rMain = RepariMainList.Where(r => r.CLOSED_FLAG == "0").FirstOrDefault();  // Find(r => r.CLOSED_FLAG == "0");
            if (rMain != null)
            {
                T_R_REPAIR_TRANSFER rTransfer = new T_R_REPAIR_TRANSFER(Station.SFCDB, Station.DBType);
                Row_R_REPAIR_TRANSFER rowTransfer = (Row_R_REPAIR_TRANSFER)rTransfer.NewRow();
                rowTransfer.ID = rTransfer.GetNewID(Station.BU, Station.SFCDB);
                rowTransfer.REPAIR_MAIN_ID = rMain.ID;
                rowTransfer.IN_SEND_EMP = sessionSendEmp.Value.ToString();
                rowTransfer.IN_RECEIVE_EMP = sessionReceiveEmp.Value.ToString();
                rowTransfer.IN_TIME = Station.GetDBDateTime();
                rowTransfer.SN = snObject.SerialNo;
                rowTransfer.LINE_NAME = Station.Line;
                rowTransfer.STATION_NAME = snObject.CurrentStation;
                rowTransfer.WORKORDERNO = snObject.WorkorderNo;
                rowTransfer.SKUNO = snObject.SkuNo;
                rowTransfer.CLOSED_FLAG = "0";
                rowTransfer.CREATE_TIME = Station.GetDBDateTime();
                rowTransfer.DESCRIPTION = "";
                rowTransfer.EDIT_TIME = Station.GetDBDateTime();
                rowTransfer.EDIT_EMP= sessionReceiveEmp.Value.ToString();
                string strRet = (Station.SFCDB).ExecSQL(rowTransfer.GetInsertString(DB_TYPE_ENUM.Oracle));
                if (Convert.ToInt32(strRet) > 0)
                {
                    Station.AddMessage("MES00000001", new string[] { }, StationMessageState.Pass);
                }
                else
                {
                    Station.AddMessage("MES00000037", new string[] { "INSET R_REPAIR_TRANSFER" }, StationMessageState.Pass);
                }
            }
            else
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000066", new string[] { snObject.SerialNo, "CLOSED" }));
            }
        }

        //產品維修CheckOut Action
        public static void SNOutRepairAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 3)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession sessionSendEmp = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSendEmp == null || sessionSendEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE }));
            }
            MESStationSession sessionReceiveEmp = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionReceiveEmp == null || sessionReceiveEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE }));
            }
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE }));
            }

            SN snObject = (SN)sessionSN.Value;

            T_R_REPAIR_TRANSFER rTransfer = new T_R_REPAIR_TRANSFER(Station.SFCDB, Station.DBType);
            Row_R_REPAIR_TRANSFER rowTransfer = (Row_R_REPAIR_TRANSFER)rTransfer.NewRow();
            T_R_SN t_r_sn = new T_R_SN(Station.SFCDB, Station.DBType);
            
            List<R_REPAIR_TRANSFER> transferList = rTransfer.GetLastRepairedBySN(snObject.SerialNo, Station.SFCDB);
            R_REPAIR_TRANSFER rRepairTransfer = transferList.Where(r => r.CLOSED_FLAG == "0").FirstOrDefault();//TRANSFER表 1 表示不良
            if (rRepairTransfer != null)
            {
                rowTransfer = (Row_R_REPAIR_TRANSFER)rTransfer.GetObjByID(rRepairTransfer.ID, Station.SFCDB);
                rowTransfer.CLOSED_FLAG = "1";
                rowTransfer.OUT_TIME = Station.GetDBDateTime();
                rowTransfer.OUT_SEND_EMP = sessionSendEmp.Value.ToString();
                rowTransfer.OUT_RECEIVE_EMP = sessionReceiveEmp.Value.ToString();

                string strRet = (Station.SFCDB).ExecSQL(rowTransfer.GetUpdateString(DB_TYPE_ENUM.Oracle));
                if (Convert.ToInt32(strRet) > 0)
                {
                    Station.AddMessage("MES00000035", new string[] { strRet }, StationMessageState.Pass); 
                }                
                else
                {
                    Station.AddMessage("MES00000025", new string[] { "REPAIR TRANSFER" }, StationMessageState.Pass);
                }
                Row_R_SN rowSN = (Row_R_SN)t_r_sn.GetObjByID(snObject.ID, Station.SFCDB);
                rowSN.REPAIR_FAILED_FLAG = "0";
                strRet = (Station.SFCDB).ExecSQL(rowSN.GetUpdateString(DB_TYPE_ENUM.Oracle));
                if (Convert.ToInt32(strRet) > 0)
                {
                    Station.AddMessage("MES00000035", new string[] { strRet }, StationMessageState.Pass);
                }
                else
                {
                    Station.AddMessage("MES00000025", new string[] { "R_SN" }, StationMessageState.Pass);
                }
            }
            else
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000066", new string[] { snObject.SerialNo, "abnormal" }));
            }
        }

        public static void SNFailAction_Old(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            string R_SN_STATION_DETAIL_ID = "";
            if (Paras.Count == 0)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession SNLoadPoint = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SNLoadPoint == null)
            {
                SNLoadPoint = new MESStationSession() { MESDataType = "SN", InputValue = Input.Value.ToString(), SessionKey = "1", ResetInput = Input };
                Station.StationSession.Add(SNLoadPoint);
            }

            //獲取頁面傳過來的數據
            string failCode = Station.Inputs.Find(s => s.DisplayName == "Fail_Code").Value.ToString();
            string failLocation = Station.Inputs.Find(s => s.DisplayName == "Fail_Location").Value.ToString();
            string failProcess = Station.Inputs.Find(s => s.DisplayName == "Fail_Process").Value.ToString();
            string failDescription = Station.Inputs.Find(s => s.DisplayName == "Description").Value.ToString();
            string strSn = Input.Value.ToString();

            OleExec oleDB = null;
            oleDB = Station.SFCDB;
            //oleDB = this.DBPools["SFCDB"].Borrow();
            oleDB.BeginTrain();  //以下執行 要么全成功，要么全失敗

            //更新R_SN REPAIR_FAILED_FLAG=’1’
            T_R_SN rSn = new T_R_SN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            Row_R_SN rrSn = (Row_R_SN)rSn.NewRow();
            R_SN r = rSn.GetDetailBySN(strSn, Station.SFCDB);
            rrSn = (Row_R_SN)rSn.GetObjByID(r.ID, Station.SFCDB);
            rrSn.REPAIR_FAILED_FLAG = "1";
            string strRet = (Station.SFCDB).ExecSQL(rrSn.GetUpdateString(DB_TYPE_ENUM.Oracle));
            if (!(Convert.ToInt32(strRet) > 0))
            {
                throw new Exception("update repair failed flag error!");
            }

            //新增一筆FAIL記錄到R_SN_STATION_DETAIL
            T_R_SN_STATION_DETAIL rSnStationDetail = new T_R_SN_STATION_DETAIL(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            R_SN_STATION_DETAIL_ID = rSnStationDetail.GetNewID(Station.BU, Station.SFCDB);
            string detailResult = rSnStationDetail.AddDetailToRSnStationDetail(R_SN_STATION_DETAIL_ID,rrSn.GetDataObject(),Station.Line,Station.StationName,Station.StationName, Station.SFCDB);
            if (!(Convert.ToInt32(detailResult) > 0))
            {
                throw new Exception("Insert sn station detail error!");
            }

            //新增一筆到R_REPAIR_MAIN
            T_R_REPAIR_MAIN tRepairMain = new T_R_REPAIR_MAIN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            Row_R_REPAIR_MAIN rRepairMain = (Row_R_REPAIR_MAIN)tRepairMain.NewRow();
            rRepairMain.ID = tRepairMain.GetNewID(Station.BU, Station.SFCDB);
            rRepairMain.SN = strSn;
            rRepairMain.WORKORDERNO = rrSn.WORKORDERNO;
            rRepairMain.SKUNO = rrSn.SKUNO;
            rRepairMain.FAIL_LINE = Station.Line;
            rRepairMain.FAIL_STATION = Station.StationName;
            rRepairMain.FAIL_EMP = Station.User.EMP_NO;
            rRepairMain.FAIL_TIME = Station.GetDBDateTime();
            rRepairMain.CLOSED_FLAG = "0";
            string insertResult = (Station.SFCDB).ExecSQL(rRepairMain.GetInsertString(DB_TYPE_ENUM.Oracle));
            if (!(Convert.ToInt32(insertResult) > 0))
            {
                throw new Exception("Insert repair main error!");
            }

            //新增一筆到R_REPAIR_FAILCODE
            T_R_REPAIR_FAILCODE tRepairFailCode = new T_R_REPAIR_FAILCODE(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            Row_R_REPAIR_FAILCODE rRepairFailCode = (Row_R_REPAIR_FAILCODE)tRepairFailCode.NewRow();
            rRepairFailCode.ID = tRepairFailCode.GetNewID(Station.BU, Station.SFCDB);
            rRepairFailCode.REPAIR_MAIN_ID = rRepairMain.ID;
            rRepairFailCode.SN = strSn;
            rRepairFailCode.FAIL_CODE = failCode;
            rRepairFailCode.FAIL_EMP = Station.User.EMP_NO;
            rRepairFailCode.FAIL_TIME = DateTime.Now;
            rRepairFailCode.FAIL_CATEGORY = "SYMPTON";
            rRepairFailCode.FAIL_LOCATION = failLocation;
            rRepairFailCode.FAIL_PROCESS = failProcess;
            rRepairFailCode.DESCRIPTION = failDescription;
            rRepairFailCode.REPAIR_FLAG = "0";
            string strResult = (Station.SFCDB).ExecSQL(rRepairFailCode.GetInsertString(DB_TYPE_ENUM.Oracle));
            if (!(Convert.ToInt32(strResult) > 0))
            {
                throw new Exception("Insert repair failcode error!");
            }

            oleDB.CommitTrain();

            Station.AddMessage("MES00000001", new string[] { }, StationMessageState.Pass);
        }

        public static void SNFailAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            string ErrMessage = "";
            Int16 FailCount = 0;
            string StrSn = "";
            string R_SN_STATION_DETAIL_ID = "";
            List<Dictionary<string, string>> FailList = null;

            if (Paras.Count == 0)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession SNSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SNSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession FailCountSession = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (FailCountSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession FailListSession = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (FailListSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE + Paras[2].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession StatusSession = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (StatusSession == null)
            {
                StatusSession = new MESStationSession() { MESDataType = Paras[3].SESSION_TYPE, InputValue = Input.Value.ToString(), Value = Paras[3].VALUE, SessionKey = Paras[3].SESSION_KEY, ResetInput = Input };
          //      Station.StationSession.Add(StatusSession);
                if (StatusSession.Value==null)
                {
                    StatusSession.Value = "FAIL";
                }
                Station.StationSession.Add(StatusSession);
            }
            MESStationSession ClearInputSession = null;
            if (Paras.Count >= 5)
            {
                ClearInputSession = Station.StationSession.Find(t => t.MESDataType.Equals(Paras[4].SESSION_TYPE) && t.SessionKey.Equals(Paras[4].SESSION_KEY));
                if (ClearInputSession == null)
                {
                    ClearInputSession = new MESStationSession() { MESDataType = Paras[4].SESSION_TYPE, SessionKey = Paras[4].SESSION_KEY };
                    Station.StationSession.Add(ClearInputSession);
                }
            }

            FailCount = Convert.ToInt16(FailCountSession.Value.ToString());
            FailList = (List<Dictionary<string, string>>)FailListSession.Value;

            T_R_SN rSn = new T_R_SN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            DateTime FailTime = rSn.GetDBDateTime(Station.SFCDB);
            if (FailList.Count >= FailCount && FailCount != 0) //允許掃描多個Fail
            {
                StrSn = SNSession.InputValue.ToString();
                string repairMainId = "";
                for (int i = 0; i < FailList.Count; i++)
                {
                    //獲取頁面傳過來的數據
                    string failCode = FailList[i]["FailCode"].ToString();
                    string failLocation = FailList[i]["FailLocation"].ToString();
                    string failProcess = FailList[i]["FailProcess"].ToString();
                    string failDescription = FailList[i]["FailDesc"].ToString();

                    OleExec oleDB = null;
                    oleDB = Station.SFCDB;
                    //oleDB.BeginTrain();  //以下執行 要么全成功，要么全失敗
                    T_R_REPAIR_MAIN tRepairMain = new T_R_REPAIR_MAIN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                    Row_R_REPAIR_MAIN rRepairMain = (Row_R_REPAIR_MAIN)tRepairMain.NewRow();

                    Row_R_SN rrSn = (Row_R_SN)rSn.NewRow();
                    if (i == 0)
                    {
                        //更新R_SN REPAIR_FAILED_FLAG=’1’
                        R_SN r = rSn.GetDetailBySN(StrSn, Station.SFCDB);
                        rrSn = (Row_R_SN)rSn.GetObjByID(r.ID, Station.SFCDB);
                        rrSn.REPAIR_FAILED_FLAG = "1";
                        //AOI工站不入维修
                        if (Station.StationName == "AOI1" || Station.StationName == "AOI2")
                        {
                            rrSn.REPAIR_FAILED_FLAG = "0";
                        }

                        rrSn.EDIT_EMP = Station.LoginUser.EMP_NO;
                        rrSn.EDIT_TIME = FailTime;
                        string strRet = (Station.SFCDB).ExecSQL(rrSn.GetUpdateString(DB_TYPE_ENUM.Oracle));
                        if (!(Convert.ToInt32(strRet) > 0))
                        {
                            throw new Exception("update repair failed flag error!");
                        }

                        //新增一筆FAIL記錄到R_SN_STATION_DETAIL
                        T_R_SN_STATION_DETAIL rSnStationDetail = new T_R_SN_STATION_DETAIL(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                        R_SN_STATION_DETAIL_ID = rSnStationDetail.GetNewID(Station.BU, Station.SFCDB);
                        // string detailResult = rSnStationDetail.AddDetailToRSnStationDetail(R_SN_STATION_DETAIL_ID,rrSn.GetDataObject(),Station.Line,Station.StationName,Station.StationName,Station.SFCDB);
                        string detailResult = rSnStationDetail.AddDetailToBipStationFailDetail(
                               R_SN_STATION_DETAIL_ID, rrSn.GetDataObject(), Station.Line, Station.StationName,
                               Station.StationName, Station.SFCDB, "1");
                        if (!(Convert.ToInt32(detailResult) > 0))
                        {
                            throw new Exception("Insert sn station detail error!");
                        }

                        //新增一筆到R_REPAIR_MAIN 
                        repairMainId = tRepairMain.GetNewID(Station.BU, Station.SFCDB);
                        rRepairMain.ID = repairMainId;
                        rRepairMain.SN = StrSn;
                        rRepairMain.WORKORDERNO = rrSn.WORKORDERNO;
                        rRepairMain.SKUNO = rrSn.SKUNO;
                        rRepairMain.FAIL_LINE = Station.Line;
                        rRepairMain.FAIL_STATION = Station.StationName;
                        rRepairMain.FAIL_EMP = Station.LoginUser.EMP_NO;
                        //rRepairMain.FAIL_TIME = Station.GetDBDateTime();//Mpdofy by LLF 2018-03-17
                        rRepairMain.FAIL_TIME = FailTime;
                        rRepairMain.CREATE_TIME = Station.GetDBDateTime();
                        rRepairMain.EDIT_EMP = Station.LoginUser.EMP_NO;
                        rRepairMain.EDIT_TIME = Station.GetDBDateTime();
                        rRepairMain.CLOSED_FLAG = "0";
                        string insertResult = (Station.SFCDB).ExecSQL(rRepairMain.GetInsertString(DB_TYPE_ENUM.Oracle));
                        if (!(Convert.ToInt32(insertResult) > 0))
                        {
                            throw new Exception("Insert repair main error!");
                        }
                    }

                    ////新增一筆到R_REPAIR_MAIN
                    //T_R_REPAIR_MAIN tRepairMain = new T_R_REPAIR_MAIN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                    //Row_R_REPAIR_MAIN rRepairMain = (Row_R_REPAIR_MAIN)tRepairMain.NewRow();
                    //rRepairMain.ID = tRepairMain.GetNewID(Station.BU, Station.SFCDB);
                    //rRepairMain.SN = StrSn;
                    //rRepairMain.WORKORDERNO = rrSn.WORKORDERNO;
                    //rRepairMain.SKUNO = rrSn.SKUNO;
                    //rRepairMain.FAIL_LINE = Station.Line;
                    //rRepairMain.FAIL_STATION = Station.StationName;
                    //rRepairMain.FAIL_EMP = Station.LoginUser.EMP_NO;
                    //rRepairMain.FAIL_TIME = FailTime.ToString();
                    //rRepairMain.CLOSED_FLAG = "0";
                    //string insertResult = (Station.SFCDB).ExecSQL(rRepairMain.GetInsertString(DB_TYPE_ENUM.Oracle));
                    //if (!(Convert.ToInt32(insertResult) > 0))
                    //{
                    //    throw new Exception("Insert repair main error!");
                    //}

                    //新增一筆到R_REPAIR_FAILCODE
                    T_R_REPAIR_FAILCODE tRepairFailCode = new T_R_REPAIR_FAILCODE(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                    Row_R_REPAIR_FAILCODE rRepairFailCode = (Row_R_REPAIR_FAILCODE)tRepairFailCode.NewRow();
                    rRepairFailCode.ID = tRepairFailCode.GetNewID(Station.BU, Station.SFCDB);
                    rRepairFailCode.REPAIR_MAIN_ID = repairMainId;
                    rRepairFailCode.SN = StrSn;
                    rRepairFailCode.FAIL_CODE = failCode;
                    rRepairFailCode.FAIL_EMP = Station.LoginUser.EMP_NO;
                    rRepairFailCode.FAIL_TIME = FailTime;
                    rRepairFailCode.FAIL_CATEGORY = "SYMPTOM";
                    rRepairFailCode.FAIL_LOCATION = failLocation;
                    rRepairFailCode.FAIL_PROCESS = failProcess;
                    rRepairFailCode.DESCRIPTION = failDescription;
                    rRepairFailCode.REPAIR_FLAG = "0";
                    rRepairFailCode.CREATE_TIME = Station.GetDBDateTime();
                    rRepairFailCode.EDIT_EMP = Station.LoginUser.EMP_NO;
                    rRepairFailCode.EDIT_TIME = Station.GetDBDateTime();

                    string strResult = (Station.SFCDB).ExecSQL(rRepairFailCode.GetInsertString(DB_TYPE_ENUM.Oracle));
                    if (!(Convert.ToInt32(strResult) > 0))
                    {
                        throw new Exception("Insert repair failcode error!");
                    }

                    //oleDB.CommitTrain();
                }
                if (ClearInputSession != null)
                {
                    ClearInputSession.Value = "true";
                }
                else
                {
                    ((List<Dictionary<string, string>>)FailListSession.Value).Clear();
                }
                Station.AddMessage("MES00000001", new string[] { }, StationMessageState.Pass);
            }
            else
            {
                Station.NextInput = Station.FindInputByName("Location");
                Station.AddMessage("MES00000162", new string[] { StrSn, FailCount.ToString(), FailList.Count.ToString() }, StationMessageState.Message);
            }
        }


        public static void HWDBIPSNFailAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            string ErrMessage = "";
            string StrSn = "";
            string APVirtualSn = "";
            string VirtualSn = "";
            AP_DLL APObj = new AP_DLL();
            OleExec APDB = null;
            R_PANEL_SN PANELObj = null;
            Int16 FailCount = 0;
            List<Dictionary<string, string>> FailList = null;
            string R_SN_STATION_DETAIL_ID = "";
            StringBuilder ReturnMessage = new StringBuilder();
            string RepairmainID = "";//add by LLF 2018-04-12
        
            if (Paras.Count < 5)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            
            MESStationSession SNSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SNSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession FailCountSession = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (FailCountSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession FailListSession = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (FailListSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE + Paras[2].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession PanelVitualSNSession = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (PanelVitualSNSession == null)
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[3].SESSION_TYPE + Paras[3].SESSION_KEY });
                throw new MESReturnMessage(ErrMessage);
            }

            MESStationSession StatusSession = Station.StationSession.Find(t => t.MESDataType == Paras[4].SESSION_TYPE && t.SessionKey == Paras[4].SESSION_KEY);
            if (StatusSession == null)
            {
                StatusSession = new MESStationSession() { MESDataType = Paras[4].SESSION_TYPE, InputValue = Input.Value.ToString(), Value = Paras[4].VALUE, SessionKey = Paras[4].SESSION_KEY, ResetInput = Input };
                Station.StationSession.Add(StatusSession);
                if (StatusSession.Value.ToString() == "")
                {
                    StatusSession.Value = "FAIL";
                }
            }

            MESStationSession ClearInputSession = null;
            if (Paras.Count >= 6)
            {
                ClearInputSession = Station.StationSession.Find(t => t.MESDataType.Equals(Paras[5].SESSION_TYPE) && t.SessionKey.Equals(Paras[5].SESSION_KEY));
                if (ClearInputSession == null)
                {
                    ClearInputSession = new MESStationSession() { MESDataType = Paras[5].SESSION_TYPE, SessionKey = Paras[5].SESSION_KEY };
                    Station.StationSession.Add(ClearInputSession);
                }
            }

            FailCount = Convert.ToInt16(FailCountSession.Value.ToString());
            FailList = (List<Dictionary<string, string>>)FailListSession.Value;

            T_R_SN rSn = new T_R_SN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            DateTime FailTime = rSn.GetDBDateTime(Station.SFCDB);
            StrSn = SNSession.Value.ToString();
            if (FailList.Count >= FailCount && FailCount != 0) //允許掃描多個Fail
            {
                #region 業務邏輯

                OleExec oleDB = null;
                oleDB = Station.SFCDB;
                //oleDB = this.DBPools["SFCDB"].Borrow();
                //oleDB.BeginTrain(); 
                //OleExecPool APDBPool = Station.DBS["APDB"];
                //APDB = APDBPool.Borrow();
                //APDB.BeginTrain();
                APDB = Station.APDB;
                try
                {
                    Row_R_SN rrSn = (Row_R_SN) rSn.NewRow();
                    for (int i = 0; i < FailList.Count; i++)
                    {
                        //獲取頁面傳過來的數據
                        string failCode = FailList[i]["FailCode"].ToString();
                        string failLocation = FailList[i]["FailLocation"].ToString();
                        string failProcess = FailList[i]["FailProcess"].ToString();
                        string failDescription = FailList[i]["FailDesc"].ToString();



                        //黃楊盛 2018年4月24日14:14:28 模擬自動做維修的動作,修正時間
                        //更新R_SN REPAIR_FAILED_FLAG=’1’
                        //modify by ZGJ 2018-03-22 BIP Fail 的產品自動清除待維修狀態，但是記錄不良

                        PANELObj = (R_PANEL_SN) PanelVitualSNSession.Value;
                        if (i == 0)
                        {
                            VirtualSn = PANELObj.SN.ToString();
                            R_SN r = rSn.GetDetailBySN(VirtualSn, Station.SFCDB);
                            rrSn = (Row_R_SN) rSn.GetObjByID(r.ID, Station.SFCDB);
                            //rrSn.REPAIR_FAILED_FLAG = "0";
                            rrSn.REPAIR_FAILED_FLAG = "1";
                            rrSn.SN = StrSn;
                            rrSn.COMPLETED_FLAG = "1";
                            rrSn.COMPLETED_TIME = Station.GetDBDateTime();
                            rrSn.EDIT_EMP = Station.LoginUser.EMP_NO; //add by LLF 2018-03-17
                            rrSn.EDIT_TIME = FailTime; //add by LLF 2018-03-17
                            string strRet = (Station.SFCDB).ExecSQL(rrSn.GetUpdateString(DB_TYPE_ENUM.Oracle));
                            if (!(Convert.ToInt32(strRet) > 0))
                            {
                                throw new MESReturnMessage("update repair failed flag error!");
                            }

                            // Update R_PANEL_SN
                            T_R_PANEL_SN RPanelSN = new T_R_PANEL_SN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                            Row_R_PANEL_SN Row_Panel = (Row_R_PANEL_SN) RPanelSN.NewRow();
                            Row_Panel = (Row_R_PANEL_SN) RPanelSN.GetObjByID(PANELObj.ID, Station.SFCDB);
                            Row_Panel.SN = StrSn;
                            strRet = (Station.SFCDB).ExecSQL(Row_Panel.GetUpdateString(DB_TYPE_ENUM.Oracle));
                            if (!(Convert.ToInt32(strRet) > 0))
                            {
                                throw new MESReturnMessage("update r_panel_sn error!");
                            }

                            //Update AP 

                            //黄杨盛 2018年4月14日09:10:50 修正不能超过9连板的情况.同时加上不支持3位数连板的约束
                            //APVirtualSn = PANELObj.PANEL + "0" + PANELObj.SEQ_NO.ToString();
                            if (PANELObj.SEQ_NO > 99)
                            {
                                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000226",
                                    new string[] {DB_TYPE_ENUM.Oracle.ToString()}));
                            }
                            APVirtualSn = PANELObj.PANEL + Convert.ToInt16(PANELObj.SEQ_NO).ToString("00");


                            string result = APObj.APUpdatePanlSN(APVirtualSn, StrSn, APDB);
                            //APDBPool.Return(APDB);
                            if (!result.Equals("OK"))
                            {
                                //2018年4月24日13:56:14 黃楊盛 
                                //throw new MESReturnMessage("already be binded to other serial number");
                                throw new MESReturnMessage(result);
                            }


                            //新增一筆FAIL記錄到R_SN_STATION_DETAIL
                            T_R_SN_STATION_DETAIL rSnStationDetail =
                                new T_R_SN_STATION_DETAIL(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                            //Add by LLF 2018-03-19
                            R_SN_STATION_DETAIL_ID = rSnStationDetail.GetNewID(Station.BU, Station.SFCDB);
                            //string detailResult = rSnStationDetail.AddDetailToRSnStationDetail(R_SN_STATION_DETAIL_ID,rrSn.GetDataObject(), Station.Line, Station.StationName, Station.StationName, Station.SFCDB);
                            string detailResult = rSnStationDetail.AddDetailToBipStationFailDetail(
                                R_SN_STATION_DETAIL_ID, rrSn.GetDataObject(), Station.Line, Station.StationName,
                                Station.StationName, Station.SFCDB, "1");
                            if (!(Convert.ToInt32(detailResult) > 0))
                            {
                                throw new MESReturnMessage("Insert sn station detail error!");
                            }

                            //update R_SN_STATION_DETAIL 
                            rSnStationDetail.UpdateRSnStationDetailBySNID(StrSn, PANELObj.SN, Station.SFCDB);

                            //新增一筆到R_REPAIR_MAIN
                            T_R_REPAIR_MAIN tRepairMain = new T_R_REPAIR_MAIN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                            Row_R_REPAIR_MAIN rRepairMain = (Row_R_REPAIR_MAIN) tRepairMain.NewRow();

                            RepairmainID = tRepairMain.GetNewID(Station.BU, Station.SFCDB);
                            //rRepairMain.ID = tRepairMain.GetNewID(Station.BU, Station.SFCDB);
                            rRepairMain.ID = RepairmainID;
                            rRepairMain.SN = StrSn;
                            rRepairMain.WORKORDERNO = rrSn.WORKORDERNO;
                            rRepairMain.SKUNO = rrSn.SKUNO;
                            rRepairMain.FAIL_LINE = Station.Line;
                            rRepairMain.FAIL_STATION = Station.StationName;
                            rRepairMain.FAIL_EMP = Station.LoginUser.EMP_NO;
                            //rRepairMain.FAIL_TIME = Station.GetDBDateTime();//Modify by LLF 2018-03-17
                            rRepairMain.FAIL_TIME = FailTime; //Modify by LLF 2018-03-17
                            rRepairMain.EDIT_EMP = Station.LoginUser.EMP_NO;
                            rRepairMain.EDIT_TIME = Station.GetDBDateTime();
                            rRepairMain.CLOSED_FLAG = "1";
                            string insertResult =
                                (Station.SFCDB).ExecSQL(rRepairMain.GetInsertString(DB_TYPE_ENUM.Oracle));
                            if (!(Convert.ToInt32(insertResult) > 0))
                            {
                                throw new Exception("Insert repair main error!");
                            }
                        }

                        //新增一筆到R_REPAIR_FAILCODE
                        T_R_REPAIR_FAILCODE tRepairFailCode =
                            new T_R_REPAIR_FAILCODE(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                        Row_R_REPAIR_FAILCODE rRepairFailCode = (Row_R_REPAIR_FAILCODE) tRepairFailCode.NewRow();
                        rRepairFailCode.ID = tRepairFailCode.GetNewID(Station.BU, Station.SFCDB);
                        //rRepairFailCode.REPAIR_MAIN_ID = rRepairMain.ID; //Modify by LLF 2018-04-11 多筆FAIL 取1一個RepairMainID
                        rRepairFailCode.REPAIR_MAIN_ID = RepairmainID;
                        rRepairFailCode.SN = StrSn;
                        rRepairFailCode.FAIL_CODE = failCode;
                        rRepairFailCode.FAIL_EMP = Station.LoginUser.EMP_NO;
                        rRepairFailCode.FAIL_TIME = FailTime;
                        rRepairFailCode.FAIL_CATEGORY = "SYMPTOM";
                        rRepairFailCode.FAIL_LOCATION = failLocation;
                        rRepairFailCode.FAIL_PROCESS = failProcess;
                        rRepairFailCode.DESCRIPTION = failDescription;
                        rRepairFailCode.REPAIR_FLAG = "1";
                        rRepairFailCode.EDIT_EMP = Station.LoginUser.EMP_NO;
                        rRepairFailCode.EDIT_TIME = Station.GetDBDateTime();

                        string strResult =
                            (Station.SFCDB).ExecSQL(rRepairFailCode.GetInsertString(DB_TYPE_ENUM.Oracle));
                        if (!(Convert.ToInt32(strResult) > 0))
                        {
                            throw new MESReturnMessage("Insert repair failcode error!");
                        }


                        //oleDB.CommitTrain();

                        ReturnMessage.Append(failDescription).Append("|");
                    }


                    //黃楊盛 2018年4月24日14:11:42 做一筆出來的記錄
                    R_SN snOut = rSn.GetDetailBySN(StrSn, Station.SFCDB);
                    rrSn = (Row_R_SN) rSn.GetObjByID(snOut.ID, Station.SFCDB);
                    rrSn.CURRENT_STATION = rrSn.NEXT_STATION;
                    rrSn.NEXT_STATION = rSn.GetNextStation(snOut.ROUTE_ID, snOut.NEXT_STATION, oleDB);
                    rrSn.REPAIR_FAILED_FLAG = "0";
                    rrSn.SN = StrSn;
                    rrSn.EDIT_EMP = Station.LoginUser.EMP_NO;
                    rrSn.EDIT_TIME = FailTime;
                    var count = (Station.SFCDB).ExecSQL(rrSn.GetUpdateString(DB_TYPE_ENUM.Oracle));
                    if (!(Convert.ToInt32(count) > 0))
                    {
                        throw new MESReturnMessage("update rsn failed flag error!");
                    }

                    // 方國剛 2018.05.02 11:45:30
                    // 因拋賬計算過站數量時，不計算REPAIR_FAILED_FLAG=1的數量，故BIP Fail 的產品自動清除待維修狀態后再在過站記錄表記錄一筆REPAIR_FAILED_FLAG=0的記錄
                    T_R_SN_STATION_DETAIL rSnStationDetailRepaired = new T_R_SN_STATION_DETAIL(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                    R_SN_STATION_DETAIL_ID = rSnStationDetailRepaired.GetNewID(Station.BU, Station.SFCDB);                    
                    string detailResultRepaired = rSnStationDetailRepaired.AddDetailToBipStationFailDetail(
                        R_SN_STATION_DETAIL_ID, rrSn.GetDataObject(), Station.Line, Station.StationName,
                        Station.StationName, Station.SFCDB, "0");
                    if (!(Convert.ToInt32(detailResultRepaired) > 0))
                    {
                        throw new MESReturnMessage("Insert sn station detail error!");
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        //APDB.RollbackTrain();
                        //oleDB.RollbackTrain();
                    }
                    catch ( Exception x)
                    {
                        ;
                    }
                    
                    throw new MESReturnMessage(e.Message + e.StackTrace);
                }
                finally
                {
                    //APDBPool.Return(APDB);
                }
              

             
                #endregion
                //add by zgj 2018-03-14
                //當記錄完當前 SN 不良後，清除保存在 session 中的不良信息
                //((List<Dictionary<string, string>>)FailListSession.Value).Clear();

                if (ClearInputSession != null)
                {
                    ClearInputSession.Value = "true";
                }

                ReturnMessage.Remove(ReturnMessage.Length - 1, 1);
                Station.NextInput = Station.FindInputByName("PanelSn");
                Station.AddMessage("MES00000158", new string[] { StrSn, ReturnMessage.ToString() }, StationMessageState.Pass); 
            }
            else
            {
                Station.NextInput = Station.FindInputByName("Location");
                Station.AddMessage("MES00000162", new string[] { StrSn, FailCount.ToString(),FailList.Count.ToString() }, StationMessageState.Message);
            }
        }


        /// <summary>
        /// add by fgg 2018.6.15
        ///PCBA單個維修動作完成Action 
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void PCBARepairSaveAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            SN SnObject = null;
            string updateSql = null;
            string failCodeID = "";
            string actionCode = "";
            string rootCause = "";
            string process = "";
            string location = "";
            string section = "";
            string repairItem = "";
            string repairItemSon = "";
            string pcbaSN = "";

            string tr_sn = "";
            string kp_no = "";
            string mfr_name = "";
            string date_code = "";
            string lot_code = "";

            string description = "";

            T_R_REPAIR_FAILCODE RepairFailcode = new T_R_REPAIR_FAILCODE(Station.SFCDB, Station.DBType);
            Row_R_REPAIR_FAILCODE FailCodeRow;
            T_r_repair_action RepairAction = new T_r_repair_action(Station.SFCDB, Station.DBType);
            Row_r_repair_action RepairRow = (Row_r_repair_action)RepairAction.NewRow();
            T_C_REPAIR_ITEMS TTRepairItems = new T_C_REPAIR_ITEMS(Station.SFCDB, Station.DBType);
            Row_C_REPAIR_ITEMS RepairItemsRow;
            T_C_REPAIR_ITEMS_SON HHRepairItemSon = new T_C_REPAIR_ITEMS_SON(Station.SFCDB, Station.DBType);
            Row_C_REPAIR_ITEMS_SON RepairItemSonRow;

            if (Paras.Count == 0)
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000057");
                throw new MESReturnMessage(errMsg);
            }
            try
            {
                //獲取到 SN 對象
                MESStationSession SNSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
                if (SNSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
                }
                else if (SNSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
                }
                SnObject = (SN)SNSession.Value;

                MESStationSession FailCodeIDSession = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
                if (FailCodeIDSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY }));
                }
                else if (FailCodeIDSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY }));
                }
                failCodeID = FailCodeIDSession.Value.ToString();

                if (failCodeID != "")
                {
                    FailCodeRow = RepairFailcode.GetByFailCodeID(failCodeID, Station.SFCDB);
                    if (FailCodeRow == null)
                    {
                        throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000191", new string[] { SnObject.SerialNo, failCodeID }));
                    }
                }
                else
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY }));
                }

                MESStationSession ActionCodeSession = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
                if (ActionCodeSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE + Paras[2].SESSION_KEY }));
                }
                else if (ActionCodeSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE + Paras[2].SESSION_KEY }));
                }
                actionCode = ActionCodeSession.Value.ToString();

                MESStationSession PCBASNSession = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
                if (PCBASNSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[3].SESSION_TYPE + Paras[3].SESSION_KEY }));
                }
                else if (PCBASNSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[3].SESSION_TYPE + Paras[3].SESSION_KEY }));
                }
                pcbaSN = PCBASNSession.Value.ToString();


                MESStationSession RootCauseSession = Station.StationSession.Find(t => t.MESDataType == Paras[4].SESSION_TYPE && t.SessionKey == Paras[4].SESSION_KEY);
                if (RootCauseSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[4].SESSION_TYPE + Paras[4].SESSION_KEY }));
                }
                else if (RootCauseSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[4].SESSION_TYPE + Paras[4].SESSION_KEY }));
                }
                rootCause = RootCauseSession.Value.ToString();


                MESStationSession ProcessSession = Station.StationSession.Find(t => t.MESDataType == Paras[5].SESSION_TYPE && t.SessionKey == Paras[5].SESSION_KEY);
                if (ProcessSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[5].SESSION_TYPE + Paras[5].SESSION_KEY }));
                }
                else if (ProcessSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[5].SESSION_TYPE + Paras[5].SESSION_KEY }));
                }
                process = ProcessSession.Value.ToString();

                MESStationSession LocationSession = Station.StationSession.Find(t => t.MESDataType == Paras[6].SESSION_TYPE && t.SessionKey == Paras[6].SESSION_KEY);
                if (LocationSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[6].SESSION_TYPE + Paras[6].SESSION_KEY }));
                }
                else if (LocationSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[6].SESSION_TYPE + Paras[6].SESSION_KEY }));
                }
                location = LocationSession.Value.ToString();

                MESStationSession SectionSession = Station.StationSession.Find(t => t.MESDataType == Paras[7].SESSION_TYPE && t.SessionKey == Paras[7].SESSION_KEY);
                if (SectionSession != null && SectionSession.Value != null)
                {
                    section = SectionSession.Value.ToString();
                }

                MESStationSession RepairItemSession = Station.StationSession.Find(t => t.MESDataType == Paras[8].SESSION_TYPE && t.SessionKey == Paras[8].SESSION_KEY);
                if (RepairItemSession != null)
                {
                    repairItem = RepairItemSession.Value.ToString();
                    if (!string.IsNullOrEmpty(repairItem))
                    {                      
                        RepairItemsRow = TTRepairItems.GetIDByItemName(repairItem, Station.SFCDB);
                        repairItem = RepairItemsRow.ID;
                    }
                }                

                MESStationSession RepairItemSonSession = Station.StationSession.Find(t => t.MESDataType == Paras[9].SESSION_TYPE && t.SessionKey == Paras[9].SESSION_KEY);
                if (RepairItemSonSession != null)
                {
                    repairItemSon = RepairItemSonSession.Value.ToString();
                    if (!string.IsNullOrEmpty(repairItemSon))
                    {                        
                        RepairItemSonRow = HHRepairItemSon.GetIDByItemsSon(repairItemSon, Station.SFCDB);
                        repairItemSon = RepairItemSonRow.ID;
                    }
                }                

                MESStationSession TR_SNSession = Station.StationSession.Find(t => t.MESDataType == Paras[10].SESSION_TYPE && t.SessionKey == Paras[10].SESSION_KEY);
                MESStationSession KPNOSession = Station.StationSession.Find(t => t.MESDataType == Paras[11].SESSION_TYPE && t.SessionKey == Paras[11].SESSION_KEY);
                MESStationSession MFRNameSession = Station.StationSession.Find(t => t.MESDataType == Paras[12].SESSION_TYPE && t.SessionKey == Paras[12].SESSION_KEY);
                MESStationSession DateCodeSession = Station.StationSession.Find(t => t.MESDataType == Paras[13].SESSION_TYPE && t.SessionKey == Paras[13].SESSION_KEY);
                MESStationSession LotCodeSession = Station.StationSession.Find(t => t.MESDataType == Paras[14].SESSION_TYPE && t.SessionKey == Paras[14].SESSION_KEY);

                //如果有輸入ALLPART條碼,則取ALLPART條碼對應的料號、廠商、DateCode、LotCode，沒有則取輸入的值
                if (TR_SNSession != null)
                {
                    if (TR_SNSession.Value == null)
                    {
                        throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[10].SESSION_TYPE + Paras[10].SESSION_KEY }));
                    }
                    DataRow tr_snRow = (DataRow)TR_SNSession.Value;
                    tr_sn = tr_snRow["TR_SN"].ToString();
                    kp_no = tr_snRow["CUST_KP_NO"].ToString();
                    mfr_name = tr_snRow["MFR_KP_NO"].ToString();
                    date_code = tr_snRow["DATE_CODE"].ToString();
                    lot_code = tr_snRow["Lot_Code"].ToString();
                }
                else
                {
                    if (KPNOSession != null && KPNOSession.Value != null)
                    {
                        kp_no = KPNOSession.Value.ToString();
                    }
                    else if (Station.Inputs.Find(input => input.DisplayName == "KP_NO") != null)
                    {
                        kp_no = Station.Inputs.Find(input => input.DisplayName == "KP_NO").Value.ToString();
                    }

                    if (MFRNameSession != null && MFRNameSession.Value != null)
                    {
                        mfr_name = MFRNameSession.Value.ToString();
                    }
                    else if (Station.Inputs.Find(input => input.DisplayName == "MFR_Name") != null)
                    {
                        Station.Inputs.Find(input => input.DisplayName == "MFR_Name").Value.ToString();
                    }

                    if (DateCodeSession != null && DateCodeSession.Value != null)
                    {
                        date_code = DateCodeSession.Value.ToString();
                    }
                    else if (Station.Inputs.Find(input => input.DisplayName == "Date_Code") != null)
                    {
                        Station.Inputs.Find(input => input.DisplayName == "Date_Code").Value.ToString();
                    }

                    if (LotCodeSession != null && LotCodeSession.Value != null)
                    {
                        lot_code = LotCodeSession.Value.ToString();
                    }
                    else if (Station.Inputs.Find(input => input.DisplayName == "Lot_Code") != null)
                    {
                        Station.Inputs.Find(input => input.DisplayName == "Lot_Code").Value.ToString();
                    }
                }

                MESStationInput DescriptionSession = Station.Inputs.Find(t => t.DisplayName == Paras[15].SESSION_TYPE);
                if (DescriptionSession == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[15].SESSION_TYPE + Paras[15].SESSION_KEY }));
                }
                else if (DescriptionSession.Value == null)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[15].SESSION_TYPE + Paras[15].SESSION_KEY }));
                }
                description = DescriptionSession.Value.ToString();
                Station.SFCDB.BeginTrain();
                RepairRow.ID = RepairAction.GetNewID(Station.BU, Station.SFCDB);
                RepairRow.REPAIR_FAILCODE_ID = failCodeID;
                RepairRow.SN = SnObject.SerialNo;
                RepairRow.ACTION_CODE = actionCode;
                RepairRow.SECTION_ID = section;
                RepairRow.PROCESS = process;
                RepairRow.ITEMS_ID = repairItem;
                RepairRow.ITEMS_SON_ID = repairItemSon;
                RepairRow.REASON_CODE = rootCause;
                RepairRow.DESCRIPTION = description;
                RepairRow.FAIL_LOCATION = location;
                RepairRow.FAIL_CODE = FailCodeRow.FAIL_CODE;
                RepairRow.KEYPART_SN = pcbaSN;
                RepairRow.NEW_KEYPART_SN = "";
                RepairRow.TR_SN = tr_sn;
                RepairRow.KP_NO = kp_no;
                RepairRow.MFR_NAME = mfr_name;
                RepairRow.DATE_CODE = date_code;
                RepairRow.LOT_CODE = lot_code;
                RepairRow.REPAIR_EMP = Station.LoginUser.EMP_NO;
                RepairRow.REPAIR_TIME = Station.GetDBDateTime();
                RepairRow.EDIT_EMP = Station.LoginUser.EMP_NO;
                RepairRow.EDIT_TIME = Station.GetDBDateTime();

                string StrRes = Station.SFCDB.ExecSQL(RepairRow.GetInsertString(Station.DBType));
                if (StrRes == "1")
                {
                    Row_R_REPAIR_FAILCODE FRow = (Row_R_REPAIR_FAILCODE)RepairFailcode.GetObjByID(failCodeID, Station.SFCDB);
                    FRow.REPAIR_FLAG = "1";  //執行完維修動作後更新R_REPAIR_FAILCODE   FLAG=1 
                    FRow.EDIT_TIME = Station.GetDBDateTime();
                    updateSql = FRow.GetUpdateString(Station.DBType);
                    Station.SFCDB.ExecSQL(updateSql);
                    for (int i = 1; i < Paras.Count; i++)
                    {
                        Station.StationSession.Remove(Station.StationSession.Find(s => s.MESDataType == Paras[i].SESSION_TYPE && s.SessionKey == Paras[i].SESSION_KEY));
                    }                   
                    foreach (MESStationInput input in Station.Inputs)
                    {
                        input.Value = null;
                    }
                    Station.AddMessage("MES00000105", new string[] { SnObject.SerialNo, failCodeID }, StationMessageState.Pass);
                }
                else
                {                   
                    Station.AddMessage("MES00000083", new string[] { SnObject.SerialNo, failCodeID }, StationMessageState.Fail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///PCBA維修產品所有維修動作完成Action
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void SNRepairFinishAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            SN SnObject = null;
            string UpdateSql = "";
            string result = "";
            T_R_REPAIR_FAILCODE RepairFailcode = new T_R_REPAIR_FAILCODE(Station.SFCDB, Station.DBType);           
            T_r_repair_action RepairAction = new T_r_repair_action(Station.SFCDB, Station.DBType);
            Row_r_repair_action RepairRow = (Row_r_repair_action)RepairAction.NewRow();
            T_R_REPAIR_MAIN RMain = new T_R_REPAIR_MAIN(Station.SFCDB, Station.DBType);
            List<R_REPAIR_MAIN> RepairMainInfo = new List<R_REPAIR_MAIN>();
            List<R_REPAIR_FAILCODE> FailCodeInfo = new List<R_REPAIR_FAILCODE>();
            T_R_SN table = new T_R_SN(Station.SFCDB, Station.DBType);
            string DeviceName = Station.StationName;

            if (Paras.Count == 0)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000057"));
            }

            //獲取到 SN 對象
            MESStationSession SNSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (SNSession == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }
            else if (SNSession.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }
            SnObject = (SN)SNSession.Value;

            //獲取 DEVICE1
            MESStationSession DeviceSession = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (DeviceSession != null)
            {
                DeviceName = DeviceSession.Value.ToString();
            }

            try
            {
                RepairMainInfo = RMain.GetRepairMainBySN(Station.SFCDB, SnObject.SerialNo).FindAll(r => r.CLOSED_FLAG == "0");
                if (RepairMainInfo == null || RepairMainInfo.Count == 0)
                {
                    return;
                }
                if (RepairMainInfo.Count > 1)
                {
                    //維修主表有多條為維修完成的記錄
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180621185023", new string[] { SnObject.SerialNo}));
                }

                FailCodeInfo = RepairFailcode.CheckSNRepairFinishAction(Station.SFCDB, SnObject.SerialNo, RepairMainInfo[0].ID);
                if (FailCodeInfo.Count != 0)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000106", new string[] { SnObject.SerialNo, FailCodeInfo[0].ID })); ///未维修完成的无法update repair_main 表信息
                }
                else
                {
                    table.RecordPassStationDetail(SnObject.SerialNo, Station.Line, Station.StationName, DeviceName, Station.BU, Station.SFCDB);   //添加过站记录

                    //執行完所有的維修動作後才能更新R_REPAIR_MAIN  FLAG=1 
                    Row_R_REPAIR_MAIN FRow = (Row_R_REPAIR_MAIN)RMain.GetObjByID(RepairMainInfo[0].ID, Station.SFCDB);
                    FRow.CLOSED_FLAG = "1";
                    FRow.EDIT_TIME = Station.GetDBDateTime();
                    UpdateSql = FRow.GetUpdateString(Station.DBType);
                    result = Station.SFCDB.ExecSQL(UpdateSql);
                    if (Convert.ToInt32(result) > 0)
                    {
                        foreach (R_Station_Output output in Station.StationOutputs)
                        {
                            if (Station.StationSession.Find(s => s.MESDataType == output.SESSION_TYPE && s.SessionKey == output.SESSION_KEY) != null)
                            {
                                Station.StationSession.Find(s => s.MESDataType == output.SESSION_TYPE && s.SessionKey == output.SESSION_KEY).Value = "";
                            }
                        }

                        foreach (MESStationInput input in Station.Inputs)
                        {
                            if (Station.StationSession.Find(s => s.MESDataType == input.DisplayName) != null)
                            {
                                Station.StationSession.Find(s => s.MESDataType == input.DisplayName).Value = "";
                            }
                            input.Value = "";
                        }
                    }
                    else
                    {
                        throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "REPAIR MAIN" })); 
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// SN掃入維修ByFailCode
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void SNFailByFailCodeAction(MESPubLab.MESStation.MESStationBase Station, MESPubLab.MESStation.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count == 0)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }
            MESStationSession sessionFailCode = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionFailCode == null || sessionFailCode.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY }));
            }
            MESStationSession sessionFailDescription = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionFailDescription == null || sessionFailDescription.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }

            T_R_SN t_r_sn = new T_R_SN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            T_R_REPAIR_MAIN tRepairMain = new T_R_REPAIR_MAIN(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            T_R_SN_STATION_DETAIL rSnStationDetail = new T_R_SN_STATION_DETAIL(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            Row_R_REPAIR_MAIN rRepairMain = (Row_R_REPAIR_MAIN)tRepairMain.NewRow();
            Row_R_SN row_r_sn = (Row_R_SN)t_r_sn.NewRow();
            string result = "";
            string repairMainID = "";
            //更新R_SN REPAIR_FAILED_FLAG=’1’
            R_SN r_sn = t_r_sn.GetDetailBySN(sessionSN.Value.ToString(), Station.SFCDB);
            row_r_sn = (Row_R_SN)t_r_sn.GetObjByID(r_sn.ID, Station.SFCDB);
            row_r_sn.REPAIR_FAILED_FLAG = "1";
            row_r_sn.EDIT_EMP = Station.LoginUser.EMP_NO;
            row_r_sn.EDIT_TIME = Station.GetDBDateTime();
            result = (Station.SFCDB).ExecSQL(row_r_sn.GetUpdateString(DB_TYPE_ENUM.Oracle));
            if (!(Convert.ToInt32(result) > 0))
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "R_SN" }));
            }

            //新增一筆FAIL記錄到R_SN_STATION_DETAIL
            result = rSnStationDetail.AddDetailToRSnStationDetail(rSnStationDetail.GetNewID(Station.BU, Station.SFCDB),
                row_r_sn.GetDataObject(), Station.Line, Station.StationName, Station.StationName, Station.SFCDB);
            if (!(Convert.ToInt32(result) > 0))
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000021", new string[] { "STATION_DETAIL" }));
            }

            //新增一筆到R_REPAIR_MAIN
            repairMainID = tRepairMain.GetNewID(Station.BU, Station.SFCDB);
            rRepairMain.ID = repairMainID;
            rRepairMain.SN = r_sn.SN;
            rRepairMain.WORKORDERNO = r_sn.WORKORDERNO;
            rRepairMain.SKUNO = r_sn.SKUNO;
            rRepairMain.FAIL_LINE = Station.Line;
            rRepairMain.FAIL_STATION = Station.StationName;
            rRepairMain.FAIL_EMP = Station.LoginUser.EMP_NO;
            rRepairMain.FAIL_TIME = Station.GetDBDateTime();
            rRepairMain.CREATE_TIME = Station.GetDBDateTime();
            rRepairMain.EDIT_EMP = Station.LoginUser.EMP_NO;
            rRepairMain.EDIT_TIME = Station.GetDBDateTime();
            rRepairMain.CLOSED_FLAG = "0";
            result = (Station.SFCDB).ExecSQL(rRepairMain.GetInsertString(DB_TYPE_ENUM.Oracle));
            if (!(Convert.ToInt32(result) > 0))
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000021", new string[] { "REPAIR_MAIN" }));
            }

            //新增一筆到R_REPAIR_FAILCODE
            C_ERROR_CODE failCodeObject = (C_ERROR_CODE)sessionFailCode.Value;
            T_R_REPAIR_FAILCODE tRepairFailCode = new T_R_REPAIR_FAILCODE(Station.SFCDB, DB_TYPE_ENUM.Oracle);
            Row_R_REPAIR_FAILCODE rRepairFailCode = (Row_R_REPAIR_FAILCODE)tRepairFailCode.NewRow();
            rRepairFailCode.ID = tRepairFailCode.GetNewID(Station.BU, Station.SFCDB);
            rRepairFailCode.REPAIR_MAIN_ID = repairMainID;
            rRepairFailCode.SN = r_sn.SN;
            rRepairFailCode.FAIL_CODE = failCodeObject.ERROR_CODE;
            rRepairFailCode.FAIL_EMP = Station.LoginUser.EMP_NO;
            rRepairFailCode.FAIL_TIME = Station.GetDBDateTime();
            rRepairFailCode.FAIL_CATEGORY = failCodeObject.ERROR_CATEGORY;
            rRepairFailCode.FAIL_LOCATION = "";
            rRepairFailCode.FAIL_PROCESS = "";
            rRepairFailCode.DESCRIPTION = sessionFailDescription.Value.ToString();
            rRepairFailCode.REPAIR_FLAG = "0";
            rRepairFailCode.CREATE_TIME = Station.GetDBDateTime();
            rRepairFailCode.EDIT_EMP = Station.LoginUser.EMP_NO;
            rRepairFailCode.EDIT_TIME = Station.GetDBDateTime();
            result = (Station.SFCDB).ExecSQL(rRepairFailCode.GetInsertString(DB_TYPE_ENUM.Oracle));
            if (!(Convert.ToInt32(result) > 0))
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000021", new string[] { "FAILCODE" }));
            }
            Station.AddMessage("MES00000001", new string[] { }, StationMessageState.Pass);         
        }

        //產品維修CheckIn Action ByPassWord 
        public static void SNInByPassWordRepairAction(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 5)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession sessionSendEmp = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSendEmp == null || sessionSendEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE }));
            }
            MESStationSession sessionSendPW = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionSendPW == null || sessionSendPW.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE }));
            }
            MESStationSession sessionReceiveEmp = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionReceiveEmp == null || sessionReceiveEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE }));
            }
            MESStationSession sessionReceivePW = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (sessionReceivePW == null || sessionReceivePW.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[3].SESSION_TYPE }));
            }
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[4].SESSION_TYPE && t.SessionKey == Paras[4].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[4].SESSION_TYPE }));
            }

            T_c_user t_c_user = new T_c_user(Station.SFCDB, Station.DBType);
            Row_c_user rowSendUser = t_c_user.getC_Userbyempno(sessionSendEmp.Value.ToString(), Station.SFCDB, Station.DBType);
            if (rowSendUser == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180620163103", new string[] { sessionSendEmp.Value.ToString() }));
            }
            if (!rowSendUser.EMP_PASSWORD.Equals(sessionSendPW.Value.ToString()))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180813154717", new string[] { sessionSendEmp.Value.ToString() }));
            }

            Row_c_user rowReceiveUser = t_c_user.getC_Userbyempno(sessionReceiveEmp.Value.ToString(), Station.SFCDB, Station.DBType);
            if (rowReceiveUser == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180620163103", new string[] { sessionReceiveEmp.Value.ToString() }));
            }
            if (!rowReceiveUser.EMP_PASSWORD.Equals(sessionReceivePW.Value.ToString()))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180813154717", new string[] { sessionReceivePW.Value.ToString() }));
            }

            SN snObject = (SN)sessionSN.Value;
            T_R_REPAIR_MAIN rRepairMain = new T_R_REPAIR_MAIN(Station.SFCDB, Station.DBType);
            List<R_REPAIR_MAIN> RepariMainList = rRepairMain.GetRepairMainBySN(Station.SFCDB, snObject.SerialNo);
            R_REPAIR_MAIN rMain = RepariMainList.Where(r => r.CLOSED_FLAG == "0").FirstOrDefault();  // Find(r => r.CLOSED_FLAG == "0");
            if (rMain != null)
            {
                T_R_REPAIR_TRANSFER rTransfer = new T_R_REPAIR_TRANSFER(Station.SFCDB, Station.DBType);
                Row_R_REPAIR_TRANSFER rowTransfer = (Row_R_REPAIR_TRANSFER)rTransfer.NewRow();
                rowTransfer.ID = rTransfer.GetNewID(Station.BU, Station.SFCDB);
                rowTransfer.REPAIR_MAIN_ID = rMain.ID;
                rowTransfer.IN_SEND_EMP = sessionSendEmp.Value.ToString();
                rowTransfer.IN_RECEIVE_EMP = sessionReceiveEmp.Value.ToString();
                rowTransfer.IN_TIME = Station.GetDBDateTime();
                rowTransfer.SN = snObject.SerialNo;
                rowTransfer.LINE_NAME = Station.Line;
                rowTransfer.STATION_NAME = snObject.CurrentStation;
                rowTransfer.WORKORDERNO = snObject.WorkorderNo;
                rowTransfer.SKUNO = snObject.SkuNo;
                rowTransfer.CLOSED_FLAG = "0";
                rowTransfer.CREATE_TIME = Station.GetDBDateTime();
                rowTransfer.DESCRIPTION = "";
                rowTransfer.EDIT_TIME = Station.GetDBDateTime();
                rowTransfer.EDIT_EMP = sessionReceiveEmp.Value.ToString();
                string strRet = (Station.SFCDB).ExecSQL(rowTransfer.GetInsertString(DB_TYPE_ENUM.Oracle));
                if (Convert.ToInt32(strRet) > 0)
                {
                    Station.AddMessage("MES00000001", new string[] { }, StationMessageState.Pass);
                }
                else
                {
                    Station.AddMessage("MES00000037", new string[] { "INSET R_REPAIR_TRANSFER" }, StationMessageState.Pass);
                }
            }
            else
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000066", new string[] { snObject.SerialNo, "CLOSED" }));
            }
        }

        //產品維修CheckOut Action ByPassWord 
        public static void SNOutByPassWordRepairAction(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 5)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession sessionSendEmp = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionSendEmp == null || sessionSendEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE }));
            }
            MESStationSession sessionSendPW = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionSendPW == null || sessionSendPW.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE }));
            }

            MESStationSession sessionReceiveEmp = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionReceiveEmp == null || sessionReceiveEmp.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE }));
            }

            MESStationSession sessionReceivePW = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (sessionReceivePW == null || sessionReceivePW.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[3].SESSION_TYPE }));
            }

            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[4].SESSION_TYPE && t.SessionKey == Paras[4].SESSION_KEY);
            if (sessionSN == null || sessionSN.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[4].SESSION_TYPE }));
            }

            T_c_user t_c_user = new T_c_user(Station.SFCDB, Station.DBType);
            Row_c_user rowSendUser = t_c_user.getC_Userbyempno(sessionSendEmp.Value.ToString(), Station.SFCDB, Station.DBType);
            if (rowSendUser == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180620163103", new string[] { sessionSendEmp.Value.ToString() }));
            }
            if (!rowSendUser.EMP_PASSWORD.Equals(sessionSendPW.Value.ToString()))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180813154717", new string[] { sessionSendEmp.Value.ToString() }));
            }

            Row_c_user rowReceiveUser = t_c_user.getC_Userbyempno(sessionReceiveEmp.Value.ToString(), Station.SFCDB, Station.DBType);
            if (rowReceiveUser == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180620163103", new string[] { sessionReceiveEmp.Value.ToString() }));
            }
            if (!rowReceiveUser.EMP_PASSWORD.Equals(sessionReceivePW.Value.ToString()))
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180813154717", new string[] { sessionReceivePW.Value.ToString() }));
            }

            SN snObject = (SN)sessionSN.Value;

            T_R_REPAIR_TRANSFER rTransfer = new T_R_REPAIR_TRANSFER(Station.SFCDB, Station.DBType);
            Row_R_REPAIR_TRANSFER rowTransfer = (Row_R_REPAIR_TRANSFER)rTransfer.NewRow();
            T_R_SN t_r_sn = new T_R_SN(Station.SFCDB, Station.DBType);

            List<R_REPAIR_TRANSFER> transferList = rTransfer.GetLastRepairedBySN(snObject.SerialNo, Station.SFCDB);
            R_REPAIR_TRANSFER rRepairTransfer = transferList.Where(r => r.CLOSED_FLAG == "0").FirstOrDefault();//TRANSFER表 1 表示不良
            if (rRepairTransfer != null)
            {
                rowTransfer = (Row_R_REPAIR_TRANSFER)rTransfer.GetObjByID(rRepairTransfer.ID, Station.SFCDB);
                rowTransfer.CLOSED_FLAG = "1";
                rowTransfer.OUT_TIME = Station.GetDBDateTime();
                rowTransfer.OUT_SEND_EMP = sessionSendEmp.Value.ToString();
                rowTransfer.OUT_RECEIVE_EMP = sessionReceiveEmp.Value.ToString();

                string strRet = (Station.SFCDB).ExecSQL(rowTransfer.GetUpdateString(DB_TYPE_ENUM.Oracle));
                if (Convert.ToInt32(strRet) > 0)
                {
                    Station.AddMessage("MES00000035", new string[] { strRet }, StationMessageState.Pass);
                }
                else
                {
                    Station.AddMessage("MES00000025", new string[] { "REPAIR TRANSFER" }, StationMessageState.Pass);
                }
                Row_R_SN rowSN = (Row_R_SN)t_r_sn.GetObjByID(snObject.ID, Station.SFCDB);
                rowSN.REPAIR_FAILED_FLAG = "0";
                strRet = (Station.SFCDB).ExecSQL(rowSN.GetUpdateString(DB_TYPE_ENUM.Oracle));
                if (Convert.ToInt32(strRet) > 0)
                {
                    Station.AddMessage("MES00000035", new string[] { strRet }, StationMessageState.Pass);
                }
                else
                {
                    Station.AddMessage("MES00000025", new string[] { "R_SN" }, StationMessageState.Pass);
                }
            }
            else
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000066", new string[] { snObject.SerialNo, "abnormal" }));
            }
        }

    }
}
