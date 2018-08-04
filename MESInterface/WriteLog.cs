using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESDataObject.Module;
using MESDBHelper;
using MESDataObject;

namespace MESInterface
{
    public class WriteLog
    {
        public static void WriteIntoMESLog(OleExec SFCDB, string bu,string programName, string className, string functionName,string logMessage,string logSql, string editEmp)
        {
            //OleExec SFCDB = new OleExec(db, false);
            T_R_MES_LOG mesLog = new T_R_MES_LOG(SFCDB, DB_TYPE_ENUM.Oracle);
            string id = mesLog.GetNewID(bu, SFCDB);
            Row_R_MES_LOG rowMESLog = (Row_R_MES_LOG)mesLog.NewRow();
            rowMESLog.ID = id;
            rowMESLog.PROGRAM_NAME = programName;
            rowMESLog.CLASS_NAME = className;
            rowMESLog.FUNCTION_NAME = functionName;
            rowMESLog.LOG_MESSAGE = logMessage;
            rowMESLog.LOG_SQL = logSql;
            rowMESLog.EDIT_EMP = editEmp;
            rowMESLog.EDIT_TIME = System.DateTime.Now;
            SFCDB.ThrowSqlExeception = true;
            SFCDB.ExecSQL(rowMESLog.GetInsertString(DB_TYPE_ENUM.Oracle));
        }

        public static void WriteIntoMESLog(string Dbstr, string bu, string programName, string className, string functionName, string logMessage, string logSql,string data1,string data2,string data3, string editEmp,string mailflag)
        {
            using (var db = OleExec.GetSqlSugarClient(Dbstr, true))
            {
                db.Insertable<R_MES_LOG>(new R_MES_LOG {
                    ID = MesDbBase.GetNewID(Dbstr,bu, "R_MES_LOG"),
                    PROGRAM_NAME= programName,
                    CLASS_NAME= className,
                    FUNCTION_NAME = functionName,
                    LOG_MESSAGE = logMessage,
                    LOG_SQL= logSql,
                    DATA1 = data1,
                    DATA2 = data2,
                    DATA3 =data3,
                    EDIT_EMP = editEmp,
                    EDIT_TIME = System.DateTime.Now,
                    MAILFLAG = mailflag
                }).ExecuteCommand();
            }
        }
    }
}
