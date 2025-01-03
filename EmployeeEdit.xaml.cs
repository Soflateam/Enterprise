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
            // Check if DataContext is bound to an EmployeeData object
            if (DataContext is EmployeeData employee)
            {
                // Check if the employee is already in the collection
                ObservableCollection<EmployeeData> employees = ((App)Application.Current).Employees;
                var existingEmployee = employees.FirstOrDefault(e => e == employee);

                if (existingEmployee != null)
                {
                    // Update the existing employee
                    existingEmployee.EmployeeName = FullNameTextBox.Text;
                    existingEmployee.EmployeeTitle = TitleTextBox.Text;
                    existingEmployee.EmployeePhone = PhoneTextBox.Text;
                    existingEmployee.EmployeeEmail = EmailTextBox.Text;
                }
                else
                {
                    // If not found, add as a new employee
                    employee.EmployeeName = FullNameTextBox.Text;
                    employee.EmployeeTitle = TitleTextBox.Text;
                    employee.EmployeePhone = PhoneTextBox.Text;
                    employee.EmployeeEmail = EmailTextBox.Text;
                    employees.Add(employee);
                }
            }

            // Save the data to the file
            ((App)Application.Current).SaveDataToFileEmployees();

            // Navigate back to the EmployeeView page
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Source = new Uri("EmployeeView.xaml", UriKind.Relative);
        }

        public void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the EmployeeView page
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Source = new Uri("EmployeeView.xaml", UriKind.Relative);

        }

    }
}
