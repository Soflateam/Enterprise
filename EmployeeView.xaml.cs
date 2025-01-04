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
using static Enterprise.EmployeeEdit;
using System.Globalization;
using System.ComponentModel;

namespace Enterprise
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Page, INotifyPropertyChanged
    {
        private string _searchText;
        private ICollectionView _filteredEmployees;

        public event PropertyChangedEventHandler PropertyChanged;

        public EmployeeView()
        {
            InitializeComponent();

            // Initialize the filtered view
            _filteredEmployees = CollectionViewSource.GetDefaultView(((App)Application.Current).Employees);

            // Add the filter method
            _filteredEmployees.Filter = FilterEmployees;

            DataContext = this;
        }

        public ICollectionView FilteredEmployees
        {
            get => _filteredEmployees;
            set
            {
                _filteredEmployees = value;
                OnPropertyChanged(nameof(FilteredEmployees));
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));

                // Refresh the filter when the search text changes
                _filteredEmployees.Refresh();
            }
        }

        private bool FilterEmployees(object obj)
        {
            if (obj is EmployeeData employee)
            {
                if (string.IsNullOrEmpty(SearchText))
                    return true;

                // Perform case-insensitive search
                return employee.EmployeeName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeTitle.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeePhone.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeEmail.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Source = new Uri("EmployeeEdit.xaml", UriKind.Relative);

            // Once navigation is complete, set a blank DataContext for adding a new employee
            mainWindow.ContentFrame.NavigationService.LoadCompleted += (s, args) =>
            {
                if (mainWindow.ContentFrame.Content is EmployeeEdit employeeEdit)
                {
                    employeeEdit.DataContext = new EmployeeData(); // Set empty DataContext for new entry
                }
            };
        }

        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected employee from the DataGrid
            EmployeeData selectedEmployee = (EmployeeData)EmployeesDataGrid.SelectedItem;

            if (selectedEmployee != null)
            {
                // Navigate to the EmployeeEdit page
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.ContentFrame.Source = new Uri("EmployeeEdit.xaml", UriKind.Relative);

                // Once navigation is complete, set the DataContext of EmployeeEdit
                mainWindow.ContentFrame.NavigationService.LoadCompleted += (s, args) =>
                {
                    if (mainWindow.ContentFrame.Content is EmployeeEdit employeeEdit)
                    {
                        employeeEdit.DataContext = selectedEmployee;
                    }
                };
            }
            else
            {
                MessageBox.Show("Please select an employee to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected employee from the ListBox
            EmployeeData selectedEmployee = (EmployeeData)EmployeesDataGrid.SelectedItem;

            if (selectedEmployee != null)
            {
                // Remove the employee from the Employees collection
                ((App)Application.Current).Employees.Remove(selectedEmployee);

                // Save the updated data to the file
                ((App)Application.Current).SaveDataToFileEmployees();


            }
        }
    }
}