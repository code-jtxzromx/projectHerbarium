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
using System.Text.RegularExpressions;

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageDeposit.xaml
    /// </summary>
    public partial class pageDeposit : Page
    {
        WebCam camera;
        bool webcamOpened = false;
        ctrlComboBox comboBox = new ctrlComboBox();

        public pageDeposit()
        {
            InitializeComponent();
            
            getCollectorList();
            getValidatorList();
            getLocalityList();
            getTaxonList();

            rbtNew.IsChecked = true;
            camera = new WebCam();
            camera.InitializeCamera(ref picCamera);
        }
        
        private void rbtDepositTransaction_Checked(object sender, RoutedEventArgs e)
        {
            bool isNew = (rbtNew.IsChecked == true);

            btnVerifiedRecord.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;
            chkSameAccession.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;

            txfAccessionNumber.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;
            cbxReferenceNumber.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;
            cbxValidator.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;
            cbxTaxonName.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;
            dpkDateDeposited.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;
            dpkDateVerified.Visibility = isNew ? Visibility.Collapsed : Visibility.Visible;

            txfAccessionNumber.RequiredField = !isNew;
            cbxReferenceNumber.RequiredField = !isNew;
            cbxValidator.RequiredField = !isNew;
            cbxTaxonName.RequiredField = !isNew;
            dpkDateDeposited.RequiredField = !isNew;
            dpkDateVerified.RequiredField = !isNew;

            defValidator.Width = isNew ? new GridLength(0) : new GridLength(1, GridUnitType.Star);
            defDateCollected.Width = isNew ? new GridLength(0) : new GridLength(1, GridUnitType.Star);

            if (btnVerifiedRecord.IsVisible)
            {
                btnVerifiedRecord.IsChecked = false;
                btnVerifiedRecord_ToggleChange(btnVerifiedRecord, null);
            }
        }

        private void btnVerifiedRecord_ToggleChange(object sender, RoutedEventArgs e)
        {
            bool isVerified = (btnVerifiedRecord.IsChecked == true);

            chkSameAccession.Visibility = isVerified ? Visibility.Visible : Visibility.Collapsed;
            cbxReferenceNumber.Visibility = isVerified ? Visibility.Visible : Visibility.Collapsed;
            cbxValidator.Visibility = isVerified ? Visibility.Visible : Visibility.Collapsed;
            cbxTaxonName.Visibility = isVerified ? Visibility.Visible : Visibility.Collapsed;
            dpkDateVerified.Visibility = isVerified ? Visibility.Visible : Visibility.Collapsed;
            
            cbxReferenceNumber.RequiredField = isVerified;
            cbxValidator.RequiredField = isVerified;
            cbxTaxonName.RequiredField = isVerified;
            dpkDateVerified.RequiredField = isVerified;

            defValidator.Width = isVerified ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
            chkSameAccession.IsChecked = isVerified;
            chkSameAccession_CheckedChanged(chkSameAccession, null);
        }

        private void txfAccessionNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cbxTaxonName_SelectionChange(object sender, EventArgs e)
        {
            try
            {
                getAccessionList(cbxTaxonName.SelectedItem.ToString());
            }
            catch (Exception) { }
        }

        private void chkSameAccession_CheckedChanged(object sender, RoutedEventArgs e)
        {
            cbxReferenceNumber.IsEnabled = (chkSameAccession.IsChecked == false);
            cbxReferenceNumber.RequiredField = (chkSameAccession.IsChecked == false);
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
            }
        }

        private void btnCapturePicture_Click(object sender, RoutedEventArgs e)
        {
            pnlCapturePicture.Visibility = Visibility.Visible;
            if (!webcamOpened)
                camera.Start();
            else
                camera.Continue();
            webcamOpened = true;

            btnCapturePic.Visibility = Visibility.Visible;
            btnDiscardPic.Visibility = Visibility.Collapsed;
            btnSavePic.Visibility = Visibility.Collapsed;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlCapturePicture.Visibility = Visibility.Collapsed;
            btnDiscardPic_Click(btnDiscardPic, null);
            camera.Stop();
        }

        private void btnResolutionSetting_Click(object sender, RoutedEventArgs e) => camera.ResolutionSetting();

        private void btnCameraSetting_Click(object sender, RoutedEventArgs e) => camera.AdvanceSetting();

        private void btnCapturePic_Click(object sender, RoutedEventArgs e)
        {
            picHerbariumSheet.Source = picCamera.Source;
            picHerbariumSheet.Visibility = Visibility.Visible;
            picCamera.Visibility = Visibility.Collapsed;

            btnCapturePic.Visibility = Visibility.Collapsed;
            btnDiscardPic.Visibility = Visibility.Visible;
            btnSavePic.Visibility = Visibility.Visible;
        }

        private void btnDiscardPic_Click(object sender, RoutedEventArgs e)
        {
            picCamera.Visibility = Visibility.Visible;
            picHerbariumSheet.Visibility = Visibility.Collapsed;

            btnCapturePic.Visibility = Visibility.Visible;
            btnDiscardPic.Visibility = Visibility.Collapsed;
            btnSavePic.Visibility = Visibility.Collapsed;
        }

        private void btnSavePic_Click(object sender, RoutedEventArgs e)
        {
            picHerbariumPlant.Source = picHerbariumSheet.Source;
            btnReturn_Click(btnReturn, null);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfAccessionNumber.Clear();
            cbxReferenceNumber.SelectedIndex = -1;
            cbxCollector.SelectedIndex = -1;
            cbxValidator.SelectedIndex = -1;
            cbxLocality.SelectedIndex = -1;
            cbxTaxonName.SelectedIndex = -1;
            dpkDateCollected.Clear();
            dpkDateDeposited.Clear();
            dpkDateVerified.Clear();
            txaDescription.Clear();
            
            picHerbariumPlant.Source = null;
            txfAccessionNumber.ErrorMessage = false;
            cbxReferenceNumber.ErrorMessage = false;
            cbxCollector.ErrorMessage = false;
            cbxValidator.ErrorMessage = false;
            cbxLocality.ErrorMessage = false;
            cbxTaxonName.ErrorMessage = false;
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
                        if (btnVerifiedRecord.IsChecked == true)
                            addExistingVerifiedDeposit();
                        else
                            addExistingDeposit();
                    }
                }
            }
        }

        private void getCollectorList()
        {
            cbxCollector.Reset();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strFullName FROM viewCollector ORDER BY strFullName");

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
            connection.setQuery("SELECT strFullName FROM viewValidator ORDER BY strFullName");

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
            connection.setQuery("SELECT strShortLocation FROM tblLocality ORDER BY strShortLocation");

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
            connection.setQuery("SELECT strScientificName FROM viewTaxonSpecies ORDER BY strScientificName");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxTaxonName.AddItem(sqlData[0]);
            }
            connection.closeResult();
        }

        private void getAccessionList(string species)
        {
            cbxReferenceNumber.Reset();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT DISTINCT strReferenceAccession FROM viewHerbariumSheet WHERE strScientificName = @taxonName ORDER BY strReferenceAccession");
            connection.addParameter("@taxonName", System.Data.SqlDbType.VarChar, species);

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxReferenceNumber.AddItem(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            txfAccessionNumber.ErrorMessage = false;
            cbxReferenceNumber.ErrorMessage = false;
            cbxCollector.ErrorMessage = false;
            cbxValidator.ErrorMessage = false;
            cbxLocality.ErrorMessage = false;
            cbxTaxonName.ErrorMessage = false;
            dpkDateCollected.ErrorMessage = false;
            dpkDateDeposited.ErrorMessage = false;
            dpkDateVerified.ErrorMessage = false;
            lblErrorPicture.Visibility = Visibility.Collapsed;

            if (txfAccessionNumber.RequiredField && txfAccessionNumber.TextContent == "")
            {
                txfAccessionNumber.ErrorMessage = true;
                formOK = false;
            }
            if (cbxReferenceNumber.RequiredField && cbxReferenceNumber.SelectedIndex == -1)
            {
                cbxReferenceNumber.ErrorMessage = true;
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

            return formOK;
        }

        private void addNewDeposit()
        {
            int status;
            bool pictureEmpty = false;
            byte[] picture = null;
            char plantType = 'X';

            try { picture = getPictureToBinary(picHerbariumPlant); }
            catch (Exception) { pictureEmpty = true; }

            if (rbtVascular.IsChecked == true) plantType = 'V';
            else if (rbtFlowering.IsChecked == true) plantType = 'F';
            else if (rbtAlgae.IsChecked == true) plantType = 'A';
            else if (rbtBryophyte.IsChecked == true) plantType = 'B';

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procInsertPlantDeposit");
            if (!pictureEmpty) connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@locality", System.Data.SqlDbType.VarChar, cbxLocality.SelectedItem);
            connection.addSprocParameter("@collector", System.Data.SqlDbType.VarChar, cbxCollector.SelectedItem);
            connection.addSprocParameter("@staff", System.Data.SqlDbType.VarChar, StaticData.staffname);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            connection.addSprocParameter("@plantType", System.Data.SqlDbType.Char, plantType);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("New Plant Deposit Received", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
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
            bool pictureEmpty = false;
            byte[] picture = null;
            char plantType = 'X';

            try { picture = getPictureToBinary(picHerbariumPlant); }
            catch (Exception) { pictureEmpty = true; }

            if (rbtVascular.IsChecked == true) plantType = 'V';
            else if (rbtFlowering.IsChecked == true) plantType = 'F';
            else if (rbtAlgae.IsChecked == true) plantType = 'A';
            else if (rbtBryophyte.IsChecked == true) plantType = 'B';

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procInsertPlantDeposit");
            if (!pictureEmpty) connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@locality", System.Data.SqlDbType.VarChar, cbxLocality.SelectedItem);
            connection.addSprocParameter("@collector", System.Data.SqlDbType.VarChar, cbxCollector.SelectedItem);
            connection.addSprocParameter("@staff", System.Data.SqlDbType.VarChar, StaticData.staffname);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            connection.addSprocParameter("@plantType", System.Data.SqlDbType.Char, plantType);
            connection.addSprocParameter("@accessionDigits", System.Data.SqlDbType.VarChar, txfAccessionNumber.TextContent);
            connection.addSprocParameter("@dateDeposited", System.Data.SqlDbType.Date, dpkDateDeposited.DateContent);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Existing Plant Deposit Recorded", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    MessageBox.Show("Transaction Failed, The system had run to an Error", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }

            btnClear_Click(btnClear, null);
        }

        private void addExistingVerifiedDeposit()
        {
            int status;
            bool pictureEmpty = false;
            byte[] picture = null;
            char plantType = 'X';

            try { picture = getPictureToBinary(picHerbariumPlant); }
            catch (Exception) { pictureEmpty = true; }

            if (rbtVascular.IsChecked == true) plantType = 'V';
            else if (rbtFlowering.IsChecked == true) plantType = 'F';
            else if (rbtAlgae.IsChecked == true) plantType = 'A';
            else if (rbtBryophyte.IsChecked == true) plantType = 'B';

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procInsertVerifiedDeposit");
            connection.addSprocParameter("@accessionDigits", System.Data.SqlDbType.VarChar, txfAccessionNumber.TextContent);
            connection.addSprocParameter("@referenceNumber", System.Data.SqlDbType.VarChar, cbxReferenceNumber.SelectedItem);
            connection.addSprocParameter("@sameAccession", System.Data.SqlDbType.Bit, (chkSameAccession.IsChecked == true));
            if (!pictureEmpty) connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@locality", System.Data.SqlDbType.VarChar, cbxLocality.SelectedItem);
            connection.addSprocParameter("@taxonName", System.Data.SqlDbType.VarChar, cbxTaxonName.SelectedItem);
            connection.addSprocParameter("@collector", System.Data.SqlDbType.VarChar, cbxCollector.SelectedItem);
            connection.addSprocParameter("@validator", System.Data.SqlDbType.VarChar, cbxValidator.SelectedItem);
            connection.addSprocParameter("@staff", System.Data.SqlDbType.VarChar, StaticData.staffname);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@dateDeposited", System.Data.SqlDbType.Date, dpkDateDeposited.DateContent);
            connection.addSprocParameter("@dateVerified", System.Data.SqlDbType.Date, dpkDateVerified.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            connection.addSprocParameter("@plantType", System.Data.SqlDbType.Char, plantType);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Plant Deposit Received and Verified", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case 1:
                    MessageBox.Show("Transaction Failed, The system had run to an Error", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }

            btnClear_Click(btnClear, null);
        }
    }
}
