# Nielsen.TechTest
This is Nielsen Tech Test Solution with 2 parts, answers for Question 1 and Question 2 and has 5 projects<br/>
Please check and read **TechTest-Solution.docx** under doc folder

# Questions and Answers
* Answer for Question 1: Created 2 main projects (one for common library) which are, sort of, simple robot moving in 2-dimension simulation
	* One for Simple WebAPI service. This is robot hosting application
	* Another for WebAPI client WPF application<br/>At first I thought web front-end for client application however I decided to go with WPF just to save time (I know canvas in html, drawing rectangle but still taking time more than WPF)
* Answer for Question 2: Created one main project, class library and another for Unit Testing using MSTest.<br/>Please read next sections about the approaches I took and the answer

# 4 Approaches to Solve Question 2
| Number Range | Using List<int>  | Using HashSet<int>  | Using BitArray  | Directly Filling in Return Array  |
| ------- | --- | --- | --- | --- |
| Small Range (Less Than 1000)| Worst | Less Than 1ms | Less Than 1ms | Less Than 1ms |
| Middle Range (Bigger Than 1000)| Worst | 3rd<br/>More than 5 milli-seconds difference from best | 2nd Best<br/>Couple of milli-seconds difference from best | Best |
| Big Range (Bigger Than 10000 Less Than Million)| NA<br/>Excluded from Test | 3rd<br/>Took almost twice than best | Best | 2nd Best<br/>Usually 10 milli-seconds difference from best<br/>Sometimes shows Best
| Bigger Range (Bigger Than Million)| NA<br/>Excluded from Test | NA<br/>Excluded from Test | Best | Took almost twice than best |

# Answer for Question 2
From the begining, I guessed that List<int> will be the worst and HashSet<int> approach might be best in small range however would be getting much slower as the range is getting bigger. However I was not sure which one will be better between **BitArray** and **Directly Filling in the Return Array**<br/>
The actual unit test result with 4 different ranges shows almost same as I guessed and **BitArray** approach shows best performance except **Middle Range (Bigger Than 1000)**<br/>
Even if **BitArray** approach is not best in **Middle Range (Bigger Than 1000)**, the difference from the best, **Directly Filling in Return Array** is not big and shows best in other 3 ranges<br/><br/>
**Therefore 'BitArray approach' is primarily My Answer for Question 2**<br/>
However still **'Directly Filling in Return Array' would be alternative if number range is not big**

# Projects
* Nielsen.TechTest.Q1.Common : .net standard 2.1 library used by **Nielsen.TechTest.Q1.RemoteRobotController** and **Nielsen.TechTest.Q1.RobotHosting**
* Nielsen.TechTest.Q1.RobotHosting: .net core 3.1 based ASP.NET WebAPI and **This is one of main solution for Question 1**
* Nielsen.TechTest.Q1.RemoteRobotController: .net core 3.1 based WPF and **This is another main solution for Question 1**
* Nielsen.TechTest.Q2.Answer: .net standard 2.1 library and **This is the main solution, answer for Question 2**, generating randomly filled array by low and high numbers
	* ArrayGeneratorUsingBitArray.cs: **This is my answer for Question 2**. Keeps the already generated random number in BitArray.
	* ArrayGeneratorUsingHashSet.cs: Keeps the already generated random number in HashSet<int>.
	* ArrayGeneratorUsingList.cs: Keeps the already generated random number in List<int>
	* ArrayGeneratorUsingReturnArray.cs: Does not keeps the generated random numbers. Instead, fill the number directly in the return array using the random number as index
* Nielsen.TechTest.Q2.UnitTest: This is MSTest project for **Nielsen.TechTest.Q2.Answer**. All unit test classes are created based on different ranges by low and high numbers
	* Test01SmallRange.cs: Test class covering all classes for **small range less than 1000**
	* Test02MiddleRange.cs: Test class covering all classes for **the ranges from bigger than 1000 but less than 10000**
	* Test03BigRange.cs: Test class covering 3 classes for **the ranges from bigger than 10000 to hundreds of thousands**. **ArrayGeneratorUsingList is excluded**
	* Test04BiggerRange.cs: Test class covering only 2 classes, **ArrayGeneratorUsingBitArray and ArrayGeneratorUsingReturnArray** for **the ranges bigger than million**
