using System;
using System.Windows;
using SingleInstanceManager;
using System.Reflection;
using System.Threading;

namespace FilWatcher
{
    public partial class App : Application
    {
        public static ManualResetEvent _manualResetEvent = new ManualResetEvent( false );

        delegate void DelegateMethod( String filePath );

        protected override void OnStartup( StartupEventArgs e )
        {
            bool isFirstInstance = ApplicationInstanceManager.CreateSingleInstance
            (
                Assembly.GetExecutingAssembly().GetName().Name,
                SingleInstanceCallback
            );

            if( false == isFirstInstance )
                return;

            base.OnStartup( e );

            ThreadPool.QueueUserWorkItem( new WaitCallback(OpenCommandlineFiles) );            
        }

        private void SingleInstanceCallback( object sender, InstanceCallbackEventArgs args )
        {
            if( args == null || Dispatcher == null )
                return;

            for( int index=1; index < args.CommandLineArgs.Length; index++ )
            {
                String filePath = args.CommandLineArgs[ index ];
                Dispatcher.Invoke( new DelegateMethod( DelegateMethodImplementation ), new object[] { filePath } );
            }
        }

        private void DelegateMethodImplementation( String filePath )
        {
            (MainWindow as MainWindow).OpenFile( filePath );
        }

        private void OpenCommandlineFiles( object o )
        {
            _manualResetEvent.WaitOne();
            
            for( int index=1; index < Environment.GetCommandLineArgs().Length; index++ )
            {
                String filePath = Environment.GetCommandLineArgs()[ index ];
                Dispatcher.Invoke( new DelegateMethod( DelegateMethodImplementation ), new object[] { filePath } );
            }
        }
    }
}
