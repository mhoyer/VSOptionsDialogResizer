using System;
using Machine.Fakes;
using Machine.Specifications;
using VSOptionsDialogResizer.Addin;

namespace VSOptionsDialogResizer.Tests
{
    public class when_loading_of_Visual_Studio_is_completed : WithSubject<Connect>
    {
        Because of = () => Subject.OnStartupComplete(ref _custom);
        
        It should_start_watching_for_options_dialog_to_open = 
            () => The<IOptionsDialogWatcher>().WasToldTo(w => w.Listen(IntPtr.Zero));

        static Array _custom;
    }
}