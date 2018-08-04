using MESStation.LogicObject;
using MESStation.MESReturnView.Station;
using System.Collections;
using MESDataObject;
using System.Data;
using MESDBHelper;
using MESStation.Stations.StationActions.DataLoaders;
using System.Data.OleDb;
using System.Collections.Generic;
using MESDataObject.Module;
using System.Reflection;
using MESStation.Label;
using MESStation.BaseClass;
using MESStation.Packing;
using System.Linq;
using System;

namespace MESStation.Stations.StationActions.ActionRunners
{
    public class PackAction
    {
        public static void CloseCartionAndPalletAction(MESStation.BaseClass.MESStationBase Station, MESStation.BaseClass.MESStationInput Input, List<MESDataObject.Module.R_Station_Action_Para> Paras)
        {
            OleExec SFCDB = Station.SFCDB;
            string Run = "";
            try
            {
                Run = (Station.StationSession.Find(T => T.MESDataType == Paras[0].SESSION_TYPE && T.SessionKey == Paras[0].SESSION_KEY).Value).ToString();
                if (Run.ToUpper() == "FALSE")
                {
                    return;
                }
            }
            catch
            {

            }

            MESStationSession sessionCarton = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionCarton == null)
            {
                throw new System.Exception("sessionCartion miss ");
            }

            MESStationSession sessionPallet = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionPallet == null)
            {
                throw new System.Exception("sessionPallet miss ");
            }
            MESStationSession sessionPrintPL = Station.StationSession.Find(t => t.MESDataType == "ISPRINT_PL" && t.SessionKey == "1");
            if (sessionPrintPL == null)
            {
                sessionPrintPL = new MESStationSession() { MESDataType = "ISPRINT_PL", SessionKey = "1", Value = "FALSE" };
                Station.StationSession.Add(sessionPrintPL);
            }
            sessionPrintPL.Value = "FALSE";
            MESStationSession sessionPrintCTN = Station.StationSession.Find(t => t.MESDataType == "ISPRINT_CTN" && t.SessionKey == "1");
            if (sessionPrintCTN == null)
            {
                sessionPrintCTN = new MESStationSession() { MESDataType = "ISPRINT_CTN", SessionKey = "1", Value = "FALSE" };
                Station.StationSession.Add(sessionPrintCTN);
            }
            sessionPrintCTN.Value = "FALSE";
            CartionBase cartion = (CartionBase)sessionCarton.Value;
            PalletBase Pallet = (PalletBase)sessionPallet.Value;

            cartion.DATA.CLOSED_FLAG = "1";
            cartion.DATA.EDIT_TIME = DateTime.Now;
            cartion.DATA.EDIT_EMP = Station.LoginUser.EMP_NO;
            if (cartion.DATA.QTY == 0)
            {
                cartion.DATA.PARENT_PACK_ID = "";
            }
            Station.SFCDB.ExecSQL(cartion.DATA.GetUpdateString(DB_TYPE_ENUM.Oracle));

            Pallet.DATA.QTY = Pallet.GetSnCount(SFCDB);
            Pallet.DATA.CLOSED_FLAG = "1";
            Pallet.DATA.EDIT_TIME = DateTime.Now;
            Pallet.DATA.EDIT_EMP = Station.LoginUser.EMP_NO;
            Station.SFCDB.ExecSQL(Pallet.DATA.GetUpdateString(DB_TYPE_ENUM.Oracle));
            sessionPrintPL.Value = "TRUE";
            sessionPrintCTN.Value = "TRUE";

            MESStationSession CTNPrintSession = Station.StationSession.Find(T => T.MESDataType == "PRINT_CTN" && T.SessionKey == "1");
            if (CTNPrintSession == null)
            {
                CTNPrintSession = new MESStationSession() { MESDataType = "PRINT_CTN", SessionKey = "1" };
                Station.StationSession.Add(CTNPrintSession);
            }
            CTNPrintSession.Value = cartion.DATA.PACK_NO;

            MESStationSession PlPrintSession = Station.StationSession.Find(T => T.MESDataType == "PRINT_PL" && T.SessionKey == "1");
            if (PlPrintSession == null)
            {
                PlPrintSession = new MESStationSession() { MESDataType = "PRINT_PL", SessionKey = "1" };
                Station.StationSession.Add(PlPrintSession);
            }
            PlPrintSession.Value = Pallet.DATA.PACK_NO;


            Station.StationSession.Remove(sessionCarton);
            Station.StationSession.Remove(sessionPallet);




        }
        public static void CartionAndPalletAction(MESStation.BaseClass.MESStationBase Station, MESStation.BaseClass.MESStationInput Input, List<MESDataObject.Module.R_Station_Action_Para> Paras)
        {
            OleExec SFCDB = Station.SFCDB;
            string Run = "";
            try
            {
                Run = (Station.StationSession.Find(T => T.MESDataType == Paras[0].SESSION_TYPE && T.SessionKey == Paras[0].SESSION_KEY).Value).ToString();
                if (Run.ToUpper() == "FALSE")
                {
                    return;
                }
            }
            catch
            {

            }
            MESStationSession sessionSN = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionSN == null)
            {
                throw new System.Exception("sessionSN miss ");
            }

            MESStationSession sessionCartion = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionCartion == null)
            {
                throw new System.Exception("sessionCartion miss ");
            }

            MESStationSession sessionPallet = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (sessionPallet == null)
            {
                throw new System.Exception("sessionPallet miss ");
            }

            MESStationSession sessionPrintPL = Station.StationSession.Find(t => t.MESDataType == "ISPRINT_PL" && t.SessionKey == "1");
            if (sessionPrintPL == null)
            {
                sessionPrintPL = new MESStationSession() { MESDataType = "ISPRINT_PL", SessionKey = "1", Value = "FALSE" };
                Station.StationSession.Add(sessionPrintPL);
            }
            sessionPrintPL.Value = "FALSE";
            MESStationSession sessionPrintCTN = Station.StationSession.Find(t => t.MESDataType == "ISPRINT_CTN" && t.SessionKey == "1");
            if (sessionPrintCTN == null)
            {
                sessionPrintCTN = new MESStationSession() { MESDataType = "ISPRINT_CTN", SessionKey = "1", Value = "FALSE" };
                Station.StationSession.Add(sessionPrintCTN);
            }
            sessionPrintCTN.Value = "FALSE";

            SN SN = (SN)sessionSN.Value;

            if (SN.isPacked(Station.SFCDB))
            {
                throw new System.Exception($@"{SN.SerialNo} is packed!");
            }

            CartionBase cartion = (CartionBase)sessionCartion.Value;
            PalletBase Pallet = (PalletBase)sessionPallet.Value;



            cartion.Add(SN, Station.BU, Station.LoginUser.EMP_NO, Station.SFCDB);

            if (cartion.DATA.MAX_QTY <= cartion.GetCount(Station.SFCDB))
            {
                sessionPrintCTN.Value = "TRUE";
                //設置打印變量
                MESStationSession CTNPrintSession = Station.StationSession.Find(T => T.MESDataType == "PRINT_CTN" && T.SessionKey == "1");
                if (CTNPrintSession == null)
                {
                    CTNPrintSession = new MESStationSession() { MESDataType = "PRINT_CTN", SessionKey = "1" };
                    Station.StationSession.Add(CTNPrintSession);
                }
                CTNPrintSession.Value = cartion.DATA.PACK_NO;
                T_C_PACKING TCP = new T_C_PACKING(Station.SFCDB, DB_TYPE_ENUM.Oracle);
                List<C_PACKING> PackConfigs = TCP.GetPackingBySku(SN.SkuNo, Station.SFCDB);
                C_PACKING CartionConfig = PackConfigs.Find(T => T.PACK_TYPE == "CARTON");
                C_PACKING PalletConfig = PackConfigs.Find(T => T.PACK_TYPE == "PALLET");
                if (CartionConfig == null)
                {
                    throw new Exception("Can't find CartionConfig");
                }
                if (PalletConfig == null)
                {
                    throw new Exception("Can't find PalletConfig");
                }
                if (Pallet.DATA.MAX_QTY <= Pallet.GetCount(Station.SFCDB))
                {
                    sessionPrintPL.Value = "TRUE";
                    //設置打印變量
                    MESStationSession PlPrintSession = Station.StationSession.Find(T => T.MESDataType == "PRINT_PL" && T.SessionKey == "1");
                    if (PlPrintSession == null)
                    {
                        PlPrintSession = new MESStationSession() { MESDataType = "PRINT_PL", SessionKey = "1" };
                        Station.StationSession.Add(PlPrintSession);
                    }
                    PlPrintSession.Value = Pallet.DATA.PACK_NO;

                    Pallet.DATA.CLOSED_FLAG = "1";
                    Pallet.DATA.EDIT_TIME = DateTime.Now;
                    Pallet.DATA.EDIT_EMP = Station.LoginUser.EMP_NO;
                    Station.SFCDB.ExecSQL(Pallet.DATA.GetUpdateString(DB_TYPE_ENUM.Oracle));

                    Pallet.DATA = PackingBase.GetNewPacking(PalletConfig, Station.Line, Station.StationName, Station.IP, Station.BU, Station.LoginUser.EMP_NO, Station.SFCDB);

                }
                cartion.DATA.CLOSED_FLAG = "1";
                cartion.DATA.EDIT_TIME = DateTime.Now;
                cartion.DATA.EDIT_EMP = Station.LoginUser.EMP_NO;
                Station.SFCDB.ExecSQL(cartion.DATA.GetUpdateString(DB_TYPE_ENUM.Oracle));
                cartion.DATA = PackingBase.GetNewPacking(CartionConfig, Station.Line, Station.StationName, Station.IP, Station.BU, Station.LoginUser.EMP_NO, Station.SFCDB);

                Pallet.Add(cartion, Station.BU, Station.LoginUser.EMP_NO, Station.SFCDB);
            }
            sessionCartion.Value = cartion;
            sessionPallet.Value = Pallet;

            cartion.DATA.AcceptChange();
            Pallet.DATA.AcceptChange();

        }

        public static void CloseLot(MESStation.BaseClass.MESStationBase Station, MESStation.BaseClass.MESStationInput Input, List<MESDataObject.Module.R_Station_Action_Para> Paras)
        {
            DisplayOutPut Dis_LotNo = Station.DisplayOutput.Find(t => t.Name == "LOTNO");
            MESStationInput Level = Station.Inputs.Find(t => t.DisplayName == "AQLLEVEL");
            T_R_LOT_STATUS tRLotStatus = new T_R_LOT_STATUS(Station.SFCDB, Station.DBType);
            Row_R_LOT_STATUS r = tRLotStatus.GetByLotNo(Dis_LotNo.Value.ToString(), Station.SFCDB);
            if (r.LOT_NO == null || !r.CLOSED_FLAG.Equals("0"))
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MSGCODE20180528103627", new string[] { Dis_LotNo.Value.ToString() }));
            try
            {
                //根據關閉時的AQL更新LotStatus
                T_C_AQLTYPE tCAqlType = new T_C_AQLTYPE(Station.SFCDB, Station.DBType);
                List<C_AQLTYPE> cAqlTypeList = tCAqlType.GetAqlTypeBySkunoAndLevel(r.SKUNO, Level.Value.ToString(), Station.SFCDB);

                r.REJECT_QTY = cAqlTypeList.Where(t => t.LOT_QTY > r.LOT_QTY).OrderBy(t => t.LOT_QTY).Take(1).ToList<C_AQLTYPE>()[0].REJECT_QTY;
                r.SAMPLE_QTY = cAqlTypeList.Where(t => t.LOT_QTY > r.LOT_QTY).OrderBy(t => t.LOT_QTY).Take(1).ToList<C_AQLTYPE>()[0].SAMPLE_QTY;
                r.SAMPLE_QTY = r.SAMPLE_QTY > r.LOT_QTY ? r.LOT_QTY : r.SAMPLE_QTY;
                r.CLOSED_FLAG = "1";
                r.AQL_LEVEL = Level.Value.ToString();
                r.EDIT_EMP = Station.LoginUser.EMP_NO;
                Station.SFCDB.ThrowSqlExeception = true;
                r.EDIT_TIME = tRLotStatus.GetDBDateTime(Station.SFCDB);
                Station.SFCDB.ExecSQL(r.GetUpdateString(Station.DBType));
            }
            catch (Exception e)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MSGCODE20180528105826", new string[] { Dis_LotNo.Value.ToString(), e.Message }));
            }
            finally { Station.SFCDB.ThrowSqlExeception = false; }
            #region 清空界面信息
            Station.StationSession.Clear();
            Station.Inputs.Find(t => t.DisplayName == Paras[0].SESSION_TYPE).DataForUse.Clear();
            Station.Inputs.Find(t => t.DisplayName =="AQLLEVEL").DataForUse.Clear();
            #endregion
        }

        public static void PackPassStation(MESStation.BaseClass.MESStationBase Station, MESStation.BaseClass.MESStationInput Input, List<MESDataObject.Module.R_Station_Action_Para> Paras)
        {
            MESStationSession packSesseion = Station.StationSession.Find(t=>t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == "1");
            T_R_SN tRSn = new T_R_SN(Station.SFCDB, Station.DBType);
            List<R_SN> rSnList = new List<R_SN>();
            rSnList = tRSn.GetSnListByPack(packSesseion.Value.ToString(), Station.SFCDB);
            if(rSnList.Count==0)
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MSGCODE20180602102010", new string[] { packSesseion.Value.ToString() }));

            tRSn.LotsPassStation(rSnList, Station.Line, rSnList[0].NEXT_STATION, rSnList[0].NEXT_STATION, Station.BU, "PASS", Station.LoginUser.EMP_NO, Station.SFCDB); // 過站
            //記錄通過數 ,UPH
            foreach (var snobj in rSnList)
            {
                tRSn.RecordUPH(snobj.WORKORDERNO, 1, snobj.SN, "PASS", Station.Line, snobj.NEXT_STATION, Station.LoginUser.EMP_NO, Station.BU, Station.SFCDB);
            }
            Station.StationMessages.Add(new StationMessage() {Message= MESReturnMessage.GetMESReturnMessage("MSGCODE20180602102159",new string[] { rSnList[0].SKUNO, packSesseion.Value.ToString(), rSnList.Count.ToString(), rSnList[0].NEXT_STATION }), State = StationMessageState.Pass});
        }

        /// <summary>
        /// 移棧板或卡通內的數據
        /// add by fgg 2018.06.08
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void MovePackingSessionValue(MESStation.BaseClass.MESStationBase Station, MESStation.BaseClass.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count != 8)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000057", new string[] { }));
            }

            MESStationSession sessionOne = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionOne == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }
            if (sessionOne.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }

            MESStationSession sessionTwo = Station.StationSession.Find(t => t.MESDataType == Paras[1].SESSION_TYPE && t.SessionKey == Paras[1].SESSION_KEY);
            if (sessionTwo == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY }));
            }
            if (sessionTwo.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[1].SESSION_TYPE + Paras[1].SESSION_KEY }));
            }

            MESStationSession sessionValue = Station.StationSession.Find(t => t.MESDataType == Paras[2].SESSION_TYPE && t.SessionKey == Paras[2].SESSION_KEY);
            if (sessionValue == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE + Paras[2].SESSION_KEY }));
            }
            if (sessionValue.Value.ToString() == "")
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[2].SESSION_TYPE + Paras[2].SESSION_KEY }));
            }

            MESStationSession sessionItemListOne = Station.StationSession.Find(t => t.MESDataType == Paras[3].SESSION_TYPE && t.SessionKey == Paras[3].SESSION_KEY);
            if (sessionItemListOne == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[3].SESSION_TYPE + Paras[3].SESSION_KEY }));
            }

            MESStationSession sessionItemListTwo = Station.StationSession.Find(t => t.MESDataType == Paras[4].SESSION_TYPE && t.SessionKey == Paras[4].SESSION_KEY);
            if (sessionItemListTwo == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[4].SESSION_TYPE + Paras[4].SESSION_KEY }));
            }

            MESStationSession sessionCountOne = Station.StationSession.Find(t => t.MESDataType == Paras[5].SESSION_TYPE && t.SessionKey == Paras[5].SESSION_KEY);
            if (sessionCountOne == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[5].SESSION_TYPE + Paras[5].SESSION_KEY }));
            }

            MESStationSession sessionCountTwo = Station.StationSession.Find(t => t.MESDataType == Paras[6].SESSION_TYPE && t.SessionKey == Paras[6].SESSION_KEY);
            if (sessionCountTwo == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[6].SESSION_TYPE + Paras[6].SESSION_KEY }));
            }

            R_Station_Action_Para moveFlag = Paras[7];
          
            bool moveToRight;
            if (moveFlag.VALUE.ToString().Equals("0"))
            {
                moveToRight = true;
            }
            else if(moveFlag.VALUE.ToString().Equals("1"))
            {
                moveToRight = false;
            }
            else
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[7].SESSION_TYPE + Paras[7].SESSION_KEY }));
            }

            try
            {
                LogicObject.Packing packOne = (LogicObject.Packing)sessionOne.Value; ;
                LogicObject.Packing packTwo = (LogicObject.Packing)sessionTwo.Value; ;
                string result = "";
                T_R_PACKING t_r_packing = new T_R_PACKING(Station.SFCDB, Station.DBType);
                T_R_SN_PACKING t_r_sn_packing = new T_R_SN_PACKING(Station.SFCDB, Station.DBType);
                T_R_SN t_r_sn = new T_R_SN(Station.SFCDB, Station.DBType);
                T_R_MOVE_LIST t_r_move_list = new T_R_MOVE_LIST(Station.SFCDB, Station.DBType);
                T_C_PACKING t_c_packing = new T_C_PACKING(Station.SFCDB, Station.DBType);
                LogicObject.Packing packOneObject = new LogicObject.Packing();
                LogicObject.Packing packTwoObject = new LogicObject.Packing();
                R_SN r_sn;
                Row_R_MOVE_LIST rowMoveList;
                Row_R_PACKING rowPacking;
                C_PACKING c_packing = new C_PACKING();               
                if (packOne.ClosedFlag == "0")
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180613090152", new string[] { packOne.PackNo }));
                }
                if (packTwo.ClosedFlag == "0")
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180613090152", new string[] { packTwo.PackNo }));
                }
                if (!packOne.PackType.Equals(packTwo.PackType))
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612114250", new string[] { packOne.PackNo, packTwo.PackNo }));
                }
                if (!packOne.Skuno.Equals(packTwo.Skuno))
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141141", new string[] { packOne.PackNo, packTwo.PackNo }));
                }
                if (!packOne.SkunoVer.Equals(packTwo.SkunoVer))
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141356", new string[] { packOne.PackNo, packTwo.PackNo }));
                }

                Newtonsoft.Json.Linq.JArray moveValueArray = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(sessionValue.Value.ToString());

                if (packOne.PackType == LogicObject.PackType.PALLET.ToString().ToUpper())
                {
                    for (int i = 0; i < moveValueArray.Count; i++)
                    {                                                
                        c_packing = t_c_packing.GetPackingBySkuAndType(packTwo.Skuno, LogicObject.PackType.CARTON.ToString().ToUpper(), Station.SFCDB);
                        if (c_packing.MAX_QTY ==1 && Station.BU.ToUpper().Equals("VERTIV"))
                        {
                            //更新棧板號
                            //VERTIV 當卡通包規為1時，調棧板顯示卡通內的SN,故更新信息另外處理
                            R_PACKING packingObjectTemp = t_r_packing.GetPackingObjectBySN(moveValueArray[i].ToString(), Station.SFCDB);
                            if (packingObjectTemp == null)
                            {
                                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180620114012", new string[] { moveValueArray[i].ToString()}));
                            }
                            if (!t_r_packing.CheckPackNoExistByParentPackID(packingObjectTemp.PACK_NO, packOne.PackID, Station.SFCDB))
                            {
                                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141600", new string[] { moveValueArray[i].ToString(), packOne.PackNo }));
                            }
                            if (t_r_packing.CheckPackNoExistByParentPackID(packingObjectTemp.PACK_NO, packTwo.PackID, Station.SFCDB))
                            {
                                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141824", new string[] { moveValueArray[i].ToString(), packTwo.PackNo }));
                            }
                            result = t_r_packing.UpdateParentPackIDByPackNo(packingObjectTemp.PACK_NO, packTwo.PackID,Station.LoginUser.EMP_NO, Station.SFCDB);
                            rowPacking = t_r_packing.GetRPackingByPackNo(Station.SFCDB, packingObjectTemp.PACK_NO);
                        }
                        else
                        {
                            if (!t_r_packing.CheckPackNoExistByParentPackID(moveValueArray[i].ToString(), packOne.PackID, Station.SFCDB))
                            {
                                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141600", new string[] { moveValueArray[i].ToString(), packOne.PackNo }));
                            }
                            if (t_r_packing.CheckPackNoExistByParentPackID(moveValueArray[i].ToString(), packTwo.PackID, Station.SFCDB))
                            {
                                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141824", new string[] { moveValueArray[i].ToString(), packTwo.PackNo }));
                            }
                            //更新棧板號
                            result = t_r_packing.UpdateParentPackIDByPackNo(moveValueArray[i].ToString(), packTwo.PackID, Station.LoginUser.EMP_NO,Station.SFCDB);
                            rowPacking = t_r_packing.GetRPackingByPackNo(Station.SFCDB, moveValueArray[i].ToString());
                        }
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "R_PACKING" }));
                        }
                        result = t_r_packing.UpdateQtyByID(packTwo.PackID,true,1, Station.LoginUser.EMP_NO, Station.SFCDB);
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "R_PACKING" }));
                        }
                        result = t_r_packing.UpdateQtyByID(packOne.PackID, false, 1, Station.LoginUser.EMP_NO, Station.SFCDB);
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "R_PACKING" }));
                        }
                        //寫入記錄
                        
                        rowMoveList = (Row_R_MOVE_LIST)t_r_move_list.NewRow();
                        rowMoveList.ID = t_r_move_list.GetNewID(Station.BU, Station.SFCDB);
                        rowMoveList.MOVE_ID = rowPacking.ID;
                        rowMoveList.FROM_LOCATION = packOne.PackID;
                        rowMoveList.TO_LOCATION = packTwo.PackID;
                        rowMoveList.PACK_TYPE = packOne.PackType;
                        rowMoveList.MOVE_EMP = Station.LoginUser.EMP_NO;
                        rowMoveList.MOVE_DATE = Station.GetDBDateTime();
                        result = Station.SFCDB.ExecSQL(rowMoveList.GetInsertString(Station.DBType));
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000021", new string[] { "R_MOVE_LIST" }));
                        }
                        
                    }
                }
                else if (packOne.PackType == LogicObject.PackType.CARTON.ToString().ToUpper())
                {
                    for (int i = 0; i < moveValueArray.Count; i++)
                    {
                        r_sn = new R_SN();
                        r_sn = t_r_sn.GetDetailBySN(moveValueArray[i].ToString(), Station.SFCDB);
                        if (!t_r_sn_packing.CheckSNExistByPackID(r_sn.ID, packOne.PackID, Station.SFCDB))
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141600", new string[] { moveValueArray[i].ToString(), packOne.PackNo }));
                        }
                        if (t_r_sn_packing.CheckSNExistByPackID(r_sn.ID, packTwo.PackID, Station.SFCDB))
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612141824", new string[] { moveValueArray[i].ToString(), packTwo.PackNo }));
                        }
                        //更新卡通號
                        result = t_r_sn_packing.UpdatePackIDBySnID(r_sn.ID, packTwo.PackID,Station.LoginUser.EMP_NO, Station.SFCDB);
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "R_SN_PACKING" }));
                        }

                        result = t_r_packing.UpdateQtyByID(packTwo.PackID, true, 1,Station.LoginUser.EMP_NO, Station.SFCDB);
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "R_PACKING" }));
                        }
                        result = t_r_packing.UpdateQtyByID(packOne.PackID, false, 1, Station.LoginUser.EMP_NO, Station.SFCDB);
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000025", new string[] { "R_PACKING" }));
                        }        

                        //寫入記錄
                        rowMoveList = (Row_R_MOVE_LIST)t_r_move_list.NewRow();
                        rowMoveList.ID = t_r_move_list.GetNewID(Station.BU, Station.SFCDB);
                        rowMoveList.MOVE_ID = r_sn.ID;
                        rowMoveList.FROM_LOCATION = packOne.PackID;
                        rowMoveList.TO_LOCATION = packTwo.PackID;
                        rowMoveList.PACK_TYPE = packOne.PackType;
                        rowMoveList.MOVE_EMP = Station.LoginUser.EMP_NO;
                        rowMoveList.MOVE_DATE = Station.GetDBDateTime();
                        result = Station.SFCDB.ExecSQL(rowMoveList.GetInsertString(Station.DBType));
                        if (Convert.ToInt32(result) == 0)
                        {
                            throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000021", new string[] { "R_MOVE_LIST" }));
                        }
                    }
                }
                else
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180607163531", new string[] { "Pack:" + packOne.PackType }));
                }
                //關閉卡通或棧板
                result = t_r_packing.UpdateCloseFlagByPackID(packOne.PackID, "1", Station.SFCDB);
                if (Convert.ToInt32(result) == 0)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612154506", new string[] { packOne.PackNo }));
                }
                //關閉卡通或棧板
                result = t_r_packing.UpdateCloseFlagByPackID(packTwo.PackID, "1", Station.SFCDB);
                if (Convert.ToInt32(result) == 0)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612154506", new string[] { packTwo.PackNo }));
                }

                packOneObject.DataLoad(packOne.PackNo,Station.BU, Station.SFCDB, Station.DBType);
                packTwoObject.DataLoad(packTwo.PackNo,Station.BU, Station.SFCDB, Station.DBType);
                sessionOne.Value = packOneObject;
                sessionTwo.Value = packTwoObject;
                if (moveToRight)
                {
                    sessionItemListOne.Value = packOneObject.PackList;
                    sessionItemListTwo.Value = packTwoObject.PackList;
                    sessionCountOne.Value = packOneObject.PackList.Count;
                    sessionCountTwo.Value = packTwoObject.PackList.Count;
                }
                else
                {
                    sessionItemListOne.Value = packTwoObject.PackList;
                    sessionItemListTwo.Value = packOneObject.PackList;
                    sessionCountOne.Value = packTwoObject.PackList.Count;
                    sessionCountTwo.Value = packOneObject.PackList.Count;
                }
                Station.StationMessages.Add(new StationMessage() {
                    Message = MESReturnMessage.GetMESReturnMessage("MSGCODE20180612142034", new string[] { packOne.PackNo.ToString(), moveValueArray.Count.ToString(), packTwo.PackNo.ToString() }),
                    State = StationMessageState.Pass });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打開卡通或棧板
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="Input"></param>
        /// <param name="Paras"></param>
        public static void OpenPackingAction(MESStation.BaseClass.MESStationBase Station, MESStation.BaseClass.MESStationInput Input, List<R_Station_Action_Para> Paras)
        {

            if (Paras.Count != 1)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000057", new string[] { }));
            }

            MESStationSession sessionPackObject = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);
            if (sessionPackObject == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }
            if (sessionPackObject.Value == null)
            {
                throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000052", new string[] { Paras[0].SESSION_TYPE + Paras[0].SESSION_KEY }));
            }
            try
            {
                string result = "";
                LogicObject.Packing packObject = (LogicObject.Packing)sessionPackObject.Value;               
                T_R_PACKING t_r_packing = new T_R_PACKING(Station.SFCDB, Station.DBType);
                result = t_r_packing.UpdateCloseFlagByPackID(packObject.PackID, "0", Station.SFCDB);
                if (Convert.ToInt32(result) == 0)
                {
                    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MSGCODE20180612154414", new string[] { packObject.PackNo })); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
