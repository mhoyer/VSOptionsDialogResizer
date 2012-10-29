using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_looking_for_the_options_dialog : WithSubject<OptionsDialogFinder>
    {
        Because of = () => Subject.Find(DevenvMainWindow);

        It should_search_for_windows_with_Options_caption = 
            () => The<IPInvoker>().WasToldTo(p => p.FindWindows("Options"));

        protected static readonly IntPtr DevenvMainWindow = new IntPtr(1);
    }
}