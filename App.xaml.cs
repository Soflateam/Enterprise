using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Enterprise
{
    public partial class App : Application
    {
        // Startup Events and Data Loading Triggers
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = this;
            mainWindow.Show();

            LoadDataFromFileEmployees();
        }

        // Employee Related Functionality
        // Creates EmployeeData Collection for Employee related functionality
        public ObservableCollection<EmployeeData> Employees { get; set; } = new ObservableCollection<EmployeeData>();

        // EmployeeData Class for Employee related functionality - Needs updating when properties are changed
        public class EmployeeData
        {
            public string EmployeeName { get; set; }
            public string EmployeeTitle { get; set; }
            public string EmployeePhone { get; set; }
            public string EmployeeEmail { get; set; }
            public string EmployeeType { get; set; }
        }

        // Save and Load Data to and from File Logic - CSV Based - Employees
        public void SaveDataToFileEmployees()
        {
            try
            {
                var saveFilePath = "C:\\Temp\\EmployeeData.csv"; // Replace

                var configCsv = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true
                };

                using (var writer = new StreamWriter(saveFilePath))
                using (var csv = new CsvWriter(writer, configCsv))
                {
                    csv.WriteRecords(Employees);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void LoadDataFromFileEmployees()
        {
            try
            {
                var openFilePath = "C:\\Temp\\EmployeeData.csv"; // Replace


                var configCsv = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true
                };

                using (var reader = new StreamReader(openFilePath))
                using (var csv = new CsvReader(reader, configCsv))
                {
                    var records = csv.GetRecords<EmployeeData>().ToList();
                    Employees.Clear();

                    foreach (var record in records)
                    {
                        Employees.Add(record);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

