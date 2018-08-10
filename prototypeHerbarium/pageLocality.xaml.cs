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
            cbxIsland.SelectedIndex = -1;
            txfCity.Clear();
            txfArea.Clear();
            txfSpecificLocation.Clear();
            txfShortcut.Clear();
            
            msgIsland.Visibility = Visibility.Collapsed;
            msgRegion.Visibility = Visibility.Collapsed;
            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
            msgArea.Visibility = Visibility.Collapsed;
            msgSpecificLocation.Visibility = Visibility.Collapsed;
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
                txfLocalityID.Text = data.LocalityID.ToString();
                cbxIsland.SelectedItem = data.Island;
                cbxRegion.SelectedItem = data.Region;
                cbxProvince.SelectedItem = data.Province;
                txfCity.Text = data.City;
                txfArea.Text = data.Area;
                txfSpecificLocation.Text = data.SpecificLocation;
                txfShortcut.Text = data.ShortLocation;
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
            connection.setQuery("SELECT intLocalityID, strIsland, strRegion, strProvince, strCity, strArea, " +
                                        "strSpecificLocation, strShortLocation, strFullLocality " +
                                "FROM viewLocality");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();
            
            while (sqlData.Read())
            {
                localities.Add(new Locality()
                {
                    LocalityID = Convert.ToInt32(sqlData[0]),
                    Island = sqlData[1].ToString(),
                    Region = sqlData[2].ToString(),
                    Province = sqlData[3].ToString(),
                    City = sqlData[4].ToString(),
                    Area = sqlData[5].ToString(),
                    SpecificLocation = sqlData[6].ToString(),
                    ShortLocation = sqlData[7].ToString(),
                    FullLocation = sqlData[8].ToString()
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
            msgIsland.Visibility = Visibility.Collapsed;
            msgRegion.Visibility = Visibility.Collapsed;
            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
            msgArea.Visibility = Visibility.Collapsed;
            msgSpecificLocation.Visibility = Visibility.Collapsed;
            msgShortcut.Visibility = Visibility.Collapsed;

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

            return formOK;
        }

        private void addLocality()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();
            
            connection.setStoredProc("dbo.procInsertLocality");
            connection.addSprocParameter("@island", SqlDbType.VarChar, cbxIsland.SelectedItem.ToString());
            connection.addSprocParameter("@region", SqlDbType.VarChar, cbxRegion.SelectedItem.ToString());
            connection.addSprocParameter("@province", SqlDbType.VarChar, cbxProvince.SelectedItem.ToString());
            connection.addSprocParameter("@city", SqlDbType.VarChar, txfCity.Text);
            connection.addSprocParameter("@area", SqlDbType.VarChar, txfArea.Text);
            connection.addSprocParameter("@specificLocation", SqlDbType.VarChar, txfSpecificLocation.Text);
            connection.addSprocParameter("@shortLocation", SqlDbType.VarChar, txfShortcut.Text);
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
            
            connection.setStoredProc("dbo.procUpdateLocality");
            connection.addSprocParameter("@localityID", SqlDbType.Int, txfLocalityID.Text);
            connection.addSprocParameter("@island", SqlDbType.VarChar, cbxIsland.SelectedItem.ToString());
            connection.addSprocParameter("@region", SqlDbType.VarChar, cbxRegion.SelectedItem.ToString());
            connection.addSprocParameter("@province", SqlDbType.VarChar, cbxProvince.SelectedItem.ToString());
            connection.addSprocParameter("@city", SqlDbType.VarChar, txfCity.Text);
            connection.addSprocParameter("@area", SqlDbType.VarChar, txfArea.Text);
            connection.addSprocParameter("@specificLocation", SqlDbType.VarChar, txfSpecificLocation.Text);
            connection.addSprocParameter("@shortLocation", SqlDbType.VarChar, txfShortcut.Text);
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
    public string Island { get; set; }
    public string Region { get; set; }
    public string Province { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string SpecificLocation { get; set; }
    public string ShortLocation { get; set; }
} 