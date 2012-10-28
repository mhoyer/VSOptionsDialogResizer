using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_listening_for_options_dialog_to_open : WithSubject<OptionsDialogWatcher>
    {
        Because of = () =>
            {
                Subject.Listen(_devenvMainWindow);
                Subject.Stop();
            };

        It should_try_to_find_the_options_dialog_once = () => 
            The<IOptionsDialogFinder>()
                .WasToldTo(f => f.Find(_devenvMainWindow))
                .OnlyOnce();

        It should_not_hook_up_the_modifier_refresh_cycle = () => 
            The<IOptionsDialogModifier>()
                .WasNotToldTo(f => f.RefreshUntilClose(Param.IsAny<IntPtr>()));

        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
    }
}
