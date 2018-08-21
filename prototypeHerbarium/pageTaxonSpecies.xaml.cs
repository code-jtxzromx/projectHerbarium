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
    /// Interaction logic for pageTaxonSpecies.xaml
    /// </summary>
    public partial class pageTaxonSpecies : Page
    {
        List<TaxonSpecies> TaxonomicSpecies = new List<TaxonSpecies>();

        public pageTaxonSpecies()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfSpecieID.Text is null || txfSpecieID.Text == "")
                    addSpecie();
                else
                    editSpecie();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            chkIsUndeterminedSpecies.IsChecked = true;
            chkIsUndeterminedSpecies.IsEnabled = true;
            txfSpecieID.Clear();
            cbxGenusName.SelectedIndex = -1;
            txfSpeciesName.Clear();
            txfCommonName.Clear();
            txfScientificName.Clear();
            txfAuthor.Clear();
            txfSynonyms.Clear();

            msgGenusName.Visibility = Visibility.Collapsed;
            msgSpeciesName.Visibility = Visibility.Collapsed;
            msgCommonName.Visibility = Visibility.Collapsed;
            msgScientificName.Visibility = Visibility.Collapsed;
            msgAuthor.Visibility = Visibility.Collapsed;
        }

        private void btnAddSpecie_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddSpecie.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddSpecie.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddSpecie.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddSpecie.Content = (state) ? "Close Panel" : "Add Species";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in TaxonomicSpecies
                         where record.SpecieID.ToUpper().Contains(input) ||
                                record.GenusName.ToUpper().Contains(input) ||
                                record.SpeciesName.ToUpper().Contains(input) ||
                                record.CommonName.ToUpper().Contains(input) ||
                                record.ScientificName.ToUpper().Contains(input)
                         select record;

            dgrSpeciesTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            TaxonSpecies SelectedSpecie = dgrSpeciesTable.SelectedValue as TaxonSpecies;

            var result = from specie in TaxonomicSpecies
                         where specie.SpecieID == SelectedSpecie.SpecieID
                         select specie;

            if (pnlAddSpecie.Visibility == Visibility.Collapsed)
                btnAddSpecie_Click(btnAddSpecie, null);

            foreach (TaxonSpecies data in result)
            {
                chkIsUndeterminedSpecies.IsChecked = (data.IdentifiedStatus == "Known");
                chkIsUndeterminedSpecies.IsEnabled = false;
                txfSpecieID.Text = data.SpecieID;
                cbxGenusName.SelectedItem = data.GenusName;
                txfSpeciesName.Text = data.SpeciesName;
                txfScientificName.Text = data.ScientificName;
                txfCommonName.Text = data.CommonName;
                txfAuthor.Text = data.SpeciesAuthor;
                txfSynonyms.Text = data.AlternateNames;
            }
        }
        
        private void chkIsUndeterminedSpecies_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if(chkIsUndeterminedSpecies.IsChecked == true)
            {
                txfSpeciesName.IsEnabled = true;
                txfSpeciesName.Clear();
            }
            else
            {
                txfSpeciesName.IsEnabled = false;
                txfSpeciesName.Text = "sp.";
            }
        }

        private void resetForm()
        {
            pnlAddSpecie.Visibility = Visibility.Collapsed;
            sprAddSpecie.Visibility = Visibility.Collapsed;
            btnAddSpecie.Content = "Add Species";
            btnClear_Click(btnClear, null);
            
            getSpecieTable();
            getGenusList();
        }

        private void getSpecieTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<TaxonSpecies> species = new List<TaxonSpecies>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT strSpeciesNo, strGenusName, strSpeciesName, strCommonName, strScientificName, " +
                                    "strSpeciesAuthor, strSpeciesAlternateName, boolSpeciesIdentified " +
                                "FROM viewTaxonSpecies");
            SqlDataReader sqlData = connection.executeResult();
             
            while (sqlData.Read())
            {
                species.Add(new TaxonSpecies()
                {
                    SpecieID = sqlData[0].ToString(),
                    GenusName = sqlData[1].ToString(),
                    SpeciesName = sqlData[2].ToString(),
                    CommonName = sqlData[3].ToString(),
                    ScientificName = sqlData[4].ToString(),
                    SpeciesAuthor = sqlData[5].ToString(),
                    AlternateNames = sqlData[6].ToString(),
                    IdentifiedStatus = (Convert.ToBoolean(sqlData[7].ToString())) ? "Known" : "Undetermined"
                });
            }
            connection.closeResult();

            dgrSpeciesTable.ItemsSource = species;
            TaxonomicSpecies = species;
        }

        private void getGenusList()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();

            cbxGenusName.Items.Clear();

            // Query Command Setting
            connection.setQuery("SELECT strGenusName FROM tblGenus");
            
            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                cbxGenusName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgGenusName.Visibility = Visibility.Collapsed;
            msgSpeciesName.Visibility = Visibility.Collapsed;
            msgCommonName.Visibility = Visibility.Collapsed;

            if (cbxGenusName.SelectedIndex == -1)
            {
                msgGenusName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfSpeciesName.Text == "")
            {
                msgSpeciesName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfCommonName.Text == "")
            {
                msgCommonName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfScientificName.Text == "")
            {
                msgScientificName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfAuthor.Text == "")
            {
                msgAuthor.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addSpecie()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertSpecies");
            connection.addSprocParameter("@genusName", SqlDbType.VarChar, cbxGenusName.SelectedItem.ToString());
            connection.addSprocParameter("@speciesName", SqlDbType.VarChar, txfSpeciesName.Text);
            connection.addSprocParameter("@commonName", SqlDbType.VarChar, txfCommonName.Text);
            connection.addSprocParameter("@scientificName", SqlDbType.VarChar, txfScientificName.Text);
            connection.addSprocParameter("@author", SqlDbType.VarChar, txfAuthor.Text);
            connection.addSprocParameter("@alternateName", SqlDbType.VarChar, txfSynonyms.Text);
            connection.addSprocParameter("@isVerified", SqlDbType.Bit, (chkIsUndeterminedSpecies.IsChecked == true));
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Species Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getSpecieTable();
        }

        private void editSpecie()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateSpecies");
            connection.addSprocParameter("@speciesNo", SqlDbType.VarChar, txfSpecieID.Text);
            connection.addSprocParameter("@genusName", SqlDbType.VarChar, cbxGenusName.SelectedItem.ToString());
            connection.addSprocParameter("@speciesName", SqlDbType.VarChar, txfSpeciesName.Text);
            connection.addSprocParameter("@commonName", SqlDbType.VarChar, txfCommonName.Text);
            connection.addSprocParameter("@scientificName", SqlDbType.VarChar, txfScientificName.Text);
            connection.addSprocParameter("@author", SqlDbType.VarChar, txfAuthor.Text);
            connection.addSprocParameter("@alternateName", SqlDbType.VarChar, txfSynonyms.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Species Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getSpecieTable();
        }
    }
}

public class TaxonSpecies
{
    public string SpecieID { get; set; }
    public string GenusName { get; set; }
    public string SpeciesName { get; set; }
    public string CommonName { get; set; }
    public string ScientificName { get; set; }
    public string SpeciesAuthor { get; set; }
    public string AlternateNames { get; set; }
    public string IdentifiedStatus { get; set; }
}
