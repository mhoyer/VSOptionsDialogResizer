using System;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_starting_the_refresh_cycle : WithSubject<OptionsDialogModifier>
    {
        Because of = () => Subject.RefreshUntilClose(_optionsDialogWindow);

        It should_start_the_cyclic_modifier = 
            () => The<ICyclicWorker>().WasToldTo(w => w.Start(0, null));
        
        static readonly IntPtr _optionsDialogWindow = new IntPtr(2);
    }
}