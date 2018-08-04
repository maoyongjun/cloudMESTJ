using MESStation.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDataObject.Module;
using MESDBHelper;
using MESDataObject;
using System.Data;

namespace MESStation.Management
{
    public class LockManager: MesAPIBase
    {
        protected APIInfo FLock = new APIInfo
        {
            FunctionName = "DoLock",
            Description = "Do lock by wo or lot_no or sn",
            Parameters = new List<APIInputInfo>() {
                new APIInputInfo() { InputName = "LockType" },
                new APIInputInfo() { InputName = "LockData" },
                new APIInputInfo() { InputName = "LockReason" },
            },
            Permissions = new List<MESPermission>()
        };

        protected APIInfo FUnlock = new APIInfo
        {
            FunctionName = "DoUnlock",
            Description = "Do unlock by id",
            Parameters = new List<APIInputInfo>() {
                new APIInputInfo(){ InputName = "ID"},
                new APIInputInfo(){ InputName = "UnlockReason"}
            },
            Permissions=new List<MESPermission>()
        };

        protected APIInfo FGetLockStation = new APIInfo
        {
            FunctionName = "GetLockStation",
            Description = "Get lock station",
            Parameters = new List<APIInputInfo>() {
                new APIInputInfo(){InputName = "LockType" },
                new APIInputInfo(){InputName = "LockData"}
            },
            Permissions = new List<MESPermission>()            
        };

        protected APIInfo FGetLockInfo = new APIInfo
        {
            FunctionName = "GetLockInfo",
            Description = "Get lock info by wo or sn or lot",
            Parameters = new List<APIInputInfo>() {
                new APIInputInfo(){ InputName = "Type"},
                new APIInputInfo(){ InputName = "Data"},
                new APIInputInfo(){ InputName = "Status"}
            },
            Permissions = new List<MESPermission>()
        };

        public LockManager()
        {
            this.Apis.Add(FLock.FunctionName,FLock);
            this.Apis.Add(FUnlock.FunctionName, FUnlock);
            this.Apis.Add(FGetLockStation.FunctionName,FGetLockStation);
            this.Apis.Add(FGetLockInfo.FunctionName, FGetLockInfo);
        }

        public void GetLockInfo(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JToken Data, MESStationReturn StationReturn)
        {
            string type = Data["Type"].ToString().ToUpper();
            string status = Data["Status"].ToString();
            List<string> snList = new List<string>();
            OleExec sfcdb = null;
            List<R_SN_LOCK> lockList = new List<R_SN_LOCK>();
            try
            {
                sfcdb = this.DBPools["SFCDB"].Borrow();
                sfcdb.ThrowSqlExeception = true;
                T_R_SN_LOCK t_r_sn_lock = new T_R_SN_LOCK(sfcdb, DBTYPE);
                if (status.ToUpper() == "ALL")
                {
                    status = "";
                }
                if (type == "BYWO")
                {
                    lockList = t_r_sn_lock.GetLockList("","", "", Data["Data"].ToString(), status, sfcdb);
                }
                else if (type == "BYLOT")
                {
                    lockList = t_r_sn_lock.GetLockList("",Data["Data"].ToString(), "", "", status, sfcdb);
                }
                else if (type == "BYSN")
                {
                    Newtonsoft.Json.Linq.JArray arraySN = (Newtonsoft.Json.Linq.JArray)Data["Data"];
                    for (int i = 0; i < arraySN.Count; i++)
                    {
                        lockList.AddRange(t_r_sn_lock.GetLockList("","", arraySN[i].ToString(), "", status, sfcdb));
                    }
                }
                else if (type == "BYID") {
                    Newtonsoft.Json.Linq.JArray arraySN = (Newtonsoft.Json.Linq.JArray)Data["Data"];
                    for (int i = 0; i < arraySN.Count; i++)
                    {
                        lockList.AddRange(t_r_sn_lock.GetLockList(arraySN[i].ToString(), "", "", "", status, sfcdb));
                    }
                }

                this.DBPools["SFCDB"].Return(sfcdb);
                StationReturn.Data = lockList;
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000001";
            }
            catch(Exception exception)
            {
                StationReturn.Status = StationReturnStatusValue.Fail;
                StationReturn.MessageCode = "MES00000037";
                StationReturn.MessagePara.Add(exception.Message);
                StationReturn.Data = exception.Message;
                if (sfcdb != null)
                {
                    this.DBPools["SFCDB"].Return(sfcdb);
                }
            }
        }

        public void GetLockStation(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JToken Data, MESStationReturn StationReturn)
        {
            string lockType = Data["LockType"].ToString().Trim();
            string lockData = Data["LockData"].ToString().Trim();
            DataTable routeTable = new DataTable();
            List<string> stationList = new List<string>();
            OleExec sfcdb = null;
            try
            {
                sfcdb = this.DBPools["SFCDB"].Borrow();
                T_C_ROUTE_DETAIL t_c_route_detail = new T_C_ROUTE_DETAIL(sfcdb, DBTYPE);
                if (lockType == "LockByWo")
                {
                    T_R_WO_BASE t_r_wo_base = new T_R_WO_BASE(sfcdb, DBTYPE);
                    R_WO_BASE r_wo_base = t_r_wo_base.GetWo(lockData, sfcdb).GetDataObject();
                    stationList = t_c_route_detail.GetByRouteIdOrderBySEQASC(r_wo_base.ROUTE_ID, sfcdb).Select(route => route.STATION_NAME).ToList();
                }
                else if (lockType == "LockByLot")
                {
                    T_R_LOT_STATUS t_r_lot_status = new T_R_LOT_STATUS(sfcdb, DBTYPE);
                    Row_R_LOT_STATUS rowLotStatus = t_r_lot_status.GetByLotNo(lockData, sfcdb);
                    if (rowLotStatus.ID == null)
                    {
                        throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000161", new string[] { })); 
                    }
                    R_LOT_STATUS r_lot_status = rowLotStatus.GetDataObject();
                    T_C_SKU t_c_sku = new T_C_SKU(sfcdb, DBTYPE);
                    C_SKU c_sku = t_c_sku.GetSku(r_lot_status.SKUNO, sfcdb, DBTYPE).GetDataObject();
                    T_R_SKU_ROUTE t_r_sku_route = new T_R_SKU_ROUTE(sfcdb, DBTYPE);
                    List<R_SKU_ROUTE> r_sku_route_list = t_r_sku_route.GetMappingBySkuId(c_sku.ID, sfcdb);
                    if (r_sku_route_list.Count > 0)
                    {                        
                        //t_c_route_detail.GetByRouteIdOrderBySEQASC(r_sku_route_list[0].ROUTE_ID, sfcdb);
                        stationList = t_c_route_detail.GetByRouteIdOrderBySEQASC(r_sku_route_list[0].ROUTE_ID, sfcdb).Select(route => route.STATION_NAME).ToList();
                    }
                    else
                    {
                        throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000179", new string[] {  }));
                    }

                }
                else
                {
                    routeTable = t_c_route_detail.GetALLStation(sfcdb);
                    foreach (DataRow row in routeTable.Rows)
                    {
                        stationList.Add(row["station_name"].ToString());
                    }
                    stationList.Sort();
                }

                this.DBPools["SFCDB"].Return(sfcdb);
                StationReturn.Data = stationList;
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000001";
            }
            catch (Exception exception)
            {
                StationReturn.Status = StationReturnStatusValue.Fail;
                StationReturn.MessageCode = "MES00000037";
                StationReturn.MessagePara.Add(exception.Message);
                StationReturn.Data = exception.Message;
                if (sfcdb != null)
                {
                    this.DBPools["SFCDB"].Return(sfcdb);
                }
            }
        }

        public void DoLock(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JToken Data, MESStationReturn StationReturn)
        {
            string lockType = Data["LockType"].ToString().Trim();            
            string lockReason = Data["LockReason"].ToString().Trim();
            string lockStation = Data["LockStation"].ToString().Trim();
            OleExec sfcdb = null;
            T_R_SN_LOCK t_r_sn_lock = null;
            Row_R_SN_LOCK rowSNLock = null;
            try
            { 
                sfcdb = this.DBPools["SFCDB"].Borrow();
                t_r_sn_lock = new T_R_SN_LOCK(sfcdb, DBTYPE);
                if (lockType == "LockByWo")
                {
                    if (t_r_sn_lock.isUnLock("","", Data["LockData"].ToString().Trim(),sfcdb))
                    {
                        StationReturn.Status = StationReturnStatusValue.Fail;
                        StationReturn.MessageCode = "MSGCODE20180730134109";
                        StationReturn.MessagePara.Add(Data["LockData"].ToString().Trim());
                        StationReturn.Data = "";
                        return;
                    }
                    rowSNLock = (Row_R_SN_LOCK)t_r_sn_lock.NewRow();
                    rowSNLock.ID = t_r_sn_lock.GetNewID(this.BU, sfcdb);
                    rowSNLock.WORKORDERNO = Data["LockData"].ToString().Trim();
                    rowSNLock.TYPE = "WO";
                    rowSNLock.LOCK_STATION = lockStation;
                    rowSNLock.LOCK_REASON = lockReason;
                    rowSNLock.LOCK_STATUS = "1";
                    rowSNLock.LOCK_EMP = this.LoginUser.EMP_NO;
                    rowSNLock.LOCK_TIME = GetDBDateTime();
                    sfcdb.ThrowSqlExeception = true;
                    sfcdb.ExecSQL(rowSNLock.GetInsertString(DBTYPE));
                    StationReturn.Status = StationReturnStatusValue.Pass;
                    StationReturn.MessageCode = "MES00000001";
                    StationReturn.Data = "";
                }
                else if (lockType == "LockByLot")
                {
                    if (t_r_sn_lock.isUnLock( Data["LockData"].ToString().Trim(), "", "", sfcdb))
                    {
                        StationReturn.Status = StationReturnStatusValue.Fail;
                        StationReturn.MessageCode = "MSGCODE20180730134109";
                        StationReturn.MessagePara.Add(Data["LockData"].ToString().Trim());
                        StationReturn.Data = "";
                        return;
                    }
                    rowSNLock = (Row_R_SN_LOCK)t_r_sn_lock.NewRow();
                    rowSNLock.ID = t_r_sn_lock.GetNewID(this.BU, sfcdb);
                    rowSNLock.LOCK_LOT = Data["LockData"].ToString().Trim();
                    rowSNLock.TYPE = "LOT";
                    rowSNLock.LOCK_STATION = lockStation;
                    rowSNLock.LOCK_REASON = lockReason;
                    rowSNLock.LOCK_STATUS = "1";
                    rowSNLock.LOCK_EMP = this.LoginUser.EMP_NO;
                    rowSNLock.LOCK_TIME = GetDBDateTime();
                    sfcdb.ThrowSqlExeception = true;
                    sfcdb.ExecSQL(rowSNLock.GetInsertString(DBTYPE));
                    StationReturn.Status = StationReturnStatusValue.Pass;
                    StationReturn.MessageCode = "MES00000001";
                    StationReturn.Data = "";
                }
                else if (lockType == "LockBySn")
                {
                    Newtonsoft.Json.Linq.JArray arraySN = (Newtonsoft.Json.Linq.JArray)Data["LockData"];
                    for (int i = 0; i < arraySN.Count; i++)
                    {
                        if (!t_r_sn_lock.isUnLock(arraySN[i].ToString(), "", "", sfcdb))
                        {
                            rowSNLock = (Row_R_SN_LOCK)t_r_sn_lock.NewRow();
                            rowSNLock.ID = t_r_sn_lock.GetNewID(this.BU, sfcdb);
                            rowSNLock.SN = arraySN[i].ToString();
                            rowSNLock.TYPE = "SN";
                            rowSNLock.LOCK_STATION = lockStation;
                            rowSNLock.LOCK_REASON = lockReason;
                            rowSNLock.LOCK_STATUS = "1";
                            rowSNLock.LOCK_EMP = this.LoginUser.EMP_NO;
                            rowSNLock.LOCK_TIME = GetDBDateTime();
                            sfcdb.ThrowSqlExeception = true;
                            sfcdb.ExecSQL(rowSNLock.GetInsertString(DBTYPE));
                        }                       
                    } 
                    StationReturn.Status = StationReturnStatusValue.Pass;
                    StationReturn.MessageCode = "MES00000001";
                    StationReturn.Data = "";
                }
                else
                {
                    throw new Exception(MESReturnMessage.GetMESReturnMessage("MSGCODE20180607163531", new string[] { "LockType" }));
                }
                this.DBPools["SFCDB"].Return(sfcdb);
            }
            catch (Exception exception)
            {       
                StationReturn.Status = StationReturnStatusValue.Fail;
                StationReturn.MessageCode = "MES00000037";
                StationReturn.MessagePara.Add(exception.Message);
                StationReturn.Data = exception.Message;
                if (sfcdb != null)
                {
                    this.DBPools["SFCDB"].Return(sfcdb);
                }
            }
        }

        public void DoUnlock(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JToken Data, MESStationReturn StationReturn)
        {
            Newtonsoft.Json.Linq.JArray arraySN = (Newtonsoft.Json.Linq.JArray)Data["ID"];
            string unlockReason = Data["UnlockReason"].ToString();
            OleExec sfcdb = null;
            try
            {
                sfcdb = this.DBPools["SFCDB"].Borrow();
                sfcdb.ThrowSqlExeception = true;
                T_R_SN_LOCK t_r_sn_lock = new T_R_SN_LOCK(sfcdb, DBTYPE);

                for (int i = 0; i < arraySN.Count; i++)
                {
                    Row_R_SN_LOCK rowLock =(Row_R_SN_LOCK)t_r_sn_lock.GetObjByID(arraySN[i].ToString(), sfcdb);
                    if (rowLock.LOCK_STATUS == "1")
                    {
                        rowLock.LOCK_STATUS = "0";
                        rowLock.UNLOCK_REASON = unlockReason;
                        rowLock.UNLOCK_EMP = this.LoginUser.EMP_NO;
                        rowLock.UNLOCK_TIME = GetDBDateTime();
                        sfcdb.ExecSQL(rowLock.GetUpdateString(DBTYPE));
                    }
                }

                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000001";
                StationReturn.Data = "";
            }
            catch (Exception exception)
            {
                StationReturn.Status = StationReturnStatusValue.Fail;
                StationReturn.MessageCode = "MES00000037";
                StationReturn.MessagePara.Add(exception.Message);
                StationReturn.Data = exception.Message;
                if (sfcdb != null)
                {
                    this.DBPools["SFCDB"].Return(sfcdb);
                }
            }
        }
    }
}
