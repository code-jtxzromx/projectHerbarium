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
    /// Interaction logic for pageLoaning.xaml
    /// </summary>
    public partial class pageLoaning : Page
    {
        DateTime loanDate;
        DateTime returnDate;
        
        public pageLoaning()
        {
            InitializeComponent();

            if (StaticData.role == "STUDENT ASSISTANT")
                colActions.Visibility = Visibility.Collapsed;
            rbtResearch.IsChecked = true;

            getCollectorList();
            setDurationType();
            getFamilyList();
            getLoanTable();
        }

        private void btnAddLoan_Click(object sender, RoutedEventArgs e) => pnlLoanTransactionForm.Visibility = Visibility.Visible;

        private void rbtPurpose_CheckChanged(object sender, RoutedEventArgs e)
        {
            lblOtherPurpose.Visibility = (rbtOther.IsChecked == true) ? Visibility.Visible : Visibility.Collapsed;
            txfOtherPurpose.Visibility = (rbtOther.IsChecked == true) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void btnLoadGenus_Click(object sender, RoutedEventArgs e)
        {
            List<ListFamily> selectedFamilies = new List<ListFamily>();
            foreach (ListFamily family in dgrTaxonFamilies.Items)
            {
                if (family.IsChecked)
                    selectedFamilies.Add(family);
            }
            getGenusList(selectedFamilies);
        }
        
        private void btnCancelTransactionA_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult response = MessageBox.Show("Do you want to Cancel this Transaction",
                                                        "Cancel Loan Transaction",
                                                        MessageBoxButton.YesNo,
                                                        MessageBoxImage.Question);
            if (response == MessageBoxResult.Yes)
                pnlLoanTransactionForm.Visibility = Visibility.Hidden;
        }

        private void btnCancelTransactionB_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult response = MessageBox.Show("Do you want to Cancel this Transaction",
                                                        "Cancel Loan Transaction",
                                                        MessageBoxButton.YesNo,
                                                        MessageBoxImage.Question);
            if (response == MessageBoxResult.Yes)
                pnlPlantLoaningForm.Visibility = Visibility.Hidden;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cbxCollector.SelectedIndex = -1;
            dpkLoanDate.Text = "";
            txfDuration.Clear();
            cbxDuration.SelectedIndex = -1;
            rbtAcademic.IsChecked = true;
            txfOtherPurpose.Clear();

            foreach (ListFamily family in dgrTaxonFamilies.Items)
                family.IsChecked = false;
            dgrTaxonGenera.ItemsSource = null;
            dgrTaxonSpecies.ItemsSource = null;

            msgCollector.Visibility = Visibility.Collapsed;
            msgLoanDate.Visibility = Visibility.Collapsed;
            msgDuration.Visibility = Visibility.Collapsed;
            msgPurpose.Visibility = Visibility.Collapsed;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (validateLoanGenus())
            {
                if (validateForm())
                {
                    List<ListGenus> selectedGenus = new List<ListGenus>();
                    foreach (ListGenus genus in dgrTaxonGenera.Items)
                    {
                        if (genus.IsChecked)
                        {
                            selectedGenus.Add(genus);
                        }
                    }
                    loadSpeciesForm();
                    getSpeciesList(selectedGenus);

                    pnlLoanTransactionForm.Visibility = Visibility.Hidden;
                    pnlPlantLoaningForm.Visibility = Visibility.Visible;
                }
            }
            else
                MessageBox.Show("No selected Genus!", "Empty Selection", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            pnlLoanTransactionForm.Visibility = Visibility.Visible;
            pnlPlantLoaningForm.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateLoanSpecies())
            {
                MessageBoxResult response = MessageBox.Show("Do you want to Process this Loan Transaction?",
                                                            "Confirm Transaction",
                                                            MessageBoxButton.YesNo,
                                                            MessageBoxImage.Question);
                if (response == MessageBoxResult.Yes)
                {
                    int status;
                    string loanStatus = (StaticData.role == "STUDENT ASSISTANT") ? "Requesting" : "Approved";
                    string loanNumber = "";
                    DatabaseConnection connection = new DatabaseConnection();

                    connection.setStoredProc("dbo.procProcessLoan");
                    connection.addSprocParameter("@collectorName", SqlDbType.VarChar, lblCollector.Text);
                    connection.addSprocParameter("@startDate", SqlDbType.Date, loanDate);
                    connection.addSprocParameter("@endDate", SqlDbType.Date, returnDate);
                    connection.addSprocParameter("@purpose", SqlDbType.VarChar, lblPurpose.Text);
                    connection.addSprocParameter("@status", SqlDbType.VarChar, loanStatus);
                    status = connection.executeProcedure();
                    
                    if (status == 0)
                    {
                        connection.setQuery("SELECT strLoanNumber " +
                                            "FROM viewPlantLoans " +
                                            "WHERE strCollector = @collector AND dateLoan = @startdate AND dateReturning = @enddate");
                        connection.addParameter("@collector", SqlDbType.VarChar, lblCollector.Text);
                        connection.addParameter("@startdate", SqlDbType.Date, loanDate);
                        connection.addParameter("@enddate", SqlDbType.Date, returnDate);

                        SqlDataReader sqlData = connection.executeResult();
                        while (sqlData.Read())
                        {
                            loanNumber = sqlData[0].ToString();
                        }
                        connection.closeResult();

                        foreach (ListSpecies list in dgrTaxonSpecies.Items)
                        {
                            if (list.IsChecked)
                            {
                                connection.setStoredProc("dbo.procLoanPlants");
                                connection.addSprocParameter("@loanNumber", SqlDbType.VarChar, loanNumber);
                                connection.addSprocParameter("@taxonName", SqlDbType.VarChar, list.TaxonName);
                                connection.addSprocParameter("@copies", SqlDbType.Int, list.Copies);
                                status = connection.executeProcedure();

                                if (status == 1)
                                    break;
                            }
                        }

                        switch (status)
                        {
                            case 0:
                                MessageBox.Show("Plant Deposit Transaction Processed Successfully");
                                break;
                            case 1:
                                MessageBox.Show("Plant Deposit Transaction Processed with Some Error Records");
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The System had run to an Error");
                    }
                }

                pnlPlantLoaningForm.Visibility = Visibility.Hidden;
                getLoanTable();
            }
        }

        private void getCollectorList()
        {
            cbxCollector.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strFullName FROM viewCollector");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                cbxCollector.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private void getFamilyList()
        {
            List<ListFamily> families = new List<ListFamily>();
            dgrTaxonFamilies.Items.Clear();

            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT strFamilyName FROM viewTaxonFamily ORDER BY strFamilyName ASC");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                families.Add(new ListFamily()
                {
                    FamilyName = sqlData[0].ToString()
                });
            }
            connection.closeResult();
            dgrTaxonFamilies.ItemsSource = families;
        }

        private void getGenusList(List<ListFamily> families)
        {
            dgrTaxonGenera.ItemsSource = null;
            List<ListGenus> genera = new List<ListGenus>();

            foreach (ListFamily family in families)
            {
                DatabaseConnection connection = new DatabaseConnection();
                connection.setQuery("SELECT strGenusName FROM viewTaxonGenus WHERE strFamilyName = @familyName ORDER BY strGenusName ASC");
                connection.addParameter("@familyName", SqlDbType.VarChar, family.FamilyName);

                SqlDataReader sqlData = connection.executeResult();
                while (sqlData.Read())
                {
                    genera.Add(new ListGenus()
                    {
                        GenusName = sqlData[0].ToString()
                    });
                }
                connection.closeResult();
            }
            dgrTaxonGenera.ItemsSource = genera;
        }

        private void getSpeciesList(List<ListGenus> genera)
        {
            dgrTaxonSpecies.ItemsSource = null;
            List<ListSpecies> species = new List<ListSpecies>();

            foreach (ListGenus genus in genera)
            {
                DatabaseConnection connection = new DatabaseConnection();
                connection.setQuery("SELECT TS.strScientificName, COUNT(HI.intStoredSheetID) - ISNULL(SUM(LS.intCopies), 0) " +
                                    "FROM viewTaxonSpecies TS " +
                                        "LEFT JOIN viewHerbariumInventory HI ON TS.strScientificName = HI.strScientificName AND HI.boolLoanAvailable = 1 " +
                                        "LEFT JOIN tblLoaningSpecies LS ON TS.intSpeciesID = LS.intSpeciesID " +
                                        "LEFT JOIN tblPlantLoanTransaction LT ON LT.intLoanID = LS.intLoanID AND LT.strStatus IN('Approved', 'Requesting') " +
                                    "WHERE TS.strGenusName = @genusname " +
                                    "GROUP BY TS.strScientificName " +
                                    "ORDER BY TS.strScientificName ASC");
                connection.addParameter("@genusName", SqlDbType.VarChar, genus.GenusName);

                SqlDataReader sqlData = connection.executeResult();
                while (sqlData.Read())
                {
                    if (Convert.ToInt32(sqlData[1]) > 0)
                    {
                        species.Add(new ListSpecies()
                        {
                            TaxonName = sqlData[0].ToString(),
                            Specimens = Convert.ToInt32(sqlData[1])
                        });
                    }
                }
                connection.closeResult();
            }
            dgrTaxonSpecies.ItemsSource = species;
        }

        public void getLoanTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<PlantLoans> loans = new List<PlantLoans>();

            // Query Command Setting
            connection.setQuery("SELECT strLoanNumber, strCollector, dateLoan, dateReturning, strDuration, " +
                                    "dateProcessed, strPurpose, strStatus " +
                                "FROM viewPlantLoans");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                loans.Add(new PlantLoans()
                {
                    LoanNumber = sqlData[0].ToString(),
                    Collector = sqlData[1].ToString(),
                    StartDate = sqlData[2].ToString(),
                    ReturningDate = sqlData[3].ToString(),
                    Duration = sqlData[4].ToString(),
                    DateProcessed = sqlData[5].ToString(),
                    Purpose = sqlData[6].ToString(),
                    Status = sqlData[7].ToString()
                });
            }
            connection.closeResult();
            
            dgrPlantLoans.ItemsSource = loans;
        }

        private void setDurationType()
        {
            string[] duration = new string[] { "Day/s", "Month/s" };

            foreach(string range in duration)
            {
                cbxDuration.Items.Add(range);
            }
        }

        private bool validateForm()
        {
            bool formOk = true;
            msgCollector.Visibility = Visibility.Collapsed;
            msgLoanDate.Visibility = Visibility.Collapsed;
            msgDuration.Visibility = Visibility.Collapsed;
            msgPurpose.Visibility = Visibility.Collapsed;

            if (cbxCollector.SelectedIndex == -1)
            {
                msgCollector.Visibility = Visibility.Visible;
                formOk = false;
            }
            if (dpkLoanDate.Text == "")
            {
                msgLoanDate.Visibility = Visibility.Visible;
                formOk = false;
            }
            if (txfDuration.Text == "" || cbxDuration.SelectedIndex == -1)
            {
                msgDuration.Visibility = Visibility.Visible;
                formOk = false;
            }
            if (rbtOther.IsChecked == true && txfOtherPurpose.Text == "")
            {
                msgPurpose.Visibility = Visibility.Visible;
                formOk = false;
            }

            return formOk;
        }

        private void loadSpeciesForm()
        {
            loanDate = Convert.ToDateTime(dpkLoanDate.Text);
            switch (cbxDuration.SelectedIndex)
            {
                case 0:
                    returnDate = loanDate.AddDays(Convert.ToDouble(txfDuration.Text));
                    break;
                case 1:
                    returnDate = loanDate.AddMonths(Convert.ToInt32(txfDuration.Text));
                    break;
            }
            if (rbtResearch.IsChecked == true)
                lblPurpose.Text = "Research";
            else if (rbtAcademic.IsChecked == true)
                lblPurpose.Text = "Academic";
            else if (rbtOther.IsChecked == true)
                lblPurpose.Text = txfOtherPurpose.Text;

            lblCollector.Text = cbxCollector.SelectedItem.ToString();
            lblDuration.Text = loanDate.ToShortDateString() + " - " + returnDate.ToShortDateString();
        }

        private bool validateLoanGenus()
        {
            bool formOK = false;

            foreach (ListGenus list in dgrTaxonGenera.Items)
                if (list.IsChecked) formOK = true;

            return formOK;
        }

        private bool validateLoanSpecies()
        {
            bool formOK = false;
            bool exceedError = false;
            bool noCheckError = true;

            foreach(ListSpecies species in dgrTaxonSpecies.Items)
            {
                if (species.IsChecked)
                {
                    noCheckError = false;
                    formOK = true;

                    if (species.Specimens < species.Copies)
                    {
                        exceedError = true;
                        formOK = false;
                    }
                }
            }
            if (exceedError)
                MessageBox.Show("The Number of Copies you Request should not be more than the actual Available Specimens",
                                "Required Copies exceeded available Specimens", MessageBoxButton.OK, MessageBoxImage.Error);
            if (noCheckError)
                MessageBox.Show("No selected Species!", "Selection Empty", MessageBoxButton.OK, MessageBoxImage.Error);

            return formOK;
        }
    }
}

public class PlantLoans
{
    public string LoanNumber { get; set; }
    public string Collector { get; set; }
    public string StartDate { get; set; }
    public string ReturningDate { get; set; }
    public string Duration { get; set; }
    public string DateProcessed { get; set; }
    public string Purpose { get; set; }
    public string Status { get; set; }
}

public class ListFamily
{
    public string FamilyName { get; set; }
    public bool IsChecked { get; set; }
}

public class ListGenus
{
    public string GenusName { get; set; }
    public bool IsChecked { get; set; }
}

public class ListSpecies
{
    public string FamilyName { get; set; }
    public string TaxonName { get; set; }
    public int Specimens { get; set; }
    public int Copies { get; set; }
    public bool IsChecked { get; set; }
}