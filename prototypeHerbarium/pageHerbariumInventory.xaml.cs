using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageHerbariumInventory.xaml
    /// </summary>
    public partial class pageHerbariumInventory : Page
    {
        public pageHerbariumInventory()
        {
            InitializeComponent();

            getFamilyBoxes();
        }
        
        private void FamilyBox_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FamilyBox box = button.DataContext as FamilyBox;
            
            getHerbariumInventory(box.BoxNumber);
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            HerbariumSheet herbariumSheet = dgrHerbariumSheets.SelectedValue as HerbariumSheet;

            // Query Command Setting
            connection.setQuery("SELECT strAccessionNumber, strBoxNumber, strFamilyName, strScientificName, " +
                                    "strCommonName, dateCollected, dateDeposited, dateVerified, strFullLocality, " +
                                    "strCollector, strValidator, strDescription, boolLoanAvailable, strStatus " +
                                    "FROM viewHerbariumInventory " +
                                "WHERE strAccessionNumber = @accessionNo");
            connection.addParameter("@accessionNo", SqlDbType.VarChar, herbariumSheet.AccessionNumber);

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                lblAccessionNumber.Text = sqlData[0].ToString();
                lblBox.Text = sqlData[1].ToString();
                lblFamilyName.Text = "  [" + sqlData[2].ToString() + "]";
                lblScientificName.Text = sqlData[3].ToString();
                lblCommonName.Text = sqlData[4].ToString();
                lblDateCollected.Text = sqlData[5].ToString();
                lblDateDeposited.Text = sqlData[6].ToString();
                lblDateVerified.Text = sqlData[7].ToString();
                lblLocality.Text = sqlData[8].ToString();
                lblCollector.Text = sqlData[9].ToString();
                lblValidator.Text = sqlData[10].ToString();
                lblDescription.Text = sqlData[11].ToString();
                lblAvail.Text = (bool)sqlData[12] ? "Available" : "Not Available";
                lblStatus.Text = sqlData[13].ToString();
                btnAvail.Visibility = (sqlData[13].ToString() == "Loaned") ? Visibility.Hidden : Visibility.Visible;

                pnlPlantDeposit.Visibility = Visibility.Visible;
            }
            connection.closeResult();

            getHerbariumImage(lblAccessionNumber.Text);
        }
        
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlPlantDeposit.Visibility = Visibility.Hidden;
        }

        private void btnAvail_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Do you want this Herbarium Sheet to available for Loaning",
                                    "Herbarium Sheet Availability",
                                    MessageBoxButton.YesNoCancel,
                                    MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                changeAvailability(lblAccessionNumber.Text, true);   
            }
            else if(result == MessageBoxResult.No)
            {
                changeAvailability(lblAccessionNumber.Text, false);
            }
        }

        public void getHerbariumInventory(string boxNumber)
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<HerbariumSheet> herbariumSheets = new List<HerbariumSheet>();

            // Query Command Setting
            connection.setQuery("SELECT strAccessionNumber, strScientificName, strBoxNumber, strStatus " +
                                "FROM viewHerbariumInventory " +
                                "WHERE strBoxNumber = @boxNumber");
            connection.addParameter("@boxNumber", SqlDbType.VarChar, boxNumber);

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                herbariumSheets.Add(new HerbariumSheet()
                {
                    AccessionNumber = sqlData[0].ToString(),
                    ScientificName = sqlData[1].ToString(),
                    BoxLocation = sqlData[2].ToString(),
                    Status = sqlData[3].ToString()
                });
            }
            connection.closeResult();

            dgrHerbariumSheets.ItemsSource = herbariumSheets;
        }

        public void getFamilyBoxes()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<FamilyBox> familyBoxes = new List<FamilyBox>();

            connection.setQuery("SELECT strBoxNumber, strFamilyName FROM viewFamilyBox");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                familyBoxes.Add(new FamilyBox()
                {
                    BoxNumber = sqlData[0].ToString(),
                    Family = sqlData[1].ToString()
                });
            }
            connection.closeResult();

            lstFamilyBox.ItemsSource = familyBoxes;
        }

        public BitmapImage getHerbariumSheet(byte[] blob)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(blob);
            image.EndInit();
            return image;
        }

        private void getHerbariumImage(string accessionNumber)
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT picHerbariumSheet FROM viewHerbariumPicture WHERE strAccessionNumber = @accessionNumber");
            connection.addParameter("@accessionNumber", SqlDbType.VarChar, accessionNumber);

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                try
                {
                    byte[] tempBlob = (byte[])sqlData[0];
                    picHerbariumSheet.Source = getHerbariumSheet(tempBlob);
                }
                catch (Exception) { }
            }
            connection.closeResult();
        }

        public void changeAvailability(string AccessionNumber, bool AvailableStatus)
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("UPDATE tblStoredHerbarium " +
                                "SET boolLoanAvailable = @availability " +
                                "WHERE intHerbariumSheetID = (SELECT intHerbariumSheetID " +
                                                             "FROM viewHerbariumSheet " +
                                                             "WHERE strAccessionNumber = @accessionNumber)");
            connection.addParameter("@availability", SqlDbType.Bit, AvailableStatus);
            connection.addParameter("@accessionNumber", SqlDbType.VarChar, AccessionNumber);

            connection.executeCommand();

            string message = AvailableStatus ?
                            "Herbarium Sheet is Now Available for Loaning" : 
                            "Herbarium Sheet is Now Unavailable for Loaning";
            MessageBox.Show(message, "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);

            pnlPlantDeposit.Visibility = Visibility.Hidden;
            btnReturn_Click(btnReturn, null);

            getFamilyBoxes();
        }
    }
}

