using System;
using System.Collections.Generic;
using System.Linq;
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

    public class when_executing_all_modifiers : WithFakes
    {
        Establish context = () =>
            {
                _modifiers = Some<IWindowModifier>();
                _subject = new WindowPatcher(An<ICyclicWorker>(), _modifiers);
            };

        Because of = () => _subject.ExecuteAllModifiers(_window);

        It should_execute_each_modifier = () => _modifiers.ToList().ForEach(m => m.Modify(_window));

        static readonly IntPtr _window = new IntPtr(2);
        static IList<IWindowModifier> _modifiers;
        static WindowPatcher _subject;
    }
}
