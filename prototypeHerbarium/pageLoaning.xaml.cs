using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageLoaning.xaml
    /// </summary>
    public partial class pageLoaning : Page
    {
        public pageLoaning()
        {
            InitializeComponent();

            //getCollectorList();
            //getTaxonList();
            setDurationType();
            //getLoanTable();
        }

        private void btnAddLoan_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddLoan.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddLoan.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddLoan.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddLoan.Content = (state) ? "Close Panel" : "New Loan";
        }

        private void cbxTaxonName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (cbxTaxonName.SelectedIndex != -1)
            {
                DatabaseConnection connection = new DatabaseConnection();
                connection.setQuery("SELECT COUNT(intStoredSheetID) FROM viewHerbariumInventory " +
                    "WHERE boolLoanAvailable = 1 AND strScientificName = @taxonName GROUP BY strScientificName");
                connection.addParameter("@taxonName", SqlDbType.VarChar, cbxTaxonName.SelectedItem.ToString());

                SqlDataReader sqlData = connection.executeResult();
                while (sqlData.Read())
                {
                    txfNoSpecimens.Text = sqlData[0].ToString();
                }
                connection.closeResult();
            }
            */
        }

        private void txfNoCopies_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*
            try
            {
                int available = Convert.ToInt32(txfNoSpecimens.Text);
                int copies = Convert.ToInt32(txfNoCopies.Text);

                if(copies > available)
                {
                    MessageBox.Show("Required Number of Copies has exceeded the number of Available Specimens",
                                    "Loan Problem",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation);
                    txfNoCopies.Text = available.ToString();
                    txfNoCopies.Focus();
                }
            }
            catch (FormatException) {}
            */
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cbxCollector.SelectedIndex = -1;
            cbxTaxonName.SelectedIndex = -1;
            txfNoSpecimens.Clear();
            dpkLoanDate.Text = "";
            txfDuration.Text = "";
            cbxDuration.SelectedIndex = -1;
            txfNoCopies.Text = "";
            txfPurpose.Text = "";
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            pnlConfirmationForm.Visibility = Visibility.Visible;

            lblCollector.Text = cbxCollector.SelectedItem.ToString();
            lblTaxonName.Text = cbxTaxonName.SelectedItem.ToString();
            lblLoanDate.Text = dpkLoanDate.Text;
            lblDuration.Text = txfDuration.Text;
            lblDurationType.Text = cbxDuration.SelectedItem.ToString();
            lblCopies.Text = txfNoCopies.Text;
            lblPurpose.Text = txfPurpose.Text;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlConfirmationForm.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            /*
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procProcessLoan");
            connection.addSprocParameter("@collectorName", SqlDbType.VarChar, lblCollector.Text);
            connection.addSprocParameter("@taxonName", SqlDbType.VarChar, lblTaxonName.Text);
            connection.addSprocParameter("@startDate", SqlDbType.Date, lblLoanDate.Text);
            connection.addSprocParameter("@duration", SqlDbType.Int, lblDuration.Text);
            connection.addSprocParameter("@durationMode", SqlDbType.VarChar, lblDurationType.Text);
            connection.addSprocParameter("@copies", SqlDbType.Int, lblCopies.Text);
            connection.addSprocParameter("@purpose", SqlDbType.VarChar, lblPurpose.Text);

            connection.executeStoredProc();

            MessageBox.Show("Loan Process Saved", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);

            pnlConfirmationForm.Visibility = Visibility.Hidden;
            btnClear_Click(btnClear, null);

            getLoanTable();
            getTaxonList();
            getCollectorList();
            */
        }

        private void getCollectorList()
        {
            /*
            cbxCollector.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strFullName FROM viewCollector");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxCollector.Items.Add(sqlData[0]);
            }
            connection.closeResult();
            */
        }

        private void getTaxonList()
        {
            /*
            cbxTaxonName.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strScientificName FROM viewHerbariumInventory " +
                                "WHERE boolLoanAvailable = 1 GROUP BY strScientificName");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxTaxonName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
            */
        }

        public void getLoanTable()
        {
            /*
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<PlantLoans> loans = new List<PlantLoans>();

            // Query Command Setting
            connection.setQuery("SELECT strLoanNumber, strCollector, strScientificName, " +
                                    "CONCAT(CONVERT(VARCHAR, dateLoan, 107), ' - ', CONVERT(VARCHAR, dateReturning, 107)), " +
                                "strStatus FROM viewPlantLoans");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                loans.Add(new PlantLoans()
                {
                    LoanNumber = sqlData[0].ToString(),
                    Collector = sqlData[1].ToString(),
                    TaxonName = sqlData[2].ToString(),
                    Duration = sqlData[3].ToString(),
                    Status = sqlData[4].ToString()
                });
            }
            connection.closeResult();
            
            dgrPlantLoans.ItemsSource = loans;
            */
        }

        private void setDurationType()
        {
            string[] duration = new string[] { "Day/s", "Month/s" };

            foreach(string range in duration)
            {
                cbxDuration.Items.Add(range);
            }
        }
    }
}

public class PlantLoans
{
    public string LoanNumber { get; set; }
    public string Collector { get; set; }
    public string TaxonName { get; set; }
    public string StartDate { get; set; }
    public string ReturningDate { get; set; }
    public string Duration { get; set; }
    public string Copies { get; set; }
    public string DateProcessed { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
}