// Project:         Herbarium Management Information System
// Language:        C#/XAML
// Proponents:      Nino Danielle Escueta, Jerome Casingal, Althea Nicole Cruz [PUP-CCIS-IT]
// File:            frmMain.xaml.cs
// Description:     Main Menu Screen

using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace prototypeHerbarium
{
    /// <summary>
    /// Interaction logic for frmMain.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        // Contructor (Form Initialization) 

        public frmMain()
        {
            InitializeComponent();
            
            lblStaff.Text = StaticData.staffname.ToUpper().Trim() + " ";
            lblAccess.Text = StaticData.role;

            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.txfTime.Text = DateTime.Now.ToString("hh:mm tt");
            }, this.Dispatcher);

            if (StaticData.role == "STUDENT ASSISTANT")
            {
                btnMaintenance.Visibility = Visibility.Collapsed;
                //btnPlantResubmit.Visibility = Visibility.Collapsed;
                //btnPlantIdentification.Visibility = Visibility.Collapsed;
                btnPlantVerification.Visibility = Visibility.Collapsed;
                btnPlantClassification.Visibility = Visibility.Collapsed;
                btnPlantLoaning.Visibility = Visibility.Collapsed;
                //btnPlantTracking.Visibility = Visibility.Collapsed;
                //btnAuditTrailing.Visibility = Visibility.Collapsed;
            }

            btnHome_Click(btnHome, null);

            if (StaticData.staffname == "Temporary Admin")
            {
                MessageBox.Show("You Are Logged in to this Temporary Account, " +
                                "\nThis Account will be Deactivated once you create Access Account Records" +
                                "\nYou cannot Process and Manage Transactions with this Account!",
                                "Temporary Account Warning", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Warning);
                btnTransaction.Visibility = Visibility.Collapsed;
                btnUtilities.Visibility = Visibility.Collapsed;
                btnQuery.Visibility = Visibility.Collapsed;
                btnReports.Visibility = Visibility.Collapsed;
            }
        }

        // Event:           Click
        // Source:          btnHome
        // Description:     Load pageHomepage to pnlPageLoader

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            pageHomepage page = new pageHomepage();
            pnlPageLoader.Content = page;
            lblPageName.Text = "DASHBOARD";
        }

        // Event:           Click
        // Source:          btnMaintenance
        // Description:     Set Visibility of pnlMaintenance to Visible or Collapsed

        private void btnMaintenance_Click(object sender, RoutedEventArgs e)
        {
            bool state = pnlMaintenance.Visibility == Visibility.Collapsed;
            pnlMaintenance.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;

            double angle = (state) ? 90 : 0;
            iconMaintenanceChev.RenderTransform = new RotateTransform(angle);
        }

        // Event:           Click
        // Source:          btnTransaction
        // Description:     Set Visibility of pnlTransaction to Visible or Collapsed

        private void btnTransaction_Click(object sender, RoutedEventArgs e)
        {
            bool state = pnlTransaction.Visibility == Visibility.Collapsed;
            pnlTransaction.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;

            double angle = (state) ? 90 : 0;
            iconTransactionChev.RenderTransform = new RotateTransform(angle);
        }

        // Event:           Click
        // Source:          btnTaxonomicHierarchy
        // Description:     Load pageTaxonomicHierarchy to pnlPageLoader

        private void btnTaxonomicHierarchy_Click(object sender, RoutedEventArgs e)
        {
            bool state = pnlTaxonomicHierarchy.Visibility == Visibility.Collapsed;
            pnlTaxonomicHierarchy.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;

            double angle = (state) ? 90 : 0;
            iconTaxonChev.RenderTransform = new RotateTransform(angle);
        }

        private void btnTaxonPhylum_Click(object sender, RoutedEventArgs e)
        {
            pageTaxonPhylum page = new pageTaxonPhylum();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        private void btnTaxonClass_Click(object sender, RoutedEventArgs e)
        {
            pageTaxonClass page = new pageTaxonClass();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        private void btnTaxonOrder_Click(object sender, RoutedEventArgs e)
        {
            pageTaxonOrder page = new pageTaxonOrder();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        private void btnTaxonFamily_Click(object sender, RoutedEventArgs e)
        {
            pageTaxonFamily page = new pageTaxonFamily();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        private void btnTaxonGenus_Click(object sender, RoutedEventArgs e)
        {
            pageTaxonGenus page = new pageTaxonGenus();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        private void btnTaxonSpecies_Click(object sender, RoutedEventArgs e)
        {
            pageTaxonSpecies page = new pageTaxonSpecies();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        private void btnFamilyBox_Click(object sender, RoutedEventArgs e)
        {
            pageFamilyBox page = new pageFamilyBox();
            pnlPageLoader.Content = page;
            lblPageName.Text = "FAMILY BOXES";
        }

        // Event:           Click
        // Source:          btnCollector
        // Description:     Load pageCollector to pnlPageLoader

        private void btnCollector_Click(object sender, RoutedEventArgs e)
        {
            pageCollector page = new pageCollector();
            pnlPageLoader.Content = page;
            lblPageName.Text = "COLLECTOR";
        }

        // Event:           Click
        // Source:          btnLocality
        // Description:     Load pageLocality to pnlPageLoader

        private void btnLocality_Click(object sender, RoutedEventArgs e)
        {
            pageLocality page = new pageLocality();
            pnlPageLoader.Content = page;
            lblPageName.Text = "LOCALITY";
        }

        // Event:           Click
        // Source:          btnValidator
        // Description:     Load pageValidator to pnlPageLoader

        private void btnValidator_Click(object sender, RoutedEventArgs e)
        {
            pageValidator page = new pageValidator();
            pnlPageLoader.Content = page;
            lblPageName.Text = "EXTERNAL VALIDATOR";
        }

        private void btnCurator_Click(object sender, RoutedEventArgs e)
        {
            pageStaff page = new pageStaff();
            pnlPageLoader.Content = page;
            lblPageName.Text = "STAFF MANAGEMENT";
        }

        private void btnAccounts_Click(object sender, RoutedEventArgs e)
        {
            pageAccounts page = new pageAccounts();
            pnlPageLoader.Content = page;
            lblPageName.Text = "ACCESS ACCOUNTS";
        }

        // Event:           Click
        // Source:          btnPlantDeposit

        private void btnPlantDeposit_Click(object sender, RoutedEventArgs e)
        {
            pageDeposit page = new pageDeposit();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        private void btnPlantResubmit_Click(object sender, RoutedEventArgs e)
        {
            pageResubmit page = new pageResubmit();
            pnlPageLoader.Content = page;
            lblPageName.Text = page.Title.ToUpper();
        }

        // Event:           Click
        // Source:          btnPlantIdentification

        private void btnPlantIdentification_Click(object sender, RoutedEventArgs e)
        {
            pageIdentification page = new pageIdentification();
            pnlPageLoader.Content = page;
            lblPageName.Text = "PLANT RECEIVING";
        }

        // Event: Click
        // Source: btnPlantVerification

        private void btnPlantVerification_Click(object sender, RoutedEventArgs e)
        {
            pageVerification page = new pageVerification();
            pnlPageLoader.Content = page;
            lblPageName.Text = "PLANT VERIFICATION";
        }

        private void btnPlantClassification_Click(object sender, RoutedEventArgs e)
        {
            pageClassification page = new pageClassification();
            pnlPageLoader.Content = page;
            lblPageName.Text = "PLANT CLASSIFICATION";
        }

        private void btnPlantLoaning_Click(object sender, RoutedEventArgs e)
        {
            pageLoaning page = new pageLoaning();
            pnlPageLoader.Content = page;
            lblPageName.Text = "LOAN PLANT";
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            pageQueries page = new pageQueries();
            pnlPageLoader.Content = page;
            lblPageName.Text = "QUERY";
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            pageReports page = new pageReports();
            pnlPageLoader.Content = page;
            lblPageName.Text = "REPORTS";
        }

        // Event:           Click
        // Source:          btnUtilities
        // Description:     Set Visibility of pnlUtilities to Visible or Collapsed

        private void btnUtilities_Click(object sender, RoutedEventArgs e)
        {
            bool state = pnlUtilities.Visibility == Visibility.Collapsed;
            pnlUtilities.Visibility = (state) ? Visibility.Visible : Visibility.Collapsed;

            double angle = (state) ? 90 : 0;
            iconUtilitiesChev.RenderTransform = new RotateTransform(angle);
        }

        private void btnHerbariumInventory_Click(object sender, RoutedEventArgs e)
        {
            pageHerbariumInventory page = new pageHerbariumInventory();
            pnlPageLoader.Content = page;
            lblPageName.Text = "HERBARIUM INVENTORY";
        }

        private void btnPlantTracking_Click(object sender, RoutedEventArgs e)
        {
            pageHerbariumTracking page = new pageHerbariumTracking();
            pnlPageLoader.Content = page;
            lblPageName.Text = "PLANT TRACKING";
        }

        private void btnAuditTrailing_Click(object sender, RoutedEventArgs e)
        {
            pageAuditTrailing page = new pageAuditTrailing();
            pnlPageLoader.Content = page;
            lblPageName.Text = "AUDIT TRAILING";
        }

        // Event:           Click
        // Source:          btnValidator
        // Description:     Return back to the previous page

        private void btnNavigationBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pnlPageLoader.GoBack();
            }
            catch (InvalidOperationException) { }
        }

        // Event:           Click
        // Source:          btnNavigationNext
        // Description:     Go forward to the next page

        private void btnNavigationNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pnlPageLoader.GoForward();
            }
            catch(InvalidOperationException) { }
        }

        // Event: Click
        // Source: btnLogout

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }


        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Herbarium Management Information System" +
                            "\nCopyright 2018" +
                            "\n\nDeveloped by: IT 4-1 ISD ",
                            "System Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        // Function:        Click
        // Source:          btnValidator
        // Description:     Load pageValidator to pnlPageLoader

        private void pnlPageLoader_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            MessageBox.Show(pnlPageLoader.Content.ToString());
        }
    }
}

// End of File [frmMain.xaml.cs]