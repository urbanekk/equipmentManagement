using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace equipmentMangement.ViewModels
{
    class EquipmentVM
    {
        public EquipmentVM(string eqName, string EID, string owner)
        {
            this.EquipmentName = eqName;
            this.EID = EID;
            this.Owner = owner;
        }

        public string EquipmentName { get; set; }
        public string EID { get; set; }
        public string Owner { get; set; }
    }
}
