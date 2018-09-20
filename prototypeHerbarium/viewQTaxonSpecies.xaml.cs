using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
    /// Interaction logic for viewQTaxonSpecies.xaml
    /// </summary>
    public partial class viewQTaxonSpecies : UserControl
    {
        public List<string> categories = new List<string>() { "Phylum", "Class", "Order", "Family", "Genus", "Author" };

        public viewQTaxonSpecies()
        {
            InitializeComponent();
            cbxCategories.ItemsSource = categories;
            //dgrSpeciesTable.ItemsSource = getAllSpeciesList();
        }

        private void cbxCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxCategories.SelectedIndex != -1)
            {
                switch (cbxCategories.SelectedValue as string)
                {
                    case "Phylum":
                        lstCategoryItems.ItemsSource = getPhylumList();
                        break;
                    case "Class":
                        lstCategoryItems.ItemsSource = getClassList();
                        break;
                    case "Order":
                        lstCategoryItems.ItemsSource = getOrderList();
                        break;
                    case "Family":
                        lstCategoryItems.ItemsSource = getFamilyList();
                        break;
                    case "Genus":
                        lstCategoryItems.ItemsSource = getGenusList();
                        break;
                    case "Author":
                        lstCategoryItems.ItemsSource = getAuthorsList();
                        break;
                }
            }
        }

        private void btnLoadTable_Click(object sender, RoutedEventArgs e)
        {
            if (cbxCategories.SelectedIndex != 1)
            {
                List<string> selectedItems = new List<string>();
                foreach (CheckListItem item in lstCategoryItems.Items)
                {
                    if (item.IsChecked)
                        selectedItems.Add(item.Item);
                }
                dgrSpeciesTable.ItemsSource = getSpeciesList(cbxCategories.SelectedItem.ToString(), selectedItems);
            }
        }

        private List<QuerySpecies> getAllSpeciesList()
        {
            List<QuerySpecies> species = new List<QuerySpecies>();
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strSpeciesNo, strDomainName, strKingdomName, strPhylumName, strClassName, " +
                                        "strOrderName, strFamilyName, strGenusName, strSpeciesName, strCommonName, " +
                                        "strScientificName, strAuthorsName, boolSpeciesIdentified " +
                                "FROM viewTaxonSpecies");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                species.Add(new QuerySpecies()
                {
                    SpeciesID = sqlData[0].ToString(),
                    DomainName = sqlData[1].ToString(),
                    KingdomName = sqlData[2].ToString(),
                    PhylumName = sqlData[3].ToString(),
                    ClassName = sqlData[4].ToString(),
                    OrderName = sqlData[5].ToString(),
                    FamilyName = sqlData[6].ToString(),
                    GenusName = sqlData[7].ToString(),
                    SpeciesName = sqlData[8].ToString(),
                    CommonName = sqlData[9].ToString(),
                    ScientificName = sqlData[10].ToString(),
                    SpeciesAuthor = sqlData[11].ToString(),
                    IdentifiedStatus = ( Convert.ToBoolean(sqlData[12]) ? "Known Species" : "Unknown Species" )
                });
            }
            connection.closeResult();

            return species;
        }

        private List<CheckListItem> getPhylumList()
        {
            List<CheckListItem> listItems = new List<CheckListItem>();
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intPhylumID, strPhylumName FROM viewTaxonPhylum");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                listItems.Add(new CheckListItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    IsChecked = false,
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();

            return listItems;
        }

        private List<CheckListItem> getClassList()
        {
            List<CheckListItem> listItems = new List<CheckListItem>();
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intClassID, strClassName FROM viewTaxonClass");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                listItems.Add(new CheckListItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    IsChecked = false,
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();

            return listItems;
        }

        private List<CheckListItem> getOrderList()
        {
            List<CheckListItem> listItems = new List<CheckListItem>();
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intOrderID, strOrderName FROM viewTaxonOrder");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                listItems.Add(new CheckListItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    IsChecked = false,
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();

            return listItems;
        }

        private List<CheckListItem> getFamilyList()
        {
            List<CheckListItem> listItems = new List<CheckListItem>();
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intFamilyID, strFamilyName FROM viewTaxonFamily");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                listItems.Add(new CheckListItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    IsChecked = false,
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();

            return listItems;
        }

        private List<CheckListItem> getGenusList()
        {
            List<CheckListItem> listItems = new List<CheckListItem>();
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intGenusID, strGenusName FROM viewTaxonGenus");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                listItems.Add(new CheckListItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    IsChecked = false,
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();

            return listItems;
        }
        
        private List<CheckListItem> getAuthorsList()
        {
            List<CheckListItem> listItems = new List<CheckListItem>();
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT intAuthorID, strAuthorsName FROM tblAuthor");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                listItems.Add(new CheckListItem()
                {
                    ID = Convert.ToInt32(sqlData[0]),
                    IsChecked = false,
                    Item = sqlData[1].ToString()
                });
            }
            connection.closeResult();

            return listItems;
        }

        private List<QuerySpecies> getSpeciesList(string category, List<string> selectedItems)
        {
            List<QuerySpecies> species = new List<QuerySpecies>();
            string column = "";

            foreach (string item in selectedItems)
            {
                DatabaseConnection connection = new DatabaseConnection();
                switch (category)
                {
                    case "Phylum":
                        column = "strPhylumName";
                        break;
                    case "Class":
                        column = "strClassName";
                        break;
                    case "Order":
                        column = "strOrderName";
                        break;
                    case "Family":
                        column = "strFamilyName";
                        break;
                    case "Genus":
                        column = "strGenusName";
                        break;
                    case "Author":
                        column = "strAuthorsName";
                        break;
                }

                connection.setQuery("SELECT strSpeciesNo, strDomainName, strKingdomName, strPhylumName, strClassName, " +
                            "strOrderName, strFamilyName, strGenusName, strSpeciesName, strCommonName, " +
                            "strScientificName, strAuthorsName, boolSpeciesIdentified " +
                    "FROM viewTaxonSpecies " +
                    "WHERE " + column + " = @value");
                connection.addParameter("@value", SqlDbType.VarChar, item);

                SqlDataReader sqlData = connection.executeResult();
                while (sqlData.Read())
                {
                    species.Add(new QuerySpecies()
                    {
                        SpeciesID = sqlData[0].ToString(),
                        DomainName = sqlData[1].ToString(),
                        KingdomName = sqlData[2].ToString(),
                        PhylumName = sqlData[3].ToString(),
                        ClassName = sqlData[4].ToString(),
                        OrderName = sqlData[5].ToString(),
                        FamilyName = sqlData[6].ToString(),
                        GenusName = sqlData[7].ToString(),
                        SpeciesName = sqlData[8].ToString(),
                        CommonName = sqlData[9].ToString(),
                        ScientificName = sqlData[10].ToString(),
                        SpeciesAuthor = sqlData[11].ToString(),
                        IdentifiedStatus = (Convert.ToBoolean(sqlData[12]) ? "Known Species" : "Unknown Species")
                    });
                }
                connection.closeResult();
            }
            return species;
        }
    }

    public class QuerySpecies
    {
        public string SpeciesID { get; set; }
        public string DomainName { get; set; }
        public string KingdomName { get; set; }
        public string PhylumName { get; set; }
        public string ClassName { get; set; }
        public string OrderName { get; set; }
        public string FamilyName { get; set; }
        public string GenusName { get; set; }
        public string SpeciesName { get; set; }
        public string CommonName { get; set; }
        public string ScientificName { get; set; }
        public string SpeciesAuthor { get; set; }
        public string IdentifiedStatus { get; set; }
    }

    public class CheckListItem
    {
        public int ID { get; set; }
        public bool IsChecked { get; set; }
        public string Item { get; set; }
    }
}
