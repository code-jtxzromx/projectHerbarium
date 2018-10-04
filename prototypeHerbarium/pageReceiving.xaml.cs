using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageIdentification.xaml
    /// </summary>
    public partial class pageReceiving : Page
    {
        public pageReceiving()
        {
            InitializeComponent();

            getPlantDeposit();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection connection = new DatabaseConnection();
            ReceivedDeposit plantDeposit = dgrDepositedSheets.SelectedValue as ReceivedDeposit;

            connection.setQuery("SELECT intDepositID, strDepositNumber, strPlantTypeName, picHerbariumSheet, " +
                                    "strCollector, strFullLocality, strStaff, " +
                                    "CONVERT(VARCHAR, dateCollected, 107), CONVERT(VARCHAR, dateDeposited, 107), " +
                                    "strDescription, strStatus " +
                                "FROM viewReceivedDeposit " +
                                "WHERE strDepositNumber = @depositNo");
            connection.addParameter("@depositNo", SqlDbType.VarChar, plantDeposit.DepositNumber);

            SqlDataReader sqlData = connection.executeResult();
            
            pnlPlantDeposit.Visibility = Visibility.Visible;
            while (sqlData.Read())
            {
                try
                {
                    byte[] tempBlob = (byte[])sqlData[3];
                    picHerbariumSheet.Source = getHerbariumSheet(tempBlob);
                }
                catch (Exception) { }

                lblDepositID.Text = sqlData[0].ToString();
                lblDepositNumber.Text = sqlData[1].ToString();
                lblPlantType.Text = sqlData[2].ToString();
                lblCollector.Text = sqlData[4].ToString();
                lblLocality.Text = sqlData[5].ToString();
                lblStaff.Text = sqlData[6].ToString();
                lblDateCollected.Text = sqlData[7].ToString();
                lblDateDeposit.Text = sqlData[8].ToString();
                lblDescription.Text = sqlData[9].ToString();
                lblMode.Text = sqlData[10].ToString();
                chkGoodCondition.IsChecked = true;
            }
            connection.closeResult();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlPlantDeposit.Visibility = Visibility.Hidden;

            picHerbariumSheet.Source = null;
            lblDepositID.Text = "";
            lblDepositNumber.Text = "";
            lblPlantType.Text = "";
            lblCollector.Text = "";
            lblLocality.Text = "";
            lblStaff.Text = "";
            lblDateCollected.Text = "";
            lblDateDeposit.Text = "";
            lblDescription.Text = "";
            lblMode.Text = "";
            chkGoodCondition.IsChecked = true;

            getPlantDeposit();
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procConfirmDeposit");
            connection.addSprocParameter("@depositNumber", SqlDbType.VarChar, lblDepositNumber.Text);
            connection.addSprocParameter("@receiveStatus", SqlDbType.VarChar, "Rejected");

            connection.executeStoredProc();
        
            MessageBox.Show("Plant Deposit Rejected", "Record Saved");
            
            pnlPlantDeposit.Visibility = Visibility.Hidden;
            btnReturn_Click(btnReturn, null);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procConfirmDeposit");
            connection.addSprocParameter("@depositNumber", SqlDbType.VarChar, lblDepositNumber.Text);
            connection.addSprocParameter("@receiveStatus", SqlDbType.VarChar, "Accepted");

            connection.executeStoredProc();

            MessageBox.Show("Plant Deposit will Processed for Verification", "Record Saved");

            pnlPlantDeposit.Visibility = Visibility.Hidden;
            btnReturn_Click(btnReturn, null);
        }

        private void getPlantDeposit()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<ReceivedDeposit> plantDeposits = new List<ReceivedDeposit>();

            connection.setQuery("SELECT intDepositID, strDepositNumber, strCollector, CONVERT(VARCHAR, dateDeposited, 107), strStaff " +
                                "FROM viewReceivedDeposit " +
                                "WHERE strStatus IN ('New Deposit', 'Resubmitted')");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                plantDeposits.Add(new ReceivedDeposit()
                {
                    DepositID = Convert.ToInt32(sqlData[0]),
                    DepositNumber = sqlData[1].ToString(),
                    Collector = sqlData[2].ToString(),
                    DateDeposited = sqlData[3].ToString(),
                    Staff = sqlData[4].ToString(),
                });
            }
            connection.closeResult();
            dgrDepositedSheets.ItemsSource = plantDeposits;
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

    public class ReceivedDeposit
    {
        public int DepositID { get; set; }
        public string DepositNumber { get; set; }
        public string PlantType { get; set; }
        public string Collector { get; set; }
        public string Locality { get; set; }
        public string Staff { get; set; }
        public string DateCollected { get; set; }
        public string DateDeposited { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}