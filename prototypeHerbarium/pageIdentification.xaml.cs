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
    public partial class pageIdentification : Page
    {
        public pageIdentification()
        {
            InitializeComponent();

            //getPlantDeposit();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            /*
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            PlantDeposit plantDeposit = dgrDepositedSheets.SelectedValue as PlantDeposit;

            // Query Command Setting
            connection.setQuery("SELECT strDepositNumber, picHerbariumSheet, strScientificName, strCommonName, strCollector " +
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
                lblCollector.Text = sqlData[4].ToString();
                
            }
            connection.closeResult();
            */
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlPlantDeposit.Visibility = Visibility.Hidden;

            lblDepositNumber.Text = "";
            lblScientificName.Text = "";
            lblCommonName.Text = "";
            lblCollector.Text = "";
            txaDescription.Clear();

            getPlantDeposit();
        }
        
        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            /*
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("UPDATE tblPlantDeposit " +
                                "SET strStatus = 'Rejected', strComment = @comment " +
                                "WHERE strDepositNumber = @depositNo");
            connection.addParameter("@comment", SqlDbType.VarChar, txaDescription.Text);
            connection.addParameter("@depositNo", SqlDbType.VarChar, lblDepositNumber.Text);

            connection.executeCommand();

            MessageBox.Show("Plant Deposit Rejected", "Record Saved");
            
            pnlPlantDeposit.Visibility = Visibility.Hidden;
            btnReturn_Click(btnReturn, null);
            */
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            /*
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("UPDATE tblPlantDeposit " +
                                "SET strStatus = 'For Verification', strComment = @comment " +
                                "WHERE strDepositNumber = @depositNo");
            connection.addParameter("@comment", SqlDbType.VarChar, txaDescription.Text);
            connection.addParameter("@depositNo", SqlDbType.VarChar, lblDepositNumber.Text);

            connection.executeCommand();

            MessageBox.Show("Plant Deposit will Processed for Verification", "Record Saved");

            pnlPlantDeposit.Visibility = Visibility.Hidden;
            btnReturn_Click(btnReturn, null);
            */
        }

        private void getPlantDeposit()
        {
            /*
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<PlantDeposit> plantDeposits = new List<PlantDeposit>();
            
            // Query Command Setting
            connection.setQuery("SELECT strDepositNumber, strScientificName, CONVERT(VARCHAR, dateDeposited, 107), strStaff " +
                                "FROM viewPlantDeposit " +
                                "WHERE strStatus = 'For Deposit'");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                plantDeposits.Add(new PlantDeposit()
                {
                    DepositNumber = sqlData[0].ToString(),
                    ScientificName = sqlData[1].ToString(),
                    DateDeposited = sqlData[2].ToString(),
                    Account = sqlData[3].ToString()
                });
            }
            connection.closeResult();

            dgrDepositedSheets.ItemsSource = plantDeposits;
            */
        }

        private void triggerButton(object sender, RoutedEventArgs e)
        {
            bool conditionA = ! (txaDescription.Text == "" || txaDescription.Text is null);
            bool conditionB = (chkCondition1.IsChecked == true) ? true : false;
            bool conditionC = (chkCondition2.IsChecked == true) ? true : false;
            
            if (conditionA)
                btnReject.IsEnabled = true;
            else
            {
                btnReject.IsEnabled = false;
                btnConfirm.IsEnabled = false;
            }

            if (conditionB && conditionC && conditionA)
                btnConfirm.IsEnabled = true;
            else
                btnConfirm.IsEnabled = false;
        }

        public BitmapImage getHerbariumSheet(byte[] blob)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(blob);
            image.EndInit();
            return image;
        }

        private void txaDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            triggerButton(txaDescription, null);
        }
    }
}
