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
            var valueDto = new ValueDto();

            var elapsedMsForCore1 = 0L;
            var elapsedMsForCore2 = 0L;
            
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    //IncrementForCoreOne(valueDto);
                    Increment(ref valueDto.CounterForCoreOne);
                }
                sw.Stop();
                elapsedMsForCore1 = sw.ElapsedMilliseconds;
                
            }));
            
            tasks.Add(Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                for (var i = 0L; i < 1000000000L; i++)
                {
                    //IncrementForCoreTwo(valueDto);
                    Increment(ref valueDto.CounterForCoreTwo);
                }
                sw.Stop();
                elapsedMsForCore2 = sw.ElapsedMilliseconds;
                
            }));

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"Thread for core one: {valueDto.CounterForCoreOne},  total elapsed {elapsedMsForCore1} ms.");
            Console.WriteLine($"Thread for core two: {valueDto.CounterForCoreTwo}, total elapsed {elapsedMsForCore2} ms.");
            Console.Read();
        }

        private static void Increment(ref long counter)
        {
            counter++;
        }

        private static void IncrementForCoreOne(ValueDto valueDto)
        {
            valueDto.CounterForCoreOne++;
        }
    }

    //[StructLayout(LayoutKind.Explicit)]
    public class ValueDto
    {
        //[FieldOffset(0)]
        public long CounterForCoreOne;
        
        //[FieldOffset(56)]
        public long CounterForCoreTwo;
        
    }
}