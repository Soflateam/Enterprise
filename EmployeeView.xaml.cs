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
using System.IO;
using Path = System.IO.Path;

namespace Enterprise
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Page, INotifyPropertyChanged
    {
        public EmployeeView()
        {
            InitializeComponent();

            // Initialize the filtered view
            _filteredEmployees = CollectionViewSource.GetDefaultView(((App)Application.Current).Employees);

            // Add the filter method
            _filteredEmployees.Filter = FilterEmployees;

            DataContext = this;
        }

        // Search and Filter Logic
        private string _searchText;
        private ICollectionView _filteredEmployees;

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

        // Filter method for the Employee Data Collection - Needs to be updated on property changes.
        private bool FilterEmployees(object obj)
        {
            if (obj is EmployeeData employee)
            {
                if (string.IsNullOrEmpty(SearchText))
                    return true;

                // Perform case-insensitive search
                return employee.EmployeeFirstName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeMiddleName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeLastName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeManager.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeDepartment.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeePreferredName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeTitle.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeePhone.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeType.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeLocation.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeAddress.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeCity.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeState.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeZip.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                       employee.EmployeeEmail.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            return false;
        }

        // Changes the active tab button style for the selected tab
        private void SetActiveTabButton(Button activeButton)
        {
            // Reset styles for all buttons in the tab panel
            foreach (var child in TabButtonPanel.Children) 
            {
                if (child is Button button)
                {
                    // Reset to the default style
                    button.Style = (Style)FindResource("TabButton"); 
                }
            }
            activeButton.Style = (Style)FindResource("ActiveTabButtonStyle");
        }

        // Tab Button for All Employees - Filter All
        public void FilterAllButton_Click(object sender, RoutedEventArgs e)
        {
            _filteredEmployees.Filter = FilterEmployees;
            _filteredEmployees.Refresh();
            SetActiveTabButton((Button)sender);
        }

        // Tab Button for Permanent Employees - Filter Permanent
        public void FilterPermanentButton_Click(object sender, RoutedEventArgs e)
        {
            _filteredEmployees.Filter = employee =>
            {
                if (employee is EmployeeData emp)
                {
                    return emp.EmployeeType.Equals("Permanent", StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
            _filteredEmployees.Refresh();
            SetActiveTabButton((Button)sender);
        }

        // Tab Button for Temporary Employees - Filter Temporary
        public void FilterTemporaryButton_Click(object sender, RoutedEventArgs e)
        {
            _filteredEmployees.Filter = employee =>
            {
                if (employee is EmployeeData emp)
                {
                    return emp.EmployeeType.Equals("Temporary", StringComparison.OrdinalIgnoreCase);
                }
                return false;
            };
            _filteredEmployees.Refresh();
            SetActiveTabButton((Button)sender);
        }

        // General purpose Property Changed Logic
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Button Click Events
        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Source = new Uri("EmployeeEdit.xaml", UriKind.Relative);

            // Once navigation is complete, set a blank DataContext for adding a new employee
            mainWindow.ContentFrame.NavigationService.LoadCompleted += (s, args) =>
            {
                if (mainWindow.ContentFrame.Content is EmployeeEdit employeeEdit)
                {
                    var newEmployee = new EmployeeData();
                    newEmployee.EmployeeImagePath = "pack://application:,,,/Assets/Images/EmployeePlaceholder.jpg"; // Set the placeholder image
                    employeeEdit.DataContext = newEmployee;

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
            // Get the selected employee from the DataGrid
            EmployeeData selectedEmployee = (EmployeeData)EmployeesDataGrid.SelectedItem;

            if (selectedEmployee != null)
            {
                // Show a confirmation dialog box
                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to Permanently delete this employee?\n",
                    "Confirm Employee Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Create and show the custom confirmation dialog
                    ConfirmationDialog confirmationDialog = new ConfirmationDialog();
                    bool? secondaryresult = confirmationDialog.ShowDialog();

                    if (secondaryresult == true && confirmationDialog.IsConfirmed)
                    {

                        // Define the directory where employee photos are stored
                        string destinationDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmployeePhotos");

                        // Search for photos associated with the employee (based on their name)
                        var matchingFiles = Directory.GetFiles(destinationDirectory)
                                                      .Where(file => file.Contains(selectedEmployee.EmployeeFirstName, StringComparison.OrdinalIgnoreCase))
                                                      .ToList();

                        // Refresh the DataGrid to update the UI and release any bindings
                        EmployeesDataGrid.Items.Refresh();

                        // Delete the matching photo files
                        foreach (var file in matchingFiles)
                        {
                            try
                            {
                                // Delete the file
                                File.Delete(file);
                            }
                            catch
                            {

                            }
                        }

                // Remove the employee from the Employees collection
                ((App)Application.Current).Employees.Remove(selectedEmployee);

                        // Save the updated data to the file
                        ((App)Application.Current).SaveDataToFileEmployees();

                        // Optionally, refresh the DataGrid to reflect the changes
                        EmployeesDataGrid.Items.Refresh();
                    }
                }
            }
        }
    }
}