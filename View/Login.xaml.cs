using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ITSWorkStation.EntityFramework;
using ITSWorkStation.Utils;

namespace ITSWorkStation.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login
    {
        public Login()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            TbtUserName.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TbtUserName.Text))
            {
                tblInfoAcount.Text = ResourceUtil.EmptyAccountInfo;
                tblInfoAcount.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                try
                {
                    var context = new cctvEntities();
                    //var list = from user in context.its_evt_login
                    //           where user.name == TbtUserName.Text && user.password == TptPassword.Password
                    //           select user;
                    var list = context.CheckAccount(TbtUserName.Text, TptPassword.Password);


                    if (list.ToArray().Count() > 0)
                    {
                        Account.Username = TbtUserName.Text;
                        this.Close();
                    }
                    else
                    {
                        Account.Username = String.Empty;
                        tblInfoAcount.Text = ResourceUtil.WrongAccountInfo;
                        tblInfoAcount.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
                catch (Exception ex)
                {
                    tblInfoAcount.Text = "Error";
                }
                
            }
        }

        public bool TryConnect(string username, string password)
        {
            return false;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
