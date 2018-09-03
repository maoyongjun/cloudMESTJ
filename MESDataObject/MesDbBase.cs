using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDBHelper;
using System.Data;
using System.Reflection;
using System.Data.OleDb;
using SqlSugar;

namespace MESDataObject
{
    public class MesDbBase
    {
        /// <summary>
        /// Get Table seqId
        /// </summary>
        /// <param name="DbStr"></param>
        /// <param name="Bu"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string GetNewID(string DbStr,string Bu,string TableName)
        {
            using (var db = OleExec.GetSqlSugarClient(DbStr,true)) {
                var IN_BU = new SugarParameter("@IN_BU", Bu);
                var IN_TYPE = new SugarParameter("@IN_TYPE", TableName);
                var OUT_RES = new SugarParameter("@OUT_RES", null, true);//isOutput=true
                var excDt = db.Ado.UseStoredProcedure().GetDataTable("SFC.GET_ID", IN_BU, IN_TYPE, OUT_RES);
                string newID = OUT_RES.Value.ToString();
                if (newID.StartsWith("ERR"))
                {
                    throw new Exception("獲取表'" + TableName + "' ID 異常! " + newID);
                }
                return newID;
            }
        }

        /// <summary>
        /// Get Table seqId
        /// </summary>
        /// <param name="DbStr"></param>
        /// <param name="Bu"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string GetNewID(SqlSugarClient db, string Bu, string TableName)
        {
            var IN_BU = new SugarParameter("@IN_BU", Bu);
            var IN_TYPE = new SugarParameter("@IN_TYPE", TableName);
            var OUT_RES = new SugarParameter("@OUT_RES", null, true); //isOutput=true
            var excDt = db.Ado.UseStoredProcedure().GetDataTable("SFC.GET_ID", IN_BU, IN_TYPE, OUT_RES);
            string newID = OUT_RES.Value.ToString();
            if (newID.StartsWith("ERR"))
            {
                throw new Exception("獲取表'" + TableName + "' ID 異常! " + newID);
            }
            return newID;
        }
    }
}
