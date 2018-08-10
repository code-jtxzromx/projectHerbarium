using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Data;

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageResubmit.xaml
    /// </summary>
    public partial class pageResubmit : Page
    {
        public pageResubmit()
        {
            InitializeComponent();

            //getRejectedDeposits();
        }

        private void btnReSubmit_Click(object sender, RoutedEventArgs e)
        {
            /*
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            PlantDeposit plantDeposit = dgrRejectedDeposited.SelectedValue as PlantDeposit;

            // Query Command Setting
            connection.setQuery("SELECT strDepositNumber, picHerbariumSheet, strScientificName, strCommonName, " +
                                        "CONVERT(VARCHAR, dateCollected, 107), strFullLocality, strCollector, " +
                                        "strDescription, strComment " +
                                "FROM viewPlantDeposit " +
                                "WHERE strDepositNumber = @depositNo");
            connection.addParameter("@depositNo", SqlDbType.VarChar, plantDeposit.DepositNumber);

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            pnlPlantDeposit.Visibility = Visibility.Visible;

            while (sqlData.Read())
            {
                byte[] tempBlob = (byte[])sqlData[1];
                picHerbariumSheet.Source = getHerbariumSheet(tempBlob);

                lblDepositNumber.Text = sqlData[0].ToString();
                lblScientificName.Text = sqlData[2].ToString();
                lblCommonName.Text = sqlData[3].ToString();
                lblDateCollected.Text = sqlData[4].ToString();
                lblLocality.Text = sqlData[5].ToString();
                lblCollector.Text = sqlData[6].ToString();
                txaDescription.Text = sqlData[7].ToString();
                lblComments.Text = sqlData[8].ToString();
            }
            connection.closeResult();
            */
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlPlantDeposit.Visibility = Visibility.Hidden;

            picHerbariumSheet = new Image();
            lblDepositNumber.Text = "";
            lblScientificName.Text = "";
            lblCommonName.Text = "";
            lblDateCollected.Text = "";
            lblLocality.Text = "";
            lblCollector.Text = "";
            txaDescription.Text = "";
        }

        private void btnReUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Upload Herbarium Sheet";
            dialog.Filter = "Picture Files [JPEG,PNG]|*.jpg; *.jpeg; *.png|" +
                            "JPEG (*.jpg, *.jpeg)|*.jpg; *.jpeg|" +
                            "PNG (*.png)|*.png";

            if (dialog.ShowDialog() == true)
            {
                picHerbariumSheet.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            /*
            byte[] picture = getPictureToBinary(picHerbariumSheet);

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procResubmitPlantDeposit");
            connection.addSprocParameter("@plantDepositNo", SqlDbType.VarChar, lblDepositNumber.Text);
            connection.addSprocParameter("@herbariumSheet", SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@staff", SqlDbType.VarChar, StaticData.staffname);
            connection.addSprocParameter("@description", SqlDbType.VarChar, txaDescription.Text);

            connection.executeStoredProc();

            MessageBox.Show("Plant Deposit Submitted", "Record Saved");

            pnlPlantDeposit.Visibility = Visibility.Hidden;

            getRejectedDeposits();
            */
        }

        public void getRejectedDeposits()
        {
            /*
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<PlantDeposit> plantDeposits = new List<PlantDeposit>();

            // Query Command Setting
            connection.setQuery("SELECT strDepositNumber, strScientificName, strCollector " +
                                "FROM viewPlantDeposit " +
                                "WHERE strStatus = 'Rejected'");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                plantDeposits.Add(new PlantDeposit()
                {
                    DepositNumber = sqlData[0].ToString(),
                    ScientificName = sqlData[1].ToString(),
                    Collector = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrRejectedDeposited.ItemsSource = plantDeposits;
            */
        }

        public BitmapImage getHerbariumSheet(byte[] blob)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(blob);
            image.EndInit();
            return image;
        }

        private byte[] getPictureToBinary(Image picture)
        {
            byte[] buffer;
            var bitmap = picture.Source as BitmapSource;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                buffer = stream.ToArray();
            }

            return buffer;
        }
    }
}
