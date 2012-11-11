using System;
using System.Diagnostics;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;

namespace VSOptionsDialogResizer.Addin
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public partial class Connect : IDTExtensibility2
    {
        readonly IOptionsDialogWatcher _optionsDialogWatcher;

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
            try
            {
                InitContainer();
                _optionsDialogWatcher = _container.GetInstance<IOptionsDialogWatcher>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("{0}\r\n{1}", ex.Message, ex.StackTrace);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        /// <param name="optionsDialogWatcher"> </param>
        public Connect(IOptionsDialogWatcher optionsDialogWatcher)
        {
            _optionsDialogWatcher = optionsDialogWatcher;
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            try
            {
                _applicationObject = (DTE2)application;
                _addInInstance = (AddIn)addInInst;

                if (_optionsDialogWatcher == null) return;
                _optionsDialogWatcher.Listen(new IntPtr(_applicationObject.MainWindow.HWnd));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            _optionsDialogWatcher.StopListen();
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}