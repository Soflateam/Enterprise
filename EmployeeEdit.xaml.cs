using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static Enterprise.App;
using static Enterprise.MainWindow;

namespace Enterprise
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeEdit : Page
    {

        public EmployeeEdit()
        {
            DataContext = (App)Application.Current;
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new employee object from the values entered in the text boxes
            EmployeeData newEmployee = new EmployeeData
            {
                EmployeeName = FullNameTextBox.Text,
                EmployeeTitle = TitleTextBox.Text,
                EmployeePhone = PhoneTextBox.Text,
                EmployeeEmail = EmailTextBox.Text
            };

            // Add the new employee to the Employees collection
            ((App)Application.Current).Employees.Add(newEmployee);

            // Save the data to the file
            ((App)Application.Current).SaveDataToFileEmployees();

            // Navigate back to the EmployeeView page
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Source = new Uri("EmployeeView.xaml", UriKind.Relative);

        }

    }
}
