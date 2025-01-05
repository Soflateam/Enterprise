using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Enterprise.App;
using Path = System.IO.Path;

namespace Enterprise
{
    /// <summary>
    /// Interaction logic for EmployeeEdit.xaml
    /// </summary>
    public partial class EmployeeEdit : Page
    {

        public EmployeeEdit()
        {
            DataContext = (App)Application.Current;
        }

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask the user for confirmation before saving
            MessageBoxResult saveresult = MessageBox.Show(
                "Are you sure you want to save the changes?",
                "Confirm Save",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (saveresult == MessageBoxResult.Yes)
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
                        existingEmployee.EmployeeFirstName = EmployeeFirstNameTextBox.Text;
                        existingEmployee.EmployeeMiddleName = EmployeeMiddleNameTextBox.Text;
                        existingEmployee.EmployeeLastName = EmployeeLastNameTextBox.Text;
                        existingEmployee.EmployeePreferredName = EmployeePreferredNameTextBox.Text;
                        existingEmployee.EmployeeTitle = EmployeeTitleTextBox.Text;
                        existingEmployee.EmployeeDepartment = EmployeeDepartmentTextBox.Text;
                        existingEmployee.EmployeeManager = EmployeeManagerTextBox.Text;
                        existingEmployee.EmployeeType = EmployeeTypeTextBox.Text;
                        existingEmployee.EmployeeEmail = EmployeeEmailTextBox.Text;
                        existingEmployee.EmployeePhone = EmployeePhoneTextBox.Text;
                        existingEmployee.EmployeeLocation = EmployeeLocationTextBox.Text;
                        existingEmployee.EmployeeAddress = EmployeeAddressTextBox.Text;
                        existingEmployee.EmployeeCity = EmployeeCityTextBox.Text;
                        existingEmployee.EmployeeState = EmployeeStateTextBox.Text;
                        existingEmployee.EmployeeZip = EmployeeZipTextBox.Text;

                        if (!string.IsNullOrEmpty(employee.EmployeeImagePath))
                        {
                            // Update the image path for the employee if a new image has been uploaded
                            EmployeeImage.Source = new BitmapImage(new Uri(employee.EmployeeImagePath));
                        }
                    }
                    else
                    {
                        // If not found, add as a new employee
                        employee.EmployeeFirstName = EmployeeFirstNameTextBox.Text;
                        employee.EmployeeMiddleName = EmployeeMiddleNameTextBox.Text;
                        employee.EmployeeLastName = EmployeeLastNameTextBox.Text;
                        employee.EmployeePreferredName = EmployeePreferredNameTextBox.Text;
                        employee.EmployeeTitle = EmployeeTitleTextBox.Text;
                        employee.EmployeeDepartment = EmployeeDepartmentTextBox.Text;
                        employee.EmployeeManager = EmployeeManagerTextBox.Text;
                        employee.EmployeeType = EmployeeTypeTextBox.Text;
                        employee.EmployeeEmail = EmployeeEmailTextBox.Text;
                        employee.EmployeePhone = EmployeePhoneTextBox.Text;
                        employee.EmployeeLocation = EmployeeLocationTextBox.Text;
                        employee.EmployeeAddress = EmployeeAddressTextBox.Text;
                        employee.EmployeeCity = EmployeeCityTextBox.Text;
                        employee.EmployeeState = EmployeeStateTextBox.Text;
                        employee.EmployeeZip = EmployeeZipTextBox.Text;

                        if (!string.IsNullOrEmpty(employee.EmployeeImagePath))
                        {
                            // Update the image path for the employee if a new image has been uploaded
                            EmployeeImage.Source = new BitmapImage(new Uri(employee.EmployeeImagePath));
                        }
                        else
                        {
                            employee.EmployeeImagePath = "pack://application:,,,/Assets/Images/EmployeePlaceholder.jpg";
                            EmployeeImage.Source = new BitmapImage(new Uri(employee.EmployeeImagePath, UriKind.Absolute));
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
        }

        public void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the EmployeeView page
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.ContentFrame.Source = new Uri("EmployeeView.xaml", UriKind.Relative);
            ClearEmployeeEditForm();
        }

        private void ClearEmployeeEditForm()
        {
            // Clear all textboxes
            EmployeeFirstNameTextBox.Clear();
            EmployeeMiddleNameTextBox.Clear();
            EmployeeLastNameTextBox.Clear();
            EmployeePreferredNameTextBox.Clear();
            EmployeeTitleTextBox.Clear();
            EmployeeDepartmentTextBox.Clear();
            EmployeeManagerTextBox.Clear();
            EmployeePhoneTextBox.Clear();
            EmployeeEmailTextBox.Clear();
            EmployeeTypeTextBox.Clear();
            EmployeeLocationTextBox.Clear();
            EmployeeAddressTextBox.Clear();
            EmployeeCityTextBox.Clear();
            EmployeeStateTextBox.Clear();
            EmployeeZipTextBox.Clear();

            // Reset the image to the placeholder or the original image
            EmployeeImage.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Images/EmployeePlaceholder.jpg"));

            ((App)Application.Current).LoadDataFromFileEmployees();
        }

        private void UploadPictureButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is EmployeeData employee)
            {
                if (string.IsNullOrEmpty(employee.EmployeeFirstName))
                {
                    MessageBox.Show("Please enter a Name first.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

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

                    EmployeeImage.Source = null;  // Release old image

                    // Access the ImageBrush by its name and clear the ImageSource
                    var imageBrush = (ImageBrush)this.FindName("EmployeeImageListView");
                    if (imageBrush != null)
                    {
                        imageBrush.ImageSource = null;  // Release old image
                    }


                    // Search for old photos in the directory containing the employee's name
                    var oldFiles = Directory.GetFiles(destinationDirectory)
                                            .Where(file => file.Contains(employee.EmployeeFirstName, StringComparison.OrdinalIgnoreCase))
                                            .ToList();

                    // Delete old files if they exist
                    foreach (var oldFile in oldFiles)
                    {
                        try
                        {
                            // Check if the file is in use (locked) before deleting
                            if (IsFileLocked(oldFile))
                            {
                                continue;
                            }

                            File.Delete(oldFile);  // Try to delete the old image
                        }
                        catch
                        {

                        }
                    }

                    // Sanitize the employee's name to create a valid file name
                    string sanitizedEmployeeName = string.Join("_", employee.EmployeeFirstName.Split(Path.GetInvalidFileNameChars()));
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
                        // Load the image
                        using (Bitmap sourceBitmap = new Bitmap(sourceFile))
                        {

                            RotateImageIfNeeded(sourceBitmap);

                            // Set the maximum width or height for the scaled image
                            int maxWidth = 500; 
                            int maxHeight = 500;

                            // Calculate the aspect ratio
                            float aspectRatio = (float)sourceBitmap.Width / sourceBitmap.Height;

                            int newWidth = maxWidth;
                            int newHeight = (int)(maxWidth / aspectRatio); // Scale height based on width

                            // If the calculated height exceeds the max height, adjust the width
                            if (newHeight > maxHeight)
                            {
                                newHeight = maxHeight;
                                newWidth = (int)(maxHeight * aspectRatio); // Scale width based on height
                            }

                            // Create a new resized bitmap while maintaining the aspect ratio
                            using (Bitmap resizedBitmap = new Bitmap(sourceBitmap, new System.Drawing.Size(newWidth, newHeight)))
                            {
                                // Save the resized image to the destination path
                                resizedBitmap.Save(destinationPath, ImageFormat.Jpeg);
                            }
                        }

                        // Update the employee's ImagePath property with the new file path (string)
                        employee.EmployeeImagePath = destinationPath;

                        // Refresh the Image control
                        if (employee.EmployeeImagePath != null)
                        {
                            // Update the Source of the Image control to show the new picture
                            EmployeeImage.Source = new BitmapImage(new Uri(destinationPath));
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        // Function to handle rotation if needed (based on EXIF data)
        private void RotateImageIfNeeded(Bitmap image)
        {
            const int orientationId = 0x0112; // EXIF tag for orientation
            if (image.PropertyIdList.Contains(orientationId))
            {
                int orientationValue = image.GetPropertyItem(orientationId).Value[0];

                switch (orientationValue)
                {
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }
            }
        }

        // Function to check if the file is locked (in use)
        private bool IsFileLocked(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    // If we can open the file for reading and writing with no sharing, the file is not locked
                    return false;
                }
            }
            catch (IOException)
            {
                // If the file is locked (in use), an IOException will be thrown
                return true;
            }
        }
    }
}
