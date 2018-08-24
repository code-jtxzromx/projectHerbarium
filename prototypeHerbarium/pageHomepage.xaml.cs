using System;
using System.Collections.Generic;
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
    /// Interaction logic for pageHomepage.xaml
    /// </summary>
    public partial class pageHomepage : Page
    {
        public pageHomepage()
        {
            InitializeComponent();

            lblUsername.Text = StaticData.staffname.Trim() + "!";

            lblCountA.Text = getPlantDeposits().ToString();
            lblCountB.Text = getVerifiedSpecies().ToString();
            lblCountC.Text = getFamilyBox().ToString();
            lblCountD.Text = getLoanAvailable().ToString();

        }

        private int getPlantDeposits()
        {
            int count = 0;
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT COUNT(intPlantDepositID) FROM viewPlantDeposit");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                count = Convert.ToInt32(sqlData[0]);
            }
            connection.closeResult();

            return count;
        }

        private int getVerifiedSpecies()
        {
            int count = 0;
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT COUNT(intSpeciesID) FROM viewTaxonSpecies WHERE boolSpeciesIdentified = 1");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                count = Convert.ToInt32(sqlData[0]);
            }
            connection.closeResult();

            return count;
        }

        private int getFamilyBox()
        {
            int count = 0;
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT COUNT(intBoxID) FROM viewFamilyBox");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                count = Convert.ToInt32(sqlData[0]);
            }
            connection.closeResult();

            return count;
        }

        private int getLoanAvailable()
        {
            int count = 0;
            DatabaseConnection connection = new DatabaseConnection();
            connection.setQuery("SELECT COUNT(intStoredSheetID) FROM viewHerbariumInventory WHERE boolLoanAvailable = 1");

            SqlDataReader sqlData = connection.executeResult();
            while (sqlData.Read())
            {
                count = Convert.ToInt32(sqlData[0]);
            }
            connection.closeResult();

            return count;
        }
    }
}
