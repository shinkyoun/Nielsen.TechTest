using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nielsen.TechTest.Q2.Answer;

namespace Nielsen.TechTest.Q2.UnitTest
{
    /// <summary>
    /// Bigger than hundreds of thousands range test
    /// <seealso cref="ArrayGeneratorUsingList"/> is excluded. Already too slow with <seealso cref="Test02MiddleRange"/>
    /// <seealso cref="ArrayGeneratorUsingHashSet"/> is excluded. Already too slow with <seealso cref="Test03BigRange"/>
    /// </summary>
    [TestClass]
    public class Test04BiggerRange
    {
        private ArrayGeneratorUsingReturnArray _genUsingRtnArray = null;
        private ArrayGeneratorUsingBitArray _genUsingBitArray = null;

        [TestInitialize]
        public void DoTestInit()
        {
            this._genUsingRtnArray = new ArrayGeneratorUsingReturnArray();
            this._genUsingBitArray = new ArrayGeneratorUsingBitArray();

            // warming up to minimize first loading impact
            this._genUsingRtnArray.GenerateByRange(-1, 3);
            this._genUsingBitArray.GenerateByRange(-1, 3);
        }

        [TestMethod]
        [DataRow(-5, 1589500)]
        [DataRow(0, 2509500)]
        [DataRow(2, 3650000)]
        [DataRow(0, 25000000)]
        public void DoTest03ReturnArray(int start, int end)
        {
            var generated = this._genUsingRtnArray.GenerateByRange(start, end);

            var expectedNoOfElement = end - start + 1;

            var noOfItems = generated.Length;
            var min = generated.Min();
            var max = generated.Max();
            var noOfUnique = generated.Distinct().Count();

            Assert.IsTrue(expectedNoOfElement == noOfItems && expectedNoOfElement == noOfUnique && min == start && max == end);
        }

        [TestMethod]
        [DataRow(-5, 1589500)]
        [DataRow(0, 2509500)]
        [DataRow(2, 3650000)]
        [DataRow(0, 25000000)]
        public void DoTest04BitArray(int start, int end)
        {
            var generated = this._genUsingBitArray.GenerateByRange(start, end);

            var expectedNoOfElement = end - start + 1;

            var noOfItems = generated.Length;
            var min = generated.Min();
            var max = generated.Max();
            var noOfUnique = generated.Distinct().Count();

            Assert.IsTrue(expectedNoOfElement == noOfItems && expectedNoOfElement == noOfUnique && min == start && max == end);
        }
    }
}
