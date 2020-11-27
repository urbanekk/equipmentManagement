using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace equipmentMangement
{
    public partial class user
    {
        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public int NumberOfReservations
        {
            get
            {
                return reservations.Count;
            }
        }
    }
}
