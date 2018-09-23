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
        PlantDeposit plantDetails = new PlantDeposit();
        string referenceAccession = "";
        string scientificName = "";

        public pageVerification()
        {
            InitializeComponent();

            getPlantDeposit();
            getSpeciesList();
        }

        private void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection connection = new DatabaseConnection();
            PlantDeposit plantDeposit = dgrVerifyingDeposit.SelectedValue as PlantDeposit;

            connection.setQuery("SELECT intPlantDepositID, strAccessionNumber, picHerbariumSheet, " +
                                        "CONVERT(VARCHAR, dateCollected, 107), CONVERT(VARCHAR, dateDeposited, 107), " +
                                        "strFullLocality, strCollector, strDescription " +
                                "FROM viewPlantDeposit " +
                                "WHERE strAccessionNumber = @accessionNo");
            connection.addParameter("@accessionNo", SqlDbType.VarChar, plantDeposit.AccessionNumber);

            SqlDataReader sqlData = connection.executeResult();
            pnlPlantDeposit.Visibility = Visibility.Visible;
            while (sqlData.Read())
            {
                try {
                    byte[] tempBlob = (byte[])sqlData[2];
                    picHerbariumSheet.Source = getHerbariumSheet(tempBlob);
                }
                catch(Exception) { }

                lblDepositID.Text = sqlData[0].ToString();
                lblAccessionNumber.Text = sqlData[1].ToString();
                lblDateCollected.Text = sqlData[3].ToString();
                lblDateDeposited.Text = sqlData[4].ToString();
                lblLocality.Text = sqlData[5].ToString();
                lblCollector.Text = sqlData[6].ToString();
                lblDescription.Text = sqlData[7].ToString();

                plantDetails.AccessionNumber = sqlData[1].ToString();
                plantDetails.DateCollected = sqlData[3].ToString();
                plantDetails.DateDeposited = sqlData[4].ToString();
                plantDetails.Locality = sqlData[5].ToString();
                plantDetails.Collector = sqlData[6].ToString();
                plantDetails.Description = sqlData[7].ToString();
            }
            connection.closeResult();
        }

        private void chkIsDuplicate_CheckChanged(object sender, RoutedEventArgs e) => ctrlReference.Visibility = (chkIsDuplicate.IsChecked == true) ? Visibility.Visible : Visibility.Collapsed;

        private void cbxScientificName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxScientificName.SelectedIndex != -1)
            {
                getAccessionNumbers((cbxScientificName.SelectedItem as ComboBoxItem).Item);

                if (isDuplicateHerbarium(plantDetails, ref referenceAccession, ref scientificName))
                {
                    chkIsDuplicate.IsChecked = true;
                    chkIsDuplicate_CheckChanged(chkIsDuplicate, null);

                    foreach (var item in cbxReferenceNumber.Items)
                        if ((item as ComboBoxItem).Item == referenceAccession)
                            cbxReferenceNumber.SelectedItem = item;
                    foreach (var item in cbxScientificName.Items)
                        if ((item as ComboBoxItem).Item == scientificName)
                            cbxScientificName.SelectedItem = item;
                }
                else
                {
                    chkIsDuplicate.IsChecked = false;
                    chkIsDuplicate_CheckChanged(chkIsDuplicate, null);
                }
            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            picHerbariumSheet.Source = null;
            chkIsDuplicate.IsChecked = false;
            cbxScientificName.SelectedIndex = -1;
            cbxReferenceNumber.ItemsSource = null;
            pnlPlantDeposit.Visibility = Visibility.Hidden;
        }

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
                                       ? lblAccessionNumber.Text : (cbxReferenceNumber.SelectedItem as ComboBoxItem).Item;
                
                DatabaseConnection connection = new DatabaseConnection();
                connection.setStoredProc("dbo.procVerifyPlantDeposit");
                connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
                connection.addSprocParameter("@orgDeposit", SqlDbType.VarChar, lblAccessionNumber.Text);
                connection.addSprocParameter("@newDeposit", SqlDbType.VarChar, refAccession);
                connection.addSprocParameter("@species", SqlDbType.VarChar, (cbxScientificName.SelectedItem as ComboBoxItem).Item);
                connection.addSprocParameter("@validator", SqlDbType.VarChar, StaticData.staffname);
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
            int status;

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procExternalVerifyDeposit");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@orgDeposit", SqlDbType.VarChar, lblAccessionNumber.Text);
            if (cbxScientificName.SelectedIndex != -1)
            {
                string refAccession = (chkIsDuplicate.IsChecked == false)
                          ? lblAccessionNumber.Text : (cbxReferenceNumber.SelectedItem as ComboBoxItem).Item;

                connection.addSprocParameter("@newDeposit", SqlDbType.VarChar, refAccession);
                connection.addSprocParameter("@species", SqlDbType.VarChar, (cbxScientificName.SelectedItem as ComboBoxItem).Item);
            }
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Herbarium Sheet will be Verified to Other Herbarium Centers", "Record Saved");
                    break;
                case 1:
                    MessageBox.Show("Transaction Failed, The system had run to an Error", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }



            pnlPlantDeposit.Visibility = Visibility.Hidden;

            getPlantDeposit();
        }

        private void getPlantDeposit()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<PlantDeposit> plantDeposits = new List<PlantDeposit>();

            connection.setQuery("SELECT strAccessionNumber, CONVERT(VARCHAR, dateDeposited, 107), strCollector " +
                                "FROM viewPlantDeposit " +
                                "WHERE strStatus = 'For Verification'");

            SqlDataReader sqlData = connection.executeResult();
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
            cbxScientificName.ItemsSource = null;
            List<ComboBoxItem> speciesList = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intSpeciesID, strScientificName " +
                                "FROM viewTaxonSpecies " +
                                "ORDER BY strScientificName");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                speciesList.Add(new ComboBoxItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();
            cbxScientificName.ItemsSource = speciesList;
        }

        private void getAccessionNumbers(string taxonname)
        {
            cbxReferenceNumber.ItemsSource = null;
            List<ComboBoxItem> accessionList = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT DISTINCT intPlantReferenceID, strReferenceAccession " +
                                "FROM viewHerbariumSheet " +
                                "WHERE strScientificName = @taxonName");
            connection.addParameter("@taxonName", SqlDbType.VarChar, taxonname);

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                accessionList.Add(new ComboBoxItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();
            cbxReferenceNumber.ItemsSource = accessionList;
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
