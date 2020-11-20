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
            if (userFullNameTextbox.Text.Equals("") && userFullNameTextbox.Text.Equals("") && userFullNameTextbox.Text.Equals(""))
                MessageBox.Show("Wypełnij wszystkie pola!");
            else
            {
                using (reservations_dbEntities1 context = new reservations_dbEntities1())
                {
                    // query db for user information
                    List<user> userList = context.user.Include(i => i.reservations).ToList<user>();

                    int userID;


                    user user = new user();
                    user = userList.Find(i => i.FullName == userFullNameTextbox.Text);

                    if (user != null)
                    {
                        //userID = user.idUser;

                        //reservations temp = new reservations()
                        //{

                        //};
                        //context.reservations.Add();
                    }
                        
                    else
                    {
                        MessageBox.Show("User not registered! Cannot create record!");
                    }
                }
            }
        }
    }
}
