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
        WebCam camera;
        bool webcamOpened = false;

        public pageResubmit()
        {
            InitializeComponent();

            getRejectedDeposits();
            getPlantTypeList();
            getLocalityList();

            camera = new WebCam();
            camera.InitializeCamera(ref picCamera);
        }

        private void btnReSubmit_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection connection = new DatabaseConnection();
            ReceivedDeposit plantDeposit = dgrRejectedDeposited.SelectedValue as ReceivedDeposit;
            
            connection.setQuery("SELECT intDepositID, strDepositNumber, strPlantTypeName, picHerbariumSheet, " +
                                    "strCollector, strShortLocation, CONVERT(VARCHAR, dateCollected, 107), strDescription " +
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
                    picHerbariumPicture.Source = getHerbariumSheet(tempBlob);
                }
                catch (Exception) { }

                lblDepositID.Text = sqlData[0].ToString();
                lblDepositNumber.Text = sqlData[1].ToString();
                lblCollector.Text = sqlData[4].ToString();
                lblDateCollected.Text = sqlData[6].ToString();
                txaDescription.Text = sqlData[7].ToString();
                
                foreach (var item in cbxPlantType.Items)
                    if ((item as ComboBoxItem).Item == sqlData[2].ToString())
                        cbxPlantType.SelectedItem = item;
                foreach (var item in cbxLocality.Items)
                    if ((item as ComboBoxItem).Item == sqlData[5].ToString())
                        cbxLocality.SelectedItem = item;
            }
            connection.closeResult();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            pnlPlantDeposit.Visibility = Visibility.Hidden;

            picHerbariumSheet.Source = null;
            lblDepositID.Text = "";
            lblDepositNumber.Text = "";
            cbxPlantType.Text = "";
            lblCollector.Text = "";
            cbxLocality.Text = "";
            lblDateCollected.Text = "";
            txaDescription.Text = "";
            chkGoodCondition.IsChecked = false;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            byte[] picture = getPictureToBinary(picHerbariumPicture);

            DatabaseConnection connection = new DatabaseConnection();
            connection.setStoredProc("dbo.procPlantResubmission");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@depositID", SqlDbType.Int, lblDepositID.Text);
            connection.addSprocParameter("@herbariumSheet", SqlDbType.VarBinary, picture);
            connection.addSprocParameter("@locality", SqlDbType.Int, (cbxLocality.SelectedItem as ComboBoxItem).Item);
            connection.addSprocParameter("@staff", SqlDbType.Int, StaticData.staffname);
            connection.addSprocParameter("@description", SqlDbType.VarChar, txaDescription.Text);
            connection.addSprocParameter("@plantType", SqlDbType.Int, (cbxPlantType.SelectedItem as ComboBoxItem).Item);
            connection.executeStoredProc();

            MessageBox.Show("Plant Deposit Submitted", "Record Saved");

            pnlPlantDeposit.Visibility = Visibility.Hidden;
            getRejectedDeposits();
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

        private void btnReCapture_Click(object sender, RoutedEventArgs e)
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

        private void btnReturnPic_Click(object sender, RoutedEventArgs e)
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
            picHerbariumPicture.Source = picHerbariumSheet.Source;
            btnReturnPic_Click(btnReturnPic, null);
        }

        public void getRejectedDeposits()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<ReceivedDeposit> plantDeposits = new List<ReceivedDeposit>();

            connection.setQuery("SELECT intDepositID, strDepositNumber, strCollector, CONVERT(VARCHAR, dateCollected, 107) " +
                                "FROM viewReceivedDeposit " +
                                "WHERE strStatus = 'Rejected'");
            
            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                plantDeposits.Add(new ReceivedDeposit()
                {
                    DepositID = Convert.ToInt32(sqlData[0]),
                    DepositNumber = sqlData[1].ToString(),
                    Collector = sqlData[2].ToString(),
                    DateCollected = sqlData[3].ToString()
                });
            }
            connection.closeResult();
            dgrRejectedDeposited.ItemsSource = plantDeposits;
        }

        private void getPlantTypeList()
        {
            cbxPlantType.Items.Clear();
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

        private void getLocalityList()
        {
            cbxLocality.Items.Clear();
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
