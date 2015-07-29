using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactivePlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            //ReplaySubjectBufferExample();
            //ReplaySubjectWindowExample();
            //BehaviorSubjectExample();
            //BehaviorSubjectExample2();
            //BehaviorSubjectExample3();
            //BehaviorSubjectCompletedExample();
            //SubjectInvalidUsageExample();
            ReactiveUISample();
            Console.ReadKey();
        }
        //Takes an IObservable<string> as its parameter. 
        //Subject<string> implements this interface.
        static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            //The next two lines are equivalent.
            //sequence.Subscribe(value=>Console.WriteLine(value));
            sequence.Subscribe(Console.WriteLine);
        }
        public static void ReplaySubjectBufferExample()
        {
            var bufferSize = 2;
            var subject = new ReplaySubject<string>(bufferSize);
            subject.OnNext("a");
            subject.OnNext("b");
            subject.OnNext("c");
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("d");
        }

        public static void ReplaySubjectWindowExample()
        {
            var window = TimeSpan.FromMilliseconds(150);
            var subject = new ReplaySubject<string>(window);
            subject.OnNext("w");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            subject.OnNext("x");
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
            subject.OnNext("y");
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("z");
        }

        public static void BehaviorSubjectExample()
        {
            //Need to provide a default value.
            var subject = new BehaviorSubject<string>("a");
            subject.Subscribe(Console.WriteLine);
        }

        public static void BehaviorSubjectExample2()
        {
            var subject = new BehaviorSubject<string>("a");
            subject.OnNext("b");
            subject.Subscribe(Console.WriteLine);
        }

        public static void BehaviorSubjectExample3()
        {
            var subject = new BehaviorSubject<string>("a");
            subject.OnNext("b");
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("c");
            subject.OnNext("d");
        }

        public static void BehaviorSubjectCompletedExample()
        {
            var subject = new BehaviorSubject<string>("a");
            subject.OnNext("b");
            subject.OnNext("c");
            subject.OnCompleted();
            subject.Subscribe(Console.WriteLine);
        }

        public static void SubjectInvalidUsageExample()
        {
            var subject = new Subject<string>();
            subject.Subscribe(Console.WriteLine);
            subject.OnNext("a");
            subject.OnNext("b");
            subject.OnCompleted();
            subject.OnNext("c");
        }

        public static void ReactiveUISample()
        {
            var blocker = new ManualResetEvent(false);
            IObservable<long> observable = Observable.Interval(TimeSpan.FromSeconds(1));
            var observer = observable
               .Take(10)
               .Subscribe(
                   value => { Console.WriteLine("{0:T} {1}", DateTime.UtcNow, value); },
                   exception => { Console.WriteLine(exception.Message); },
                   () => {
                       Console.WriteLine("Completed");
                       blocker.Set();
                   }
               );
            blocker.WaitOne();
        }
    }
}
