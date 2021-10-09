using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Nielsen.TechTest.Q2.Answer
{
    /// <summary>
    /// This class is not keeping the randomly generated index at all
    /// Fastest for small range with others showing less than 1 milli-seconds (except <seealso cref="ArrayGeneratorUsingList"/>)
    /// For middle range, usually the first test run, fastest however afteer that 2nd fastest still but sometimes same as <seealso cref="ArrayGeneratorUsingBitArray"/>
    /// 2nd Fastest for big range (but fastest for the first test run), getting slower than <seealso cref="ArrayGeneratorUsingBitArray"/>
    /// 2nd Fastest for biggest range, showing clearly slowness than <seealso cref="ArrayGeneratorUsingBitArray"/>
    /// 
    /// Therefore this is not an answer however could be alternative only if the range is relatively small
    /// </summary>
    public class ArrayGeneratorUsingReturnArray : IArrayGenerator
    {
        public int[] GenerateByRange(int lowNumber, int highNumber)
        {
            if (highNumber < lowNumber)
            {
                throw new ArgumentException("Invalid Argument (Cannot be smaller then \"lowNumber\")");
            }

            var min = 0;
            var max = highNumber - lowNumber;

            var random = new Random();
            int[] rtn = new int[highNumber - lowNumber + 1];

            for (int idx = lowNumber; idx <= highNumber; idx++)
            {
                // if the number is 0, does not have to because it is already 0 by default
                if (idx != 0)
                {
                    var rndIndex = random.Next(min, max + 1);
                    while (rtn[rndIndex] != 0)
                    {
                        rndIndex = random.Next(min, max + 1);
                    }
                    rtn[rndIndex] = idx;
                }
            }

            return rtn;
        }
    }
}
