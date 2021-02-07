using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace equipmentMangement
{
    // to be done
    public class ReservationVM
    {
        public ReservationVM(int id, string eqName, string EID, string userName, string startDate, string stopDate)
        {
            this.id = id;
            this.EquipmentName = eqName;
            this.EID = EID;
            this.UserFullName = userName;
            this.StartDate = startDate;
            this.StopDate = stopDate;
        }

        public int id { get; set; }
        public string EquipmentName { get; set; }
        public string EID { get; set; }
        public string UserFullName { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }
}
