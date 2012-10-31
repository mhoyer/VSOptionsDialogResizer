using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_starting_the_refresh_cycle : WithSubject<OptionsDialogModifier>
    {
        Establish context = () =>
        {
            The<ICyclicWorker>()
                .WhenToldTo(c => c.Start(Param<int>.IsAnything, Param<Action>.IsAnything))
                .Callback<int, Action>((s, a) => a.Invoke());
        };

        Because of = () => Subject.RefreshUntilClose(_optionsDialogWindow);

        It should_start_the_cyclic_modifier = 
            () => The<ICyclicWorker>().WasToldTo(w => w.Start(20, Param.IsAny<Action>()));
        
        static readonly IntPtr _optionsDialogWindow = new IntPtr(2);
    }
}