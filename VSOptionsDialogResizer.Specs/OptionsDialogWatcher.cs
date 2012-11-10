using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Specs
{
    public class when_listening_for_options_dialog_to_open : WithOptionsDialogWatcher
    {
        Establish context = () =>
            {
                The<ICyclicWorker>()
                    .WhenToldTo(c => c.Start(Param<int>.IsAnything, Param<Action>.IsAnything))
                    .Callback<int, Action>((s,a) => a.Invoke());
            };

        Because of = () => Subject.Listen(DevenvMainWindow);

        It should_start_the_cyclic_lookup_with_200ms_sleep =
            () => The<ICyclicWorker>()
                    .WasToldTo(c => c.Start(200, Param<Action>.IsAnything));

        It should_start_the_cyclic_lookup_with_the_actual_find_action =
            () => The<IOptionsDialogFinder>()
                    .WasToldTo(c => c.Find(DevenvMainWindow));
    }
    
    public class when_waiting_for_the_options_dialog_to_open : WithOptionsDialogWatcher
    {
        Because of = () => Subject.FindOptionsDialog(DevenvMainWindow);

        It should_try_to_find_the_options_dialog_once = () => 
            The<IOptionsDialogFinder>()
                .WasToldTo(f => f.Find(DevenvMainWindow))
                .OnlyOnce();

        It should_not_hook_up_the_window_patcher = () => 
            The<IWindowPatcher>()
                .WasNotToldTo(f => f.PatchUntilClose(Param.IsAny<IntPtr>()));
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

        It should_hook_up_the_window_patcher = () => 
            The<IWindowPatcher>()
                .WasToldTo(f => f.PatchUntilClose(OptionsDialogWindow))
                .OnlyOnce();
    }
    
    public class WithOptionsDialogWatcher : WithSubject<OptionsDialogWatcher>
    {
        protected static readonly IntPtr DevenvMainWindow = new IntPtr(1);
        protected static readonly IntPtr OptionsDialogWindow = new IntPtr(2);
    }
}
