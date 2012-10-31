using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_starting_the_refresh_cycle : WithSubject<WindowPatcher>
    {
        Because of = () => Subject.PatchUntilClose(Param<IntPtr>.IsAnything);

        It should_start_the_cyclic_modifier_with_20ms_sleep =
            () => The<ICyclicWorker>().WasToldTo(w => w.Start(20, Param.IsAny<Action>()));
    }

    public class when_executing_a_modifier : WithSubject<WindowPatcher>
    {
        Because of = () => Subject.ExecuteModifier(_optionsDialogWindow);

        It should_execute_ = () => The<IWindowModifier>().WasToldTo(m => m.Modify(_optionsDialogWindow));

        static readonly IntPtr _optionsDialogWindow = new IntPtr(2);
    }
}
