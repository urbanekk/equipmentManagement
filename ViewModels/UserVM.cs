using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace equipmentMangement
{
    // to be done
    public class UserVM
    {
        public UserVM(string name, string surname, int nrOfReservations)
        {
            this.Name = name;
            this.Surname = surname;
            this.NumberOfReservations = nrOfReservations;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int NumberOfReservations { get; set; }
    }
}
