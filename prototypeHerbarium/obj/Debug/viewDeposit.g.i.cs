﻿#pragma checksum "..\..\viewDeposit.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "66C90DB9C8ACCBD48B1FF8FA4FC8C6395488D9C4"
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
    /// viewDeposit
    /// </summary>
    public partial class viewDeposit : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUploadPicture;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCapturePicture;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnProceed;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClear;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblErrorPicture;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image picHerbariumPlant;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.Controls.ToggleSwitch btnVerifiedRecord;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxTaxonName;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlTextField txfAccessionNumber;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxReferenceNumber;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkSameAccession;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition defValidator;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxCollector;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxValidator;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition defDateCollected;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlDateField dpkDateCollected;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlDateField dpkDateDeposited;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlDateField dpkDateVerified;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxLocality;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal prototypeHerbarium.ctrlComboBox cbxPlantType;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txaDescription;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkGoodCondition;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid pnlCapturePicture;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReturn;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnResolutionSetting;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCameraSetting;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image picHerbariumSheet;
        
        #line default
        #line hidden
        
        
        #line 160 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image picCamera;
        
        #line default
        #line hidden
        
        
        #line 164 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCapturePic;
        
        #line default
        #line hidden
        
        
        #line 166 "..\..\viewDeposit.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDiscardPic;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\viewDeposit.xaml"
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
            System.Uri resourceLocater = new System.Uri("/prototypeHerbarium;component/viewdeposit.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\viewDeposit.xaml"
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
            
            #line 19 "..\..\viewDeposit.xaml"
            this.btnUploadPicture.Click += new System.Windows.RoutedEventHandler(this.btnUploadPicture_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnCapturePicture = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\viewDeposit.xaml"
            this.btnCapturePicture.Click += new System.Windows.RoutedEventHandler(this.btnCapturePicture_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnProceed = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\viewDeposit.xaml"
            this.btnProceed.Click += new System.Windows.RoutedEventHandler(this.btnProceed_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnClear = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\viewDeposit.xaml"
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
            this.btnVerifiedRecord = ((MahApps.Metro.Controls.ToggleSwitch)(target));
            
            #line 47 "..\..\viewDeposit.xaml"
            this.btnVerifiedRecord.Checked += new System.EventHandler<System.Windows.RoutedEventArgs>(this.btnVerifiedRecord_CheckChanged);
            
            #line default
            #line hidden
            
            #line 47 "..\..\viewDeposit.xaml"
            this.btnVerifiedRecord.Unchecked += new System.EventHandler<System.Windows.RoutedEventArgs>(this.btnVerifiedRecord_CheckChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.cbxTaxonName = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 9:
            this.txfAccessionNumber = ((prototypeHerbarium.ctrlTextField)(target));
            return;
            case 10:
            this.cbxReferenceNumber = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 11:
            this.chkSameAccession = ((System.Windows.Controls.CheckBox)(target));
            
            #line 66 "..\..\viewDeposit.xaml"
            this.chkSameAccession.Checked += new System.Windows.RoutedEventHandler(this.chkSameAccession_CheckChanged);
            
            #line default
            #line hidden
            
            #line 66 "..\..\viewDeposit.xaml"
            this.chkSameAccession.Unchecked += new System.Windows.RoutedEventHandler(this.chkSameAccession_CheckChanged);
            
            #line default
            #line hidden
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
            this.defDateCollected = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 16:
            this.dpkDateCollected = ((prototypeHerbarium.ctrlDateField)(target));
            return;
            case 17:
            this.dpkDateDeposited = ((prototypeHerbarium.ctrlDateField)(target));
            return;
            case 18:
            this.dpkDateVerified = ((prototypeHerbarium.ctrlDateField)(target));
            return;
            case 19:
            this.cbxLocality = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 20:
            this.cbxPlantType = ((prototypeHerbarium.ctrlComboBox)(target));
            return;
            case 21:
            this.txaDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 22:
            this.chkGoodCondition = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 23:
            this.pnlCapturePicture = ((System.Windows.Controls.Grid)(target));
            return;
            case 24:
            this.btnReturn = ((System.Windows.Controls.Button)(target));
            
            #line 139 "..\..\viewDeposit.xaml"
            this.btnReturn.Click += new System.Windows.RoutedEventHandler(this.btnReturn_Click);
            
            #line default
            #line hidden
            return;
            case 25:
            this.btnResolutionSetting = ((System.Windows.Controls.Button)(target));
            
            #line 144 "..\..\viewDeposit.xaml"
            this.btnResolutionSetting.Click += new System.Windows.RoutedEventHandler(this.btnResolutionSetting_Click);
            
            #line default
            #line hidden
            return;
            case 26:
            this.btnCameraSetting = ((System.Windows.Controls.Button)(target));
            
            #line 149 "..\..\viewDeposit.xaml"
            this.btnCameraSetting.Click += new System.Windows.RoutedEventHandler(this.btnCameraSetting_Click);
            
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
            
            #line 164 "..\..\viewDeposit.xaml"
            this.btnCapturePic.Click += new System.Windows.RoutedEventHandler(this.btnCapturePic_Click);
            
            #line default
            #line hidden
            return;
            case 30:
            this.btnDiscardPic = ((System.Windows.Controls.Button)(target));
            
            #line 166 "..\..\viewDeposit.xaml"
            this.btnDiscardPic.Click += new System.Windows.RoutedEventHandler(this.btnDiscardPic_Click);
            
            #line default
            #line hidden
            return;
            case 31:
            this.btnSavePic = ((System.Windows.Controls.Button)(target));
            
            #line 168 "..\..\viewDeposit.xaml"
            this.btnSavePic.Click += new System.Windows.RoutedEventHandler(this.btnSavePic_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

