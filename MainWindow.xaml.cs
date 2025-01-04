using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Enterprise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = (App)Application.Current;
        }


        public void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /* Removed logic for maximizing window on double click
                private bool IsMaximized = false;
                public void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
                {
                    if (e.ClickCount == 2)
                    {
                        if (IsMaximized)
                        {
                            this.WindowState = WindowState.Normal;
                            this.Width = 1080;
                            this.Height = 720;

                            IsMaximized = false;
                        }
                        else
                        {
                            this.WindowState = WindowState.Maximized;

                            IsMaximized = true;
                        }
                    }
                }
        */
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
                this.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SoftwareViewButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EmployeesViewButton_Click(object sender, RoutedEventArgs e)
        {

            ContentFrame.Source = new Uri("EmployeeView.xaml", UriKind.Relative);
        }

        private void DashboardViewButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Source = new Uri("Dashboard.xaml", UriKind.Relative);
        }

    }
}