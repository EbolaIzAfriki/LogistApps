﻿#pragma checksum "..\..\..\..\Views\Pages\CarriersPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "60DF39C8A7E90DF21C7552C2EA8ADA608EA83AA12BAE43ABC3A24B46AF2D2E06"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using DiplomAdminEditonBeta;
using Microsoft.Windows.Themes;
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


namespace DiplomAdminEditonBeta {
    
    
    /// <summary>
    /// CarriersPage
    /// </summary>
    public partial class CarriersPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 36 "..\..\..\..\Views\Pages\CarriersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SearchTB;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\Views\Pages\CarriersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddBut;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\Views\Pages\CarriersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid CarriersDataGrid;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\..\Views\Pages\CarriersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ServiceDataGrid;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\Views\Pages\CarriersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid StaticServiceDataGrid;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\Views\Pages\CarriersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddServiceButton;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\..\Views\Pages\CarriersPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DeleteServiceButton;
        
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
            System.Uri resourceLocater = new System.Uri("/DiplomAdminEditonBeta;component/views/pages/carrierspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Pages\CarriersPage.xaml"
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
            this.SearchTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.AddBut = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            this.AddBut.Click += new System.Windows.RoutedEventHandler(this.AddBut_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CarriersDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 45 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            this.CarriersDataGrid.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.CarriersDataGrid_CellEditEnding);
            
            #line default
            #line hidden
            
            #line 45 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            this.CarriersDataGrid.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.CarriersDataGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ServiceDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 84 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            this.ServiceDataGrid.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.ServiceDataGrid_CellEditEnding);
            
            #line default
            #line hidden
            return;
            case 6:
            this.StaticServiceDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 93 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            this.StaticServiceDataGrid.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.CarriersDataGrid_CellEditEnding);
            
            #line default
            #line hidden
            return;
            case 7:
            this.AddServiceButton = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            this.AddServiceButton.Click += new System.Windows.RoutedEventHandler(this.AddServiceButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.DeleteServiceButton = ((System.Windows.Controls.Button)(target));
            
            #line 105 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            this.DeleteServiceButton.Click += new System.Windows.RoutedEventHandler(this.DeleteServiceButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 4:
            
            #line 55 "..\..\..\..\Views\Pages\CarriersPage.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteCarriersBut_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

