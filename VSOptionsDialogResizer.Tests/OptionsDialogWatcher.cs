using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_listening_for_options_dialog_to_open : WithSubject<OptionsDialogWatcher>
    {
        Establish context = () =>
            {
                The<IOptionsDialogFinder>()
                    .WhenToldTo(f => f.Find(Param.IsAny<IntPtr>()))
                    .Return(_optionsDlgWindow);
            };

        Because of = () => Subject.Listen(_devenvMainWindow);

        It should_try_to_find_the_options_dialog = () => 
            The<IOptionsDialogFinder>()
                .WasToldTo(f => f.Find(_devenvMainWindow));

        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
        static readonly IntPtr _optionsDlgWindow = new IntPtr(2);
    }
}