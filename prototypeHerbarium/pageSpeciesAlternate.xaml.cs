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
    /// Interaction logic for pageSpeciesAlternate.xaml
    /// </summary>
    public partial class pageSpeciesAlternate : Page
    {
        List<SpeciesAlternate> AlternateNames = new List<SpeciesAlternate>();

        public pageSpeciesAlternate()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfAltNameID.Text is null || txfAltNameID.Text == "")
                    addAlternateName();
                else
                    editAlternateName();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lblSpeciesAlternateForm.Text = "Add Species Alternate Name";
            txfAltNameID.Clear();
            cbxTaxonName.SelectedIndex = -1;
            txfLanguage.Clear();
            txfAlternateName.Clear();
            btnSave.Content = "Save";

            msgTaxonName.Visibility = Visibility.Collapsed;
            msgLanguage.Visibility = Visibility.Collapsed;
            msgAlternateName.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            lblSpeciesAlternateForm.Text = "Edit Species Alternate Name";
            SpeciesAlternate selectedAlternate = dgrSpeciesAlternateTable.SelectedValue as SpeciesAlternate;
            
            var result = from alternate in AlternateNames
                         where alternate.AltNameID == selectedAlternate.AltNameID
                         select alternate;

            if (pnlAddSpeciesAlternate.Visibility == Visibility.Collapsed)
                btnAddSpeciesAlternate_Click(btnAddSpeciesAlternate, null);

            foreach (SpeciesAlternate data in result)
            {
                txfAltNameID.Text = data.AltNameID.ToString();
                cbxTaxonName.SelectedItem = data.TaxonName;
                txfLanguage.Text = data.Language;
                txfAlternateName.Text = data.AlternateName;
            }
        }

        private void btnAddSpeciesAlternate_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddSpeciesAlternate.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddSpeciesAlternate.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddSpeciesAlternate.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddSpeciesAlternate.Content = (state) ? "Close Panel" : "Add Alternate Name";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in AlternateNames
                         where record.TaxonName.ToUpper().Contains(input) ||
                                record.Language.ToUpper().Contains(input) ||
                                record.AlternateName.ToUpper().Contains(input)
                         select record;
            
            dgrSpeciesAlternateTable.ItemsSource = result;
        }

        private void resetForm()
        {
            lblSpeciesAlternateForm.Text = "Add Species Alternate Name";
            pnlAddSpeciesAlternate.Visibility = Visibility.Collapsed;
            sprAddSpeciesAlternate.Visibility = Visibility.Collapsed;
            btnAddSpeciesAlternate.Content = "Add Alternate Name";
            btnClear_Click(btnClear, null);

            getSpeciesList();
            getSpeciesAlternateTable();
        }

        private void getSpeciesList()
        {
            cbxTaxonName.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strScientificName FROM viewTaxonSpecies ORDER BY strScientificName");
            
            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxTaxonName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private void getSpeciesAlternateTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<SpeciesAlternate> alternates = new List<SpeciesAlternate>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT intAltNameID, strScientificName, strLanguage, strAlternateName " +
                                "FROM viewSpeciesAlternate " +
                                "ORDER BY strScientificName, strLanguage");
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                alternates.Add(new SpeciesAlternate()
                {
                    AltNameID = Convert.ToInt32(sqlData[0]),
                    TaxonName = sqlData[1].ToString(),
                    Language = sqlData[2].ToString(),
                    AlternateName = sqlData[3].ToString()
                });
            }
            connection.closeResult();

            dgrSpeciesAlternateTable.ItemsSource = alternates;
            AlternateNames = alternates;
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgTaxonName.Visibility = Visibility.Collapsed;
            msgLanguage.Visibility = Visibility.Collapsed;
            msgAlternateName.Visibility = Visibility.Collapsed;

            if (cbxTaxonName.SelectedIndex == -1)
            {
                msgTaxonName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfLanguage.Text == "")
            {
                msgLanguage.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfAlternateName.Text == "")
            {
                msgAlternateName.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addAlternateName()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertSpeciesAlternate");
            connection.addSprocParameter("@taxonName", SqlDbType.VarChar, cbxTaxonName.SelectedItem);
            connection.addSprocParameter("@language", SqlDbType.VarChar, txfLanguage.Text);
            connection.addSprocParameter("@alternateName", SqlDbType.VarChar, txfAlternateName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Species Alternate Name Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getSpeciesAlternateTable();
        }

        private void editAlternateName()
        {

            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateSpeciesAlternate");
            connection.addSprocParameter("@altNameID", SqlDbType.VarChar, txfAltNameID.Text);
            connection.addSprocParameter("@taxonName", SqlDbType.VarChar, cbxTaxonName.SelectedItem);
            connection.addSprocParameter("@language", SqlDbType.VarChar, txfLanguage.Text);
            connection.addSprocParameter("@alternateName", SqlDbType.VarChar, txfAlternateName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Species Alternate Name Updated to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getSpeciesAlternateTable();
        }
    }
}

public class SpeciesAlternate
{
    public int AltNameID { get; set; }
    public string TaxonName { get; set; }
    public string Language { get; set; }
    public string AlternateName { get; set; }
}