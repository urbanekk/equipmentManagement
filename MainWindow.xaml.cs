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

        private void load_Equipment_Data_Click2(object sender, RoutedEventArgs e)
        {
            using (reservations_dbEntities1 context = new reservations_dbEntities1())
            {
                // debug sql queries
                //context.Database.Log = Console.WriteLine;

                // query db for equipment information
                List<equipment> equipmentObjectsList = context.equipment.Include(i => i.reservations).ToList<equipment>();


                Sprzet.ItemsSource = equipmentObjectsList;
            }
        }

        private void Sprzet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listaRezerwacji.Text = "";

            foreach (reservations r in ((equipment)(Sprzet.SelectedItem)).reservations)
            {
                //if (r.StartDate > DateTime.Now)
                    listaRezerwacji.Text += r.startDate + "-" + r.stopDate + '\t' + r.idUser + '\n';
            }

            kalendarzyk.BlackoutDates.Clear();

            foreach (reservations res in ((equipment)(Sprzet.SelectedItem)).reservations)
            {
                CalendarDateRange cdr = new CalendarDateRange((DateTime)res.StartDate, (DateTime)res.StopDate);
                kalendarzyk.BlackoutDates.Add(cdr);
            }
        }

        private void kalendarzyk_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kalendarzyk.SelectedDates.Count > 0)
            {
                poczatekRezerwacji.Text = (kalendarzyk.SelectedDates.First()).ToShortDateString();
                koniecRezerwacji.Text = (kalendarzyk.SelectedDates.Last()).ToShortDateString();
            }
        }

        private void rezerwuj_Click(object sender, RoutedEventArgs e)
        {
            bool overlap = false;
            DateTime poczatek, koniec;
            if (Sprzet.SelectedIndex == -1)
            {
                MessageBox.Show("Wybierz przedmiot z listy");
                return;
            }

            try
            {
                poczatek = DateTime.Parse(poczatekRezerwacji.Text);
                koniec = DateTime.Parse(koniecRezerwacji.Text);
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Podaj datę w odpowednim formacie DD.MM.YYYY");
                return;
            }


            foreach (reservations r in ((equipment)(Sprzet.SelectedItem)).reservations)
            {
                if (koniec > r.StartDate && koniec < r.StopDate)
                    overlap = true;
                if (poczatek > r.StartDate && poczatek < r.StopDate)
                    overlap = true;
                if (poczatek == r.StartDate || poczatek == r.StopDate
                    || koniec == r.StartDate || koniec == r.StopDate)
                    overlap = true;
            }

            if (overlap == false && DateTime.Parse(poczatekRezerwacji.Text) >= DateTime.Now)
            {

                //dodaj polecenie do bazy dancyh dodające rezerwację

                //((equipment)Sprzet.SelectedItem).reservations.Add();

                CalendarDateRange cdr = new CalendarDateRange(poczatek, koniec);
                kalendarzyk.SelectedDates.Clear();
                kalendarzyk.BlackoutDates.Add(cdr);
            }
            else if (overlap == true)
            {
                MessageBox.Show("Daty rezerwacji nie mogą na siebie nachodzić!");
            }
            else if (DateTime.Parse(poczatekRezerwacji.Text) < DateTime.Now)
                MessageBox.Show("Nie można rezerwować przeszłych dat");

            listaRezerwacji.Text = "";

            foreach (reservations r in ((equipment)(Sprzet.SelectedItem)).reservations)
            {
                if (r.StopDate > DateTime.Now)
                    listaRezerwacji.Text += r.startDate + "-" + r.stopDate + '\n';
            }

        }



    }
}
