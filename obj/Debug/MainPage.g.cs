﻿#pragma checksum "..\..\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "29FBC1569DC062F68534FA6CC23549EE6D83D1006F8C764DF2BC374354DAA358"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Laboratory_work_No._5;
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


namespace Laboratory_work_No._5 {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TitleTextBlock;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ProfilesListBox;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateProfileButton;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton DeleteProfileToggleButton;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExitButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Laboratory work No. 5;component/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainPage.xaml"
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
            this.TitleTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.ProfilesListBox = ((System.Windows.Controls.ListBox)(target));
            
            #line 38 "..\..\MainPage.xaml"
            this.ProfilesListBox.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ProfilesListBox_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CreateProfileButton = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\MainPage.xaml"
            this.CreateProfileButton.Click += new System.Windows.RoutedEventHandler(this.CreateProfileButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DeleteProfileToggleButton = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 60 "..\..\MainPage.xaml"
            this.DeleteProfileToggleButton.Checked += new System.Windows.RoutedEventHandler(this.DeleteProfileToggleButton_Checked);
            
            #line default
            #line hidden
            
            #line 61 "..\..\MainPage.xaml"
            this.DeleteProfileToggleButton.Unchecked += new System.Windows.RoutedEventHandler(this.DeleteProfileToggleButton_Unchecked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ExitButton = ((System.Windows.Controls.Button)(target));
            
            #line 70 "..\..\MainPage.xaml"
            this.ExitButton.Click += new System.Windows.RoutedEventHandler(this.ExitButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
