using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nielsen.TechTest.Q2.Answer
{
    /// <summary>
    /// This class is using <seealso cref="BitArray"/> to keep the randomly generated array index.
    /// Fastest for small range with others showing less than 1 milli-seconds (except <seealso cref="ArrayGeneratorUsingList"/>)
    /// For middle range, usually still fastest (but sometimes 2nd, specially for the first test run) after <seealso cref="ArrayGeneratorUsingReturnArray"/>
    /// but sometimes, showing pretty same performence with <seealso cref="ArrayGeneratorUsingReturnArray"/>
    /// However, 
    /// Fastest for big range (but for the first test run sometimes, 2nd), starts showing better performance than <seealso cref="ArrayGeneratorUsingReturnArray"/>
    /// Fastest still for biggest range always, showing clearly better performance than <seealso cref="ArrayGeneratorUsingReturnArray"/>
    /// 
    /// Even if this approach is not fastest for small, overall clearly shows much better performance
    /// Therefore this is the answer
    /// </summary>
    public class ArrayGeneratorUsingBitArray : IArrayGenerator
    {
        public int[] GenerateByRange(int lowNumber, int highNumber)
        {
            if (highNumber < lowNumber)
            {
                throw new ArgumentException("Invalid Argument (Cannot be smaller then \"lowNumber\")");
            }

            var generatedIndex = new BitArray(highNumber - lowNumber + 1);
            var random = new Random();
            int[] rtn = new int[highNumber - lowNumber + 1];

            var min = 0;
            var max = highNumber - lowNumber;
            for (int idx = lowNumber; idx <= highNumber; idx++)
            {
                var rnd = random.Next(min, max + 1);
                while (generatedIndex[rnd] == true)
                {
                    rnd = random.Next(min, max + 1);
                }
                generatedIndex[rnd] = true;
                rtn[rnd] = idx;
            }

            return rtn;
        }
    }
}
