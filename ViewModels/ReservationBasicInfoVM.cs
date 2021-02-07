using equipmentMangement.Command;
using equipmentMangement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace equipmentMangement.ViewModels
{
    // ViewModel for TabItem main window - Equipment reservations 
    class ReservationBasicInfoVM : BasicVM
    {
        private ModelQueries query;

        public ReservationBasicInfoVM()
        {

        }

        public ReservationBasicInfoVM(string equipmentName, string startDate, string stopDate, string userName)
        {
            EquipmentName = equipmentName;
            StartDate = startDate;
            StopDate = stopDate;
            UserName = userName;
        }

        public ICommand LoadData 
        {
            get
            {
                this.query = new ModelQueries();
                return new RelayCommand(query.ReservationListQuery);
                //return new RelayCommand();
            }
        } 

        public string EquipmentName { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public string UserName { get; set; }

        public ObservableCollection<ReservationBasicInfoVM> Reservations { get; set; }
    }
}
