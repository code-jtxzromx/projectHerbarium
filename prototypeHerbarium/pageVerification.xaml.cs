using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageVerification.xaml
    /// </summary>
    public partial class pageVerification : Page
    {
        public pageVerification()
        {
            InitializeComponent();

            getPlantDeposit();
        }

        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            PlantDeposit plantDeposit = dgrVerifyingDeposit.SelectedValue as PlantDeposit;
            PlantDeposit plantDetails = new PlantDeposit();
            string referenceAccession = "";
            string scientificName = "";

            // Query Command Setting
            connection.setQuery("SELECT strAccessionNumber, picHerbariumSheet, " +
                                        "CONVERT(VARCHAR, dateCollected, 107), CONVERT(VARCHAR, dateDeposited, 107), " +
                                        "strFullLocality, strCollector, strDescription " +
                                "FROM viewPlantDeposit " +
                                "WHERE strAccessionNumber = @accessionNo");
            connection.addParameter("@accessionNo", SqlDbType.VarChar, plantDeposit.AccessionNumber);

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            pnlPlantDeposit.Visibility = Visibility.Visible;

            while (sqlData.Read())
            {
                byte[] tempBlob = (byte[])sqlData[1];
                picHerbariumSheet.Source = getHerbariumSheet(tempBlob);

                lblAccessionNumber.Text = sqlData[0].ToString();
                lblDateCollected.Text = sqlData[2].ToString();
                lblDateDeposited.Text = sqlData[3].ToString();
                lblLocality.Text = sqlData[4].ToString();
                lblCollector.Text = sqlData[5].ToString();
                lblDescription.Text = sqlData[6].ToString();

                plantDetails.AccessionNumber = sqlData[0].ToString();
                plantDetails.DateCollected = sqlData[2].ToString();
                plantDetails.DateDeposited = sqlData[3].ToString();
                plantDetails.Locality = sqlData[4].ToString();
                plantDetails.Collector = sqlData[5].ToString();
                plantDetails.Description = sqlData[6].ToString();
            }
            connection.closeResult();

            getSpeciesList();
            getAccessionNumbers(lblAccessionNumber.Text);
            if(isDuplicateHerbarium(plantDetails, ref referenceAccession, ref scientificName))
            {
                pnlDuplicateMessage.Visibility = Visibility.Visible;
                chkIsDuplicate.IsChecked = true;
                chkIsDuplicate_CheckChanged(chkIsDuplicate, null);

                cbxReferenceNumber.SelectedItem = referenceAccession;
                cbxScientificName.SelectedItem = scientificName;
            }
            else
            {
                pnlDuplicateMessage.Visibility = Visibility.Collapsed;
                chkIsDuplicate.IsChecked = false;
                chkIsDuplicate_CheckChanged(chkIsDuplicate, null);
            }
        }

        private void chkIsDuplicate_CheckChanged(object sender, RoutedEventArgs e) 
            => ctrlReference.Visibility = (chkIsDuplicate.IsChecked == true) ? Visibility.Visible : Visibility.Collapsed;

        private void cbxScientificName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DatabaseConnection connection = new DatabaseConnection();
            if (cbxScientificName.SelectedIndex == -1)
            {
                txfCommonName.Text = "";
            }
            else
            {
                connection.setQuery("SELECT strCommonName FROM viewTaxonSpecies WHERE strScientificName = @name");
                connection.addParameter("@name", System.Data.SqlDbType.VarChar, cbxScientificName.SelectedItem.ToString());

                SqlDataReader sqlData = connection.executeResult();
                while (sqlData.Read())
                {
                    txfCommonName.Text = sqlData[0].ToString();
                }
                connection.closeResult();
            }
        }

        private void txfCommonName_TextChanged(object sender, TextChangedEventArgs e)
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strScientificName FROM viewTaxonSpecies WHERE strCommonName = @name");
            connection.addParameter("@name", System.Data.SqlDbType.VarChar, txfCommonName.Text);

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxScientificName.SelectedItem = sqlData[0].ToString();
            }
            connection.closeResult();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e) => pnlPlantDeposit.Visibility = Visibility.Hidden;

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Do you want to Confirm Verification of this Plant Deposit", 
                                     "Confirm Verify Plant",
                                     MessageBoxButton.YesNo,
                                     MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                int status;
                string refAccession = (chkIsDuplicate.IsChecked == false) 
                                       ? lblAccessionNumber.Text : cbxReferenceNumber.SelectedItem.ToString();

                //MessageBox.Show(lblAccessionNumber.Text + "\n" + refAccession + "\n" + cbxScientificName.SelectedItem + "\n" + StaticData.staffname);

                DatabaseConnection connection = new DatabaseConnection();
                connection.setStoredProc("dbo.procVerifyPlantDeposit");
                connection.addSprocParameter("@accessionNo", SqlDbType.VarChar, lblAccessionNumber.Text);
                connection.addSprocParameter("@referenceNo", SqlDbType.VarChar, refAccession);
                connection.addSprocParameter("@taxonName", SqlDbType.VarChar, cbxScientificName.SelectedItem);
                connection.addSprocParameter("@validatorName", SqlDbType.VarChar, StaticData.staffname);
                status = connection.executeProcedure();

                switch (status)
                {
                    case 0:
                        MessageBox.Show("Herbarium Sheet is Now Verified", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case 1:
                        MessageBox.Show("Transaction Failed, The system had run to an Error", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
                pnlPlantDeposit.Visibility = Visibility.Hidden;

                getPlantDeposit();
            }
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            /*
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("UPDATE tblPlantDeposit SET strStatus = 'Pending Verification' " +
                                "WHERE strDepositNumber = @plantDepositNo");
            connection.addParameter("@plantDepositNo", SqlDbType.VarChar, lblDepositNumber.Text);

            connection.executeCommand();

            MessageBox.Show("Herbarium Sheet will be Verified to Other Herbarium Centers", "Record Saved");

            pnlPlantDeposit.Visibility = Visibility.Hidden;

            getPlantDeposit();
            */
        }

        private void getPlantDeposit()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<PlantDeposit> plantDeposits = new List<PlantDeposit>();

            // Query Command Setting
            connection.setQuery("SELECT strAccessionNumber, CONVERT(VARCHAR, dateDeposited, 107), strCollector " +
                                "FROM viewPlantDeposit " +
                                "WHERE strStatus = 'For Verification'");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                plantDeposits.Add(new PlantDeposit()
                {
                    AccessionNumber = sqlData[0].ToString(),
                    DateDeposited = sqlData[1].ToString(),
                    Collector = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrVerifyingDeposit.ItemsSource = plantDeposits;
        }

        public BitmapImage getHerbariumSheet(byte[] blob)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(blob);
            image.EndInit();
            return image;
        }

        private void getSpeciesList()
        {
            cbxScientificName.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strScientificName FROM viewTaxonSpecies");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxScientificName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private void getAccessionNumbers(string accessionNumber)
        {
            cbxReferenceNumber.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strReferenceAccession FROM viewHerbariumSheet WHERE strReferenceAccession <> @accessionNo");
            connection.addParameter("@accessionNo", SqlDbType.VarChar, accessionNumber);

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxReferenceNumber.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool isDuplicateHerbarium(PlantDeposit deposit, ref string refAccession, ref string taxonName)
        {
            bool result;

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT DISTINCT strReferenceAccession, strScientificName " +
                                "FROM viewHerbariumSheet " +
                                "WHERE strCollector = @collector " +
                                    "AND strFullLocality = @locality " +
                                    "AND dateCollected = @dateCollected " +
                                    "AND strDescription = @description");
            connection.addParameter("@collector", SqlDbType.VarChar, deposit.Collector);
            connection.addParameter("@locality", SqlDbType.VarChar, deposit.Locality);
            connection.addParameter("@dateCollected", SqlDbType.Date, deposit.DateCollected);
            connection.addParameter("@description", SqlDbType.VarChar, deposit.Description);

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                refAccession = sqlData[0].ToString();
                taxonName = sqlData[1].ToString();
            }
            result = sqlData.HasRows;
            connection.closeResult();

            return result;
        }
    }
}

public class PlantDeposit
{
    public string AccessionNumber { get; set; }
    public string DateCollected { get; set; }
    public string DateDeposited { get; set; }
    public string Locality { get; set; }
    public string Collector { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Account { get; set; }
}
