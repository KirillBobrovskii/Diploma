﻿#pragma checksum "..\..\..\Pages\Groups.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FCB8D4A9DF45F0AF1D6D4A73EC23264E5C443A4F1C86A6A58222F372DF5FC049"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using ScholarshipAppointment.Pages;
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


namespace ScholarshipAppointment.Pages {
    
    
    /// <summary>
    /// Groups
    /// </summary>
    public partial class Groups : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\Pages\Groups.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition GridSplitter;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Pages\Groups.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition Dialog;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Pages\Groups.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ButtonsPanel;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\Pages\Groups.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox FilterBox;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\Pages\Groups.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGrid;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Pages\Groups.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame Frame;
        
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
            System.Uri resourceLocater = new System.Uri("/ScholarshipAppointment;component/pages/groups.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\Groups.xaml"
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
            
            #line 10 "..\..\..\Pages\Groups.xaml"
            ((ScholarshipAppointment.Pages.Groups)(target)).Loaded += new System.Windows.RoutedEventHandler(this.PageLoad);
            
            #line default
            #line hidden
            return;
            case 2:
            this.GridSplitter = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 3:
            this.Dialog = ((System.Windows.Controls.ColumnDefinition)(target));
            return;
            case 4:
            this.ButtonsPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            
            #line 28 "..\..\..\Pages\Groups.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.AddButtonClick);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 29 "..\..\..\Pages\Groups.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CopyButtonClick);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 30 "..\..\..\Pages\Groups.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.EditButtonClick);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 31 "..\..\..\Pages\Groups.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteButtonClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.FilterBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            
            #line 34 "..\..\..\Pages\Groups.xaml"
            ((System.Windows.Controls.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.FilterTextBoxTextChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.DataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 38 "..\..\..\Pages\Groups.xaml"
            this.DataGrid.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.DataGridMouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 12:
            this.Frame = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

