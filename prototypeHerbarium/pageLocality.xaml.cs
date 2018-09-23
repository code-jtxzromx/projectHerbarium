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

        List<string> Countries = new List<string>();
        List<string> Provinces = new List<string>();
        List<string[]> Cities = new List<string[]>();

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
            lblLocalityForm.Text = "Add Plant Locality";
            txfLocalityID.Clear();
            cbxCountry.SelectedItem = "Philippines";
            cbxProvince.SelectedIndex = -1;
            txfSpecificLocation.Clear();
            txfShortcut.Clear();
            txfFullLocality.Clear();
            txfLatitude.Clear();
            txfLongtitude.Clear();
            btnSave.Content = "Save";
            
            msgCountry.Visibility = Visibility.Collapsed;
            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
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
                                record.SpecificLocation.ToUpper().Contains(input)
                         select record;

            dgrLocalityTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            lblLocalityForm.Text = "Edit Plant Locality";
            Locality SelectedLocality = dgrLocalityTable.SelectedValue as Locality;

            var result = from locality in Origins
                         where locality.ShortLocation == SelectedLocality.ShortLocation
                         select locality;

            if (pnlAddLocality.Visibility == Visibility.Collapsed)
                btnAddLocality_Click(btnAddLocality, null);

            foreach(Locality data in result)
            {
                txfLocalityID.Text = data.LocalityID.ToString();
                cbxCountry.SelectedItem = data.Country;
                txfShortcut.Text = data.ShortLocation;
                txfLatitude.Text = data.Latitude;
                txfLongtitude.Text = data.Longtitude;

                if (data.Country == "Philippines")
                {
                    cbxProvince.SelectedItem = data.Province;
                    cbxCity.SelectedItem = data.City;
                    txfSpecificLocation.Text = data.SpecificLocation;
                }
                else
                {
                    txfFullLocality.Text = data.FullLocation;
                }

            }
        }
        
        private void cbxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isCountryPH = (cbxCountry.SelectedItem.ToString() == "Philippines");

            cbxProvince.Visibility = (isCountryPH) ? Visibility.Visible : Visibility.Collapsed;
            cbxCity.Visibility = (isCountryPH) ? Visibility.Visible : Visibility.Collapsed;
            txfSpecificLocation.Visibility = (isCountryPH) ? Visibility.Visible : Visibility.Collapsed;
            txfFullLocality.Visibility = (isCountryPH) ? Visibility.Collapsed : Visibility.Visible;

            lblProvince.Visibility = (isCountryPH) ? Visibility.Visible : Visibility.Collapsed;
            lblCity.Visibility = (isCountryPH) ? Visibility.Visible : Visibility.Collapsed;
            lblSpecificLocation.Visibility = (isCountryPH) ? Visibility.Visible : Visibility.Collapsed;
            lblFullLocality.Visibility = (isCountryPH) ? Visibility.Collapsed : Visibility.Visible;

            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
            msgSpecificLocation.Visibility = Visibility.Collapsed;
            msgFullLocality.Visibility = Visibility.Collapsed;
            msgShortcut.Visibility = Visibility.Collapsed;
        }

        private void cbxProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxProvince.SelectedIndex != -1)
            {
                string selectedProvince = cbxProvince.SelectedItem.ToString();
                var resultCities = from city in Cities
                                   where city[0] == selectedProvince
                                   select city[1];
                cbxCity.ItemsSource = resultCities;
            }
            else
            {
                cbxCity.ItemsSource = null;
            }
        }

        public void resetForm()
        {
            cbxCountry.SelectedItem = "Philippines";

            lblLocalityForm.Text = "Add Plant Locality";
            pnlAddLocality.Visibility = Visibility.Collapsed;
            sprAddLocality.Visibility = Visibility.Collapsed;
            btnAddLocality.Content = "Add Locality";
            btnClear_Click(btnClear, null);
            
            getLocalityTable();
            getCountryList();
            getProvinceList();
            getCityList();

            cbxCountry.ItemsSource = Countries;
            cbxProvince.ItemsSource = Provinces;
        }

        public void getLocalityTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<Locality> localities = new List<Locality>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT intLocalityID, strCountry, strProvince, strCity, strSpecificLocation, " +
                                        "strShortLocation, strFullLocality, strLatitude, strLongtitude " +
                                "FROM viewLocality " +
                                "ORDER BY strFullLocality");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();
            
            while (sqlData.Read())
            {
                localities.Add(new Locality()
                {
                    LocalityID = Convert.ToInt32(sqlData[0]),
                    Country = sqlData[1].ToString(),
                    Province = sqlData[2].ToString(),
                    City = sqlData[3].ToString(),
                    SpecificLocation = sqlData[4].ToString(),
                    ShortLocation = sqlData[5].ToString(),
                    FullLocation = sqlData[6].ToString(),
                    Latitude = sqlData[7].ToString(),
                    Longtitude = sqlData[8].ToString()
                });
            }
            connection.closeResult();

            dgrLocalityTable.ItemsSource = localities;
            Origins = localities;
        }

        public void getCountryList()
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strCountry FROM tblCountry");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                Countries.Add(sqlData[0].ToString());
            }
            connection.closeResult();
        }

        public void getProvinceList()
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strProvince FROM tblProvince");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                Provinces.Add(sqlData[0].ToString());
            }
            connection.closeResult();
        }

        public void getCityList()
        {
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strProvince, strCity FROM tblCity Ci INNER JOIN tblProvince Pr ON Ci.intProvinceID = Pr.intProvinceID");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                Cities.Add(new string[]
                {
                    sqlData[0].ToString(), sqlData[1].ToString()
                });
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgCountry.Visibility = Visibility.Collapsed;
            msgProvince.Visibility = Visibility.Collapsed;
            msgCity.Visibility = Visibility.Collapsed;
            msgSpecificLocation.Visibility = Visibility.Collapsed;
            msgFullLocality.Visibility = Visibility.Collapsed;
            msgShortcut.Visibility = Visibility.Collapsed;

            if (cbxCountry.SelectedItem.ToString() == "Philippines" && cbxProvince.SelectedIndex == -1)
            {
                msgProvince.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (cbxCountry.SelectedItem.ToString() == "Philippines" && cbxProvince.SelectedIndex == -1)
            {
                msgCity.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (cbxCountry.SelectedItem.ToString() == "Philippines" && txfSpecificLocation.Text == "")
            {
                msgSpecificLocation.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (cbxCountry.SelectedItem.ToString() != "Philippines" && txfFullLocality.Text == "")
            {
                msgFullLocality.Visibility = Visibility.Visible;
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
            string country = (cbxCountry.SelectedIndex == -1) ? "" : cbxCountry.SelectedItem.ToString();
            string province = (cbxProvince.SelectedIndex == -1) ? "" : cbxProvince.SelectedItem.ToString();
            string city = (cbxCity.SelectedIndex == -1) ? "" : cbxCity.SelectedItem.ToString();

            connection.setStoredProc("dbo.procInsertLocality");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@country", SqlDbType.VarChar, country);
            connection.addSprocParameter("@province", SqlDbType.VarChar, province);
            connection.addSprocParameter("@city", SqlDbType.VarChar, city);
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
            string country = (cbxCountry.SelectedIndex == -1) ? "" : cbxCountry.SelectedItem.ToString();
            string province = (cbxProvince.SelectedIndex == -1) ? "" : cbxProvince.SelectedItem.ToString();
            string city = (cbxCity.SelectedIndex == -1) ? "" : cbxCity.SelectedItem.ToString();

            connection.setStoredProc("dbo.procUpdateLocality");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@localityID", SqlDbType.Int, txfLocalityID.Text);
            connection.addSprocParameter("@country", SqlDbType.VarChar, country);
            connection.addSprocParameter("@province", SqlDbType.VarChar, province);
            connection.addSprocParameter("@city", SqlDbType.VarChar, city);
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
    public string Province { get; set; }
    public string City { get; set; }
    public string SpecificLocation { get; set; }
    public string ShortLocation { get; set; }
    public string Latitude { get; set; }
    public string Longtitude { get; set; }
} 