using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using SimpleInjector;
using SimpleInjector.Extensions;

namespace VSOptionsDialogResizer.Addin
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2
    {
        readonly IOptionsDialogWatcher _optionsDialogWatcher;

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
            try
            {
                var assembly = typeof(IOptionsDialogWatcher).Assembly;
                var exportedTypes = assembly.GetExportedTypes();

                var interfaces = from type in exportedTypes
                                 where type.IsInterface
                                 select type;

                var registrations = from type in exportedTypes
                                    where type.GetInterfaces().Length > 0
                                    where interfaces.Any(i => i.IsAssignableFrom(type))
                                    select new
                                    {
                                        Interface = type.GetInterfaces().First(),
                                        Implementation = type
                                    };

                var container = new Container();
                registrations.ToList().ForEach(r => container.Register(r.Interface, r.Implementation));

                _optionsDialogWatcher = container.GetInstance<IOptionsDialogWatcher>();
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