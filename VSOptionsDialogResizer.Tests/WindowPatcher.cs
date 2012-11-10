using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_starting_the_refresh_cycle : WithWindowPatcher
    {
        Because of = () => Subject.PatchUntilClose(_window);

        It should_set_an_explicit_stop_condition =
            () => Worker.StopAction.ShouldNotBeNull();
        
        It should_start_the_cyclic_modifier_with_20ms_sleep =
            () => Worker.WasToldTo(w => w.Start(20, Param.IsAny<Action>()));

        It should_execute_each_modifier =
            () => Modifiers.ToList().ForEach(m => m.WasToldTo(mod => mod.Modify(_window)));
    }

    public class when_executing_all_modifiers : WithWindowPatcher
    {
        Because of = () => Subject.ExecuteAllModifiers(_window);

        It should_execute_each_modifier =
            () => Modifiers.ToList().ForEach(m => m.WasToldTo(mod => mod.Modify(_window)));
    }

    public class WithWindowPatcher : WithFakes
    {
        Establish context = () =>
            {
                Worker = An<ICyclicWorker>();
                Worker
                    .WhenToldTo(c => c.Start(Param<int>.IsAnything, Param<Action>.IsAnything))
                    .Callback<int, Action>((s, a) => a.Invoke());

                Modifiers = Some<IWindowModifier>();
                Subject = new WindowPatcher(Worker, Modifiers);
            };

        protected static IList<IWindowModifier> Modifiers;
        protected static WindowPatcher Subject;
        protected static ICyclicWorker Worker;
        protected static readonly IntPtr _window = new IntPtr(2);
    }
}
