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
using System.IO;
using System.Threading;
using System.Reflection;

namespace FilWatcher
{
    public partial class TabContent : UserControl
    {
        String              _filePath;
        FileSystemWatcher   _fileSystemWatcher;

        delegate void DelegateMethod();

        public TabContent( String filePath )
        {
            InitializeComponent();

            _filePath = filePath;

            FilePath.Text = filePath;

            _fileSystemWatcher              = new FileSystemWatcher();
            _fileSystemWatcher.Path         = System.IO.Path.GetDirectoryName( filePath );
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileSystemWatcher.Changed     += new FileSystemEventHandler( OnChanged );
            _fileSystemWatcher.EnableRaisingEvents = true;

            ReLoadFile();
        }

        private void OnChanged( object source, FileSystemEventArgs e )
        {
            if( false == e.FullPath.StartsWith( _filePath ) )
                return;

            Dispatcher.Invoke( new DelegateMethod( ReLoadFile ), new object[] { } );
        }

        private void ReLoadFile()
        {
            FileStream fileStream = File.Open( _filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite );
            
            StreamReader streamReader = new StreamReader( fileStream );

            FileContent.Loaded += new RoutedEventHandler( ReadyToScrollToEnd );
            //FileContent.Load( fileStream );
            FileContent.ScrollToEnd();
            FileContent.Text = streamReader.ReadToEnd();

            streamReader.Close();
            fileStream.Close();
        }

        private void ReadyToScrollToEnd( object source, RoutedEventArgs args )
        {
            FileContent.ScrollToEnd();
        }
    }
}
