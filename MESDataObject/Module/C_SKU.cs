using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MESDBHelper;
using System.Data.OleDb;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace MESDataObject.Module
{
    public class T_C_SKU : DataObjectTable
    {
        #region Constructors
        public T_C_SKU(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_C_SKU(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_C_SKU);
            TableName = "C_SKU".ToUpper().Trim();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }
        #endregion

        #region 業務方法

        /// <summary>
        /// 根據數據表的內容構建一個機種類實例
        /// Construct an instance of type C_SKU by using a row in database.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public SkuObject ConstructSku(DataRow dr, OleExec DB)
        {
            string sql = string.Empty;
            SkuObject sku = new SkuObject();
            //C_SKU _SkuBase = new C_SKU();
            Row_C_SKU row_sku = (Row_C_SKU)NewRow();
            //row_sku.loadData(dr);
            //sku = row_sku.GetDataObject();
            //_SkuBase = SessionManager<C_SKU>.ConstructObject(dr);
            row_sku.loadData(dr);
            sku.SkuBase = row_sku.GetDataObject();

            //T_C_SERIES table = new T_C_SERIES(DB, DBType);
            //sql = "SELECT ID,ID AS SERIES_ID,CUSTOMER_ID,SERIES_NAME,DESCRIPTION,EDIT_TIME,EDIT_EMP FROM C_SERIES WHERE ID=:ID AND ROWNUM=1";
            C_SERIES _SkuSeries = DB.ORM.Queryable<C_SERIES>().Where(series => series.ID == dr["C_SERIES_ID"].ToString()).First();
            //OleDbParameter[] parameters = new OleDbParameter[] { new OleDbParameter("ID", dr["C_SERIES_ID"].ToString()) };
            //C_SERIES _SkuSeries = new C_SERIES();
           // Row_C_SERIES row_series = (Row_C_SERIES)table.NewRow();
            //row_series.loadData(DB.ExecSelect(sql,parameters).Tables[0].Rows[0]);
            //DataTable dt = DB.ExecSelect(sql, parameters).Tables[0];
            //if (dt.Rows.Count > 0)
            //{
                //row_series.loadData(dt.Rows[0]);
                //_SkuSeries = row_series.GetDataObject();
            //}
            //sku.SERIES = series;
            sku.SkuSeries = _SkuSeries;

            return sku;
        }

        public SkuObject ConstructSku(C_SKU sku, OleExec DB)
        {
            SkuObject so = new SkuObject();
            so.SkuBase = sku;

            C_SERIES SkuSeries= DB.ORM.Queryable<C_SERIES>().Where(series => series.ID == sku.C_SERIES_ID).First();
            so.SkuSeries = SkuSeries;
            return so;


        }

        /// <summary>
        /// 獲取所有機種信息
        /// Get all c_sku .
        /// </summary>
        /// <param name="DB"></param>
        /// <returns></returns>
        public List<C_SKU> GetAllCSku(OleExec DB)
        {
        //    List<C_SKU> SkuList = new List<C_SKU>();
        //    string sql = "SELECT * FROM C_SKU WHERE SKUNO IS NOT NULL ORDER BY EDIT_TIME DESC";
            //DataTable dt = null;

            //dt= DB.ORM.Queryable<C_SKU>().Where(t=>t.SKUNO!=null).OrderBy(t => t.EDIT_TIME, SqlSugar.OrderByType.Desc).ToDataTable();
            //SqlSugar.ISugarQueryable<C_SKU> query = DB.ORM.Queryable<C_SKU>("SKUNO").Where(t => t.SKUNO != null).OrderBy(t => t.EDIT_TIME, SqlSugar.OrderByType.Desc);
            return DB.ORM.Queryable<C_SKU>().Where(t => t.SKUNO != null).OrderBy(t => t.EDIT_TIME, SqlSugar.OrderByType.Desc).ToList();
            //return query.ToList();
            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    //dt = DB.ExecSelect(sql).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            Row_C_SKU rowCSku = (Row_C_SKU)this.NewRow();
            //            rowCSku.loadData(dr);
            //            SkuList.Add(rowCSku.GetDataObject());
            //        }
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
            //return SkuList;
        }

        /// <summary>
        /// 獲取所有機種信息
        /// Get all sku infomations.
        /// </summary>
        /// <param name="DB"></param>
        /// <returns></returns>
        public List<SkuObject> GetAllSku(OleExec DB)
        {
            //string sql = "SELECT * FROM (SELECT * FROM C_SKU ORDER BY EDIT_TIME DESC) WHERE ROWNUM<200";
            //DataTable dt = null;

            return DB.ORM.Queryable<C_SKU,C_SERIES>((sku,series)=>sku.C_SERIES_ID==series.ID)
                    .Select((sku, series) => new SkuObject { SkuBase = sku, SkuSeries = series })
                    .OrderBy(so=>so.SkuBase.EDIT_TIME,SqlSugar.OrderByType.Desc)
                    .Take(200).ToList();
           
            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    dt = DB.ExecSelect(sql).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            SkuList.Add(ConstructSku(dr, DB));
            //        }
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
        }

        public List<string> GetStationBySku(OleExec DB,string Skuno)
        {
            List<string> StationList = new List<string>();
            //edit lc //distinct
            string sql = $@"SELECT distinct C.STATION_NAME,c.seq_no 
                              FROM c_sku a, r_sku_route b, c_route_detail c
                             WHERE A.ID = B.SKU_ID AND B.ROUTE_ID = C.ROUTE_ID AND A.SKUNO = '{Skuno}'
                          ORDER BY C.SEQ_NO";
            DataTable dt = null;

            List<string> list = DB.ORM.Queryable<C_SKU, R_SKU_ROUTE, C_ROUTE_DETAIL>(
                (sku, sku_route, route_detail) => new object[] { sku.ID == sku_route.ID && sku_route.ID == route_detail.ID })
                .Where(sku => sku.SKUNO == Skuno)
                .GroupBy((sku,sku_route,route_detail)=>route_detail.STATION_NAME)
                .Select((sku,sku_route,route_detail)=> route_detail.STATION_NAME)
                .ToList();
            return list;
            
                

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    dt = DB.ExecSelect(sql).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            StationList.Add(dr[0].ToString());
            //        }
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
            //return StationList;
        }
        public string GetModelTypeBySku(OleExec DB, string Skuno)
        {
            string strModelType = "";
            string sql = $@"Select value  From c_sku_ex   Where ID='{Skuno}'";
            DataTable dt = null;
            if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            {
                dt = DB.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    strModelType = dt.Rows[0][0].ToString();
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            
            return strModelType;
        }

        /// <summary>
        /// 根據 ID 獲取 C_SKU 實例
        /// Get an instance of type C_SKU according to ID
        /// </summary>
        /// <param name="SkuID"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public SkuObject GetSkuByID(string SkuID, OleExec DB)
        {
            //SkuObject NowSku = new SkuObject();
            //string sql = string.Empty;

            
            return DB.ORM.Queryable<C_SKU, C_SERIES>((t1, t2) => new object[] { t1.C_SERIES_ID == t2.ID,t1.BU==t2.CUSTOMER_ID })
                .Where((t1, t2) => t1.ID == SkuID && t1.VERSION=="")
                .Select((t1, t2) => new SkuObject { SkuBase = t1, SkuSeries = t2 }).ToList().First();


            //DataRow dr =DB.ORM.Queryable<C_SKU>().Where(sku => sku.ID == SkuID).ToDataTable().Rows[0];

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    //sql = "SELECT * FROM C_SKU WHERE ID='" + SkuID + "'";
            //    //DataRow dr = DB.ExecSelect(sql).Tables[0].Rows[0];
            //    NowSku = ConstructSku(DB.ORM.Queryable<C_SKU>().Where(sku => sku.ID == SkuID).ToDataTable().Rows[0], DB);
            //    return NowSku;
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
        }

        //add by LLF 2017-12-08 begin
        /// <summary>
        /// 檢查單個料號是否存在
        /// </summary>
        /// <param name="Skuno"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public bool CheckSku(string Skuno, OleExec DB)
        {
            //bool CheckSku = false;
            //string sql = string.Empty;

            //try
            //{
            //    sql = $@"SELECT * FROM C_SKU WHERE SKUNO='{Skuno}'";
            //    DataTable dt = DB.ExecSelect(sql).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        CheckSku = true;
            //    }
            //}
            //catch
            //{
            //    CheckSku = false;
            //}
            return DB.ORM.Queryable<C_SKU>().Where(t => t.SKUNO == Skuno).ToList().Count > 0;
            //return CheckSku;
        }
        /// <summary>
        /// 獲取料號
        /// </summary>
        /// <param name="Table_Name"></param>
        /// <param name="DB"></param>
        /// <param name="DBType"></param>
        /// <returns></returns>
        public Row_C_SKU GetSku(string SKUNO, OleExec DB, DB_TYPE_ENUM DBType)
        {
            string strSql = $@"select * from C_SKU where skuno='{SKUNO}' ";
            DataSet res = DB.ExecSelect(strSql);
            if (res.Tables[0].Rows.Count > 0)
            {
                Row_C_SKU ret = (Row_C_SKU)NewRow();
                ret.loadData(res.Tables[0].Rows[0]);
                return ret;
            }
            else
            {
                return null;
            }
        }
        //end

        /// <summary>
        /// 根據機種名獲取所有模糊查詢得到的機種信息
        /// Get an C_SKU instance according to SKU name in the way of fuzzy query.
        /// </summary>
        /// <param name="SkuNo"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public List<SkuObject> GetSkuByName(string SkuNo, OleExec DB)
        {
            //List<SkuObject> SkuList = new List<SkuObject>();
            //string sql = $@"SELECT * FROM 
            //                    (SELECT * FROM C_SKU WHERE INSTR(SKUNO,:SKUNO)>0 ORDER BY EDIT_TIME DESC) 
            //                WHERE ROWNUM<=20";
            //OleDbParameter[] parameters = new OleDbParameter[] { new OleDbParameter("SKUNO", SkuNo) };
            //DataTable dt = null;

            return DB.ORM.Queryable<C_SKU, C_SERIES>((sku, series) => new object[] { sku.C_SERIES_ID == series.ID }).Where((sku, series) => sku.SKUNO == SkuNo)
                .Select((sku, series) => new SkuObject { SkuBase = sku, SkuSeries = series }).ToList();

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    dt = DB.ExecSelect(sql, parameters).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            SkuList.Add(ConstructSku(dr, DB));
            //        }
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}

            //return SkuList;
        }

        /// <summary>
        /// 根據機種、版本獲取機種信息
        /// </summary>
        /// <param name="SkuNo"></param>
        /// <param name="version"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public SkuObject GetSkuByNameAndVersion(string SkuNo, string version, OleExec DB)
        {
            //SkuObject sku = null;
            //DataTable dt = null;
            //string sql = string.Empty;

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    sql = $@"SELECT * FROM C_SKU WHERE SKUNO='{SkuNo}' AND VERSION='{version}' AND ROWNUM=1";
            //    dt = DB.ExecSelect(sql).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        sku = ConstructSku(dt.Rows[0], DB);
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
            //return sku;

            return DB.ORM.Queryable<C_SKU, C_SERIES>((sku, series) => new object[] { sku.C_SERIES_ID == series.ID })
                .Where((sku, series) => sku.SKUNO == SkuNo && sku.VERSION == version)
                .Select((sku, series) => new SkuObject { SkuBase = sku, SkuSeries = series })
                .First();

        }

        public SkuObject GetSkuBySkuno(string SkuNo, OleExec DB)
        {
            //SkuObject sku = null;
            //DataTable dt = null;
            //string sql = string.Empty;

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    sql = $@"SELECT * FROM C_SKU WHERE SKUNO='{SkuNo}' ";
            //    dt = DB.ExecSelect(sql).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        sku = ConstructSku(dt.Rows[0], DB);
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
            //return sku;
            var skuObject= DB.ORM.Queryable<C_SKU, C_SERIES>((sku, series) =>  sku.C_SERIES_ID == series.ID )
                .Where((sku, series) => sku.SKUNO == SkuNo)
                .Select((sku, series) => new { SkuBase = sku, SkuSeries = series })
                .First();

            return new SkuObject { SkuBase = skuObject.SkuBase, SkuSeries = skuObject.SkuSeries };
        }

        public SkuObject GetSkuBySn(string Sn, OleExec DB)
        {
            //SkuObject sku = null;
            //DataTable dt = null;
            //string sql = string.Empty;

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    sql = $@" SELECT * FROM C_SKU WHERE SKUNO IN (SELECT SKUNO FROM R_SN WHERE SN='{Sn}' ) ";
            //    dt = DB.ExecSelect(sql).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //sku = ConstructSku(dt.Rows[0], DB);
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
            try
            {

                return DB.ORM.Queryable<C_SKU, C_SERIES, R_SN, R_WO_BASE>(
                        (sku, series, sn, wo) =>
                            sku.C_SERIES_ID == series.ID &&
                            sn.WORKORDERNO == wo.WORKORDERNO &&
                            wo.SKUNO == sku.SKUNO &&
                            wo.SKU_VER == sku.VERSION)
                         .Where((sku, series, sn, wo) => sn.SN == Sn)
                         .Select((sku, series) => new SkuObject { SkuBase = sku, SkuSeries = series })
                         .ToList().First();
               


                //List<SkuObject> skus= DB.ORM
                //    .Queryable<C_SKU, C_SERIES, R_SN,R_WO_BASE>(
                //        (_sku, _series, _sn,_wo) => 
                //            _sku.C_SERIES_ID == _series.ID && 
                //            _sn.WORKORDERNO==_wo.WORKORDERNO && 
                //            _wo.SKUNO==_sku.SKUNO && 
                //            _wo.SKU_VER==_sku.VERSION)
                //    .Where((_sku, _series, _sn) => _sn.SN == Sn)
                //    .Select((_sku, _series) => new SkuObject { SkuBase = _sku, SkuSeries = _series }).ToList();
               
            }
            catch (Exception e)
            {
                return null;
            }

            //SkuObject so= DB.ORM.Queryable<C_SKU, C_SERIES, R_SN>((_sku, _series, _sn) => new object[] { _sku.C_SERIES_ID == _series.ID && _sku.SKUNO == _sn.SKUNO })
            //    .Where((_sku, _series, _sn) => _sn.SN == Sn).Select((_sku, _series, SN) => new SkuObject { SkuBase = _sku, SkuSeries = _series }).First();

 

            //C_SKU _sku = DB.ORM.Queryable<C_SKU, R_SN>((Sku, sn) => new object[] { Sku.SKUNO == sn.SKUNO }).Where((Sku, sn) => sn.SN == Sn).Select((Sku, SN) => Sku).First();

            //return ConstructSku(_sku, DB);


            //DB.ORM.Queryable<C_SKU>().In<string>(t => t.SKUNO, DB.ORM.Queryable<R_SN>().Where(t=>t.SN==Sn).Select(t=>t.SKUNO).ToList().ToArray().ToList());
            //return sku;


        }

        /// <summary>
        /// 增、刪、改 機種信息
        /// Update the data in the table through a C_SKU object
        /// </summary>
        /// <param name="BU"></param>
        /// <param name="Sku"></param>
        /// <param name="Operation"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public string UpdateSku(string BU, C_SKU Sku, string Operation, DateTime EditTime,out StringBuilder SkuId, OleExec DB)
        {
            string result = string.Empty;
            bool changed = false;
            int i = 0;
            SkuId = new StringBuilder();
            //string sql = string.Empty;
            //string format = "yyyy-MM-dd HH:mm:ss";
            //Row_C_SKU row = (Row_C_SKU)NewRow();
            //SkuId = new StringBuilder();

            //if (Sku.ID != null && Sku.ID!="")
            //{
            //    row = (Row_C_SKU)GetObjByID(Sku.ID, DB);
            //    row.ID = Sku.ID;
            //}
            //row.BU = Sku.BU;
            //row.SKUNO = Sku.SKUNO;
            //row.VERSION = Sku.VERSION;
            //row.SKU_NAME = Sku.SKU_NAME;
            //row.SKU_TYPE = Sku.SKU_TYPE;
            //row.C_SERIES_ID = Sku.C_SERIES_ID;
            //row.CUST_PARTNO = Sku.CUST_PARTNO;
            //row.CUST_SKU_CODE = Sku.CUST_SKU_CODE;
            //row.SN_RULE = Sku.SN_RULE;
            //row.PANEL_RULE = Sku.PANEL_RULE;
            //row.DESCRIPTION = Sku.DESCRIPTION;
            //row.EDIT_EMP = Sku.EDIT_EMP;


            switch (Operation.ToUpper())
            {
                case "ADD":
                    ////插入之前先檢查是否已經存在
                    //if (SkuIsExist(Sku.SKUNO, Sku.VERSION, DB))
                    //{
                    //    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000151", new string[] { Sku.SKUNO, Sku.VERSION }));
                    //}
                    //row.ID = GetNewID(BU, DB);
                    //row.EDIT_TIME = EditTime;
                    //sql = row.GetInsertString(DBType);

                    Sku.ID = GetNewID(BU, DB);
                    Sku.EDIT_TIME = EditTime;

                    i = DB.ORM.Insertable<C_SKU>(Sku).ExecuteCommand();
                    //i = DB.ORM.Insertable<C_SKU>(Sku).ExecuteReturnIdentity();
                    if(i>0)
                    {
                        
                        SkuId.Append(Sku.ID);
                    }
                    result = i.ToString();
                    break;
                case "UPDATE":
                    //SkuObject NowSku = GetSkuByID(Sku.ID, DB);
                    //string Current = NowSku.LastEditTime.ToString(format);
                    //string Before = Sku.EDIT_TIME.ToString(format).Replace('T', ' ');
                    //// 模擬樂觀鎖機制，如果出現贓讀情況，重新從數據庫加載實例
                    //if (Before.Equals("") && !Current.Equals(Before))
                    //{
                    //    throw new MESReturnMessage(MESReturnMessage.GetMESReturnMessage("MES00000152", new string[] { }));
                    //}
                    //row.EDIT_TIME = EditTime;
                    //sql = row.GetUpdateString(DBType);

                    i = DB.ORM.Updateable<C_SKU>(Sku).Where(t=>t.ID==Sku.ID).ExecuteCommand();
                    
                    if(i>0)
                    {
                        SkuId.Append(Sku.ID);
                    }
                    result = i.ToString();
                    break;
                case "DELETE":
                    //sql = row.GetDeleteString(DBType);
                    //result = DB.ORM.Deleteable<C_SKU>(Sku).ExecuteCommandHasChange().ToString();

                    //result =  DB.ORM.Deleteable<C_SKU>().Where(t => t.ID == Sku.ID).ExecuteCommandHasChange().ToString();
                    //changed = DB.ORM.Updateable<C_SKU>().Where(t => t.ID == Sku.ID).ExecuteCommandHasChange();
                    i = DB.ORM.Updateable<C_SKU>().Where(t => t.ID == Sku.ID).ExecuteCommand();
                    if (i>0)
                    {
                        SkuId.Append(Sku.ID);
                    }
                    result = i.ToString();
                    break;
                default:
                    break;
            }

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    DB.BeginTrain();
            //    result = DB.ExecSQL(sql);
            //    SkuId.Append(row.ID);
            //    DB.CommitTrain();
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}

            return result;
        }

        /// <summary>
        /// 判斷機種是否已經存在，不Check vesion
        /// Judge the SKU is exist or not by comparing the SKU name and version.
        /// </summary>
        /// <param name="SkuName"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public bool SkuIsExist(string SkuName, OleExec DB)
        {
            return DB.ORM.Queryable<C_SKU>().Where(t => t.SKU_NAME == SkuName).ToList().Count > 0;
            //string sql = string.Empty;
            //DataTable dt = new DataTable();

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    if (string.IsNullOrEmpty(SkuName))
            //    {
            //        return false;
            //    }

            //    sql = "SELECT * FROM C_SKU WHERE SKUNO=:SKUNO ";
            //    OleDbParameter[] parameters = new OleDbParameter[] {
            //    new OleDbParameter("SKUNO",SkuName)
            //    };
            //    dt = DB.ExecSelect(sql, parameters).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
        }

        /// <summary>
        /// 判斷機種是否已經存在，機種名和版本相同認定為機種已經存在
        /// Judge the SKU is exist or not by comparing the SKU name and version.
        /// </summary>
        /// <param name="SkuName"></param>
        /// <param name="Version"></param>
        /// <param name="DB"></param>
        /// <returns></returns>
        public bool SkuIsExist(string SkuName, string Version, OleExec DB)
        {
            //string sql = string.Empty;
            //DataTable dt = new DataTable();

            return DB.ORM.Queryable<C_SKU>().Where(t => t.SKU_NAME == SkuName && t.VERSION == Version).ToList().Count > 0;

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    if (string.IsNullOrEmpty(SkuName) || string.IsNullOrEmpty(Version))
            //    {
            //        return false;
            //    }

            //    sql = "SELECT * FROM C_SKU WHERE SKUNO=:SKUNO AND VERSION=:VERSION";
            //    OleDbParameter[] parameters = new OleDbParameter[] {
            //    new OleDbParameter("SKUNO",SkuName),
            //    new OleDbParameter("VERSION",Version)
            //};
            //    dt = DB.ExecSelect(sql, parameters).Tables[0];
            //    if (dt.Rows.Count > 0)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
        }

        public DataTable GetALLSkuno(OleExec db)
        {
            //List<string> skulist = new List<string>();
            //DataTable dt = new DataTable();
            //string sql = $@"select * from c_sku order by skuno";
            //dt = db.ExecSelect(sql).Tables[0];
            //return dt;
            return db.ORM.Queryable<C_SKU>().OrderBy(t => t.SKUNO, SqlSugar.OrderByType.Asc).ToDataTable();
        }

        public List<string> GetAllSkunoList(OleExec DB)
        {
            //List<string> SkuList = new List<string>();
            //DataTable dt = null;

            //if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            //{
            //    dt = GetALLSkuno(DB);
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            SkuList.Add(dr["SKUNO"].ToString());
            //        }
            //    }
            //}
            //else
            //{
            //    string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
            //    throw new MESReturnMessage(errMsg);
            //}
            //return SkuList;
            return DB.ORM.Queryable<C_SKU>().Select(t => t.SKUNO ).ToList();
        }
        #endregion


    }
    public class Row_C_SKU : DataObjectBase
    {
        #region 數據庫行級映射機種類
        public Row_C_SKU(DataObjectInfo info) : base(info)
        {

        }
        public C_SKU GetDataObject()
        {
            C_SKU DataObject = new C_SKU();
            DataObject.ID = this.ID;
            DataObject.BU = this.BU;
            DataObject.SKUNO = this.SKUNO;
            DataObject.VERSION = this.VERSION;
            DataObject.SKU_NAME = this.SKU_NAME;
            DataObject.C_SERIES_ID = this.C_SERIES_ID;
            DataObject.CUST_PARTNO = this.CUST_PARTNO;
            DataObject.CUST_SKU_CODE = this.CUST_SKU_CODE;
            DataObject.SN_RULE = this.SN_RULE;
            DataObject.PANEL_RULE = this.PANEL_RULE;
            DataObject.DESCRIPTION = this.DESCRIPTION;
            DataObject.EDIT_EMP = this.EDIT_EMP;
            DataObject.EDIT_TIME = this.EDIT_TIME;
            DataObject.SKU_TYPE = this.SKU_TYPE;
            return DataObject;
        }
        public string ID
        {
            get
            {
                return (string)this["ID"];
            }
            set
            {
                this["ID"] = value;
            }
        }
        public string BU
        {
            get
            {
                return (string)this["BU"];
            }
            set
            {
                this["BU"] = value;
            }
        }
        public string SKUNO
        {
            get
            {
                return (string)this["SKUNO"];
            }
            set
            {
                this["SKUNO"] = value;
            }
        }
        public string VERSION
        {
            get
            {
                return (string)this["VERSION"];
            }
            set
            {
                this["VERSION"] = value;
            }
        }
        public string SKU_TYPE
        {
            get
            {
                return (string)this["SKU_TYPE"];
            }
            set
            {
                this["SKU_TYPE"] = value;
            }
        }
        public string SKU_NAME
        {
            get
            {
                return (string)this["SKU_NAME"];
            }
            set
            {
                this["SKU_NAME"] = value;
            }
        }
        public string C_SERIES_ID
        {
            get
            {
                return (string)this["C_SERIES_ID"];
            }
            set
            {
                this["C_SERIES_ID"] = value;
            }
        }
        public string CUST_PARTNO
        {
            get
            {
                return (string)this["CUST_PARTNO"];
            }
            set
            {
                this["CUST_PARTNO"] = value;
            }
        }
        public string CUST_SKU_CODE
        {
            get
            {
                return (string)this["CUST_SKU_CODE"];
            }
            set
            {
                this["CUST_SKU_CODE"] = value;
            }
        }
        public string SN_RULE
        {
            get
            {
                return (string)this["SN_RULE"];
            }
            set
            {
                this["SN_RULE"] = value;
            }
        }

        public string PANEL_RULE
        {
            get
            {
                return (string)this["PANEL_RULE"];
            }
            set
            {
                this["PANEL_RULE"] = value;
            }
        }

        public string DESCRIPTION
        {
            get
            {
                return (string)this["DESCRIPTION"];
            }
            set
            {
                this["DESCRIPTION"] = value;
            }
        }
        public string EDIT_EMP
        {
            get
            {
                return (string)this["EDIT_EMP"];
            }
            set
            {
                this["EDIT_EMP"] = value;
            }
        }
        public DateTime EDIT_TIME
        {
            get
            {
                if (this["EDIT_TIME"] == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return (DateTime)this["EDIT_TIME"];
                }
            }
            set
            {
                this["EDIT_TIME"] = value;
            }
        }

        #endregion
    }
    public class C_SKU
    {
        #region 機種實體類
        public string ID{ get; set; }
        public string BU{ get; set; }
        public string SKUNO{ get; set; }
        public string VERSION{ get; set; }
        public string SKU_NAME{ get; set; }
        public string C_SERIES_ID{ get; set; }
        public string CUST_PARTNO{ get; set; }
        public string CUST_SKU_CODE{ get; set; }
        public string SN_RULE{ get; set; }
        public string PANEL_RULE{ get; set; }
        public string DESCRIPTION{ get; set; }
        public string EDIT_EMP { get; set; }
        public DateTime EDIT_TIME{ get; set; }
        public string SKU_TYPE{ get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
        #endregion
    }

    public class SkuObject
    {
        #region 屬性
        public string SkuId
        {
            get
            {
                return SkuBase.ID;
            }
            set
            {
                SkuBase.ID = value;
            }
        }
        public string Bu
        {
            get
            {
                return SkuBase.BU;
            }
            set
            {
                SkuBase.BU = value;
            }
        }
        public string SkuNo
        {
            get
            {
                return SkuBase.SKUNO;
            }
            set
            {
                SkuBase.SKUNO = value;
            }
        }
        public string Version
        {
            get
            {
                return SkuBase.VERSION;
            }
            set
            {
                SkuBase.VERSION = value;
            }
        }
        public string SkuName
        {
            get
            {
                return SkuBase.SKU_NAME;
            }
            set
            {
                SkuBase.SKU_NAME = value;
            }
        }

        public string SkuType
        {
            get
            {
                return SkuBase.SKU_TYPE;
            }
            set
            {
                SkuBase.SKU_TYPE = value;
            }
        }
        public string CSeriesId
        {
            get
            {
                return SkuBase.C_SERIES_ID;
            }
            set
            {
                SkuBase.C_SERIES_ID = value;
            }
        }
        public string CustPartNo
        {
            get
            {
                return SkuBase.CUST_PARTNO;
            }
            set
            {
                SkuBase.CUST_PARTNO = value;
            }
        }
        public string CustSkuCode
        {
            get
            {
                return SkuBase.CUST_SKU_CODE;
            }
            set
            {
                SkuBase.CUST_SKU_CODE = value;
            }
        }
        public string SnRule
        {
            get
            {
                return SkuBase.SN_RULE;
            }
            set
            {
                SkuBase.SN_RULE = value;
            }
        }

        public string PanelRule
        {
            get
            {
                return SkuBase.PANEL_RULE;
            }
            set
            {
                SkuBase.PANEL_RULE = value;
            }
        }

        public string Description
        {
            get
            {
                return SkuBase.DESCRIPTION;
            }
            set
            {
                SkuBase.DESCRIPTION = value;
            }
        }
        public string LastEditUser
        {
            get
            {
                return SkuBase.EDIT_EMP;
            }
            set
            {
                SkuBase.EDIT_EMP = value;
            }
        }
        public DateTime LastEditTime
        {
            get
            {
                return SkuBase.EDIT_TIME;
            }
            set
            {
                SkuBase.EDIT_TIME = value;
            }
        }
        public string SeriesId
        {
            get
            {
                return SkuSeries.ID;
            }
            set
            {
                SkuSeries.ID = value;
            }
        }
        public string SeriesCustomerId
        {
            get
            {
                return SkuSeries.CUSTOMER_ID;
            }
            set
            {
                SkuSeries.CUSTOMER_ID = value;
            }
        }
        public string SeriesName
        {
            get
            {
                return SkuSeries.SERIES_NAME;
            }
            set
            {
                SkuSeries.SERIES_NAME = value;
            }
        }
        public string SeriesDescription
        {
            get
            {
                return SkuSeries.DESCRIPTION;
            }
            set
            {
                SkuSeries.DESCRIPTION = value;
            }
        }
        public string SeriesEditEmp
        {
            get
            {
                return SkuSeries.EDIT_EMP;
            }
            set
            {
                SkuSeries.EDIT_EMP = value;
            }
        }
        public DateTime? SeriesEditTime
        {
            get
            {
                return SkuSeries.EDIT_TIME;
            }
            set
            {
                SkuSeries.EDIT_TIME = value;
            }
        }

        [JsonIgnore]
        [ScriptIgnore]
        public C_SKU SkuBase;
        [JsonIgnore]
        [ScriptIgnore]
        public C_SERIES SkuSeries;
        #endregion

        public SkuObject()
        {
            if (SkuBase == null)
            {
                SkuBase = new C_SKU();
            }
            if (SkuSeries == null)
            {
                SkuSeries = new C_SERIES();
            }
        }
    }

    public class T_R_SKU_ROUTE : DataObjectTable
    {
        public T_R_SKU_ROUTE(string _TableName, OleExec DB, DB_TYPE_ENUM DBType) : base(_TableName, DB, DBType)
        {

        }
        public T_R_SKU_ROUTE(OleExec DB, DB_TYPE_ENUM DBType)
        {
            RowType = typeof(Row_R_SKU_ROUTE);
            TableName = "R_SKU_ROUTE".ToUpper();
            DataInfo = GetDataObjectInfo(TableName, DB, DBType);
        }

        #region 業務方法
        /// <summary>
        /// 根據機種名或者路由名返回所有的機種路由對應關係，也可以不加條件而返回全部的對應關係 R_SKU_ROUTE 實例
        /// </summary>
        /// <param name="sfcdb"></param>
        /// <param name="DB_TYPE"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<R_SKU_ROUTE> Get_SKU_ROUTE_Mappings(OleExec sfcdb,params string[] parameters)
        {
            List<R_SKU_ROUTE> mappings = new List<R_SKU_ROUTE>();
            string sql = string.Empty;
            T_C_SKU table_sku = null;
            T_C_ROUTE table_route = null;
            DataTable dt = new DataTable();
            Row_R_SKU_ROUTE row = null;//(Row_R_SKU_ROUTE)NewRow();
           

            if(DB_TYPE_ENUM.Oracle.Equals(DBType))
            { 
                table_sku = new T_C_SKU(sfcdb,DBType);
                table_route = new T_C_ROUTE(sfcdb, DBType);
                if (parameters.Length > 0)
                {
                    sql = $@"SELECT C.* FROM C_SKU A,C_ROUTE B,R_SKU_ROUTE C WHERE C.SKU_ID=A.ID AND C.ROUTE_ID=B.ID 
                            AND (A.SKUNO LIKE '%{parameters[0]}%' OR B.ROUTE_NAME LIKE '%{parameters[0]}%')";
                }
                else
                {
                    sql = "SELECT C.* FROM C_SKU A,C_ROUTE B,R_SKU_ROUTE C WHERE C.SKU_ID=A.ID AND C.ROUTE_ID=B.ID";
                }
                dt = sfcdb.ExecSelect(sql).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    row = (Row_R_SKU_ROUTE) NewRow();
                    row.loadData(dr);
                    mappings.Add(row.GetDataObject());
                    //mappings.Add(ConstructMapping(dr, table_sku, table_route, sfcdb));
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return mappings;
        }

        /// <summary>
        /// 刪除機種路由對應關係
        /// </summary>
        /// <param name="MappingID"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public string DeleteMapping(string MappingID,OleExec sfcdb)
        {
            string result = string.Empty;
            string DeleteString = string.Empty;
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();

            row = (Row_R_SKU_ROUTE)GetObjByID(MappingID,sfcdb);

            if(DBType.Equals(DB_TYPE_ENUM.Oracle))
            {
                DeleteString = row.GetDeleteString(this.DBType);
                result = sfcdb.ExecSQL(DeleteString);
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return result;
        }

        public string DeleteMapping(R_SKU_ROUTE Mapping, OleExec sfcdb)
        {
            string result = string.Empty;
            string DeleteString = string.Empty;
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();
            string sql = string.Empty;
            DataTable dt = new DataTable();

            if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            {
                sql = $@"SELECT * FROM R_SKU_ROUTE WHERE SKU_ID='{Mapping.SKU_ID}' AND ROUTE_ID='{Mapping.ROUTE_ID}'";
                dt = sfcdb.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    row.loadData(dt.Rows[0]);
                    DeleteString = row.GetDeleteString(DBType);
                    result = sfcdb.ExecSQL(DeleteString);
                }
                else
                {
                    result = "0";
                }
                
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return result;
        }

        public string DeleteMapping(string SKU_ID,string ROUTE_ID, OleExec sfcdb)
        {
            string result = string.Empty;
            string DeleteString = string.Empty;
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();
            string sql = string.Empty;
            DataTable dt = new DataTable();

            if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            {
                sql = $@"SELECT * FROM R_SKU_ROUTE WHERE SKU_ID='{SKU_ID}' AND ROUTE_ID='{ROUTE_ID}'";
                dt = sfcdb.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    row.loadData(dt.Rows[0]);
                    DeleteString = row.GetDeleteString(DBType);
                    result = sfcdb.ExecSQL(DeleteString);
                }
                else
                {
                    result = "0";
                }

            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return result;
        }

        /// <summary>
        /// 檢查 R_SKU_ROUTE 來判斷該路由-機種映射關係是否可以添加到 R_SKU_ROUTE 中
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public bool CheckMappingExists(R_SKU_ROUTE mapping, OleExec sfcdb)
        {
            string sql = string.Empty;
            DataTable dt = new DataTable();
            string ErrMessage = string.Empty;

            if(DB_TYPE_ENUM.Oracle.Equals(DBType))
            { 
                sql = $@"SELECT * FROM R_SKU_ROUTE WHERE SKU_ID='{mapping.SKU_ID}' AND ROUTE_ID='{mapping.ROUTE_ID}'"; //判斷該映射關係是否已經存在於 R_SKU_ROUTE 中
                dt = sfcdb.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0) //如果存在，則不插入到 R_SKU_ROUTE 中
                {
                    ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000177");
                    throw new MESReturnMessage(ErrMessage);
                }
                else //不存在，判斷當前需要設置的映射關係是否想設置成默認的
                {
                    if (mapping.DEFAULT_FLAG == "Y") //如果用戶想設置成默認的，即設置該機種的默認路由
                    {
                        sql = $@"SELECT * FROM R_SKU_ROUTE WHERE SKU_ID='{mapping.SKU_ID}' AND DEFAULT_FLAG='Y'"; // 判斷該機種是否已經存在默認路由
                        dt = sfcdb.ExecSelect(sql).Tables[0];
                        if (dt.Rows.Count > 0) //存在則不允許插入到 R_SKU_ROUTE 中
                        {
                            ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000178");
                            throw new MESReturnMessage(ErrMessage);
                        }
                        else //不存在則允許插入到 R_SKU_ROUTE 中
                        {
                            return false;
                        }
                    }
                    else //如果用戶不想設置成默認路由，則直接插入到 R_SKU_ROUTE 中
                    {
                        return false;
                    }
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            //return false;
        }

        /// <summary>
        /// 檢查 C_ROUTE 來確定要加入的路由-機種映射關係是否可以添加 R_SKU_ROUTE 中
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public bool CheckMappingCanAdd(R_SKU_ROUTE mapping, OleExec sfcdb)
        {
            string sql = string.Empty;
            DataTable dt = new DataTable();
            T_C_SKU table = new T_C_SKU(sfcdb, this.DBType);
            string ErrMessage = string.Empty;

            if(DBType.Equals(DB_TYPE_ENUM.Oracle))
            { 
                sql = $@"SELECT * FROM C_ROUTE WHERE ID='{mapping.ROUTE_ID}' AND DEFAULT_SKUNO IS NOT NULL"; //判斷該路由是否配置了默認機種
                dt = sfcdb.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0)  //如果有配置默認機種
                {
                    //判斷配置的默認機種和當前需要添加的機種-路由映射中的機種是否一致
                    if (dt.Rows[0]["DEFAULT_SKUNO"].ToString().Equals(((Row_C_SKU)table.GetObjByID(mapping.SKU_ID,sfcdb)).SKUNO)) //如果一致，則判斷該機種-路由映射關係是否可以添加到 R_SKU_ROUTE 中
                    {
                        return !CheckMappingExists(mapping, sfcdb);
                    }
                    else //如果不一致，不可以添加到 R_SKU_ROUTE 中，因為有默認機種的路由只可以用到一個機種上，也就是說在 R_SKU_ROUTE 中只能存在最多一條數據
                    {
                        ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000176");
                        throw new MESReturnMessage(ErrMessage);
                        //return false;
                    }
                }
                else //沒有配置默認機種
                {
                    //判斷該路由-機種映射關係是否可以添加到 R_SKU_ROUTE 中
                    return !CheckMappingExists(mapping, sfcdb);
                }
            }
            else
            {
                ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(ErrMessage);
            }
        }

        /// <summary>
        /// 添加機種和路由對應關係
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public string AddMapping(R_SKU_ROUTE mapping,string BU, OleExec sfcdb)
        {
            string InsertString = string.Empty;
            string result = string.Empty;
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();

            row.ROUTE_ID = mapping.ROUTE_ID;
            row.SKU_ID = mapping.SKU_ID;
            row.DEFAULT_FLAG = mapping.DEFAULT_FLAG;
            row.EDIT_EMP = mapping.EDIT_EMP;
            row.EDIT_TIME = mapping.EDIT_TIME;

            if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            {
                if (CheckMappingCanAdd(mapping, sfcdb))
                {
                    row.ID = GetNewID(BU, sfcdb);
                    InsertString = row.GetInsertString(this.DBType);
                    result = sfcdb.ExecSQL(InsertString);
                }
                else
                {
                    result = "0";
                }
                   
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }

            return result;
        }

        /// <summary>
        /// 根據 ID 獲取機種路由對應關係
        /// </summary>
        /// <param name="MappingId"></param>
        /// <param name="sfcdb"></param>
        /// <param name="DB_TYPE"></param>
        /// <returns></returns>
        public R_SKU_ROUTE GetMappingById(string MappingId, OleExec sfcdb)
        {
            R_SKU_ROUTE mapping = new R_SKU_ROUTE();
            string sql = string.Empty;
            DataTable dt = new DataTable();
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();

            sql = $@"SELECT * FROM R_SKU_ROUTE WHERE ID='{MappingId}'";
            if(DB_TYPE_ENUM.Oracle.Equals(DBType))
            { 
                dt = sfcdb.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    row.loadData(dt.Rows[0]);
                    mapping = row.GetDataObject();
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return mapping;
        }

        /// <summary>
        /// 根據機種 ID 獲取到所有對應的路由
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="skuid"></param>
        /// <returns></returns>
        public List<SKU_ROUTE> GetBySKU(OleExec DB, string skuid)
        {
            OleDbParameter[] paramet;
            DataTable res = new DataTable();          
            List<SKU_ROUTE> getC_ROUTE = new List<SKU_ROUTE>();
            string strSql = $@"SELECT c.id,b.id ROUTE_ID,a.id SKU_ID,c.default_flag,b.route_name,b.default_skuno,b.route_type,c.edit_time,c.edit_emp"+
                              $@" FROM C_SKU A,C_ROUTE B,R_SKU_ROUTE C WHERE C.SKU_ID=A.ID AND C.ROUTE_ID=B.ID AND A.ID=:SKUID";
            //oldb 的參數只能是按照順序對應，不能復用，
            paramet = new OleDbParameter[] { new OleDbParameter(":SKUID", skuid) };
            res = DB.ExecuteDataTable(strSql, CommandType.Text, paramet);
            if (res.Rows.Count > 0)
            {
                for (int i = 0; i < res.Rows.Count; i++)
                {
                    SKU_ROUTE skuroute = new SKU_ROUTE();
                    skuroute.ID = res.Rows[i]["ID"].ToString();
                    skuroute.ROUTE_ID = res.Rows[i]["ROUTE_ID"].ToString();
                    skuroute.SKU_ID = res.Rows[i]["SKU_ID"].ToString();
                    skuroute.ROUTE_NAME =(res.Rows[i]["ROUTE_NAME"]==null)?"":res.Rows[i]["ROUTE_NAME"].ToString();
                    skuroute.ROUTE_TYPE = (res.Rows[i]["ROUTE_TYPE"] == null) ? "" : res.Rows[i]["ROUTE_TYPE"].ToString();
                    skuroute.DEFAULT_FLAG = (res.Rows[i]["DEFAULT_FLAG"] == null) ? "" : res.Rows[i]["DEFAULT_FLAG"].ToString();
                    skuroute.DEFAULT_SKUNO = (res.Rows[i]["DEFAULT_SKUNO"] == null) ? "" : res.Rows[i]["DEFAULT_SKUNO"].ToString();
                    skuroute.EDIT_EMP = (res.Rows[i]["EDIT_EMP"] == null) ? "" : res.Rows[i]["EDIT_EMP"].ToString();                   
                    skuroute.EDIT_TIME = (res.Rows[i]["EDIT_TIME"] == null|| res.Rows[i]["EDIT_TIME"].ToString().Trim().Length<=0) ? null : (DateTime?)res.Rows[i]["EDIT_TIME"];                   
                    getC_ROUTE.Add(skuroute);
                }
                return getC_ROUTE;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 檢查機種路由映射關係是否可以更新到數據庫
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="sfcdb"></param>
        /// <param name="DB_TYPE"></param>
        /// <returns></returns>
        public bool CheckMappingCanUpdate(R_SKU_ROUTE mapping, OleExec sfcdb)
        {
            string format = "yyyy-MM-dd HH:mm:ss";
            R_SKU_ROUTE originalMapping = null;
            string sql = string.Empty;
            DataTable dt = new DataTable();
            T_C_SKU table = new T_C_SKU(sfcdb, DBType);
            string ErrMessage = string.Empty;

            if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            {
                originalMapping = GetMappingById(mapping.ID, sfcdb);

                if (originalMapping.EDIT_TIME.ToString(format) != mapping.EDIT_TIME.ToString(format).Replace("T", ""))
                {
                    ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000179");
                    throw new MESReturnMessage(ErrMessage);
                }
                else
                {
                    if (mapping.DEFAULT_FLAG == "Y")
                    {
                        if (originalMapping.DEFAULT_FLAG == "N")
                        {
                            sql = $@"SELECT * FROM C_ROUTE WHERE ID='{mapping.ROUTE_ID}' AND DEFAULT_SKUNO IS NOT NULL";
                            dt = sfcdb.ExecSelect(sql).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                if (!dt.Rows[0]["DEFAULT_SKUNO"].ToString().Equals(((Row_C_SKU)table.GetObjByID(mapping.SKU_ID, sfcdb)).SKUNO))
                                {
                                    ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000176");
                                    throw new MESReturnMessage(ErrMessage);
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                sql = $@"SELECT * FROM R_SKU_ROUTE WHERE SKU_ID='{mapping.SKU_ID}' AND DEFAULT_FLAG='Y'";
                                dt = sfcdb.ExecSelect(sql).Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    ErrMessage = MESReturnMessage.GetMESReturnMessage("MES00000178");
                                    throw new MESReturnMessage(ErrMessage);
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
        }

        /// <summary>
        /// 更新機種和路由的對應關係
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="sfcdb"></param>
        /// <param name="DB_TYPE"></param>
        /// <returns></returns>
        public string UpdateMapping(R_SKU_ROUTE mapping,OleExec sfcdb)
        {
            string UpdateString = string.Empty;
            string result = string.Empty;
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();

            row = (Row_R_SKU_ROUTE)GetObjByID(mapping.ID,sfcdb);
            row.ROUTE_ID = mapping.ROUTE_ID;
            row.SKU_ID = mapping.SKU_ID;
            row.DEFAULT_FLAG = mapping.DEFAULT_FLAG;
            row.EDIT_EMP = mapping.EDIT_EMP;
            row.EDIT_TIME = mapping.EDIT_TIME;

            if(DBType.Equals(DB_TYPE_ENUM.Oracle))
            {
                if (CheckMappingCanUpdate(mapping, sfcdb))
                {
                    UpdateString = row.GetUpdateString(this.DBType);
                    result = sfcdb.ExecSQL(UpdateString);
                }
                else
                {
                    result = "0";
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }

            return result;
        }

        /// <summary>
        /// 根據路由ID找到對應的所有使用這個路由的機種 C_SKU 實例
        /// </summary>
        /// <param name="MappingRouteId"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public List<C_SKU> GetSkuListByMappingRouteID(string MappingRouteId, OleExec sfcdb)
        {
            List<C_SKU> SkuList = new List<C_SKU>();
            string sql = string.Empty;
            DataTable dt = new DataTable();
            T_C_SKU table = new T_C_SKU(sfcdb, this.DBType);
            Row_C_SKU row = (Row_C_SKU)table.NewRow();

            if (DBType.Equals(DB_TYPE_ENUM.Oracle))
            { 
                sql = $@"SELECT A.* FROM C_SKU A JOIN R_SKU_ROUTE B ON A.ID=B.SKU_ID WHERE B.ROUTE_ID='{MappingRouteId}'";
                dt = sfcdb.ExecSelect(sql).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    row.loadData(dr);
                    C_SKU Sku=row.GetDataObject();
                    SkuList.Add(Sku);
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return SkuList;
        }

        /// <summary>
        /// 根據機種名和版本唯一確定機種ID之後獲取所有路由映射關係
        /// </summary>
        /// <param name="SkuNo"></param>
        /// <param name="version"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public List<R_SKU_ROUTE> GetMappingBySkuAndVersion(string SkuNo, string version, OleExec sfcdb)
        {
            List<R_SKU_ROUTE> mappings = new List<R_SKU_ROUTE>();
            string sql = string.Empty;
            DataTable dt = new DataTable();
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();

            sql = $@"SELECT * FROM R_SKU_ROUTE WHERE SKU_ID IN (SELECT ID FROM C_SKU WHERE SKUNO='{SkuNo}' AND VERSION='{version}') AND ROWNUM=1";
            if (DB_TYPE_ENUM.Oracle.Equals(DBType))
            {
                dt = sfcdb.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        row.loadData(dt.Rows[0]);
                        mappings.Add(row.GetDataObject());
                    }
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return mappings;
        }

        /// <summary>
        /// 根據機種 ID 獲取所有的映射關係
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="sfcdb"></param>
        /// <returns></returns>
        public List<R_SKU_ROUTE> GetMappingBySkuId(string SkuId, OleExec sfcdb)
        {
            List<R_SKU_ROUTE> mappings = new List<R_SKU_ROUTE>();
            string sql = string.Empty;
            DataTable dt = new DataTable();
            Row_R_SKU_ROUTE row = (Row_R_SKU_ROUTE)NewRow();

            sql = $@"SELECT * FROM R_SKU_ROUTE WHERE SKU_ID='{SkuId}'";
            if (DB_TYPE_ENUM.Oracle.Equals(DBType))
            {
                dt = sfcdb.ExecSelect(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        row.loadData(dr);
                        mappings.Add(row.GetDataObject());
                    }
                }
            }
            else
            {
                string errMsg = MESReturnMessage.GetMESReturnMessage("MES00000019", new string[] { DBType.ToString() });
                throw new MESReturnMessage(errMsg);
            }
            return mappings;
        }
        #endregion
    }

    public class Row_R_SKU_ROUTE : DataObjectBase
    {
        public Row_R_SKU_ROUTE(DataObjectInfo info) : base(info)
        {

        }
        public R_SKU_ROUTE GetDataObject()
        {
            R_SKU_ROUTE DataObject = new R_SKU_ROUTE();
            DataObject.ID = this.ID;
            DataObject.ROUTE_ID = this.ROUTE_ID;
            DataObject.SKU_ID = this.SKU_ID;
            DataObject.DEFAULT_FLAG = this.DEFAULT_FLAG;
            DataObject.EDIT_TIME = this.EDIT_TIME;
            DataObject.EDIT_EMP = this.EDIT_EMP;
            return DataObject;
        }
        public string ID
        {
            get
            {
                return (string)this["ID"];
            }
            set
            {
                this["ID"] = value;
            }
        }
        public string ROUTE_ID
        {
            get
            {
                return (string)this["ROUTE_ID"];
            }
            set
            {
                this["ROUTE_ID"] = value;
            }
        }
        public string SKU_ID
        {
            get
            {
                return (string)this["SKU_ID"];
            }
            set
            {
                this["SKU_ID"] = value;
            }
        }
        public string DEFAULT_FLAG
        {
            get
            {
                return (string)this["DEFAULT_FLAG"];
            }
            set
            {
                this["DEFAULT_FLAG"] = value;
            }
        }
        public DateTime EDIT_TIME
        {
            get
            {
                return (DateTime)this["EDIT_TIME"];
            }
            set
            {
                this["EDIT_TIME"] = value;
            }
        }
        public string EDIT_EMP
        {
            get
            {
                return (string)this["EDIT_EMP"];
            }
            set
            {
                this["EDIT_EMP"] = value;
            }
        }
    }
    public class R_SKU_ROUTE
    {
        public string ID{get;set;}
        public string ROUTE_ID{get;set;}
        public string SKU_ID{get;set;}
        public string DEFAULT_FLAG{get;set;}
        public DateTime EDIT_TIME{get;set;}
        public string EDIT_EMP{get;set;}
    }  
    public class SKU_ROUTE 
    {
        public string ID{get;set;}
        public string ROUTE_ID{get;set;}
        public string SKU_ID{get;set;}
        public string DEFAULT_FLAG{get;set;}
        public string ROUTE_NAME{get;set;}
        public string DEFAULT_SKUNO{get;set;}
        public string ROUTE_TYPE{get;set;}
        public DateTime? EDIT_TIME{get;set;}
        public string EDIT_EMP{get;set;}
    }
}