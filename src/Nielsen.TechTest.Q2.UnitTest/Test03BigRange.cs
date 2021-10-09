using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Nielsen.TechTest.Q2.Answer;

namespace Nielsen.TechTest.Q2.UnitTest
{
    /// <summary>
    /// Minimum couple of tens of thousands range test
    /// </summary>
    [TestClass]
    public class Test03BigRange
    {
        private ArrayGeneratorUsingHashSet _genUsingHash = null;
        private ArrayGeneratorUsingReturnArray _genUsingRtnArray = null;
        private ArrayGeneratorUsingBitArray _genUsingBitArray = null;

        [TestInitialize]
        public void DoTestInit()
        {
            this._genUsingHash = new ArrayGeneratorUsingHashSet();
            this._genUsingRtnArray = new ArrayGeneratorUsingReturnArray();
            this._genUsingBitArray = new ArrayGeneratorUsingBitArray();

            // warming up to minimize first loading impact
            this._genUsingHash.GenerateByRange(-1, 3);
            this._genUsingRtnArray.GenerateByRange(-1, 3);
            this._genUsingBitArray.GenerateByRange(-1, 3);
        }

        [TestMethod]
        [DataRow(0, 12890)]
        [DataRow(-2, 43599)]
        [DataRow(3, 89950)]
        [DataRow(7, 158000)]
        [DataRow(-8, 398890)]
        public void DoTest02Hash(int start, int end)
        {
            var generated = this._genUsingHash.GenerateByRange(start, end);

            var expectedNoOfElement = end - start + 1;

            var noOfItems = generated.Length;
            var min = generated.Min();
            var max = generated.Max();
            var noOfUnique = generated.Distinct().Count();

            Assert.IsTrue(expectedNoOfElement == noOfItems && expectedNoOfElement == noOfUnique && min == start && max == end);
        }

        [TestMethod]
        [DataRow(0, 12890)]
        [DataRow(-2, 43599)]
        [DataRow(3, 89950)]
        [DataRow(7, 158000)]
        [DataRow(-8, 398890)]
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
        [DataRow(0, 12890)]
        [DataRow(-2, 43599)]
        [DataRow(3, 89950)]
        [DataRow(7, 158000)]
        [DataRow(-8, 398890)]
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
