using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace equipmentMangement
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new reservations_dbEntities1())
            {
                try
                {
                    user us = context.user.Where(u => u.Login == login.Text).Select(u => u).First();
                    if (us != null && us.Password == passtxt.Password)
                    {
                        App.Current.Properties["CurrentUser"] = us.FullName;
                        App.Current.Properties["CurrentUserID"] = us.idUser;
                        App.Current.Properties["CurrentUserType"] = us.UserType;
                        Window Main = new MainWindow();
                        Main.Show();

                        
                        
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Bledny login lub haslo");
                    }
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Bledny login lub haslo2");
                }
            }
            
        }
    }
}
