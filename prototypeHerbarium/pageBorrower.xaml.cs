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
    /// Interaction logic for pageBorrower.xaml
    /// </summary>
    public partial class pageBorrower : Page
    {
        List<Borrower> BorrowersList = new List<Borrower>();

        public pageBorrower()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfBorrowerID.Text is null || txfBorrowerID.Text == "")
                    addBorrower();
                else
                    editBorrower();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfBorrowerID.Clear();
            txfFirstname.Clear();
            txfMiddlename.Clear();
            txfLastname.Clear();
            txfMiddleInitial.Clear();
            txfNameSuffix.Clear();
            txfHomeAddress.Clear();
            txfContactNumber.Clear();
            txfEmailAddress.Clear();
            txfAffiliation.Clear();
            btnSave.Content = "Save";

            msgFirstname.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgHomeAddress.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            msgAffiliation.Visibility = Visibility.Collapsed;
        }

        private void btnAddBorrower_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddBorrower.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddBorrower.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddBorrower.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddBorrower.Content = (state) ? "Close Panel" : "Add Borrower";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in BorrowersList
                         where record.LastName.ToUpper().Contains(input) ||
                                record.FirstName.ToUpper().Contains(input)
                         select record;

            dgrBorrowerTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            Borrower SelectedBorrower = dgrBorrowerTable.SelectedValue as Borrower;

            var result = from collector in BorrowersList
                         where collector.BorrowerID == SelectedBorrower.BorrowerID
                         select collector;

            if (pnlAddBorrower.Visibility == Visibility.Collapsed)
                btnAddBorrower_Click(btnAddBorrower, null);

            foreach (Borrower data in result)
            {
                txfBorrowerID.Text = data.BorrowerID.ToString();
                txfFirstname.Text = data.FirstName;
                txfMiddlename.Text = data.MiddleName;
                txfLastname.Text = data.LastName;
                txfMiddleInitial.Text = data.MiddleInitial;
                txfHomeAddress.Text = data.HomeAddress;
                txfNameSuffix.Text = data.NameSuffix;
                txfContactNumber.Text = data.ContactNumber;
                txfEmailAddress.Text = data.Email;
                txfAffiliation.Text = data.Affiliation;
            }
        }

        private void resetForm()
        {
            pnlAddBorrower.Visibility = Visibility.Collapsed;
            sprAddBorrower.Visibility = Visibility.Collapsed;
            btnAddBorrower.Content = "Add Borrower";
            btnClear_Click(btnClear, null);

            getBorrowerTable();
        }

        private void getBorrowerTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<Borrower> borrowers = new List<Borrower>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT intBorrowerID, strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, " +
                                    "strHomeAddress, strContactNumber, strEmailAddress, strFullName, strAffiliation  " +
                                "FROM viewBorrower " +
                                "ORDER BY strLastname, strFirstname");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            while (sqlData.Read())
            {
                borrowers.Add(new Borrower()
                {
                    BorrowerID = Convert.ToInt32(sqlData[0]),
                    FirstName = sqlData[1].ToString(),
                    MiddleName = sqlData[2].ToString(),
                    LastName = sqlData[3].ToString(),
                    MiddleInitial = sqlData[4].ToString(),
                    NameSuffix = sqlData[5].ToString(),
                    HomeAddress = sqlData[6].ToString(),
                    ContactNumber = sqlData[7].ToString(),
                    Email = sqlData[8].ToString(),
                    FullName = sqlData[9].ToString(),
                    Affiliation = sqlData[10].ToString()
                });
            }
            connection.closeResult();

            dgrBorrowerTable.ItemsSource = borrowers;
            BorrowersList = borrowers;
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgFirstname.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgHomeAddress.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            msgAffiliation.Visibility = Visibility.Collapsed;

            if (txfFirstname.Text == "")
            {
                msgFirstname.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfLastname.Text == "")
            {
                msgLastname.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfHomeAddress.Text == "")
            {
                msgHomeAddress.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfContactNumber.Text == "")
            {
                msgContactNumber.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfEmailAddress.Text == "")
            {
                msgEmailAddress.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfAffiliation.Text == "")
            {
                msgAffiliation.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addBorrower()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertBorrower");
            connection.addSprocParameter("@firstname", SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@middlename", SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@lastname", SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middleinitial", SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@address", SqlDbType.VarChar, txfHomeAddress.Text);
            connection.addSprocParameter("@contactno", SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", SqlDbType.VarChar, txfEmailAddress.Text);
            connection.addSprocParameter("@affiliation", SqlDbType.VarChar, txfAffiliation.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Borrower Added on the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getBorrowerTable();
        }

        private void editBorrower()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateBorrower");
            connection.addSprocParameter("@borrowerID", SqlDbType.Int, txfBorrowerID.Text);
            connection.addSprocParameter("@firstname", SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@lastname", SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middlename", SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@middleinitial", SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@address", SqlDbType.VarChar, txfHomeAddress.Text);
            connection.addSprocParameter("@contactno", SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", SqlDbType.VarChar, txfEmailAddress.Text);
            connection.addSprocParameter("@affiliation", SqlDbType.VarChar, txfAffiliation.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Borrower Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getBorrowerTable();
        }
    }
}

public class Borrower
{
    public int BorrowerID { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string MiddleInitial { get; set; }
    public string NameSuffix { get; set; }
    public string HomeAddress { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Affiliation { get; set; }
}