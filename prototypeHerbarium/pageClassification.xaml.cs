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
    /// Interaction logic for pageClassification.xaml
    /// </summary>
    public partial class pageClassification : Page
    {
        List<FamilyBox> availableBoxes = new List<FamilyBox>();

        public pageClassification()
        {
            InitializeComponent();

            getHerbariumSheet();
        }

        private void btnClassify_Click(object sender, RoutedEventArgs e)
        {
            getAvailableBoxes();

            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            HerbariumSheet herbariumSheet = dgrHerbariumSheets.SelectedValue as HerbariumSheet;
            
            // Query Command Setting
            connection.setQuery("SELECT strAccessionNumber, strReferenceAccession, picHerbariumSheet, " +
                                    "strFamilyName, strScientificName, strCommonName, CONVERT(VARCHAR, dateCollected, 107), " +
                                    "CONVERT(VARCHAR, dateDeposited, 107), CONVERT(VARCHAR, dateVerified, 107), strFullLocality, " +
                                    "strCollector, strValidator, strDescription " +
                                "FROM viewHerbariumSheet " +
                                "WHERE strAccessionNumber = @accessionNo");
            connection.addParameter("@accessionNo", SqlDbType.VarChar, herbariumSheet.AccessionNumber);

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                byte[] tempBlob = (byte[])sqlData[2];
                string family = sqlData[3].ToString();

                var result = from box in availableBoxes
                             where box.Family == family
                             where box.BoxLimit > 0
                             select box.BoxNumber;

                if (result.Count() == 0)
                {
                    MessageBox.Show("No Available Family Box for this Herbarium Sheet");
                }
                else
                {
                    picHerbariumSheet.Source = getHerbariumSheet(tempBlob);

                    lblAccessionNumber.Text = sqlData[0].ToString();
                    lblReferenceNumber.Text = sqlData[1].ToString();
                    lblScientificName.Text = sqlData[4].ToString();
                    lblCommonName.Text = sqlData[5].ToString();
                    lblDateCollected.Text = sqlData[6].ToString();
                    lblDateDeposited.Text = sqlData[7].ToString();
                    lblDateVerified.Text = sqlData[8].ToString();
                    lblLocality.Text = sqlData[9].ToString();
                    lblCollector.Text = sqlData[10].ToString();
                    lblValidator.Text = sqlData[11].ToString();
                    lblDescription.Text = sqlData[12].ToString();

                    lblBox.Text = result.First();
                    lblFamilyName.Text = "  [" + sqlData[3].ToString() + "]";

                    pnlPlantDeposit.Visibility = Visibility.Visible;
                }
            }
            connection.closeResult();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlPlantDeposit.Visibility = Visibility.Hidden;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procStoreHerbariumSheet");
            connection.addSprocParameter("@accessionNumber", SqlDbType.VarChar, lblAccessionNumber.Text);
            connection.addSprocParameter("@boxNumber", SqlDbType.VarChar, lblBox.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Herbarium Sheet is now Stored in " + lblBox.Text, "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    MessageBox.Show("Transaction Failed, The system had run to an Error", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
            pnlPlantDeposit.Visibility = Visibility.Hidden;
            btnReturn_Click(btnReturn, null);

            getHerbariumSheet();
        }

        private void getHerbariumSheet()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<HerbariumSheet> herbariumSheets = new List<HerbariumSheet>();

            // Query Command Setting
            connection.setQuery("SELECT strAccessionNumber, strScientificName, CONVERT(VARCHAR, dateVerified, 107), strCollector " +
                                "FROM viewHerbariumSheet " +
                                "WHERE strStatus = 'Verified'");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                herbariumSheets.Add(new HerbariumSheet()
                {
                    AccessionNumber = sqlData[0].ToString(),
                    ScientificName = sqlData[1].ToString(),
                    DateValidated = sqlData[2].ToString(),
                    Collector = sqlData[3].ToString()
                });
            }
            connection.closeResult();

            dgrHerbariumSheets.ItemsSource = herbariumSheets;
        }

        public void getAvailableBoxes()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<FamilyBox> familyBoxes = new List<FamilyBox>();

            // Query Command Setting
            connection.setQuery("SELECT FB.strBoxNumber, FB.strFamilyName, FB.intBoxLimit - COUNT(HI.intStoredSheetID) " +
                                "FROM viewFamilyBox FB LEFT JOIN viewHerbariumInventory HI ON FB.strFamilyName = HI.strFamilyName " +
                                "GROUP BY FB.strBoxNumber, FB.strFamilyName, FB.intBoxLimit");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                familyBoxes.Add(new FamilyBox()
                {
                    BoxNumber = sqlData[0].ToString(),
                    Family = sqlData[1].ToString(),
                    BoxLimit = Convert.ToInt32(sqlData[2])
                });
            }
            connection.closeResult();

            availableBoxes = familyBoxes;
        }

        public BitmapImage getHerbariumSheet(byte[] blob)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(blob);
            image.EndInit();
            return image;
        }
    }
}

public class HerbariumSheet
{
    public string AccessionNumber { get; set; }
    public string ReferenceNumber { get; set; }
    public string ScientificName { get; set; }
    public string CommonName { get; set; } 
    public string DateCollected { get; set; }
    public string DateDeposited { get; set; }
    public string DateValidated { get; set; }
    public string Locality { get; set; }
    public string Collector { get; set; }
    public string Validator { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string BoxLocation { get; set; }
    public string Account { get; set; }
}