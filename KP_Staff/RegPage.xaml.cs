// RegPage.xaml.cs
using KP_Staff.Actions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace KP_Staff
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        RegAndAuth regAndAuth;
        string log;
        string pass;
        public AuthPage()
        {
            InitializeComponent();
            regAndAuth = new RegAndAuth();
        }
       
        private void btnRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            string role = "";
            log = loginReg.Text.ToString();
            pass = passwordReg.Password.ToString();
            NavigationService n = NavigationService.GetNavigationService(this);
            NewWorkerPage newWorkerPage = new NewWorkerPage(log, pass);
            NewChallengerPage newChallengerPage = new NewChallengerPage(log, pass);

            if (workerRole.IsChecked == true)
            {
                role = "Работник";
            }
            if(challengerRole.IsChecked == true)
            {
                role = "Претендент";
            }

            if(regAndAuth.AddUser(role, log, pass) == true)
            {                
                g1.Children.Clear();
                Frame mainFrame = new Frame();
                g2.Children.Add(mainFrame);

                if (role == "Работник")
                {
                    mainFrame.NavigationService.Navigate(newWorkerPage, UriKind.Relative);
                }

                if (role == "Претендент")
                {
                    mainFrame.NavigationService.Navigate(newChallengerPage, UriKind.Relative);
                }
            }
        }
    }
}
