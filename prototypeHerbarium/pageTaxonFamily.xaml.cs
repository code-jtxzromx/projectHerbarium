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
    /// Interaction logic for pageTaxonFamily.xaml
    /// </summary>
    public partial class pageTaxonFamily : Page
    {
        List<TaxonFamily> TaxonomicFamilies = new List<TaxonFamily>();

        public pageTaxonFamily()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfFamilyID.Text is null || txfFamilyID.Text == "")
                    addFamily();
                else
                    editFamily();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lblFamilyForm.Text = "Add Family";
            txfFamilyID.Clear();
            cbxOrderName.SelectedIndex = -1;
            txfFamilyName.Clear();
            btnSave.Content = "Save";

            msgOrderName.Visibility = Visibility.Collapsed;
            msgFamilyName.Visibility = Visibility.Collapsed;
        }

        private void btnAddFamily_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddFamily.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddFamily.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddFamily.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddFamily.Content = (state) ? "Close Panel" : "Add Family";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in TaxonomicFamilies
                         where record.FamilyID.ToUpper().Contains(input) ||
                                record.OrderName.ToUpper().Contains(input) ||
                                record.FamilyName.ToUpper().Contains(input)
                         select record;

            dgrFamilyTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            lblFamilyForm.Text = "Edit Family";
            TaxonFamily selectedFamily = dgrFamilyTable.SelectedValue as TaxonFamily;

            var result = from family in TaxonomicFamilies
                         where family.FamilyID == selectedFamily.FamilyID
                         select family;

            if (pnlAddFamily.Visibility == Visibility.Collapsed)
                btnAddFamily_Click(btnAddFamily, null);

            foreach (TaxonFamily data in result)
            {
                txfFamilyID.Text = data.FamilyID;
                cbxOrderName.SelectedItem = data.OrderName;
                txfFamilyName.Text = data.FamilyName;
            }
        }

        private void resetForm()
        {
            lblFamilyForm.Text = "Add Family";
            pnlAddFamily.Visibility = Visibility.Collapsed;
            sprAddFamily.Visibility = Visibility.Collapsed;
            btnAddFamily.Content = "Add Family";
            btnClear_Click(btnClear, null);

            getFamilyTable();
            getOrderList();
        }

        private void getFamilyTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<TaxonFamily> families = new List<TaxonFamily>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT strFamilyNo, strOrderName, strFamilyName " +
                                "FROM viewTaxonFamily " +
                                "ORDER BY strOrderName, strFamilyName");
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                families.Add(new TaxonFamily()
                {
                    FamilyID = sqlData[0].ToString(),
                    OrderName = sqlData[1].ToString(),
                    FamilyName = sqlData[2].ToString()
                });
            }
            connection.closeResult();

            dgrFamilyTable.ItemsSource = families;
            TaxonomicFamilies = families;
        }

        private void getOrderList()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();

            cbxOrderName.Items.Clear();

            // Query Command Setting
            connection.setQuery("SELECT strOrderName FROM tblOrder ORDER BY strOrderName");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                cbxOrderName.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgOrderName.Visibility = Visibility.Collapsed;
            msgFamilyName.Visibility = Visibility.Collapsed;

            if (cbxOrderName.SelectedIndex == -1)
            {
                msgOrderName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfFamilyName.Text == "")
            {
                msgFamilyName.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addFamily()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertFamily");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@orderName", SqlDbType.VarChar, cbxOrderName.SelectedItem.ToString());
            connection.addSprocParameter("@familyName", SqlDbType.VarChar, txfFamilyName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Family Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getFamilyTable();
        }

        private void editFamily()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateFamily");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@familyNo", SqlDbType.VarChar, txfFamilyID.Text);
            connection.addSprocParameter("@orderName", SqlDbType.VarChar, cbxOrderName.SelectedItem.ToString());
            connection.addSprocParameter("@familyName", SqlDbType.VarChar, txfFamilyName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Family Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getFamilyTable();
        }
    }
}

public class TaxonFamily
{
    public string FamilyID { get; set; }
    public string OrderName { get; set; }
    public string FamilyName { get; set; }
}