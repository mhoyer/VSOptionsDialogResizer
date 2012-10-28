﻿using System;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.Addin;

namespace VSOptionsDialogResizer.Tests
{
    public class when_loading_of_Visual_Studio_is_completed : WithSubject<Connect>
    {
        Establish context = () =>
            {
                var mainWindow = An<Window>();
                mainWindow.WhenToldTo(w => w.HWnd).Return(42);

                _application = An<DTE2>();
                _application.WhenToldTo(a => a.MainWindow).Return(mainWindow);
                _addin = An<AddIn>();
            };

        Because of = () => Subject.OnConnection(_application, ext_ConnectMode.ext_cm_Startup, _addin, ref _customArray);
        
        It should_start_watching_for_options_dialog_to_open =
            () => The<IOptionsDialogWatcher>().WasToldTo(w => w.Listen(new IntPtr(42)));

        static Array _customArray;
        static DTE2 _application;
        static AddIn _addin;
    }
}