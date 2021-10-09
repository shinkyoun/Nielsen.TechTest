using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nielsen.TechTest.Q2.Answer
{
    /// <summary>
    /// This class is using <seealso cref="List{int}"/> to keep the randomly generated array index.
    /// Overall slowest in all ranges.
    /// 
    /// Therefore This is not an answer at all as guessed from the begining
    /// </summary>
    [Obsolete("Overall showing worst performence therefore not an answer")]
    public class ArrayGeneratorUsingList : IArrayGenerator
    {
        public int[] GenerateByRange(int lowNumber, int highNumber)
        {
            if (highNumber < lowNumber)
            {
                throw new ArgumentException("Invalid Argument (Cannot be smaller then \"lowNumber\")");
            }

            var generatedIndex = new List<int>();
            var random = new Random();
            int[] rtn = new int[highNumber - lowNumber + 1];

            var min = 0;
            var max = highNumber - lowNumber;
            for (int idx = lowNumber; idx <= highNumber; idx++)
            {
                var rnd = random.Next(min, max + 1);
                while (generatedIndex.Contains(rnd))
                {
                    rnd = random.Next(min, max + 1);
                }
                generatedIndex.Add(rnd);
                rtn[rnd] = idx;
            }

            return rtn;
        }
    }
}
