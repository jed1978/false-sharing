using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace FalseSharing.DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var paddedValueDto = new PaddedValueDto();
            var normalValueDto = new ValueDto();

            var elapsedMsForCore1 = 0L;
            var elapsedMsForCore2 = 0L;
            var elapsedMsForCore3 = 0L;
            var elapsedMsForCore4 = 0L;
            
            var tasks = new List<Task>();
            
            Console.WriteLine("================ Run 4 tasks with normal value dto ================");
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    Increment(ref normalValueDto.CounterForCore1);
                }
                sw.Stop();
                elapsedMsForCore1 = sw.ElapsedMilliseconds;
            }));
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    Increment(ref normalValueDto.CounterForCore2);
                }
                sw.Stop();
                elapsedMsForCore2 = sw.ElapsedMilliseconds;
            }));
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    //IncrementForCoreTwo(valueDto);
                    Increment(ref normalValueDto.CounterForCore3);
                }
                sw.Stop();
                elapsedMsForCore3 = sw.ElapsedMilliseconds;
            }));
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    //IncrementForCoreTwo(valueDto);
                    Increment(ref normalValueDto.CounterForCore4);
                }
                sw.Stop();
                elapsedMsForCore4 = sw.ElapsedMilliseconds;           
            }));

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Thread for core 1st: {normalValueDto.CounterForCore1}, total elapsed {elapsedMsForCore1} ms.");
            Console.WriteLine($"Thread for core 2nd: {normalValueDto.CounterForCore2}, total elapsed {elapsedMsForCore2} ms.");
            Console.WriteLine($"Thread for core 3th: {normalValueDto.CounterForCore3}, total elapsed {elapsedMsForCore3} ms.");
            Console.WriteLine($"Thread for core 4th: {normalValueDto.CounterForCore4}, total elapsed {elapsedMsForCore4} ms.");
            
            tasks.Clear();
            
            Console.WriteLine("================ Run 4 tasks with padded value dto ================");
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    Increment(ref paddedValueDto.CounterForCore1);
                }
                sw.Stop();
                elapsedMsForCore1 = sw.ElapsedMilliseconds;
            }));
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    Increment(ref paddedValueDto.CounterForCore2);
                }
                sw.Stop();
                elapsedMsForCore2 = sw.ElapsedMilliseconds;
            }));
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    //IncrementForCoreTwo(valueDto);
                    Increment(ref paddedValueDto.CounterForCore3);
                }
                sw.Stop();
                elapsedMsForCore3 = sw.ElapsedMilliseconds;
            }));
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    //IncrementForCoreTwo(valueDto);
                    Increment(ref paddedValueDto.CounterForCore4);
                }
                sw.Stop();
                elapsedMsForCore4 = sw.ElapsedMilliseconds;           
            }));

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Thread for core 1st: {paddedValueDto.CounterForCore1}, total elapsed {elapsedMsForCore1} ms.");
            Console.WriteLine($"Thread for core 2nd: {paddedValueDto.CounterForCore2}, total elapsed {elapsedMsForCore2} ms.");
            Console.WriteLine($"Thread for core 3th: {paddedValueDto.CounterForCore3}, total elapsed {elapsedMsForCore3} ms.");
            Console.WriteLine($"Thread for core 4th: {paddedValueDto.CounterForCore4}, total elapsed {elapsedMsForCore4} ms.");

            Console.Read();
        }

        private static void Increment(ref long counter)
        {
            counter++;
        }

    }

    [StructLayout(LayoutKind.Explicit)]
    public class PaddedValueDto
    {
        [FieldOffset(56)]
        public long CounterForCore1;
        
        [FieldOffset(120)]
        public long CounterForCore2;
        
        [FieldOffset(184)]
        public long CounterForCore3;
        
        [FieldOffset(248)]
        public long CounterForCore4;
    }

    public class ValueDto
    {
        public long CounterForCore1;
        
        public long CounterForCore2;
        
        public long CounterForCore3;
        
        public long CounterForCore4;
    }
}