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
    /// Interaction logic for pageCollector.xaml
    /// </summary>
    public partial class pageCollector : Page
    {
        List<Collector> CollectorsList = new List<Collector>();
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

        public pageCollector()
        {
            InitializeComponent();

            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm())
            {
                if (txfCollectorID.Text is null || txfCollectorID.Text == "")
                {
                    //addCollector();
                }
                else
                {
                    //editCollector();
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txfCollectorID.Clear();
            txfFirstname.Clear();
            txfMiddlename.Clear();
            txfLastname.Clear();
            txfMiddleInitial.Clear();
            txfNameSuffix.Clear();
            txfContactNumber.Clear();
            txfEmailAddress.Clear();
            //cbxDepartment.SelectedIndex = -1;
            //txfSection.Clear();

            msgFirstname.Visibility = Visibility.Collapsed;
            msgMiddlename.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgMiddleInitial.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            //msgDepartment.Visibility = Visibility.Collapsed;
            //msgSection.Visibility = Visibility.Collapsed;
        }

        private void btnAddCollector_Click(object sender, RoutedEventArgs e)
        {
            bool state = (pnlAddCollector.Visibility == Visibility.Collapsed) ? true : false;
            pnlAddCollector.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            sprAddCollector.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;
            btnAddCollector.Content = (state) ? "Close Panel" : "Add Collector";

            if (!state)
                btnClear_Click(btnClear, null);
        }

        private void txfSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = txfSearch.Text.ToUpper();

            var result = from record in CollectorsList
                         where record.LastName.ToUpper().Contains(input) ||
                                record.FirstName.ToUpper().Contains(input) ||
                                record.CollegeSection.ToUpper().Contains(input)
                         select record;

            dgrCollectorTable.ItemsSource = result;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Collector SelectedCollector = dgrCollectorTable.SelectedValue as Collector;

            var result = from collector in CollectorsList
                         where collector.CollectorID == SelectedCollector.CollectorID
                         select collector;

            if (pnlAddCollector.Visibility == Visibility.Collapsed)
                btnAddCollector_Click(btnAddCollector, null);

            foreach(Collector data in result)
            {
                txfCollectorID.Text = data.CollectorID.ToString();
                txfFirstname.Text = data.FirstName;
                txfMiddlename.Text = data.MiddleName;
                txfLastname.Text = data.LastName;
                txfMiddleInitial.Text = data.MiddleInitial;
                txfNameSuffix.Text = data.NameSuffix;
                txfContactNumber.Text = data.ContactNumber;
                txfEmailAddress.Text = data.Email;
                //cbxDepartment.SelectedItem = data.College;
                //txfSection.Text = data.CollegeSection;
            }
        }

        public void resetForm()
        {
            pnlAddCollector.Visibility = Visibility.Collapsed;
            sprAddCollector.Visibility = Visibility.Collapsed;
            btnAddCollector.Content = "Add Collector";
            btnClear_Click(btnClear, null);

            getCollectorTable();
            //foreach (string college in colleges)
            //    cbxDepartment.Items.Add(college);
        }

        public void getCollectorTable()
        {
            // Database - Program Declaration
            DatabaseConnection connection = new DatabaseConnection();
            List<Collector> collectors = new List<Collector>();

            btnClear_Click(btnClear, null);

            // Query Command Setting
            connection.setQuery("SELECT intCollectorID, strFirstname, strMiddlename, strLastname, strMiddleInitial, strNameSuffix, " +
                                        "strContactNumber, strEmailAddress, strFullName, strCollege, strSection " +
                                "FROM viewCollector");

            // Query Execution
            SqlDataReader sqlData = connection.executeResult();

            while(sqlData.Read())
            {
                collectors.Add(new Collector()
                {
                    CollectorID = Convert.ToInt32(sqlData[0]),
                    FirstName = sqlData[1].ToString(),
                    MiddleName = sqlData[2].ToString(),
                    LastName = sqlData[3].ToString(),
                    MiddleInitial = sqlData[4].ToString(),
                    NameSuffix = sqlData[5].ToString(),
                    ContactNumber = sqlData[6].ToString(),
                    Email = sqlData[7].ToString(),
                    FullName = sqlData[8].ToString(),
                    College = sqlData[9].ToString(),
                    CollegeSection = sqlData[10].ToString()
                });
            }
            connection.closeResult();

            dgrCollectorTable.ItemsSource = collectors;
            CollectorsList = collectors;
        }

        private bool validateForm()
        {
            bool formOK = true;
            msgFirstname.Visibility = Visibility.Collapsed;
            msgMiddlename.Visibility = Visibility.Collapsed;
            msgLastname.Visibility = Visibility.Collapsed;
            msgMiddleInitial.Visibility = Visibility.Collapsed;
            msgContactNumber.Visibility = Visibility.Collapsed;
            msgEmailAddress.Visibility = Visibility.Collapsed;
            //msgDepartment.Visibility = Visibility.Collapsed;
            //msgSection.Visibility = Visibility.Collapsed;

            if (txfFirstname.Text == "")
            {
                msgFirstname.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfMiddlename.Text == "")
            {
                msgMiddlename.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfLastname.Text == "")
            {
                msgLastname.Visibility = Visibility.Visible;
                formOK = false;
            }
            if (txfMiddleInitial.Text == "")
            {
                msgMiddleInitial.Visibility = Visibility.Visible;
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
            //if (cbxDepartment.SelectedIndex == -1)
            //{
            //    msgDepartment.Visibility = Visibility.Visible;
            //    formOK = false;
            //}
            //if (txfSection.Text == "")
            //{
            //    msgSection.Visibility = Visibility.Visible;
            //    formOK = false;
            //}

            return formOK;
        }

        public void addCollector()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procInsertCollector");
            connection.addSprocParameter("@firstname", SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@middlename", SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@lastname", SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middleinitial", SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@contactno", SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", SqlDbType.VarChar, txfEmailAddress.Text);
            //connection.addSprocParameter("@college", SqlDbType.VarChar, cbxDepartment.SelectedItem.ToString());
            //connection.addSprocParameter("@section", SqlDbType.VarChar, txfSection.Text);
            status = connection.executeProcedure();

            switch (status)
            {
                case 0:
                    MessageBox.Show("Collector Added on the Database");
                    break;
                case 1:
                    MessageBox.Show("The System had run to an Error");
                    break;
                case 2:
                    MessageBox.Show("Information is Already Exists in the Database");
                    break;
            }
            getCollectorTable();
        }

        public void editCollector()
        {
            int status;
            DatabaseConnection connection = new DatabaseConnection();

            connection.setStoredProc("dbo.procUpdateCollector");
            connection.addSprocParameter("@collectorID", SqlDbType.Int, txfCollectorID.Text);
            connection.addSprocParameter("@firstname", SqlDbType.VarChar, txfFirstname.Text);
            connection.addSprocParameter("@lastname", SqlDbType.VarChar, txfLastname.Text);
            connection.addSprocParameter("@middlename", SqlDbType.VarChar, txfMiddlename.Text);
            connection.addSprocParameter("@middleinitial", SqlDbType.VarChar, txfMiddleInitial.Text);
            connection.addSprocParameter("@namesuffix", SqlDbType.VarChar, txfNameSuffix.Text);
            connection.addSprocParameter("@contactno", SqlDbType.VarChar, txfContactNumber.Text);
            connection.addSprocParameter("@email", SqlDbType.VarChar, txfEmailAddress.Text);
            //connection.addSprocParameter("@college", SqlDbType.VarChar, cbxDepartment.SelectedItem.ToString());
            //connection.addSprocParameter("@section", SqlDbType.VarChar, txfSection.Text);
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
            getCollectorTable();
        }
    }
}

public class Collector
{
    public int CollectorID { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string MiddleInitial { get; set; }
    public string NameSuffix { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string CollegeSection { get; set; }
    public string College { get; set; }
    public string Section { get; set; }
}