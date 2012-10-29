using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_listening_for_options_dialog_to_open : WithOptionsDialogWatcher
    {
        Because of = () => Subject.Listen(DevenvMainWindow);

        It should_start_the_cyclic_background_checks =
            () => The<ICyclicWorker>()
                    .WasToldTo(c => c.Start(200, Param.IsAny<Action>()));
    }

    public class when_waiting_for_the_options_dialog_to_open : WithOptionsDialogWatcher
    {
        Because of = () => Subject.FindOptionsDialog(DevenvMainWindow);

        It should_try_to_find_the_options_dialog_once = () => 
            The<IOptionsDialogFinder>()
                .WasToldTo(f => f.Find(DevenvMainWindow))
                .OnlyOnce();

        It should_not_hook_up_the_modifier_refresh_cycle = () => 
            The<IOptionsDialogModifier>()
                .WasNotToldTo(f => f.RefreshUntilClose(Param.IsAny<IntPtr>()));
    }

    public class when_options_dialog_was_opened : WithOptionsDialogWatcher
    {
        Establish context = () =>
            {
                The<IOptionsDialogFinder>()
                    .WhenToldTo(f => f.Find(Param.IsAny<IntPtr>()))
                    .Return(OptionsDialogWindow);
            };

        Because of = () => Subject.FindOptionsDialog(DevenvMainWindow);

        It should_hook_up_the_arrangers = () => 
            The<IOptionsDialogModifier>()
                .WasToldTo(f => f.RefreshUntilClose(OptionsDialogWindow))
                .OnlyOnce();
    }
    
    public class WithOptionsDialogWatcher : WithSubject<OptionsDialogWatcher>
    {
        protected static readonly IntPtr DevenvMainWindow = new IntPtr(1);
        protected static readonly IntPtr OptionsDialogWindow = new IntPtr(2);
    }
}
