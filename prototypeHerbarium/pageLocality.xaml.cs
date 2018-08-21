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

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for pageLocality.xaml
    /// </summary>
    public partial class pageLocality : Page
    {
        List<Locality> Origins = new List<Locality>();
        private string[] islands;
        private string[][] regions;
        private string[][][] provinces;

        public pageLocality()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfLocalityID.Text is null || txfLocalityID.Text == "")
                    addLocality();
                else
                    editLocality();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfLocalityID.Clear();
            txfCountry.Clear();
            cbxIsland.SelectedIndex = -1;
            txfCity.Clear();
            txfArea.Clear();
            txfSpecificLocation.Clear();
            txfShortcut.Clear();
            txfFullLocality.Clear();
            txfLatitude.Clear();
            txfLongtitude.Clear();
            
            msgIsland.Visibility = Visibility.Collapsed;
            msgRegion.Visibility = Visibility.Collapsed;
            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
            msgArea.Visibility = Visibility.Collapsed;
            msgSpecificLocation.Visibility = Visibility.Collapsed;
            msgShortcut.Visibility = Visibility.Collapsed;

            msgCountry.Visibility = Visibility.Collapsed;
            msgIsland.Visibility = Visibility.Collapsed;
            msgRegion.Visibility = Visibility.Collapsed;
            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
            msgArea.Visibility = Visibility.Collapsed;
            msgSpecificLocation.Visibility = Visibility.Collapsed;
            msgFullLocality.Visibility = Visibility.Collapsed;
            msgShortcut.Visibility = Visibility.Collapsed;
        }

        private void btnAddLocality_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddLocality.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddLocality.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddLocality.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddLocality.Content = (state) ? "Close Panel" : "Add Locality";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in Origins
                         where record.Province.ToUpper().Contains(input) ||
                                record.City.ToUpper().Contains(input) ||
                                record.Area.ToUpper().Contains(input) ||
                                record.SpecificLocation.ToUpper().Contains(input)
                         select record;

            dgrLocalityTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Locality SelectedLocality = dgrLocalityTable.SelectedValue as Locality;

            var result = from locality in Origins
                         where locality.ShortLocation == SelectedLocality.ShortLocation
                         select locality;

            if (pnlAddLocality.Visibility == Visibility.Collapsed)
                btnAddLocality_Click(btnAddLocality, null);

            foreach(Locality data in result)
            {
                if (data.Country == "Philippines")
                {
                    rbtPhilippines.IsChecked = true;
                    txfLocalityID.Text = data.LocalityID.ToString();
                    cbxIsland.SelectedItem = data.Island;
                    cbxRegion.SelectedItem = data.Region;
                    cbxProvince.SelectedItem = data.Province;
                    txfCity.Text = data.City;
                    txfArea.Text = data.Area;
                    txfSpecificLocation.Text = data.SpecificLocation;
                    txfShortcut.Text = data.ShortLocation;
                    txfLatitude.Text = data.Latitude;
                    txfLongtitude.Text = data.Longtitude;
                }
                else
                {
                    rbtOtherCountries.IsChecked = true;
                    txfCountry.Text = data.Country;
                    txfFullLocality.Text = data.FullLocation;
                    txfShortcut.Text = data.ShortLocation;
                    txfLatitude.Text = data.Latitude;
                    txfLongtitude.Text = data.Longtitude;
                }

            }
        }
        
        private void rbtCountries_Checked(object sender, RoutedEventArgs e)
        {
            if (rbtPhilippines.IsChecked == true)
            {
                txfCountry.Visibility = Visibility.Collapsed;
                cbxIsland.Visibility = Visibility.Visible;
                cbxRegion.Visibility = Visibility.Visible;
                cbxProvince.Visibility = Visibility.Visible;
                txfCity.Visibility = Visibility.Visible;
                txfArea.Visibility = Visibility.Visible;
                txfSpecificLocation.Visibility = Visibility.Visible;
                txfFullLocality.Visibility = Visibility.Collapsed;

                lblCountryName.Visibility = Visibility.Collapsed;
                lblIsland.Visibility = Visibility.Visible;
                lblRegion.Visibility = Visibility.Visible;
                lblProvince.Visibility = Visibility.Visible;
                lblCity.Visibility = Visibility.Visible;
                lblArea.Visibility = Visibility.Visible;
                lblSpecificLocation.Visibility = Visibility.Visible;
                lblFullLocality.Visibility = Visibility.Collapsed;

                msgCountry.Visibility = Visibility.Collapsed;
                msgIsland.Visibility = Visibility.Collapsed;
                msgRegion.Visibility = Visibility.Collapsed;
                msgProvince.Visibility = Visibility.Collapsed;
                msgCity.Visibility = Visibility.Collapsed;
                msgArea.Visibility = Visibility.Collapsed;
                msgSpecificLocation.Visibility = Visibility.Collapsed;
                msgFullLocality.Visibility = Visibility.Collapsed;
                msgShortcut.Visibility = Visibility.Collapsed;
            }
            else if (rbtOtherCountries.IsChecked == true)
            {
                txfCountry.Visibility = Visibility.Visible;
                cbxIsland.Visibility = Visibility.Collapsed;
                cbxRegion.Visibility = Visibility.Collapsed;
                cbxProvince.Visibility = Visibility.Collapsed;
                txfCity.Visibility = Visibility.Collapsed;
                txfArea.Visibility = Visibility.Collapsed;
                txfSpecificLocation.Visibility = Visibility.Collapsed;
                txfFullLocality.Visibility = Visibility.Visible;

                lblCountryName.Visibility = Visibility.Visible;
                lblIsland.Visibility = Visibility.Collapsed;
                lblRegion.Visibility = Visibility.Collapsed;
                lblProvince.Visibility = Visibility.Collapsed;
                lblCity.Visibility = Visibility.Collapsed;
                lblArea.Visibility = Visibility.Collapsed;
                lblSpecificLocation.Visibility = Visibility.Collapsed;
                lblFullLocality.Visibility = Visibility.Visible;

                msgCountry.Visibility = Visibility.Collapsed;
                msgIsland.Visibility = Visibility.Collapsed;
                msgRegion.Visibility = Visibility.Collapsed;
                msgProvince.Visibility = Visibility.Collapsed;
                msgCity.Visibility = Visibility.Collapsed;
                msgArea.Visibility = Visibility.Collapsed;
                msgSpecificLocation.Visibility = Visibility.Collapsed;
                msgFullLocality.Visibility = Visibility.Collapsed;
                msgShortcut.Visibility = Visibility.Collapsed;
            }
        }

        private void cbxIsland_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = cbxIsland.SelectedIndex;

            cbxRegion.Items.Clear();
            if(index != -1)
            {
                foreach(string region in regions[index])
                {
                    cbxRegion.Items.Add(region);
                }
            }
        }

        private void cbxRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int indexIsland = cbxIsland.SelectedIndex;
            int indexRegion = cbxRegion.SelectedIndex;

            cbxProvince.Items.Clear();
            cbxProvince.IsEnabled = true;
            if(indexRegion != -1)
            {
                foreach (string province in provinces[indexIsland][indexRegion])
                {
                    cbxProvince.Items.Add(province);
                }
            }
            if(indexIsland == 0 && indexRegion == 0)
            {
                cbxProvince.IsEnabled = false;
                cbxProvince.SelectedIndex = 0;
            }
        }

        public void resetForm()
        {
            rbtPhilippines.IsChecked = true;

            pnlAddLocality.Visibility = Visibility.Collapsed;
            sprAddLocality.Visibility = Visibility.Collapsed;
            btnAddLocality.Content = "Add Locality";
            btnClear_Click(btnClear, null);

            initializeLocality();
            setIslandList();
            getLocalityTable();
        }

        public void getLocalityTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<Locality> localities = new List<Locality>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT intLocalityID, strCountry, strIsland, strRegion, strProvince, strCity, strArea, " +
                                    "strSpecificLocation, strShortLocation, strFullLocality, strLatitude, strLongtitude " +
                                "FROM viewLocality");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();
            
            while (sqlData.Read())
            {
                localities.Add(new Locality()
                {
                    LocalityID = Convert.ToInt32(sqlData[0]),
                    Country = sqlData[1].ToString(),
                    Island = sqlData[2].ToString(),
                    Region = sqlData[3].ToString(),
                    Province = sqlData[4].ToString(),
                    City = sqlData[5].ToString(),
                    Area = sqlData[6].ToString(),
                    SpecificLocation = sqlData[7].ToString(),
                    ShortLocation = sqlData[8].ToString(),
                    FullLocation = sqlData[9].ToString(),
                    Latitude = sqlData[10].ToString(),
                    Longtitude = sqlData[11].ToString()
                });
            }
            connection.closeResult();

            dgrLocalityTable.ItemsSource = localities;
            Origins = localities;
        }

        public void initializeLocality()
        {
            islands = new string[] {"Luzon", "Visayas", "Mindanao"};
            regions = new string[3][];

            regions[0] = new string[] {"National Capital Region", "Cordillera Administrative Region", "Ilocos Region",
                                        "Cagayan Valley", "Central Luzon", "CALABARZON", "MIMAROPA", "Bicol Region"};
            regions[1] = new string[] {"Western Visayas", "Central Visayas", "Eastern Visayas"};
            regions[2] = new string[] {"Zamboanga Peninsula", "Northern Mindanao", "Davao Region",
                                        "SOCCSKSARGEN", "Caraga Region", "Autonomous Region in Muslim Mindanao"};

            provinces = new string[3][][];

            provinces[0] = new string[8][];
            provinces[1] = new string[3][];
            provinces[2] = new string[6][];

            provinces[0][0] = new string[] { "Metro Manila" };
            provinces[0][1] = new string[] { "Abra", "Apayao", "Benguet", "Ifugao", "Kalinga", "Mountain Province" };
            provinces[0][2] = new string[] { "Ilocos Norte", "Ilocos Sur", "La Union", "Pangasinan" };
            provinces[0][3] = new string[] { "Cagayan", "Isabela", "Nueva Vizcaya", "Quirino" };
            provinces[0][4] = new string[] { "Aurora", "Bataan", "Bulacan", "Nueva Ecija", "Pampanga", "Tarlac", "Zambales" };
            provinces[0][5] = new string[] { "Batangas", "Cavite", "Laguna", "Quezon", "Rizal" };
            provinces[0][6] = new string[] { "Marinduque", "Occidental Mindoro", "Oriental Mindoro", "Palawan", "Romblon" };
            provinces[0][7] = new string[] { "Albay", "Camarines Norte", "Camarines Sur", "Catanduanes", "Masbate", "Sorsogon" };
            provinces[1][0] = new string[] { "Aklan", "Antique", "Capiz", "Guimaras", "Negros Occidental", "Iloilo" };
            provinces[1][1] = new string[] { "Bohol", "Cebu", "Negros Oriental", "Siquijor" };
            provinces[1][2] = new string[] { "Biliran", "Eastern Samar", "Leyte", "Northern Samar", "Southern Leyte" };
            provinces[2][0] = new string[] { "Zamboanga del Norte", "Zamboanga del Sur", "Zamboanga Sibugay" };
            provinces[2][1] = new string[] { "Bukidnon", "Camiguin", "Lanao del Norte", "Misamis Occidental", "Misamis Oriental" };
            provinces[2][2] = new string[] { "Compostella Valley", "Davao del Norte", "Davao del Sur", "Davao Occidental", "Davao Oriental" };
            provinces[2][3] = new string[] { "Cotabato", "Saranggani", "South Cotabato", "Sultan Kudarat" };
            provinces[2][4] = new string[] { "Agusan del Norte", "Agusan del Sur", "Dinagat Islands", "Surigao del Norte", "Surigao del Sur" };
            provinces[2][5] = new string[] { "Basilan", "Lanao del Sur", "Maguindanao", "Sulu", "Tawi-Tawi" };
        }

        public void setIslandList()
        {
            foreach (string island in islands)
            {
                cbxIsland.Items.Add(island);
            }
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgCountry.Visibility = Visibility.Collapsed;
            msgIsland.Visibility = Visibility.Collapsed;
            msgRegion.Visibility = Visibility.Collapsed;
            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
            msgArea.Visibility = Visibility.Collapsed;
            msgSpecificLocation.Visibility = Visibility.Collapsed;
            msgFullLocality.Visibility = Visibility.Collapsed;
            msgShortcut.Visibility = Visibility.Collapsed;

            if (rbtPhilippines.IsChecked == true)
            {
                if (cbxIsland.SelectedIndex == -1)
                {
                    msgIsland.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (cbxRegion.SelectedIndex == -1)
                {
                    msgRegion.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (cbxProvince.SelectedIndex == -1)
                {
                    msgProvince.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (txfCity.Text == "")
                {
                    msgCity.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (txfArea.Text == "")
                {
                    msgArea.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (txfSpecificLocation.Text == "")
                {
                    msgSpecificLocation.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (txfShortcut.Text == "")
                {
                    msgShortcut.Visibility = Visibility.Visible;
                    formOK = false;
                }
            }
            else if (rbtOtherCountries.IsChecked == true)
            {
                if (txfCountry.Text == "")
                {
                    msgCountry.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (txfFullLocality.Text == "")
                {
                    msgFullLocality.Visibility = Visibility.Visible;
                    formOK = false;
                }
                if (txfShortcut.Text == "")
                {
                    msgShortcut.Visibility = Visibility.Visible;
                    formOK = false;
                }
            }

            return formOK;
        }

        private void addLocality()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();
            string country = (rbtPhilippines.IsChecked == true) ? "Philippines" : txfCountry.Text;

            connection.setStoredProc("dbo.procInsertLocality");
            connection.addSprocParameter("@country", SqlDbType.VarChar, country);
            connection.addSprocParameter("@island", SqlDbType.VarChar, cbxIsland.SelectedItem.ToString());
            connection.addSprocParameter("@region", SqlDbType.VarChar, cbxRegion.SelectedItem.ToString());
            connection.addSprocParameter("@province", SqlDbType.VarChar, cbxProvince.SelectedItem.ToString());
            connection.addSprocParameter("@city", SqlDbType.VarChar, txfCity.Text);
            connection.addSprocParameter("@area", SqlDbType.VarChar, txfArea.Text);
            connection.addSprocParameter("@specificLocation", SqlDbType.VarChar, txfSpecificLocation.Text);
            connection.addSprocParameter("@shortLocation", SqlDbType.VarChar, txfShortcut.Text);
            connection.addSprocParameter("@fullLocation", SqlDbType.VarChar, txfFullLocality.Text);
            connection.addSprocParameter("@latitude", SqlDbType.VarChar, txfLatitude.Text);
            connection.addSprocParameter("@longtitude", SqlDbType.VarChar, txfLongtitude.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Locality Added on the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getLocalityTable();
        }

        private void editLocality()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();
            string country = (rbtPhilippines.IsChecked == true) ? "Philippines" : txfCountry.Text;

            connection.setStoredProc("dbo.procUpdateLocality");
            connection.addSprocParameter("@localityID", SqlDbType.Int, txfLocalityID.Text);
            connection.addSprocParameter("@country", SqlDbType.VarChar, country);
            connection.addSprocParameter("@island", SqlDbType.VarChar, cbxIsland.SelectedItem.ToString());
            connection.addSprocParameter("@region", SqlDbType.VarChar, cbxRegion.SelectedItem.ToString());
            connection.addSprocParameter("@province", SqlDbType.VarChar, cbxProvince.SelectedItem.ToString());
            connection.addSprocParameter("@city", SqlDbType.VarChar, txfCity.Text);
            connection.addSprocParameter("@area", SqlDbType.VarChar, txfArea.Text);
            connection.addSprocParameter("@specificLocation", SqlDbType.VarChar, txfSpecificLocation.Text);
            connection.addSprocParameter("@shortLocation", SqlDbType.VarChar, txfShortcut.Text);
            connection.addSprocParameter("@fullLocation", SqlDbType.VarChar, txfFullLocality.Text);
            connection.addSprocParameter("@latitude", SqlDbType.VarChar, txfLatitude.Text);
            connection.addSprocParameter("@longtitude", SqlDbType.VarChar, txfLongtitude.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Locality Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getLocalityTable();
        }
    }
}

public class Locality
{
    public int LocalityID { get; set; }
    public string FullLocation { get; set; }
    public string Country { get; set; }
    public string Island { get; set; }
    public string Region { get; set; }
    public string Province { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string SpecificLocation { get; set; }
    public string ShortLocation { get; set; }
    public string Latitude { get; set; }
    public string Longtitude { get; set; }
} 