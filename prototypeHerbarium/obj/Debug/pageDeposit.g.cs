﻿#pragma checksum "..\..\pageDeposit.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9909EBC58D0E142CC9BE494EF531B126DC2C02A0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using prototypeHerbarium;


namespace prototypeHerbarium {
    
    
    /// <summary>
    /// pageDeposit
    /// </summary>
    public partial class pageDeposit : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUploadPicture;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCapturePicture;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnProceed;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClear;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblErrorPicture;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image picHerbariumPlant;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtNew;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtExisting;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.ToggleSwitch btnDuplicateRecord;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlTextField txfAccessionNumber;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlTextField txfReferenceNumber;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition defValidator;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxCollector;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxValidator;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxLocality;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxTaxonName;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition defDateCollected;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlDateField dpkDateCollected;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlDateField dpkDateDeposited;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlDateField dpkDateVerified;
        
        #line default
        #line hidden
        
        
        #line 124 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtVascular;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtFlowering;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtAlgae;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txaDescription;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid pnlCapturePicture;
        
        #line default
        #line hidden
        
        
        #line 172 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReturn;
        
        #line default
        #line hidden
        
        
        #line 181 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image picHerbariumSheet;
        
        #line default
        #line hidden
        
        
        #line 182 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image picCamera;
        
        #line default
        #line hidden
        
        
        #line 186 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCapturePic;
        
        #line default
        #line hidden
        
        
        #line 188 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDiscardPic;
        
        #line default
        #line hidden
        
        
        #line 190 "..\..\pageDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSavePic;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/prototypeHerbarium;component/pagedeposit.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\pageDeposit.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnUploadPicture = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\pageDeposit.xaml"
            this.btnUploadPicture.Click += new System.Windows.RoutedEventHandler(this.btnUploadPicture_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnCapturePicture = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\pageDeposit.xaml"
            this.btnCapturePicture.Click += new System.Windows.RoutedEventHandler(this.btnCapturePicture_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnProceed = ((System.Windows.Controls.Button)(target));
            
            #line 34 "..\..\pageDeposit.xaml"
            this.btnProceed.Click += new System.Windows.RoutedEventHandler(this.btnProceed_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnClear = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\pageDeposit.xaml"
            this.btnClear.Click += new System.Windows.RoutedEventHandler(this.btnClear_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lblErrorPicture = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.picHerbariumPlant = ((System.Windows.Controls.Image)(target));
            return;
            case 7:
            this.rbtNew = ((System.Windows.Controls.RadioButton)(target));
            
            #line 48 "..\..\pageDeposit.xaml"
            this.rbtNew.Checked += new System.Windows.RoutedEventHandler(this.rbtDepositTransaction_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.rbtExisting = ((System.Windows.Controls.RadioButton)(target));
            
            #line 52 "..\..\pageDeposit.xaml"
            this.rbtExisting.Checked += new System.Windows.RoutedEventHandler(this.rbtDepositTransaction_Checked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnDuplicateRecord = ((MahApps.Metro.Controls.ToggleSwitch)(target));
            
            #line 72 "..\..\pageDeposit.xaml"
            this.btnDuplicateRecord.Checked += new System.EventHandler<System.Windows.RoutedEventArgs>(this.btnDuplicateRecord_ToggleChange);
            
            #line default
            #line hidden
            
            #line 72 "..\..\pageDeposit.xaml"
            this.btnDuplicateRecord.Unchecked += new System.EventHandler<System.Windows.RoutedEventArgs>(this.btnDuplicateRecord_ToggleChange);
            
            #line default
            #line hidden
            return;
            case 10:
            this.txfAccessionNumber = ((prototypeHerbarium.ctrlTextField)(target));
            return;
            case 11:
            this.txfReferenceNumber = ((prototypeHerbarium.ctrlTextField)(target));
            return;
            case 12:
            this.defValidator = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 13:
            this.cbxCollector = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 14:
            this.cbxValidator = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 15:
            this.cbxLocality = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 16:
            this.cbxTaxonName = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 17:
            this.defDateCollected = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 18:
            this.dpkDateCollected = ((prototypeHerbarium.ctrlDateField)(target));
            return;
            case 19:
            this.dpkDateDeposited = ((prototypeHerbarium.ctrlDateField)(target));
            return;
            case 20:
            this.dpkDateVerified = ((prototypeHerbarium.ctrlDateField)(target));
            return;
            case 21:
            this.rbtVascular = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 22:
            this.rbtFlowering = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 23:
            this.rbtAlgae = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 24:
            this.txaDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 25:
            this.pnlCapturePicture = ((System.Windows.Controls.Grid)(target));
            return;
            case 26:
            this.btnReturn = ((System.Windows.Controls.Button)(target));
            
            #line 172 "..\..\pageDeposit.xaml"
            this.btnReturn.Click += new System.Windows.RoutedEventHandler(this.btnReturn_Click);
            
            #line default
            #line hidden
            return;
            case 27:
            this.picHerbariumSheet = ((System.Windows.Controls.Image)(target));
            return;
            case 28:
            this.picCamera = ((System.Windows.Controls.Image)(target));
            return;
            case 29:
            this.btnCapturePic = ((System.Windows.Controls.Button)(target));
            
            #line 186 "..\..\pageDeposit.xaml"
            this.btnCapturePic.Click += new System.Windows.RoutedEventHandler(this.btnCapturePic_Click);
            
            #line default
            #line hidden
            return;
            case 30:
            this.btnDiscardPic = ((System.Windows.Controls.Button)(target));
            
            #line 188 "..\..\pageDeposit.xaml"
            this.btnDiscardPic.Click += new System.Windows.RoutedEventHandler(this.btnDiscardPic_Click);
            
            #line default
            #line hidden
            return;
            case 31:
            this.btnSavePic = ((System.Windows.Controls.Button)(target));
            
            #line 190 "..\..\pageDeposit.xaml"
            this.btnSavePic.Click += new System.Windows.RoutedEventHandler(this.btnSavePic_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

