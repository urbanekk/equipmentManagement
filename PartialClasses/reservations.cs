using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace equipmentMangement
{
    public partial class reservations
    {
        public string startDate
        {
            get { return StartDate.Value.ToString("dd/MM/yyyy"); }
        }

        public string stopDate
        {
            get { return StopDate.Value.ToString("dd/MM/yyyy"); }
        }
    }
}
