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
    /// Interaction logic for pageTaxonGenus.xaml
    /// </summary>
    public partial class pageTaxonGenus : Page
    {
        List<TaxonGenus> TaxonomicGenus = new List<TaxonGenus>();

        public pageTaxonGenus()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfGenusID.Text is null || txfGenusID.Text == "")
                    addGenus();
                else
                    editGenus();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfGenusID.Clear();
            cbxFamilyName.SelectedIndex = -1;
            txfGenusName.Clear();

            msgFamilyName.Visibility = Visibility.Collapsed;
            msgGenusName.Visibility = Visibility.Collapsed;
        }

        private void btnAddGenus_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddGenus.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddGenus.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddGenus.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddGenus.Content = (state) ? "Close Panel" : "Add Genus";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in TaxonomicGenus
                         where record.GenusID.ToUpper().Contains(input) ||
                                record.FamilyName.ToUpper().Contains(input) ||
                                record.GenusName.ToUpper().Contains(input)
                         select record;

            dgrGenusTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            TaxonGenus SelectedGenus = dgrGenusTable.SelectedValue as TaxonGenus;

            var result = from genus in TaxonomicGenus
                         where genus.GenusID == SelectedGenus.GenusID
                         select genus;

            if (pnlAddGenus.Visibility == Visibility.Collapsed)
                btnAddGenus_Click(btnAddGenus, null);

            foreach (TaxonGenus data in result)
            {
                txfGenusID.Text = data.GenusID;
                cbxFamilyName.SelectedItem = data.FamilyName.ToString();
                txfGenusName.Text = data.GenusName;
            }
        }

        private void getGenusTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<TaxonGenus> genus = new List<TaxonGenus>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT strGenusNo, strFamilyName, strGenusName FROM viewTaxonGenus");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                genus.Add(new TaxonGenus()
                {
                    GenusID = sqlData[0].ToString(),
                    FamilyName = sqlData[1].ToString(),
                    GenusName = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrGenusTable.ItemsSource = genus;
            TaxonomicGenus = genus;
        }

        private void resetForm()
        {
            pnlAddGenus.Visibility = Visibility.Collapsed;
            sprAddGenus.Visibility = Visibility.Collapsed;
            btnAddGenus.Content = "Add Genus";
            btnClear_Click(btnClear, null);

            getFamilyList();
            getGenusTable();
        }

        private void getFamilyList()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();

            cbxFamilyName.Items.Clear();

            // Query Command Setting
            connection.setQuery("SELECT strFamilyName FROM tblFamily");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                cbxFamilyName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgFamilyName.Visibility = Visibility.Collapsed;
            msgGenusName.Visibility = Visibility.Collapsed;

            if (cbxFamilyName.SelectedIndex == -1)
            {
                msgFamilyName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfGenusName.Text == "")
            {
                msgGenusName.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addGenus()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertGenus");
            connection.addSprocParameter("@familyName", SqlDbType.VarChar, cbxFamilyName.SelectedItem.ToString());
            connection.addSprocParameter("@genusName", SqlDbType.VarChar, txfGenusName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Genus Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getGenusTable();
        }

        private void editGenus()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateGenus");
            connection.addSprocParameter("@genusNo", SqlDbType.VarChar, txfGenusID.Text);
            connection.addSprocParameter("@familyName", SqlDbType.VarChar, cbxFamilyName.SelectedItem.ToString());
            connection.addSprocParameter("@genusName", SqlDbType.VarChar, txfGenusName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Genus Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getGenusTable();
        }
    }
}

public class TaxonGenus
{
    public string GenusID { get; set; }
    public string FamilyName { get; set; }
    public string GenusName { get; set; }
}
