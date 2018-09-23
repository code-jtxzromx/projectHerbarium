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
    /// Interaction logic for pageTaxonPhylum.xaml
    /// </summary>
    public partial class pageTaxonPhylum : Page
    {
        List<TaxonPhylum> TaxonomicPhyla = new List<TaxonPhylum>();

        public pageTaxonPhylum()
        {
            InitializeComponent();

            chkIsKingdomPlant.IsChecked = true;
            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(validateForm())
            {
                if (txfPhylumID.Text is null || txfPhylumID.Text == "")
                    addPhylum();
                else
                    editPhylum();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lblPhylumForm.Text = "Add Phylum";
            txfPhylumID.Clear();
            txfPhylumName.Clear();
            chkIsKingdomPlant.IsChecked = true;
            chkIsKingdomPlant_CheckChanged(chkIsKingdomPlant, null);
            btnSave.Content = "Save";

            msgDomainName.Visibility = Visibility.Collapsed;
            msgKingdomName.Visibility = Visibility.Collapsed;
            msgPhylumName.Visibility = Visibility.Collapsed;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            lblPhylumForm.Text = "Edit Phylum";
            TaxonPhylum selectedFamily = dgrPhylumTable.SelectedValue as TaxonPhylum;

            var result = from phylum in TaxonomicPhyla
                         where phylum.PhylumID == selectedFamily.PhylumID
                         select phylum;

            if (pnlAddPhylum.Visibility == Visibility.Collapsed)
                btnAddPhylum_Click(btnAddPhylum, null);

            foreach (TaxonPhylum data in result)
            {
                txfPhylumID.Text = data.PhylumID;
                txfPhylumName.Text = data.PhylumName;

                if(data.DomainName == "Eukaryota" && data.KingdomName == "Plantae")
                {
                    chkIsKingdomPlant.IsChecked = true;
                    chkIsKingdomPlant_CheckChanged(chkIsKingdomPlant, null);
                }
                else
                {
                    chkIsKingdomPlant.IsChecked = false;
                    chkIsKingdomPlant_CheckChanged(chkIsKingdomPlant, null);
                    txfDomainName.Text = data.DomainName;
                    txfKingdomName.Text = data.KingdomName;
                }
            }
        }

        private void btnAddPhylum_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddPhylum.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddPhylum.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddPhylum.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddPhylum.Content = (state) ? "Close Panel" : "Add Family";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in TaxonomicPhyla
                         where record.PhylumID.ToUpper().Contains(input) ||
                                record.KingdomName.ToUpper().Contains(input) ||
                                record.PhylumName.ToUpper().Contains(input)
                         select record;

            dgrPhylumTable.ItemsSource = result;
        }

        private void chkIsKingdomPlant_CheckChanged(object sender, RoutedEventArgs e)
        {
            if(chkIsKingdomPlant.IsChecked == true)
            {
                txfDomainName.IsEnabled = false;
                txfDomainName.Text = "Eukaryota";
                txfKingdomName.IsEnabled = false;
                txfKingdomName.Text = "Plantae";
            }
            else
            {
                txfDomainName.IsEnabled = true;
                txfDomainName.Clear();
                txfKingdomName.IsEnabled = true;
                txfKingdomName.Clear();
            }
        }

        private void resetForm()
        {
            lblPhylumForm.Text = "Add Phylum";
            pnlAddPhylum.Visibility = Visibility.Collapsed;
            sprAddPhylum.Visibility = Visibility.Collapsed;
            btnAddPhylum.Content = "Add Phylum";
            btnClear_Click(btnClear, null);

            getPhylumTable();
        }

        private void getPhylumTable()
        {
            DatabaseConnection connection = new DatabaseConnection();
            List<TaxonPhylum> phyla = new List<TaxonPhylum>();

            btnClear_Click(btnClear, null);

            connection.setQuery("SELECT strPhylumNo, strDomainName, strKingdomName, strPhylumName " +
                                "FROM viewTaxonPhylum " +
                                "ORDER BY strDomainName, strKingdomName, strPhylumName");
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                phyla.Add(new TaxonPhylum()
                {
                    PhylumID = sqlData[0].ToString(),
                    DomainName = sqlData[1].ToString(),
                    KingdomName = sqlData[2].ToString(),
                    PhylumName = sqlData[3].ToString()
                });
            }
            connection.closeResult();

            dgrPhylumTable.ItemsSource = phyla;
            TaxonomicPhyla = phyla;
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgDomainName.Visibility = Visibility.Collapsed;
            msgKingdomName.Visibility = Visibility.Collapsed;
            msgPhylumName.Visibility = Visibility.Collapsed;

            if (txfDomainName.Text == "")
            {
                msgDomainName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfKingdomName.Text == "")
            {
                msgKingdomName.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfPhylumName.Text == "")
            {
                msgPhylumName.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addPhylum()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertPhylum");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@domainName", SqlDbType.VarChar, txfDomainName.Text);
            connection.addSprocParameter("@kingdomName", SqlDbType.VarChar, txfKingdomName.Text);
            connection.addSprocParameter("@phylumName", SqlDbType.VarChar, txfPhylumName.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Phylum Added to the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getPhylumTable();
        }

        private void editPhylum()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdatePhylum");
            connection.addSprocParameter("@isIDBase", SqlDbType.Bit, 0);
            connection.addSprocParameter("@phylumNo", SqlDbType.VarChar, txfPhylumID.Text);
            connection.addSprocParameter("@domainName", SqlDbType.VarChar, txfDomainName.Text);
            connection.addSprocParameter("@kingdomName", SqlDbType.VarChar, txfKingdomName.Text);
            connection.addSprocParameter("@phylumName", SqlDbType.VarChar, txfPhylumName.Text);
            status = connection.executeProcedure();
            
            switch (status)
            {
                case 0:
                    MessageBox.Show("Phylum Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getPhylumTable();
        }
    }
}

public class TaxonPhylum
{
    public string PhylumID { get; set; }
    public string DomainName { get; set; }
    public string KingdomName { get; set; }
    public string PhylumName { get; set; }
}