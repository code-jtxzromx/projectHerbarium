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
            lblSpeciesForm.Text = "Add Species";
            chkIsUndeterminedSpecies.IsChecked = true;
            chkIsUndeterminedSpecies.IsEnabled = true;
            txfSpecieID.Clear();
            cbxGenusName.SelectedIndex = -1;
            txfSpeciesName.Clear();
            txfCommonName.Clear();
            cbxAuthor.SelectedIndex = -1;
            btnSave.Content = "Save";

            msgGenusName.Visibility = Visibility.Collapsed;
            msgSpeciesName.Visibility = Visibility.Collapsed;
            msgCommonName.Visibility = Visibility.Collapsed;
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
            btnSave.Content = "Update";
            lblSpeciesForm.Text = "Edit Species";
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
                txfCommonName.Text = data.CommonName;
                cbxAuthor.SelectedItem = data.SpeciesAuthor;
            }
        }
        
        private void chkIsUndeterminedSpecies_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if(chkIsUndeterminedSpecies.IsChecked == true)
            {
                txfSpeciesName.IsEnabled = true;
                txfSpeciesName.Clear();
                lblAuthor.Visibility = Visibility.Visible;
                cbxAuthor.Visibility = Visibility.Visible;
            }
            else
            {
                txfSpeciesName.IsEnabled = false;
                txfSpeciesName.Text = "sp.";
                lblAuthor.Visibility = Visibility.Collapsed;
                cbxAuthor.Visibility = Visibility.Collapsed;
            }
        }

        private void resetForm()
        {
            lblSpeciesForm.Text = "Add Species";
            pnlAddSpecie.Visibility = Visibility.Collapsed;
            sprAddSpecie.Visibility = Visibility.Collapsed;
            btnAddSpecie.Content = "Add Species";
            btnClear_Click(btnClear, null);
            
            getSpecieTable();
            getGenusList();
            getAuthorsList();
        }

        private void getSpecieTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<TaxonSpecies> species = new List<TaxonSpecies>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT strSpeciesNo, strGenusName, strSpeciesName, strCommonName, strScientificName, " +
                                    "strAuthorsName, boolSpeciesIdentified " +
                                "FROM viewTaxonSpecies " +
                                "ORDER BY strScientificName");
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
                    IdentifiedStatus = (Convert.ToBoolean(sqlData[6].ToString())) ? "Known" : "Undetermined"
                });
            }
            connection.closeResult();

            dgrSpeciesTable.ItemsSource = species;
            TaxonomicSpecies = species;
        }

        private void getGenusList()
        {
            cbxGenusName.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strGenusName FROM tblGenus ORDER BY strGenusName");
            
            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxGenusName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private void getAuthorsList()
        {
            cbxAuthor.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strAuthorsName FROM tblAuthor ORDER BY strAuthorsName");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxAuthor.Items.Add(sqlData[0]);
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
            if (chkIsUndeterminedSpecies.IsChecked == true && cbxAuthor.SelectedIndex == -1)
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
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@genusName", SqlDbType.VarChar, cbxGenusName.SelectedItem.ToString());
            connection.addSprocParameter("@speciesName", SqlDbType.VarChar, txfSpeciesName.Text);
            connection.addSprocParameter("@commonName", SqlDbType.VarChar, txfCommonName.Text);
            connection.addSprocParameter("@author", SqlDbType.VarChar, cbxAuthor.SelectedItem.ToString());
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
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@speciesNo", SqlDbType.VarChar, txfSpecieID.Text);
            connection.addSprocParameter("@genusName", SqlDbType.VarChar, cbxGenusName.SelectedItem.ToString());
            connection.addSprocParameter("@speciesName", SqlDbType.VarChar, txfSpeciesName.Text);
            connection.addSprocParameter("@commonName", SqlDbType.VarChar, txfCommonName.Text);
            connection.addSprocParameter("@author", SqlDbType.VarChar, cbxAuthor.SelectedItem.ToString());
            connection.addSprocParameter("@isVerified", SqlDbType.Bit, (chkIsUndeterminedSpecies.IsChecked == true));
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
    public string IdentifiedStatus { get; set; }
}
