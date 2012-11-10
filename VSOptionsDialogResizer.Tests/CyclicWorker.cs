using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_starting_a_cyclic_action : WithCyclicWorker
    {
        Because of = () => Subject.Start(SLEEP, () => Triggered = true);

        It should_have_triggered_the_action_at_least_once = () => Triggered.ShouldBeTrue();
    }
    
    public class when_running_a_cyclic_worker_again : WithCyclicWorker
    {
        Establish context = () => Subject.Start(SLEEP, () => { }); // trigger first job with noop

        Because of_starting_a_second_time = () => Subject.Start(SLEEP, () => Triggered = true);

        It should_have_allow_starting_the_action_again = () => Triggered.ShouldBeTrue();
    }

    public class WithCyclicWorker : WithFakes
    {
        Establish context = () => Subject = new CyclicWorker();

        protected static bool Triggered;
        protected static CyclicWorker Subject;
        protected const int SLEEP = 5;
    }
}