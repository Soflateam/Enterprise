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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _userName;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            UserName = Environment.UserName;

            ObservableCollection<EmployeeData> employeeDatas = new ObservableCollection<EmployeeData>();
            employeeDatas.Add(new EmployeeData { EmployeeName = "Bill Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "3607181107", EmployeeEmail = "Bill@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Alexander Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "9542634342", EmployeeEmail = "Alexander@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Stellan Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "7572535555", EmployeeEmail = "Stellan@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Bill Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "3607181107", EmployeeEmail = "Bill@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Alexander Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "9542634342", EmployeeEmail = "Alexander@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Stellan Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "7572535555", EmployeeEmail = "Stellan@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Bill Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "3607181107", EmployeeEmail = "Bill@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Alexander Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "9542634342", EmployeeEmail = "Alexander@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Stellan Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "7572535555", EmployeeEmail = "Stellan@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Bill Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "3607181107", EmployeeEmail = "Bill@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Alexander Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "9542634342", EmployeeEmail = "Alexander@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Stellan Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "7572535555", EmployeeEmail = "Stellan@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Bill Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "3607181107", EmployeeEmail = "Bill@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Alexander Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "9542634342", EmployeeEmail = "Alexander@Skarsgard.com" });
            employeeDatas.Add(new EmployeeData { EmployeeName = "Stellan Skarsgard", EmployeeTitle = "Actor", EmployeePhone = "7572535555", EmployeeEmail = "Stellan@Skarsgard.com" });

            EmployeesDataGrid.ItemsSource = employeeDatas;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool IsMaximized = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
    }

    public class EmployeeData
    {
        public string EmployeeName { get; set; }
        public string EmployeeTitle { get; set; }
        public string EmployeePhone { get; set; }
        public string EmployeeEmail { get; set; }

    }

}