using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_listening_for_options_dialog_to_open : WithSubject<OptionsDialogWatcher>
    {
        Because of = () => Subject.Listen(new IntPtr(42));

        It should_try_to_find_the_options_dialog = 
            () => The<IOptionsDialogFinder>().WasToldTo(f => f.Find(new IntPtr(42)));

        It should_try_to_find_the_options_dialog_multiple_times = 
            () => The<IOptionsDialogFinder>().WasToldTo(f => f.Find(new IntPtr(42)));
    }
}