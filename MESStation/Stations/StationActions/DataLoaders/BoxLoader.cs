using MESDataObject;
using MESDataObject.Module;
using MESPubLab.MESStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESStation.Stations.StationActions.DataLoaders
{
    class BoxLoader
    {
        public static void loaderBox(MESStationBase Station, MESStationInput Input, List<R_Station_Action_Para> Paras)
        {
            if (Paras.Count == 0)
            {
                throw new Exception(MESReturnMessage.GetMESReturnMessage("MES00000050"));
            }
            MESStationSession PackNoSession = Station.StationSession.Find(t => t.MESDataType == Paras[0].SESSION_TYPE && t.SessionKey == Paras[0].SESSION_KEY);


            T_R_PACKING t_R_PACKING = new T_R_PACKING(Station.SFCDB, DB_TYPE_ENUM.Oracle);


            List<R_PACKING> list = new List<R_PACKING>();
            if (Input.Value != null)
            {

                list = t_R_PACKING.GetListPackByPackno((string)Input.Value, Station.SFCDB);
                if (list.Count > 0)
                {

                    PackNoSession = new MESStationSession() { MESDataType = Paras[0].SESSION_TYPE, InputValue = Input.Value.ToString(), SessionKey = Paras[0].SESSION_KEY, ResetInput = Input, Value = list[0] };
                }

                Station.StationSession.Add(PackNoSession);
            }

        }

    }
}
