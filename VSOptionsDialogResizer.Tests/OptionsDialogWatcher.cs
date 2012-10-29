using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_listening_for_options_dialog_to_open : WithSubject<OptionsDialogWatcher>
    {
        Because of = () => Subject.Listen(_devenvMainWindow);

        It should_start_the_cyclic_background_checks =
            () => The<ICyclicBackgroundWorker>()
                    .WasToldTo(c => c.Start(200, Param.IsAny<Action>()));

        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
    }

    public class when_looking_for_the_options_dialog : WithSubject<OptionsDialogWatcher>
    {
        Because of = () => Subject.FindOptionsDialog(_devenvMainWindow);

        It should_try_to_find_the_options_dialog_once = () => 
            The<IOptionsDialogFinder>()
                .WasToldTo(f => f.Find(_devenvMainWindow))
                .OnlyOnce();

        It should_not_hook_up_the_modifier_refresh_cycle = () => 
            The<IOptionsDialogModifier>()
                .WasNotToldTo(f => f.RefreshUntilClose(Param.IsAny<IntPtr>()));

        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
    }

    public class when_options_dialog_was_opened : WithSubject<OptionsDialogWatcher>
    {
        Establish context = () =>
            {
                The<IOptionsDialogFinder>()
                    .WhenToldTo(f => f.Find(Param.IsAny<IntPtr>()))
                    .Return(_optionsDialogWindow);
            };

        Because of = () => Subject.FindOptionsDialog(_devenvMainWindow);

        It should_hook_up_the_arrangers = () => 
            The<IOptionsDialogModifier>()
                .WasToldTo(f => f.RefreshUntilClose(_optionsDialogWindow))
                .OnlyOnce();

        static readonly IntPtr _devenvMainWindow = new IntPtr(1);
        static readonly IntPtr _optionsDialogWindow = new IntPtr(2);
    }
}
