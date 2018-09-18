using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for viewNewDeposit.xaml
    /// </summary>
    public partial class viewDeposit : UserControl
    {
        public enum DepositModes : int { New = 0, Existing = 1 }
        private DepositModes _DepositMode;

        WebCam camera;
        bool webcamOpened = false;

        public DepositModes DepositMode
        {
            get { return _DepositMode; }
            set
            {
                _DepositMode = value;
                setTransactionMode(_DepositMode);
                if (_DepositMode == DepositModes.Existing)
                {
                    btnVerifiedRecord.IsChecked = false;
                    btnVerifiedRecord_CheckChanged(btnVerifiedRecord, null);
                }
            }
        }

        public viewDeposit()
        {
            InitializeComponent();

            getCollectorList();
            getValidatorList();
            getLocalityList();
            getTaxonList();
            getPlantTypeList();
            
            camera = new WebCam();
            camera.InitializeCamera(ref picCamera);
        }

        private void btnVerifiedRecord_CheckChanged(object sender, RoutedEventArgs e)
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
            chkSameAccession_CheckChanged(chkSameAccession, null);
        }

        private void cbxTaxonName_SelectionChange(object sender, EventArgs e)
        {
            try { getAccessionList(cbxTaxonName.SelectedItem.ToString()); } catch (Exception) { }
        }

        private void chkSameAccession_CheckChanged(object sender, RoutedEventArgs e)
        {
            cbxReferenceNumber.IsEnabled = (chkSameAccession.IsChecked == false);
            cbxReferenceNumber.RequiredField = (chkSameAccession.IsChecked == false && btnVerifiedRecord.IsChecked == true);
        }

        private void txfAccessionNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlCapturePicture.Visibility = Visibility.Collapsed;
            btnDiscardPic_Click(btnDiscardPic, null);
            camera.Stop();
        }

        private void btnResolutionSetting_Click(object sender, RoutedEventArgs e) => camera.ResolutionSetting();

        private void btnCameraSetting_Click(object sender, RoutedEventArgs e) => camera.AdvanceSetting();

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
                    if (_DepositMode == DepositModes.New)
                        addNewDeposit();
                    else
                    {
                        if (btnVerifiedRecord.IsChecked == true)
                            addExistingVerifiedDeposit();
                        else
                            addExistingDeposit();
                    }
                }
            }
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
            cbxPlantType.SelectedIndex = -1;
            txaDescription.Clear();
            chkGoodCondition.IsChecked = false;

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

        public void setTransactionMode(DepositModes deposit)
        {
            bool isNew = (deposit == DepositModes.New);

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

            if (btnVerifiedRecord.Visibility == Visibility.Collapsed)
            {
                btnVerifiedRecord.IsChecked = false;
                btnVerifiedRecord_CheckChanged(btnVerifiedRecord, null);
            }
        }

        private void getCollectorList()
        {
            cbxCollector.Reset();
            List<ComboBoxItem> staffsList = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intCollectorID, strFullName " +
                                "FROM viewCollector " +
                                "ORDER BY strFullName");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                staffsList.Add(new ComboBoxItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();
            cbxCollector.ItemsSource = staffsList;
        }

        private void getValidatorList()
        {
            cbxValidator.Reset();
            List<ComboBoxItem> validatorsList = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intValidatorID, strFullName " +
                                "FROM viewValidator " +
                                "ORDER BY strFullName");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                validatorsList.Add(new ComboBoxItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();
            cbxValidator.ItemsSource = validatorsList;
        }

        private void getLocalityList()
        {
            cbxLocality.Reset();
            List<ComboBoxItem> localityList = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intLocalityID, strShortLocation " +
                                "FROM tblLocality " +
                                "ORDER BY strShortLocation");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                localityList.Add(new ComboBoxItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();
            cbxLocality.ItemsSource = localityList;
        }

        private void getTaxonList()
        {
            cbxTaxonName.Reset();
            List<ComboBoxItem> taxonList = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intSpeciesID, strScientificName " +
                                "FROM viewTaxonSpecies " +
                                "ORDER BY strScientificName");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                taxonList.Add(new ComboBoxItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();
            cbxTaxonName.ItemsSource = taxonList;
        }

        private void getAccessionList(string species)
        {
            cbxReferenceNumber.Reset();
            List<ComboBoxItem> accessionList = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT DISTINCT intPlantReferenceID, strReferenceAccession " +
                                "FROM viewHerbariumSheet " +
                                "WHERE strScientificName = @taxonName " +
                                "ORDER BY strReferenceAccession");
            connection.addParameter("@taxonName", System.Data.SqlDbType.VarChar, species);

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

        private void getPlantTypeList()
        {
            cbxPlantType.Reset();
            List<ComboBoxItem> plantTypes = new List<ComboBoxItem>();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intPlantTypeID, strPlantTypeName " +
                                "FROM tblPlantType " +
                                "ORDER BY strPlantTypeCode");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                plantTypes.Add(new ComboBoxItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();
            cbxPlantType.ItemsSource = plantTypes;
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
            cbxPlantType.ErrorMessage = false;
            lblErrorPicture.Visibility = Visibility.Collapsed;

            if (txfAccessionNumber.RequiredField && txfAccessionNumber.TextContent == "")
            {
                MessageBox.Show("1");
                txfAccessionNumber.ErrorMessage = true;
                formOK = false;
            }
            if (cbxReferenceNumber.RequiredField && cbxReferenceNumber.SelectedIndex == -1)
            {
                MessageBox.Show("2");
                cbxReferenceNumber.ErrorMessage = true;
                formOK = false;
            }
            if (cbxCollector.RequiredField && cbxCollector.SelectedIndex == -1)
            {
                MessageBox.Show("3");
                cbxCollector.ErrorMessage = true;
                formOK = false;
            }
            if (cbxValidator.RequiredField && cbxValidator.SelectedIndex == -1)
            {
                MessageBox.Show("4");
                cbxValidator.ErrorMessage = true;
                formOK = false;
            }
            if (cbxLocality.RequiredField && cbxLocality.SelectedIndex == -1)
            {
                MessageBox.Show("5");
                cbxLocality.ErrorMessage = true;
                formOK = false;
            }
            if (cbxTaxonName.RequiredField && cbxTaxonName.SelectedIndex == -1)
            {
                MessageBox.Show("6");
                cbxTaxonName.ErrorMessage = true;
                formOK = false;
            }
            if (dpkDateCollected.RequiredField && dpkDateCollected.DateContent == "")
            {
                MessageBox.Show("7");
                dpkDateCollected.ErrorMessage = true;
                formOK = false;
            }
            if (dpkDateDeposited.RequiredField && dpkDateDeposited.DateContent == "")
            {
                MessageBox.Show("8");
                dpkDateDeposited.ErrorMessage = true;
                formOK = false;
            }
            if (dpkDateVerified.RequiredField && dpkDateVerified.DateContent == "")
            {
                MessageBox.Show("9");
                dpkDateVerified.ErrorMessage = true;
                formOK = false;
            }
            if (cbxPlantType.RequiredField && cbxPlantType.SelectedIndex == -1)
            {
                MessageBox.Show("10");
                cbxPlantType.ErrorMessage = true;
                formOK = false;
            }

            return formOK;
        }

        private void addNewDeposit()
        {
            int status;
            bool pictureEmpty = false;
            byte[] picture = null;

            try { picture = getPictureToBinary(picHerbariumPlant); }
            catch (Exception) { pictureEmpty = true; }

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procInsertPlantDeposit");
            if (!pictureEmpty) connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@localityID", System.Data.SqlDbType.Int, (cbxLocality.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@collectorID", System.Data.SqlDbType.Int, (cbxCollector.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@staffID", System.Data.SqlDbType.Int, StaticData.ID);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            connection.addSprocParameter("@plantTypeID", System.Data.SqlDbType.Int, (cbxPlantType.SelectedItem as ComboBoxItem).ID);
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
            
            try { picture = getPictureToBinary(picHerbariumPlant); }
            catch (Exception) { pictureEmpty = true; }

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procInsertPlantDeposit");
            if (!pictureEmpty) connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@localityID", System.Data.SqlDbType.Int, (cbxLocality.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@collectorID", System.Data.SqlDbType.Int, (cbxCollector.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@staffID", System.Data.SqlDbType.Int, StaticData.ID);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            connection.addSprocParameter("@plantTypeID", System.Data.SqlDbType.Int, (cbxPlantType.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@accessionDigits", System.Data.SqlDbType.Int, txfAccessionNumber.TextContent);
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

            try { picture = getPictureToBinary(picHerbariumPlant); }
            catch (Exception) { pictureEmpty = true; }

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procInsertVerifiedDeposit");
            connection.addSprocParameter("@accessionDigits", System.Data.SqlDbType.VarChar, txfAccessionNumber.TextContent);
            if (cbxReferenceNumber.SelectedIndex != -1)
                connection.addSprocParameter("@newDepositID", System.Data.SqlDbType.VarChar, (cbxReferenceNumber.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@sameAccession", System.Data.SqlDbType.Bit, (chkSameAccession.IsChecked == true));
            if (!pictureEmpty)
                connection.addSprocParameter("@herbariumSheet", System.Data.SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@localityID", System.Data.SqlDbType.Int, (cbxLocality.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@speciesID", System.Data.SqlDbType.Int, (cbxTaxonName.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@collectorID", System.Data.SqlDbType.Int, (cbxCollector.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@validatorID", System.Data.SqlDbType.Int, (cbxValidator.SelectedItem as ComboBoxItem).ID);
            connection.addSprocParameter("@staffID", System.Data.SqlDbType.Int, StaticData.ID);
            connection.addSprocParameter("@dateCollected", System.Data.SqlDbType.Date, dpkDateCollected.DateContent);
            connection.addSprocParameter("@dateDeposited", System.Data.SqlDbType.Date, dpkDateDeposited.DateContent);
            connection.addSprocParameter("@dateVerified", System.Data.SqlDbType.Date, dpkDateVerified.DateContent);
            connection.addSprocParameter("@description", System.Data.SqlDbType.VarChar, txaDescription.Text);
            connection.addSprocParameter("@plantTypeID", System.Data.SqlDbType.Int, (cbxPlantType.SelectedItem as ComboBoxItem).ID);
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
