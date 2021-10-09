# Nielsen.TechTest
This is Nielsen Tech Test Solution and has 5 projects

* Nielsen.TechTest.Q1.Common : .net standard 2.1 library used by **Nielsen.TechTest.Q1.RemoteRobotController** and **Nielsen.TechTest.Q1.RobotHosting**
* Nielsen.TechTest.Q1.RobotHosting: .net core 3.1 based ASP.NET WebAPI and **This is one of main solution for Question 1**
* Nielsen.TechTest.Q1.RemoteRobotController: .net core 3.1 based WPF and **This is another main solution for Question 1**
* Nielsen.TechTest.Q2.Answer: .net standard 2.1 library and **This is another main solution for Question 2**, generating randomly filled array by low and high numbers
	* ArrayGeneratorUsingBitArray.cs: **This is my answer for Question 2**. Keeps the already generated random number in BitArray.
	* ArrayGeneratorUsingHashSet.cs: Keeps the already generated random number in HashSet<int>.
	* ArrayGeneratorUsingList.cs: Keeps the already generated random number in List<int>
	* ArrayGeneratorUsingReturnArray.cs: Does not keeps the generated random numbers. Instead, fill the number directly in the return array using the random number as index
* Nielsen.TechTest.Q2.UnitTest: This is MSUnit project for **Nielsen.TechTest.Q2.Answer**. All unit test classes are created based on different ranges by low and high numbers
	* Test01SmallRange.cs: Test class covering all classes for **small range less than 1000**
	* Test02MiddleRange.cs: Test class covering all classes for **the ranges from bigger than 1000 but less than 10000**
	* Test03BigRange.cs: Test class covering 3 classes for **the ranges from bigger than 10000 to hundreds of thousands**. **ArrayGeneratorUsingList is excluded**
	* Test04BiggerRange.cs: Test class covering only 2 classes, **ArrayGeneratorUsingBitArray and ArrayGeneratorUsingReturnArray** for **the ranges bigger than million**
