using System.Threading;
using Machine.Fakes;
using Machine.Specifications;

namespace VSOptionsDialogResizer.Tests
{
    public class when_starting_a_cyclic_background_action : WithCyclicBackgroundWorker
    {
        Establish context = () => Triggered = false;

        Because of = () =>
            {
                Subject.Start(SLEEP, () => Triggered = true);
                Thread.Sleep(SLEEP+1); // wait longer than cycle sleep
                Subject.Stop();
            };

        It should_have_triggered_the_action_at_least_once = () => Triggered.ShouldBeTrue();
    }

    public class when_starting_a_cyclic_background_action_with_custom_stop_condition : WithCyclicBackgroundWorker
    {
        Establish context = () => Subject.StopAction = () => _twice == 2;

        Because of = () =>
            {
                Subject.Start(SLEEP, () => _twice++);
                Thread.Sleep(3*SLEEP); // wait longer than cycle sleep
            };

        It should_have_triggered_the_action_at_least_once = () => _twice.ShouldEqual(2);

        static int _twice = 0;
    }

    public class when_running_a_cyclic_background_worker_for_longer : WithCyclicBackgroundWorker
    {
        Establish context = () => _counter = 0;

        Because of = () =>
            {
                Subject.Start(SLEEP, () => _counter++);
                Thread.Sleep(2*SLEEP+10);
                Subject.Stop();
                Thread.Sleep(2 * SLEEP); // wait background thread to finish and be NOT busy anymore
            };

        It should_have_triggered_the_action_at_least_once = () => _counter.ShouldEqual(3);

        static int _counter;
    }

    public class when_running_a_cyclic_background_worker_again : WithCyclicBackgroundWorker
    {
        Establish context =
            () =>
                {
                    Triggered = false;
                    Subject.Start(SLEEP, () => { }); //noop
                    Subject.Stop();
                    Thread.Sleep(2*SLEEP); // wait background thread to finish and be NOT busy anymore
                };

        Because of = () =>
            {
                Subject.Start(SLEEP, () => Triggered = true);
                Thread.Sleep(2*SLEEP);
                Subject.Stop();
            };

        It should_have_triggered_the_action_at_least_once = () => Triggered.ShouldBeTrue();
    }

    public class WithCyclicBackgroundWorker : WithSubject<CyclicBackgroundWorker>
    {
        protected static bool Triggered;
        protected const int SLEEP = 50;
    }
}