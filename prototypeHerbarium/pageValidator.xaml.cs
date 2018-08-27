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
    /// Interaction logic for pageValidator.xaml
    /// </summary>
    public partial class pageValidator : Page
    {
        List<Validator> ExternalValidators = new List<Validator>();

        public pageValidator()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfValidatorID.Text is null || txfValidatorID.Text == "")
                    addValidator();
                else
                    editValidator();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfValidatorID.Clear();
            txfFirstname.Clear();
            txfMiddlename.Clear();
            txfLastname.Clear();
            txfMiddleInitial.Clear();
            txfNameSuffix.Clear();
            txfContactNumber.Clear();
            txfEmailAddress.Clear();
            txfInstitution.Clear();
            btnSave.Content = "Save";

            msgFirstname.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            msgInstitution.Visibility = Visibility.Collapsed;
        }

        private void btnAddValidator_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddValidator.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddValidator.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddValidator.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddValidator.Content = (state) ? "Close Panel" : "Add External Validator";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in ExternalValidators
                         where record.LastName.ToUpper().Contains(input) ||
                                record.FirstName.ToUpper().Contains(input) ||
                                record.Institution.ToUpper().Contains(input)
                         select record;

            dgrValidatorTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            Validator SelectedValidator = dgrValidatorTable.SelectedValue as Validator;

            var result = from validator in ExternalValidators
                         where validator.ValidatorID == SelectedValidator.ValidatorID
                         select validator;

            if (pnlAddValidator.Visibility == Visibility.Collapsed)
                btnAddValidator_Click(btnAddValidator, null);

            foreach (Validator data in result)
            {
                txfValidatorID.Text = data.ValidatorID.ToString();
                txfFirstname.Text = data.FirstName;
                txfMiddlename.Text = data.MiddleName;
                txfLastname.Text = data.LastName;
                txfMiddleInitial.Text = data.MiddleInitial;
                txfNameSuffix.Text = data.NameSuffix;
                txfContactNumber.Text = data.ContactNumber;
                txfEmailAddress.Text = data.Email;
                txfInstitution.Text = data.Institution;
            }
        }
        
        public void resetForm()
        {
            pnlAddValidator.Visibility = Visibility.Collapsed;
            sprAddValidator.Visibility = Visibility.Collapsed;
            btnAddValidator.Content = "Add Validator";
            btnClear_Click(btnClear, null);

            getValidatorTable();
        }

        public void getValidatorTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<Validator> validators = new List<Validator>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT intValidatorID, strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, " +
                                        "strContactNumber, strEmailAddress, strFullName, strInstitution " +
                                "FROM viewValidator " +
                                "WHERE strValidatorType = 'External' " +
                                "ORDER BY strLastname, strFirstname");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();
            
            // Query Result
            while (sqlData.Read())
            {
                validators.Add(new Validator()
                {
                    ValidatorID = Convert.ToInt32(sqlData[0]),
                    FirstName = sqlData[1].ToString(),
                    MiddleName = sqlData[2].ToString(),
                    LastName = sqlData[3].ToString(),
                    MiddleInitial = sqlData[4].ToString(),
                    NameSuffix = sqlData[5].ToString(),
                    ContactNumber = sqlData[6].ToString(),
                    Email = sqlData[7].ToString(),
                    FullName = sqlData[8].ToString(),
                    Institution = sqlData[9].ToString()
                });
            }
            connection.closeResult();

            dgrValidatorTable.ItemsSource = validators;
            ExternalValidators = validators;
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgFirstname.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            msgInstitution.Visibility = Visibility.Collapsed;
            
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
            if (txfInstitution.Text == "")
            {
                msgInstitution.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        public void addValidator()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();
            
            connection.setStoredProc("dbo.procInsertValidator");
            connection.addSprocParameter("@firstname", SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@lastname", SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middlename", SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@middleinitial", SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@contactno", SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", SqlDbType.VarChar, txfEmailAddress.Text);
            connection.addSprocParameter("@institution", SqlDbType.VarChar, txfInstitution.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("External Validator Added on the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getValidatorTable();
        }

        public void editValidator()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateValidator");
            connection.addSprocParameter("@validatorID", SqlDbType.Int, txfValidatorID.Text);
            connection.addSprocParameter("@firstname", SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@lastname", SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middlename", SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@middleinitial", SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@contactno", SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", SqlDbType.VarChar, txfEmailAddress.Text);
            connection.addSprocParameter("@institution", SqlDbType.VarChar, txfInstitution.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Collector Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getValidatorTable();
        }
    }
}

public class Validator
{
    public int ValidatorID { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string MiddleInitial { get; set; }
    public string NameSuffix { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Institution { get; set; }
}