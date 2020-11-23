using equipmentMangement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace equipmentMangement
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ReservationBasicInfoVM> reservationsListViewModel = new List<ReservationBasicInfoVM>();
        //List<user> users = new List<user>();

        public MainWindow()
        {
            InitializeComponent();

            //load data to comboboxes
            using (reservations_dbEntities1 context = new reservations_dbEntities1())
            {
                // query db for user and equipment information - combobox lists
                List<user> userList = context.user.ToList<user>();
                List<equipment> eqList = context.equipment.ToList<equipment>();

                foreach (var item in userList)
                    userComboBox.Items.Add(item.FullName);

                foreach (var item in eqList)
                    equipmentComboBox.Items.Add(item.Name);
            }
        }

        private void load_User_Data_Click(object sender, RoutedEventArgs e)
        {
            using (reservations_dbEntities1 context = new reservations_dbEntities1())
            {
                // debug sql queries
                //context.Database.Log = Console.WriteLine;

                // query db for user information
                List<user> userList = context.user.Include(i => i.reservations).ToList<user>();


                List<UserVM> userRes = new List<UserVM>();
                foreach (var el in userList)
                {
                    UserVM tempUserRes = new UserVM(el.Name, el.Surname, el.NumberOfReservations);
                    userRes.Add(tempUserRes);
                }

                dataGrid.ItemsSource = userRes;
            }
        }


        private void load_Equipment_Data_Click(object sender, RoutedEventArgs e)
        {
            using (reservations_dbEntities1 context = new reservations_dbEntities1())
            {
                // debug sql queries
                //context.Database.Log = Console.WriteLine;

                // query db for equipment information
                List<equipment> equipmentObjectsList = context.equipment.ToList<equipment>();


                dataGrid.ItemsSource = equipmentObjectsList;
            }
        }

        private void load_Reservation_Data_Click(object sender, RoutedEventArgs e)
        {
            using (reservations_dbEntities1 context = new reservations_dbEntities1())
            {
                // debug sql queries
                //context.Database.Log = Console.WriteLine;

                // query db for equipment information
                List<reservations> reservationsList = context.reservations.Include(i => i.equipment).Include(i => i.user).ToList<reservations>();
                //List<user> userList = context.user.ToList<user>();


                List<ReservationVM> reservationsListVM = new List<ReservationVM>();
                foreach (var el in reservationsList)
                {
                    ReservationVM tempReservation = new ReservationVM(el.idReservations, el.equipment.FirstOrDefault().Name, el.equipment.FirstOrDefault().EID, el.user.FullName, el.startDate, el.stopDate);
                    reservationsListVM.Add(tempReservation);
                }

                dataGrid.ItemsSource = reservationsListVM;
            }
        }

        private void add_New_Record_Click(object sender, RoutedEventArgs e)
        {
         
            if (userComboBox.Text.Equals("") || equipmentComboBox.Text.Equals("") || startDateDataPicker.Text.Equals("") || stopDateDataPicker.Text.Equals(""))
                MessageBox.Show("Wypełnij wszystkie pola!");
            else
            {
                using (reservations_dbEntities1 context = new reservations_dbEntities1())
                {
                    string tempName = userComboBox.Text.Split(' ')[0];
                    string tempSurname = userComboBox.Text.Split(' ')[1];
                    user tempUser = context.user.Where(i => i.Name == tempName).Where(i => i.Surname == tempSurname).FirstOrDefault();
                    equipment tempEq = context.equipment.Where(i => i.Name == equipmentComboBox.Text).FirstOrDefault();
                    ICollection<equipment> eqList = new List<equipment>();
                    eqList.Add(tempEq);


                    // data to save to db
                    var reservation = new reservations { idUser = tempUser.idUser, StartDate = startDateDataPicker.SelectedDate.Value, StopDate = stopDateDataPicker.SelectedDate.Value, user = tempUser};
                    context.reservations.Add(reservation);


                    context.SaveChanges();
                }
            }
            
        }
    }
}
