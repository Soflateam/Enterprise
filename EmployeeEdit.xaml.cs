using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Path = System.IO.Path;

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
                    existingEmployee.EmployeeType = EmployeeTypeTextBox.Text;
                    if (!string.IsNullOrEmpty(employee.EmployeeImagePath))
                    {
                        // Update the image path for the employee if a new image has been uploaded
                        EmployeeImage.Source = new BitmapImage(new Uri(employee.EmployeeImagePath));
                    }
                }
                else
                {
                    // If not found, add as a new employee
                    employee.EmployeeName = FullNameTextBox.Text;
                    employee.EmployeeTitle = TitleTextBox.Text;
                    employee.EmployeePhone = PhoneTextBox.Text;
                    employee.EmployeeEmail = EmailTextBox.Text;
                    employee.EmployeeType = EmployeeTypeTextBox.Text;
                    if (!string.IsNullOrEmpty(employee.EmployeeImagePath))
                    {
                        // Update the image path for the employee if a new image has been uploaded
                        EmployeeImage.Source = new BitmapImage(new Uri(employee.EmployeeImagePath));
                    }

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

        private void UploadPictureButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an OpenFileDialog instance
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select a Picture"
            };

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == true)
            {
                string sourceFile = openFileDialog.FileName;
                string destinationDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmployeePhotos");

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                if (DataContext is EmployeeData employee)
                {
                    // Delete the old photo if it exists
                    if (!string.IsNullOrEmpty(employee.EmployeeImagePath) && File.Exists(employee.EmployeeImagePath))
                    {
                        try
                        {
                            File.Delete(employee.EmployeeImagePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting old photo: {ex.Message}");
                        }
                    }

                    // Sanitize the employee's name to create a valid file name
                    string sanitizedEmployeeName = string.Join("_", employee.EmployeeName.Split(Path.GetInvalidFileNameChars()));
                    string fileExtension = Path.GetExtension(sourceFile);
                    string destinationPath = Path.Combine(destinationDirectory, $"{sanitizedEmployeeName}{fileExtension}");

                    // Ensure no overwrites by appending a number if the file already exists
                    int counter = 1;
                    while (File.Exists(destinationPath))
                    {
                        destinationPath = Path.Combine(destinationDirectory, $"{sanitizedEmployeeName}_{counter}{fileExtension}");
                        counter++;
                    }

                    try
                    {
                        // Copy the file to the destination directory
                        File.Copy(sourceFile, destinationPath, true);

                        // Update the employee's ImagePath property with the new file path
                        employee.EmployeeImagePath = destinationPath;

                        // Refresh the Image control (if you have an Image control bound to the EmployeeImagePath)
                        if (employee.EmployeeImagePath != null)
                        {
                            // Assuming you have an Image control called "EmployeeImage"
                            // Update the Source of the Image control to show the new picture
                            EmployeeImage.Source = new BitmapImage(new Uri(destinationPath));
                        }

                        MessageBox.Show("Picture uploaded and replaced successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error uploading picture: {ex.Message}");
                    }
                }
            }
        }

    }
}
