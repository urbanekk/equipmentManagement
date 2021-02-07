using equipmentMangement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace equipmentMangement.Model
{
    class ModelQueries
    {
        List<ReservationBasicInfoVM> reservationsList;

        public ModelQueries()
        {
            reservationsList = new List<ReservationBasicInfoVM>();
        }

        public void ReservationListQuery()
        {
            reservationsList.Add(new ReservationBasicInfoVM("VDS - old", "2020-10-07", "2020-10-17", "Krzysztof Rybak"));
        }
    }
}
