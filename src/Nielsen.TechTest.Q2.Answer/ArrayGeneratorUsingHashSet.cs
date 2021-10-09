using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nielsen.TechTest.Q2.Answer
{
    /// <summary>
    /// This class is using <seealso cref="HashSet{int}"/> to keep the randomly generated array index.
    /// Fastest for small range with others showing less than 1 milli-seconds (except <seealso cref="ArrayGeneratorUsingList"/>)
    /// For middle range, Usually 3rd fastest (but sometimes 2nd), however not much difference with others (couple of milli-seconds)
    /// However,
    /// 
    /// Getting slower and slower for big range and biggest range (performance will get worse as range is getting bigger)
    /// 
    /// Therefore This is not an answer at all as guessed from the begining
    /// </summary>
    [Obsolete("Overall showing 2nd worst performence therefore not an answer")]
    public class ArrayGeneratorUsingHashSet : IArrayGenerator
    {
        public int[] GenerateByRange(int lowNumber, int highNumber)
        {
            if (highNumber < lowNumber)
            {
                throw new ArgumentException("Invalid Argument (Cannot be smaller then \"lowNumber\")");
            }

            var generatedIndex = new HashSet<int>();
            var random = new Random();
            int[] rtn = new int[highNumber - lowNumber + 1];

            var min = 0;
            var max = highNumber - lowNumber;
            for (int idx = lowNumber; idx <= highNumber; idx++)
            {
                var rnd = random.Next(min, max + 1);
                while (generatedIndex.Contains(rnd))
                {
                    //System.Diagnostics.Debug.WriteLine($"Contains {rnd}");
                    rnd = random.Next(min, max + 1);
                }
                generatedIndex.Add(rnd);
                rtn[rnd] = idx;
            }

            return rtn;
        }
    }
}
