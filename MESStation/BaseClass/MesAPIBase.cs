using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDBHelper;
using MESDataObject;
using System.Reflection;
using System.Data;
//using WebServer.SocketService;

namespace MESStation.BaseClass
{
  
    /// <summary>
    /// 作為所有API的基礎類
    /// </summary>
    public class MesAPIBase
    {


        protected Dictionary<string, OleExecPool> _DBPools = new Dictionary<string, OleExecPool>();

        public MESStation.LogicObject.User LoginUser;
        public string BU;
        public string Language= "CHINESE";
        public String SystemName = "MES";
        public DB_TYPE_ENUM DBTYPE = DB_TYPE_ENUM.Oracle;
        protected bool _MastLogin = true;
        public string IP = "";

        public bool MastLogin
        {
            get
            {
                return _MastLogin;
            }
        }

        public Dictionary<string, OleExecPool> DBPools
        {
            
            get
            {
                return _DBPools;
            }
        }




        Dictionary<string, APIInfo> _Apis= new Dictionary<string ,APIInfo>();
        public Dictionary<string, APIInfo> Apis
        {
            get
            {
                
                return _Apis;
            }
        }

        public DateTime GetDBDateTime()
        {
            OleExec sfcdb = _DBPools["SFCDB"].Borrow();
            try
            {
                string strSql = "select sysdate from dual";
                if (DBTYPE == DB_TYPE_ENUM.Oracle)
                {
                    strSql = "select sysdate from dual";
                }
                else if (DBTYPE == DB_TYPE_ENUM.SqlServer)
                {
                    strSql = "select get_date() ";
                }
                else
                {
                    throw new Exception(DBTYPE.ToString() + " not Work");
                }
                DateTime DBTime = (DateTime)sfcdb.ExecSelectOneValue(strSql);
                _DBPools["SFCDB"].Return(sfcdb);
                return DBTime;
            }
            catch (Exception e)
            {
                _DBPools["SFCDB"].Return(sfcdb);
                throw e;
            }

        }

        public static System.Data.DataTable GetApiListTable()
        {
            Assembly assenbly = Assembly.Load("MESStation");
            Type tagType = typeof(BaseClass.MesAPIBase);
            Type[] t = assenbly.GetTypes();
            DataTable ret = new DataTable();
            ret.Columns.Add("No");
            ret.Columns.Add("廠區");
            ret.Columns.Add("API名稱");
            ret.Columns.Add("API位置");
            ret.Columns.Add("API內容");
            ret.Columns.Add("負責人員");
            ret.Columns.Add("主要用戶單位");
            ret.Columns.Add("主要用戶");
            ret.Columns.Add("使用者訪問頻率");
            ret.Columns.Add("備註");

            int count = 1;
            for (int i = 0; i < t.Length; i++)
            {
                TypeInfo ti = t[i].GetTypeInfo();
                Type baseType = ti.BaseType;
                if (baseType == tagType)
                {
                    object obj = assenbly.CreateInstance(ti.FullName);
                    MesAPIBase API = (MesAPIBase)obj;
                    string[] keys = API.Apis.Keys.ToArray();
                    foreach (var I in keys)
                    {
                        DataRow dr = ret.NewRow();
                        dr[0] = count++;
                        dr[1] = "NN";
                        dr[2] = ti.FullName + "." + API.Apis[I].FunctionName;
                        dr[3] = "10.120.115.142";
                        dr[4] = API.Apis[I].Description;
                        dr[5] = "黃和關";
                        dr[6] = "IT";
                        dr[7] = "Cloud MES";
                        dr[8] = "高";
                        ret.Rows.Add(dr);
                        // API.Apis[I]
                    }

                }
            }

            return ret;
        }
        

    }

   
}
