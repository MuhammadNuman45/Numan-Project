﻿#pragma checksum "..\..\NewScanWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "31C4610B99E9927083B48FF18E9D13E6C8167D2E8EABCB8863A208ABDA7BA0C2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace Login_SignUp {
    
    
    /// <summary>
    /// NewScanWindow
    /// </summary>
    public partial class NewScanWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\NewScanWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ScanDetailsPlaceholder;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\NewScanWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ScanDetailsTextBox;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\NewScanWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PatientIdTextBox;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\NewScanWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton OutpatientRadioButton;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\NewScanWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton InpatientRadioButton;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\NewScanWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SelectedFilePathTextBox;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\NewScanWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveScanButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Login_SignUp;component/newscanwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\NewScanWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            
            #line 15 "..\..\NewScanWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CloseButton_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ScanDetailsPlaceholder = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.ScanDetailsTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 37 "..\..\NewScanWindow.xaml"
            this.ScanDetailsTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.ScanDetailsTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.PatientIdTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.OutpatientRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 63 "..\..\NewScanWindow.xaml"
            this.OutpatientRadioButton.Checked += new System.Windows.RoutedEventHandler(this.RadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.InpatientRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 71 "..\..\NewScanWindow.xaml"
            this.InpatientRadioButton.Checked += new System.Windows.RoutedEventHandler(this.RadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 76 "..\..\NewScanWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SelectFileButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.SelectedFilePathTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.SaveScanButton = ((System.Windows.Controls.Button)(target));
            
            #line 99 "..\..\NewScanWindow.xaml"
            this.SaveScanButton.Click += new System.Windows.RoutedEventHandler(this.SaveScanButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
