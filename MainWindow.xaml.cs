using equipmentMangement.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

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

            //userTyp.ItemsSource = Enum.GetValues(typeof(UserTypeEnum)).Cast<UserTypeEnum>();

            if (App.Current.Properties["CurrentUser"] != null)
            {
                userLabel.Content = App.Current.Properties["CurrentUser"];
            }

            if((string)App.Current.Properties["CurrentUserType"] != "admin")
            {
                addUser.Visibility = Visibility.Collapsed;
                addEquipment.Visibility = Visibility.Collapsed;
            }

            wyborStart.SelectedDate = DateTime.Today;
            wyborKoniec.SelectedDate = DateTime.Today.AddDays(10);
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
            try
            {
                using (reservations_dbEntities1 context = new reservations_dbEntities1())
                {
                    // debug sql queries
                    //context.Database.Log = Console.WriteLine;

                    // query db for equipment information
                    List<equipment> equipmentObjectsList = context.equipment.Include(i => i.reservations).ToList<equipment>();
                    foreach (var eq in equipmentObjectsList)
                    {
                        eq.SprRes((DateTime)wyborStart.SelectedDate, (DateTime)wyborKoniec.SelectedDate);
                    }
                    Sprzet.ItemsSource = equipmentObjectsList;
                    Sprzet.Columns.Clear();
                    DataGridTextColumn nazwaSprzetu = new DataGridTextColumn();
                    nazwaSprzetu.Header = "Nazwa";
                    nazwaSprzetu.Binding = new Binding("Name");
                    Sprzet.Columns.Add(nazwaSprzetu);

                    for (int i = 0; i < ((DateTime)wyborKoniec.SelectedDate - (DateTime)wyborStart.SelectedDate).TotalDays+1; i++)
                    {
                        DataGridTextColumn textColumn = new DataGridTextColumn();
                        textColumn.Header = ((DateTime)wyborStart.SelectedDate).AddDays(i);
                        //textColumn.Header = DateTime.Today.AddDays(i);
                        textColumn.Binding = new Binding("res[" + i + "]");
     

                        Sprzet.Columns.Add(textColumn);
                    }

                }
            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("Wybierz początkową i końcową datę wyświetlanego zakresu");
            }
        }

        private void Sprzet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            kalendarzyk.SelectedDates.Clear();

            listaRezerwacji.Text = "";

            try
            {
                using (var context = new reservations_dbEntities1())
                {

                    foreach (reservations r in ((equipment)(Sprzet.SelectedItem)).reservations)
                    {
                        if (r.StartDate > DateTime.Now)
                            listaRezerwacji.Text += r.startDate + "-" + r.stopDate + '\t' + context.user.Find(r.idUser).FullName + '\n';
                    }
                }

                kalendarzyk.BlackoutDates.Clear();

                foreach (reservations res in ((equipment)(Sprzet.SelectedItem)).reservations)
                {
                    CalendarDateRange cdr = new CalendarDateRange((DateTime)res.StartDate, (DateTime)res.StopDate);
                    kalendarzyk.BlackoutDates.Add(cdr);
                }
            }
            catch(NullReferenceException)
            {
                //ok
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
                using (reservations_dbEntities1 context = new reservations_dbEntities1())
                {
                    var reservation1 = new reservations
                    {
                        idUser = 1,
                        StartDate = poczatek,
                        StopDate = koniec
                    };

                    var eq = context.equipment.Find(((equipment)Sprzet.SelectedItem).idEquipment);

                    reservation1.equipment.Add(eq);
                    reservation1.user = context.user.Find(App.Current.Properties["CurrentUserID"]);
                    context.reservations.Add(reservation1);
                    MessageBox.Show("reservtionID:" + reservation1.idReservations + " equipmentID: " + eq.idEquipment);
                    context.SaveChanges();

                    List<equipment> equipmentObjectsList = context.equipment.Include(i => i.reservations).ToList<equipment>();

                    foreach (var eqip in equipmentObjectsList)
                    {
                        eqip.SprRes((DateTime)wyborStart.SelectedDate, (DateTime)wyborKoniec.SelectedDate);
                    }

                    Sprzet.ItemsSource = equipmentObjectsList;

                    Sprzet.Columns.Clear();
                    DataGridTextColumn nazwaSprzetu = new DataGridTextColumn();
                    nazwaSprzetu.Header = "Nazwa";
                    nazwaSprzetu.Binding = new Binding("Name");
                    Sprzet.Columns.Add(nazwaSprzetu);

                    for (int i = 0; i < ((DateTime)wyborKoniec.SelectedDate - (DateTime)wyborStart.SelectedDate).TotalDays + 1; i++)
                    {
                        DataGridTextColumn textColumn = new DataGridTextColumn();
                        textColumn.Header = ((DateTime)wyborStart.SelectedDate).AddDays(i);
                        //textColumn.Header = DateTime.Today.AddDays(i);
                        textColumn.Binding = new Binding("res[" + i + "]");


                        Sprzet.Columns.Add(textColumn);
                    }

                }

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

            try
            {
                foreach (reservations r in ((equipment)(Sprzet.SelectedItem)).reservations)
                {
                    if (r.StopDate > DateTime.Now)
                        listaRezerwacji.Text += r.startDate + "-" + r.stopDate + '\n';
                }
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Oops");
            }

        }

        private void userBox_DropDownOpened(object sender, EventArgs e)
        {
            using(var context = new reservations_dbEntities1())
            {

                userBox.ItemsSource = context.user.ToList<user>();
                userBox.DisplayMemberPath = "FullName";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                var ulist = context.user.ToList<user>();
                user us = new user { Name = Imie.Text, Surname = Nazwisko.Text, Login = Login.Text, Password=Hasło.Text, UserType = userTyp.Text };

                context.user.Add(us);
                context.SaveChanges();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                var us = context.user.Find(((user)userBox.SelectedItem).idUser);
                us.Name = Imie.Text;
                us.Surname = Nazwisko.Text;
                us.Login = Login.Text;
                us.Password = Hasło.Text;
                us.UserType = userTyp.Text;
                context.SaveChanges();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                var us = context.user.Find(((user)userBox.SelectedItem).idUser);
                context.user.Remove(us);
                Imie.Text = "";
                Nazwisko.Text = "";
                Login.Text = "";
                Hasło.Text = "";
                //typ.Text = "";
                userBox.SelectedItem = null;
                context.SaveChanges();
            }

        }

        private void DodajSprzet_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                var eq = new equipment {Name = eqName.Text, Owner=owner.Text, EID=EID.Text };

                context.equipment.Add(eq);
                context.SaveChanges();
            }
        }

        private void eqBox_DropDownOpened(object sender, EventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                eqBox.ItemsSource = context.equipment.ToList<equipment>();
                eqBox.DisplayMemberPath = "Name";
            }
        }

        private void EdytujSprzet_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                var eq = context.equipment.Find(((equipment)eqBox.SelectedItem).idEquipment);
                eq.Name = eqName.Text;
                eq.Owner = owner.Text;
                eq.EID = EID.Text;
                context.SaveChanges();
            }
        }

        private void UsunSprzet_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                var eq = context.equipment.Find(((equipment)eqBox.SelectedItem).idEquipment);
                context.equipment.Remove(eq);
                eqName.Text = "";
                owner.Text = "";
                EID.Text = "";
                eqBox.SelectedItem = null;
                context.SaveChanges();
            }
        }
    }
}
