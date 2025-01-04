using System.Windows;

namespace Enterprise
{
    public partial class ConfirmationDialog : Window
    {
        public bool IsConfirmed { get; private set; }

        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if the user entered the correct string
            if (ConfirmationTextBox.Text.Equals("DeleteThisRecord", StringComparison.Ordinal))
            {
                IsConfirmed = true;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please enter DeleteThisRecord to confirm deletion.\n" +
                    "\n" +
                    "\n" +
                    "             Press Cancel or close window to exit.");
            }
        }

        private void CancelPromptButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
