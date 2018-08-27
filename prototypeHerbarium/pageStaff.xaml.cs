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
    /// Interaction logic for pageStaff.xaml
    /// </summary>
    public partial class pageStaff : Page
    {
        List<Staff> HerbariumStaff = new List<Staff>();
        string[] roles = new string[] { "ADMINISTRATOR", "CURATOR", "STUDENT ASSISTANT" };
        string[] colleges = new string[]
        {
            "College of Accountancy and Finance",
            "College of Architecture and Fine Arts",
            "College of Arts and Letters",
            "College of Business Administration",
            "College of Communication",
            "College of Computer and Information Sciences",
            "College of Education",
            "College of Engineering",
            "College of Human Kinetics",
            "College of Law",
            "College of Public Administration",
            "College of Science",
            "College of Social Sciences and Development",
            "College of Tourism, Hospitality and Transportation Management",
            "Institute of Technology",
            "Laboratory High School",
            "Senior High School",
            "Graduate School"
        };

        public pageStaff()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfStaffID.Text is null || txfStaffID.Text == "")
                    addStaff();
                else
                    editStaff();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfStaffID.Clear();
            cbxRole.SelectedIndex = -1;
            txfFirstname.Clear();
            txfMiddlename.Clear();
            txfLastname.Clear();
            txfMiddleInitial.Clear();
            txfNameSuffix.Clear();
            txfContactNumber.Clear();
            txfEmailAddress.Clear();
            cbxDepartment.SelectedIndex = -1;
            lblNote.Visibility = Visibility.Collapsed;
            btnSave.Content = "Save";

            msgRole.Visibility = Visibility.Collapsed;
            msgFirstname.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            msgDepartment.Visibility = Visibility.Collapsed;
        }

        private void btnAddStaff_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddStaff.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddStaff.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddStaff.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddStaff.Content = (state) ? "Close Panel" : "Add Staff";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in HerbariumStaff
                         where record.LastName.ToUpper().Contains(input) ||
                                record.FirstName.ToUpper().Contains(input) ||
                                record.Role.ToUpper().Contains(input)
                         select record;

            dgrStaffTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Content = "Update";
            Staff SelectedStaff = dgrStaffTable.SelectedValue as Staff;

            var result = from staff in HerbariumStaff
                         where staff.StaffID == SelectedStaff.StaffID
                         select staff;

            if (pnlAddStaff.Visibility == Visibility.Collapsed)
                btnAddStaff_Click(btnAddStaff, null);

            foreach (Staff data in result)
            {
                txfStaffID.Text = data.StaffID.ToString();
                txfFirstname.Text = data.FirstName;
                txfMiddlename.Text = data.MiddleName;
                txfLastname.Text = data.LastName;
                txfMiddleInitial.Text = data.MiddleInitial;
                txfNameSuffix.Text = data.NameSuffix;
                txfContactNumber.Text = data.ContactNumber;
                txfEmailAddress.Text = data.Email;
                cbxRole.SelectedItem = data.Role;
                cbxDepartment.SelectedItem = data.College;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Do you want to Deactive this Herbarium Staff Record Permanently?",
                                     "Remove Record",
                                     MessageBoxButton.YesNo,
                                     MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Database - Program Declaration
                DatabaseConnection connection = new DatabaseConnection();
                Staff staff = dgrStaffTable.SelectedValue as Staff;

                // Query Command Setting
                connection.setQuery("UPDATE tblHerbariumStaff SET boolActive = 0 WHERE intStaffID = @staffID");
                connection.addParameter("@staffID", System.Data.SqlDbType.Int, staff.StaffID);

                // Query Execution
                connection.executeResult();

                MessageBox.Show("Herbarium Staff Deactivated");
                getStaffTable();
            }
        }

        private void cbxRole_SelectionChanged(object sender, SelectionChangedEventArgs e) 
            => lblNote.Visibility = (cbxRole.SelectedIndex == 2) ? Visibility.Collapsed : Visibility.Visible;

        private void initializingFields()
        {
            foreach (string role in roles)
                cbxRole.Items.Add(role);
            
            foreach (string college in colleges)
                cbxDepartment.Items.Add(college);
        }

        public void resetForm()
        {
            pnlAddStaff.Visibility = Visibility.Collapsed;
            sprAddStaff.Visibility = Visibility.Collapsed;
            btnAddStaff.Content = "Add Staff";
            btnClear_Click(btnClear, null);

            getStaffTable();
            initializingFields();
        }

        private void getStaffTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<Staff> staffs = new List<Staff>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT intStaffID, strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, " +
                                        "strContactNumber, strEmailAddress, strFullName, strRole, strCollegeDepartment " +
                                "FROM viewHerbariumStaff " +
                                "ORDER BY strLastname, strFirstname");
            
            // Query Execution
            SqlDataReader sqlData = connection.executeResult();
            
            // Query Result
            while (sqlData.Read())
            {
                staffs.Add(new Staff()
                {
                    StaffID = Convert.ToInt32(sqlData[0]),
                    FirstName = sqlData[1].ToString(),
                    MiddleName = sqlData[2].ToString(),
                    LastName = sqlData[3].ToString(),
                    MiddleInitial = sqlData[4].ToString(),
                    NameSuffix = sqlData[5].ToString(),
                    ContactNumber = sqlData[6].ToString(),
                    Email = sqlData[7].ToString(),
                    FullName = sqlData[8].ToString(),
                    Role = sqlData[9].ToString(),
                    College = sqlData[10].ToString()
                });
            }
            connection.closeResult();

            dgrStaffTable.ItemsSource = staffs;
            HerbariumStaff = staffs;
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgRole.Visibility = Visibility.Collapsed;
            msgFirstname.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            msgDepartment.Visibility = Visibility.Collapsed;

            if (cbxRole.SelectedIndex == -1)
            {
                msgRole.Visibility = Visibility.Visible;
                formOK = false;
            }
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
            if (cbxDepartment.SelectedIndex == -1)
            {
                msgDepartment.Visibility = Visibility.Visible;
                formOK = false;
            }

            return formOK;
        }

        private void addStaff()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertHerbariumStaff");
            connection.addSprocParameter("@firstname", System.Data.SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@middlename", System.Data.SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@lastname", System.Data.SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middleinitial", System.Data.SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", System.Data.SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@contactno", System.Data.SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", System.Data.SqlDbType.VarChar, txfEmailAddress.Text);
            connection.addSprocParameter("@role", System.Data.SqlDbType.VarChar, cbxRole.SelectedItem.ToString());
            connection.addSprocParameter("@department", System.Data.SqlDbType.VarChar, cbxDepartment.SelectedItem.ToString());
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Herbarium Staff Added on the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getStaffTable();
        }

        private void editStaff()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateHerbariumStaff");
            connection.addSprocParameter("@staffID", System.Data.SqlDbType.Int, txfStaffID.Text);
            connection.addSprocParameter("@firstname", System.Data.SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@middlename", System.Data.SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@lastname", System.Data.SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middleinitial", System.Data.SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", System.Data.SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@contactno", System.Data.SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", System.Data.SqlDbType.VarChar, txfEmailAddress.Text);
            connection.addSprocParameter("@role", System.Data.SqlDbType.VarChar, cbxRole.SelectedItem.ToString());
            connection.addSprocParameter("@department", System.Data.SqlDbType.VarChar, cbxDepartment.SelectedItem.ToString());
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Herbarium Staff Updated in the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
            }
            getStaffTable();
        }
    }
}

public class Staff
{
    public int StaffID { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string MiddleInitial { get; set; }
    public string NameSuffix { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string College { get; set; }
}