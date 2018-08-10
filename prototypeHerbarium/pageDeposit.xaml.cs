using System;
using System.IO;
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

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageDeposit.xaml
    /// </summary>
    public partial class pageDeposit : Page
    {
        public pageDeposit()
        {
            InitializeComponent();

            rbtNew.IsChecked = true;

            getCollectorList();
            getValidatorList();
            getLocalityList();
            getTaxonList();
        }
        
        private void rbtDepositTransaction_Checked(object sender, RoutedEventArgs e)
        {
            if (rbtNew.IsChecked == true)
            {
                txfAccessionNumber.Visibility = Visibility.Collapsed;
                txfReferenceNumber.Visibility = Visibility.Collapsed;
                cbxValidator.Visibility = Visibility.Collapsed;
                cbxTaxonName.Visibility = Visibility.Collapsed;
                //txfCommonName.Visibility = Visibility.Collapsed;
                dpkDateDeposited.Visibility = Visibility.Collapsed;
                dpkDateVerified.Visibility = Visibility.Collapsed;

                txfAccessionNumber.RequiredField = false;
                txfReferenceNumber.RequiredField = false;
                cbxValidator.RequiredField = false;
                cbxTaxonName.RequiredField = false;
                //txfCommonName.RequiredField = false;
                dpkDateDeposited.RequiredField = false;
                dpkDateVerified.RequiredField = false;

                defValidator.Width = new GridLength(0);
                defDateCollected.Width = new GridLength(0);
            }
            else if (rbtExisting.IsChecked == true)
            {
                txfAccessionNumber.Visibility = Visibility.Visible;
                txfReferenceNumber.Visibility = Visibility.Visible;
                cbxValidator.Visibility = Visibility.Visible;
                cbxTaxonName.Visibility = Visibility.Visible;
                //txfCommonName.Visibility = Visibility.Visible;
                dpkDateDeposited.Visibility = Visibility.Visible;
                dpkDateVerified.Visibility = Visibility.Visible;

                txfAccessionNumber.RequiredField = true;
                txfReferenceNumber.RequiredField = true;
                cbxValidator.RequiredField = true;
                cbxTaxonName.RequiredField = true;
                //txfCommonName.RequiredField = true;
                dpkDateDeposited.RequiredField = true;
                dpkDateVerified.RequiredField = true;

                defValidator.Width = new GridLength(1, GridUnitType.Star);
                defDateCollected.Width = new GridLength(1, GridUnitType.Star);
            }
        }

        private void btnUploadPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Upload Herbarium Sheet";
            dialog.Filter = "Picture Files [JPEG,PNG]|*.jpg; *.jpeg; *.png|" +
                            "JPEG (*.jpg, *.jpeg)|*.jpg; *.jpeg|" +
                            "PNG (*.png)|*.png";

            if (dialog.ShowDialog() == true)
            {
                picHerbariumPlant.Source = new BitmapImage(new Uri(dialog.FileName));
                lblPictureFile.Text = dialog.FileName;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfAccessionNumber.Clear();
            txfReferenceNumber.Clear();
            cbxCollector.SelectedIndex = -1;
            cbxValidator.SelectedIndex = -1;
            cbxLocality.SelectedIndex = -1;
            cbxTaxonName.SelectedIndex = -1;
            //txfCommonName.Clear();
            dpkDateCollected.Clear();
            dpkDateDeposited.Clear();
            dpkDateVerified.Clear();
            txaDescription.Clear();
            
            picHerbariumPlant.Source = null;
            txfAccessionNumber.ErrorMessage = false;
            txfReferenceNumber.ErrorMessage = false;
            cbxCollector.ErrorMessage = false;
            cbxValidator.ErrorMessage = false;
            cbxLocality.ErrorMessage = false;
            cbxTaxonName.ErrorMessage = false;
            //txfCommonName.ErrorMessage = false;
            dpkDateCollected.ErrorMessage = false;
            dpkDateDeposited.ErrorMessage = false;
            dpkDateVerified.ErrorMessage = false;
            lblErrorPicture.Visibility = Visibility.Collapsed;
        }

        private void btnProceed_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                MessageBoxResult result;
                result = MessageBox.Show("Do you want to process this deposit transaction?",
                                         "Confirm Deposit Transaction",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (rbtNew.IsChecked == true)
                    {
                        addNewDeposit();
                    }
                    else if (rbtExisting.IsChecked == true)
                    {
                        addExistingDeposit();
                    }
                }
            }
        }

        private void getCollectorList()
        {
            cbxCollector.Reset();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strFullName FROM viewCollector");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxCollector.AddItem(sqlData[0]);
            }
            connection.closeResult();
        }

        private void getValidatorList()
        {
            cbxValidator.Reset();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strFullName FROM viewValidator");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxValidator.AddItem(sqlData[0]);
            }
            connection.closeResult();
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

        private void getLocalityList()
        {
            cbxLocality.Reset();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strShortLocation FROM tblLocality");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxLocality.AddItem(sqlData[0]);
            }
            connection.closeResult();
        }

        private void getTaxonList()
        {
            cbxTaxonName.Reset();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strScientificName FROM viewTaxonSpecies");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxTaxonName.AddItem(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            txfAccessionNumber.ErrorMessage = false;
            txfReferenceNumber.ErrorMessage = false;
            cbxCollector.ErrorMessage = false;
            cbxValidator.ErrorMessage = false;
            cbxLocality.ErrorMessage = false;
            cbxTaxonName.ErrorMessage = false;
            //txfCommonName.ErrorMessage = false;
            dpkDateCollected.ErrorMessage = false;
            dpkDateDeposited.ErrorMessage = false;
            dpkDateVerified.ErrorMessage = false;
            lblErrorPicture.Visibility = Visibility.Collapsed;

            if (txfAccessionNumber.RequiredField && txfAccessionNumber.TextContent == "")
            {
                txfAccessionNumber.ErrorMessage = true;
                formOK = false;
            }
            if (txfReferenceNumber.RequiredField && txfReferenceNumber.TextContent == "")
            {
                txfReferenceNumber.ErrorMessage = true;
                formOK = false;
            }
            if (cbxCollector.RequiredField && cbxCollector.SelectedIndex == -1)
            {
                cbxCollector.ErrorMessage = true;
                formOK = false;
            }
            if (cbxValidator.RequiredField && cbxValidator.SelectedIndex == -1)
            {
                cbxValidator.ErrorMessage = true;
                formOK = false;
            }
            if (cbxLocality.RequiredField && cbxLocality.SelectedIndex == -1)
            {
                cbxLocality.ErrorMessage = true;
                formOK = false;
            }
            if (cbxTaxonName.RequiredField && cbxTaxonName.SelectedIndex == -1)
            {
                cbxTaxonName.ErrorMessage = true;
                formOK = false;
            }
            //if (txfCommonName.RequiredField && txfCommonName.TextContent == "")
            //{
            //    txfCommonName.ErrorMessage = true;
            //    formOK = false;
            //}
            if (dpkDateCollected.RequiredField && dpkDateCollected.DateContent == "")
            {
                dpkDateCollected.ErrorMessage = true;
                formOK = false;
            }
            if (dpkDateDeposited.RequiredField && dpkDateDeposited.DateContent == "")
            {
                dpkDateDeposited.ErrorMessage = true;
                formOK = false;
            }
            if (dpkDateVerified.RequiredField && dpkDateVerified.DateContent == "")
            {
                dpkDateVerified.ErrorMessage = true;
                formOK = false;
            }
            if (lblPictureFile.Text == "[No Uploaded Photo]")
            {
                lblErrorPicture.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addNewDeposit()
        {
            int status;
            byte[] picture = getPictureToBinary(picHerbariumPlant);
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertPlantDeposit");
            connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@locality", System.Data.SqlDbType.VarChar, cbxLocality.SelectedItem);
            connection.addSprocParameter("@collector", System.Data.SqlDbType.VarChar, cbxCollector.SelectedItem);
            connection.addSprocParameter("@staff", System.Data.SqlDbType.VarChar, StaticData.staffname);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Plant Deposit Received", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    MessageBox.Show("Transaction Failed, The system had run to an Error", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }

            btnClear_Click(btnClear, null);
        }

        private void addExistingDeposit()
        {
            int status;
            byte[] picture = getPictureToBinary(picHerbariumPlant);
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertVerifiedDeposit");
            connection.addSprocParameter("@accessionNumber", System.Data.SqlDbType.VarChar, txfAccessionNumber.TextContent);
            connection.addSprocParameter("@referenceNumber", System.Data.SqlDbType.VarChar, txfReferenceNumber.TextContent);
            connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@locality", System.Data.SqlDbType.VarChar, cbxLocality.SelectedItem);
            connection.addSprocParameter("@taxonName", System.Data.SqlDbType.VarChar, cbxTaxonName.SelectedItem);
            connection.addSprocParameter("@collector", System.Data.SqlDbType.VarChar, cbxCollector.SelectedItem);
            connection.addSprocParameter("@validator", System.Data.SqlDbType.VarChar, cbxValidator.SelectedItem);
            connection.addSprocParameter("@staff", System.Data.SqlDbType.VarChar, StaticData.staffname);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@dateDeposited", System.Data.SqlDbType.Date, dpkDateDeposited.DateContent);
            connection.addSprocParameter("@dateVerified", System.Data.SqlDbType.Date, dpkDateVerified.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Plant Deposit Received", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    MessageBox.Show("Transaction Failed, The system had run to an Error", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }

            btnClear_Click(btnClear, null);
        }
    }
}
