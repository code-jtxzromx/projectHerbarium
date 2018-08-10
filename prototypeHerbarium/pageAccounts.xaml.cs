using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    /// Interaction logic for pageAccounts.xaml
    /// </summary>
    public partial class pageAccounts : Page
    {
        List<Account> accessAccounts = new List<Account>();

        public pageAccounts()
        {
            InitializeComponent();

            getStaffList();
            getAccountTable();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfAccountID.Text is null || txfAccountID.Text == "")
                    addAccount();
                else
                    editAccount();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfAccountID.Clear();
            cbxStaff.SelectedIndex = -1;
            txfUsername.Clear();
            txfPassword.Clear();

            msgStaff.Visibility = Visibility.Collapsed;
            msgUsername.Visibility = Visibility.Collapsed;
            msgPassword.Visibility = Visibility.Collapsed;
        }

        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddAccount.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddAccount.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddAccount.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddAccount.Content = (state) ? "Close Panel" : "Add Account";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in accessAccounts
                         where record.Staff.ToUpper().Contains(input) ||
                                record.Username.ToUpper().Contains(input)
                         select record;

            dgrAccountTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Account SelectedAccount = dgrAccountTable.SelectedValue as Account;

            var result = from account in accessAccounts
                         where account.AccountID == SelectedAccount.AccountID
                         select account;

            if (pnlAddAccount.Visibility == Visibility.Collapsed)
                btnAddAccount_Click(btnAddAccount, null);

            foreach (Account data in result)
            {
                txfAccountID.Text = data.AccountID.ToString();
                cbxStaff.SelectedItem = data.Staff;
                txfUsername.Text = data.Username;
                txfPassword.Password = data.Password;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Do you want to Deactive this Access Account Record Permanently?",
                                     "Remove Record",
                                     MessageBoxButton.YesNo,
                                     MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Database - Program Declaration
                DatabaseConnection connection = new DatabaseConnection();
                Account account = dgrAccountTable.SelectedValue as Account;

                // Query Command Setting
                connection.setQuery("UPDATE tblAccounts SET boolActive = 0 WHERE intAccountID = @accountID");
                connection.addParameter("@accountID", System.Data.SqlDbType.Int, account.AccountID);

                // Query Execution
                connection.executeResult();

                MessageBox.Show("Access Account Deactivated");
                getAccountTable();
            }
        }

        public void getAccountTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<Account> accounts = new List<Account>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT intAccountID, strFullName, strUsername, strPassword, strRole FROM viewAccounts");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                accounts.Add(new Account()
                {
                    AccountID = Convert.ToInt32(sqlData[0]),
                    Staff = sqlData[1].ToString(),
                    Username = sqlData[2].ToString(),
                    Password = sqlData[3].ToString(),
                    Role = sqlData[4].ToString()
                });
            }
            connection.closeResult();
            
            dgrAccountTable.ItemsSource = accounts;
            accessAccounts = accounts;
        }

        public void getStaffList()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();

            cbxStaff.Items.Clear();

            // Query Command Setting
            connection.setQuery("SELECT strFullName FROM viewHerbariumStaff");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            // Query Result
            while (sqlData.Read())
            {
                cbxStaff.Items.Add(sqlData[0]);
            }
            connection.closeResult();
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgStaff.Visibility = Visibility.Collapsed;
            msgUsername.Visibility = Visibility.Collapsed;
            msgPassword.Visibility = Visibility.Collapsed;

            if (cbxStaff.SelectedIndex == -1)
            {
                msgStaff.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfUsername.Text == "")
            {
                msgUsername.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfPassword.Password == "")
            {
                msgPassword.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        public void addAccount()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertAccount");
            connection.addSprocParameter("@staffName", System.Data.SqlDbType.VarChar, cbxStaff.SelectedItem.ToString());
            connection.addSprocParameter("@username", System.Data.SqlDbType.VarChar, txfUsername.Text);
            connection.addSprocParameter("@password", System.Data.SqlDbType.VarChar, txfPassword.Password);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Access Account Added on the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getAccountTable();
        }

        public void editAccount()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateAccount");
            connection.addSprocParameter("@accountID", System.Data.SqlDbType.Int, txfAccountID.Text);
            connection.addSprocParameter("@staffName", System.Data.SqlDbType.VarChar, cbxStaff.SelectedItem.ToString());
            connection.addSprocParameter("@username", System.Data.SqlDbType.VarChar, txfUsername.Text);
            connection.addSprocParameter("@password", System.Data.SqlDbType.VarChar, txfPassword.Password);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Access Account Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getAccountTable();
        }
    }
}

public class Account
{
    public int AccountID { get; set; }
    public string Staff { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}
