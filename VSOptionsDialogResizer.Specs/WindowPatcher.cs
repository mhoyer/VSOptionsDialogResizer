using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.PInvoke;
using VSOptionsDialogResizer.WindowModifiers;

namespace VSOptionsDialogResizer.Specs
{
    public class when_starting_the_refresh_cycle : WithWindowPatcher
    {
        Because of = () => Subject.PatchUntilClose(_window);

        It should_set_an_explicit_stop_condition =
            () => Worker.StopAction.ShouldNotBeNull();

        It should_set_the_stop_condition_to_check_for_existence_of_options_window =
            () => PInvoker.WasToldTo(p => p.GetWindow(_window, GetWindowCmd.GW_OWNER));

        It should_make_the_options_window_resizable =
            () => PInvoker.WasToldTo(p => p.SetWindowLong(_window, GetWindowLong.GWL_STYLE, 0));
        
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
                    .Callback<int, Action>((s, a) => { 
                        a.Invoke();
                        Worker.StopAction.Invoke();
                    });

                Modifiers = Some<IWindowModifier>();
                PInvoker = An<IPInvoker>();
                Subject = new WindowPatcher(Worker, Modifiers, PInvoker);
            };

        protected static IList<IWindowModifier> Modifiers;
        protected static IPInvoker PInvoker;
        protected static WindowPatcher Subject;
        protected static ICyclicWorker Worker;
        protected static readonly IntPtr _window = new IntPtr(2);
    }
}
