using MESDataObject;
using MESDataObject.Module;
using MESDBHelper;
using MESPubLab.MESStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MESDataObject.Common;
using MESStation.Interface.SAPRFC;
using SqlSugar;

namespace MESStation.Config
{
    public class WhsConfig : MesAPIBase
    {
        protected APIInfo FGetToList = new APIInfo()
        {
            FunctionName = "GetToListData",
            Description = "Get ToList",
            Parameters = new List<APIInputInfo>()
            {
            },
            Permissions = new List<MESPermission>() { }
        };

        protected APIInfo FGetToDetailDataByToNo = new APIInfo()
        {
            FunctionName = "GetToDetailDataByToNo",
            Description = "GetToDetailDataByToNo",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "ToNo", InputType = "string", DefaultValue = "" }
            },
            Permissions = new List<MESPermission>() { }
        };

        protected APIInfo FShipOutDoCqa = new APIInfo()
        {
            FunctionName = "ShipOutDoCqa",
            Description = "ShipOutDoCqa",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "DnNo", InputType = "string", DefaultValue = "" },
                new APIInputInfo() {InputName = "DnLine", InputType = "string", DefaultValue = "" }
            },
            Permissions = new List<MESPermission>() { }
        };

        protected APIInfo FGetGtDataByDnAndDnLine = new APIInfo()
        {
            FunctionName = "GetGtDataByDnAndDnLine",
            Description = "GetGtDataByDnAndDnLine",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "Dn", InputType = "string", DefaultValue = "" },
                new APIInputInfo() {InputName = "DnLine", InputType = "string", DefaultValue = "" }
            },
            Permissions = new List<MESPermission>() { }
        };
        

        protected APIInfo FGetDnDetailDataByDnNo = new APIInfo()
        {
            FunctionName = "GetDnDetailDataByDnNo",
            Description = "GetDnDetailDataByDnNo",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "DnNo", InputType = "string", DefaultValue = "" }
            },
            Permissions = new List<MESPermission>() { }
        };

        protected APIInfo FShipOutGtAll = new APIInfo()
        {
            FunctionName = "ShipOutGtAll",
            Description = "ShipOutGtAll",
            Parameters = new List<APIInputInfo>()
            {
                new APIInputInfo() {InputName = "DnNo", InputType = "string", DefaultValue = "" },
                new APIInputInfo() {InputName = "DnLine", InputType = "string", DefaultValue = "" },
                new APIInputInfo() {InputName = "Bu", InputType = "string", DefaultValue = "" }
            },
            Permissions = new List<MESPermission>() { }
        };

        public WhsConfig()
        {
            this.Apis.Add(FGetToList.FunctionName, FGetToList);
            this.Apis.Add(FGetToDetailDataByToNo.FunctionName, FGetToDetailDataByToNo);
            this.Apis.Add(FShipOutGtAll.FunctionName, FShipOutGtAll);
            this.Apis.Add(FShipOutDoCqa.FunctionName, FShipOutDoCqa);
        }

        public void GetToListData(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var res = oleDB.ORM
                    .Queryable<R_TO_HEAD, R_TO_DETAIL, R_DN_STATUS>((rth, rtd, rds) =>
                        rth.TO_NO == rtd.TO_NO && rtd.DN_NO == rds.DN_NO && rds.DN_FLAG == "0")
                    .OrderBy((rth) => rth.TO_CREATETIME, OrderByType.Desc)
                    .GroupBy(rth => new { rth.TO_NO, rth.PLAN_STARTIME, rth.PLAN_ENDTIME, rth.TO_CREATETIME})
                    .Select(rth => new { rth.TO_NO, rth.PLAN_STARTIME, rth.PLAN_ENDTIME, rth.TO_CREATETIME}).ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        public void GetToDetailDataByToNo(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            string ToNo = Data["ToNo"].ToString().Trim();
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var res = oleDB.ORM.Queryable<R_TO_DETAIL>().Where(x => x.TO_NO == ToNo)
                    .OrderBy(x => x.TO_ITEM_NO, OrderByType.Asc).ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        public void GetWaitShipOutDnDetailDataByDnNo(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            string DnNo = Data["DnNo"].ToString().Trim();
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var res = oleDB.ORM.Queryable<R_DN_STATUS>().Where(x => x.DN_NO == DnNo&& x.DN_FLAG=="0")
                    .OrderBy(x => x.DN_LINE, OrderByType.Asc).ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        public void GetWaitShipOutToDetailDataByToNo(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            string ToNo = Data["ToNo"].ToString().Trim();
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var res = oleDB.ORM.Queryable<R_TO_DETAIL,R_DN_STATUS>((rtd,rds)=>rtd.DN_NO==rds.DN_NO && rtd.TO_NO== ToNo && rds.DN_FLAG=="0").Select((rtd, rds) =>rtd)
                    .OrderBy(rtd => rtd.TO_ITEM_NO, OrderByType.Asc).ToList().Distinct().ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        public void GetDnDetailDataByDnNo(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            string DnNo = Data["DnNo"].ToString().Trim();
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var res = oleDB.ORM.Queryable<R_DN_STATUS>().Where(x => x.DN_NO == DnNo)
                    .OrderBy(x => x.DN_LINE, OrderByType.Asc).ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        public void GetGtDataByDnAndDnLine(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data,
            MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            string dn = Data["Dn"].ToString().Trim(),
                   dnLine = Data["DnLine"].ToString().Trim();
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var res = oleDB.ORM.Queryable<R_DN_STATUS, C_SHIPPING_ROUTE_DETAIL>((rds, csrd) => rds.GTTYPE == csrd.ROUTENAME && rds.DN_NO == dn && rds.DN_LINE == dnLine)
                    .OrderBy((rds, csrd) => csrd.SEQ, OrderByType.Asc)
                    .Select((rds, csrd) => new { csrd.ID, csrd.ROUTENAME, csrd.SEQ, csrd.ACTIONNAME, csrd.ACTIONTYPE, csrd.FROM_PLANT, csrd.TO_PLANT, csrd.FROM_STOCK, csrd.TO_STOCK, csrd.RFC_NAME, GTEVENT = rds.GTEVENT })
                    .ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        public void GetAllWaitGtData(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data, MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var res = oleDB.ORM.Queryable<R_DN_STATUS>()
                    .OrderBy(rds => rds.GT_FLAG, OrderByType.Asc).OrderBy(rds => rds.DN_FLAG, OrderByType.Asc).OrderBy(rds => rds.EDITTIME, OrderByType.Asc)
                    .ToList().Distinct().ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        void doGT(C_SHIPPING_ROUTE_DETAIL csrdDetail,R_DN_STATUS rDnStatus,string bu)
        {
            System.Threading.Thread.Sleep(20000);
            if (csrdDetail.RFC_NAME == "ZRFC_SFC_NSG_0011")
            {
                ZRFC_SFC_NSG_0011 ZRFC_SFC_NSG_0011 = new ZRFC_SFC_NSG_0011(bu);
                ZRFC_SFC_NSG_0011.SetValue("I_BKTXT", rDnStatus.DN_NO);
                ZRFC_SFC_NSG_0011.SetValue("I_MATNR", rDnStatus.SKUNO);
                ZRFC_SFC_NSG_0011.SetValue("I_ERFMG", rDnStatus.QTY.ToString());
                ZRFC_SFC_NSG_0011.SetValue("I_FROM", csrdDetail.FROM_STOCK);
                ZRFC_SFC_NSG_0011.SetValue("I_TO", csrdDetail.TO_STOCK);
                ZRFC_SFC_NSG_0011.SetValue("I_LMNGA", csrdDetail.TO_PLANT);
                //ZRFC_SFC_NSG_0011.CallRFC();
            }
            else if (csrdDetail.RFC_NAME == "ZRFC_NSG_SD_0003")
            {
                ZRFC_SFC_NSG_0003 ZRFC_SFC_NSG_0003 = new ZRFC_SFC_NSG_0003(bu);
                ZRFC_SFC_NSG_0003.SetValue("P_VBELN", rDnStatus.DN_NO);
                ZRFC_SFC_NSG_0003.SetValue("P_WADAT", System.DateTime.Now.ToString("yyyy-MM-dd"));
                //ZRFC_SFC_NSG_0003.CallRFC();
            }
        }

        public void ShipOutDoCqa(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data,
            MESStationReturn StationReturn)
        {
            OleExec oleDB = null;
            string dn = Data["Dn"].ToString().Trim(),
                   dnLine = Data["DnLine"].ToString().Trim();
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var resRds = oleDB.ORM.Queryable<R_DN_STATUS>().Where(x => x.DN_NO == dn && x.DN_LINE == dnLine && x.DN_FLAG == "1")
                    .ToList();
                if (resRds.Count == 0)
                {
                    StationReturn.Status = StationReturnStatusValue.Fail;
                    StationReturn.MessageCode = "MSGCODE20180804171505";
                    return;
                }
                resRds.FirstOrDefault().DN_FLAG = "2";
                resRds.FirstOrDefault().EDITTIME = System.DateTime.Now;
                oleDB.ORM.Updateable(resRds.FirstOrDefault()).WhereColumns(x => new { x.DN_NO, x.DN_LINE }).ExecuteCommand();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }

        public void ShipOutGtAll(Newtonsoft.Json.Linq.JObject requestValue, Newtonsoft.Json.Linq.JObject Data,
            MESStationReturn StationReturn)
        {
            OleExec oleDB = null;

            string dn = Data["Dn"].ToString().Trim(),
                   dnLine = Data["DnLine"].ToString().Trim(),
                   bu = Data["Bu"].ToString().Trim();
            try
            {
                oleDB = this.DBPools["SFCDB"].Borrow();
                var resCsrds = oleDB.ORM.Queryable<R_DN_STATUS, C_SHIPPING_ROUTE_DETAIL>((rds, csrd) => rds.GTTYPE == csrd.ROUTENAME && rds.DN_NO == dn && rds.DN_LINE == dnLine)
                    .OrderBy((rds, csrd) => csrd.SEQ, OrderByType.Asc)
                    .Select((rds, csrd) => csrd)
                    .ToList();

                var resRds = oleDB.ORM.Queryable<R_DN_STATUS>().Where(x => x.DN_NO == dn && x.DN_LINE == dnLine&&x.GT_FLAG=="0"&&x.DN_FLAG=="2")
                    .ToList();
                if (resRds.Count == 0)
                {
                    StationReturn.Status = StationReturnStatusValue.Fail;
                    StationReturn.MessageCode = "MSGCODE20180804171505";
                    return;
                }

                for (int i = 0; i < resCsrds.Count; i++)
                {
                    if (resCsrds[i].SEQ == resRds.FirstOrDefault().GTEVENT)
                    {
                        doGT(resCsrds[i], resRds.FirstOrDefault(), bu);
                        resRds.FirstOrDefault().GTEVENT = i == resCsrds.Count - 1 ? "END" : resCsrds[i + 1].SEQ;
                        resRds.FirstOrDefault().DN_FLAG = i == resCsrds.Count - 1 ? "3" : "2";
                        resRds.FirstOrDefault().GT_FLAG = i == resCsrds.Count - 1 ? "1" : "0";
                        resRds.FirstOrDefault().EDITTIME = System.DateTime.Now;
                        resRds.FirstOrDefault().GTDATE = System.DateTime.Now;
                        oleDB.ORM.Updateable(resRds.FirstOrDefault()).WhereColumns(x => new {x.DN_NO,x.DN_LINE }).ExecuteCommand();
                    }
                }
                var res = oleDB.ORM.Queryable<R_DN_STATUS, C_SHIPPING_ROUTE_DETAIL>((rds, csrd) => rds.GTTYPE == csrd.ROUTENAME && rds.DN_NO == dn && rds.DN_LINE == dnLine)
                    .OrderBy((rds, csrd) => csrd.SEQ, OrderByType.Asc)
                    .Select((rds, csrd) => new { csrd.ID, csrd.ROUTENAME, csrd.SEQ, csrd.ACTIONNAME, csrd.ACTIONTYPE, csrd.FROM_PLANT, csrd.TO_PLANT, csrd.FROM_STOCK, csrd.TO_STOCK, csrd.RFC_NAME, GTEVENT = rds.GTEVENT })
                    .ToList();
                StationReturn.Status = StationReturnStatusValue.Pass;
                StationReturn.MessageCode = "MES00000026";
                StationReturn.Data = res;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                this.DBPools["SFCDB"].Return(oleDB);
            }
        }
    }
}
