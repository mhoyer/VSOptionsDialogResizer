﻿using System;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;

namespace VSOptionsDialogResizer.Tests
{
    public class when_looking_for_the_options_dialog : WithSubject<OptionsDialogFinder>
    {
        Establish context = () =>
        {
            The<IPInvoker>()
                .WhenToldTo(p => p.FindWindows("Options"))
                .Return(new[] { OptionsWindow, OtherOptionsWindow });
            
            The<IPInvoker>()
                .WhenToldTo(p => p.GetWindow(OptionsWindow, GetWindowCmd.GW_OWNER))
                .Return(DevenvMainWindow);
        };

        Because of = () => _result = Subject.Find(DevenvMainWindow);

        It should_search_for_windows_with_Options_caption = 
            () => The<IPInvoker>().WasToldTo(p => p.FindWindows("Options"));

        It should_return_found_Options_window = () => _result.ShouldEqual(OptionsWindow);

        It should_compare_found_Options_windows_with_given_parent_window_handle = 
            () => The<IPInvoker>().WasToldTo(p => p.GetWindow(OptionsWindow, GetWindowCmd.GW_OWNER));

        protected static readonly IntPtr DevenvMainWindow = new IntPtr(1);
        protected static readonly IntPtr OptionsWindow = new IntPtr(2);
        protected static readonly IntPtr OtherOptionsWindow = new IntPtr(3);
        static IntPtr _result;
    }
}