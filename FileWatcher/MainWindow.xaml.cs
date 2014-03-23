using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace FilWatcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // inform the app that we're ready to load files pass in command line
            App._manualResetEvent.Set();

            // handle file drops
            Drop += new DragEventHandler( FileDrop );
        }

        void FileDrop( object sender, DragEventArgs e )
        {
            DataObject dataObject = e.Data as DataObject;

            foreach( String file in dataObject.GetFileDropList() )
                OpenFile( file );
        }

        private void SettingsClick( object sender, RoutedEventArgs e )
        {}

        private void OpenClick( object sender, RoutedEventArgs e )
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            Nullable<bool> result = fileDialog.ShowDialog();
            if( false == result )
                return;

            OpenFile( fileDialog.FileName );
        }
        
        private void CloseClick( object sender, RoutedEventArgs e )
        {
            if( tabControl.Items.Count > 0 )
                tabControl.Items.Remove( tabControl.Items[ tabControl.SelectedIndex ] );
        }

        #region OpenFile()

        public void OpenFile( String filePath )
        {
            //MenuItem    menuItem        = new MenuItem();
            //            menuItem.Header = "Close";
            //            menuItem.Name   = "Close";
            //            menuItem.Click += new RoutedEventHandler( CloseTab );

            //ContextMenu contextMenu = new ContextMenu();
            //            contextMenu.Items.Add( menuItem );

            TabContent tabContent = new TabContent( filePath );

            TabItem tabItem             = new TabItem();
                    tabItem.Header      = System.IO.Path.GetFileName( filePath );
                    tabItem.Content     = tabContent;
                    //tabItem.ContextMenu = contextMenu;

            tabControl.Items.Add( tabItem );
            tabControl.SelectedIndex = tabControl.Items.Count - 1;
        }

        #endregion
    }
}
