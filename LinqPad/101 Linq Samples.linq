<Query Kind="Program" />

void Main()
{
	Linq1();
}

// Define other methods and classes here
public void Linq1() 
{ 
string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
string[] words = { "cherry", "apple", "blueberry" }; 
//add 1 to returned result
int?[] numbers1 = { null, 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
int[] numbers2 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
int[] numbers3 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
var lowNums = numbers1.Where(x => x < 5).Select(x=>x+1);
Console.WriteLine("add 1 to returned result"); 
Console.WriteLine(lowNums); 

//use index to get text name from an array
var shortDigits = digits.Where((digitStr, indexInt) => digitStr.Length < indexInt); 
foreach (var x in shortDigits) 
{ 
	 Console.WriteLine("The word {0} is shorter than its value.", x); 
} 

//select to produce a sequence of strings representing the text version of a sequence of ints.
var textNums = numbers2.Select(x=>digits[x].ToUpper());
Console.WriteLine("index && ToUpper");
Console.WriteLine(textNums); 

//produce a sequence containing text representations of digits and whether their length is even or odd.
//rename column
var digitOddEvens = numbers3.Select(x=> new{ Digit = digits[x], Even = (x % 2 == 0) });
Console.WriteLine("index && Even = (x % 2 == 0) ");
Console.WriteLine(digitOddEvens);

// indexed Select clause to determine if the value of ints in an array match their position in the array.
var numsInPlace = numbers3.Select((num, index)=> new{Num=num, InPlace =(num==index)});
Console.WriteLine("(num, index)");
Console.WriteLine(numsInPlace);
	
//multiple from translates to Selectmany
//uses a compound from clause to make a query that returns 
//all pairs of numbers from both arrays such that the number 
//from numbersA is less than the number from numbersB.
int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 }; 
int[] numbersB = { 1, 3, 5, 7, 8 }; 
 var pairs1 = 
   from a in numbersA 
   from b in numbersB 
   where a < b 
   select new { a, b }; 
var pairs2 = numbersA
			.SelectMany(a => numbersB, (a,b)=> new{a,b})
			.Where(x => x.a<x.b);
// The SelectMany method actually "flattens" your list(s) so in this case, for all of the elements in 
// numbersA, you are selecting all of the elements in your numbersB collection and generating a new 
// object for each that stores both a and b.
// Next, you'll simply use a Where clause on this collection of pairs to remove any elements where your
// a value is not less than your b value (which was the constraint you defined earlier
Console.WriteLine("SelectMany");
Console.WriteLine(pairs2);

//Linq20 use Take to get only the first 3 elements of the array.
var first3Numbers = numbers3.Take(3);
Console.WriteLine("Take(3)");
Console.WriteLine(first3Numbers);

//Linq22 use Skip first 4
var allButFirst4Numbers = numbers3.Skip(4); 
Console.WriteLine("Skip first 4");
Console.WriteLine(allButFirst4Numbers);

//Linq24 uses TakeWhile to return elements starting from the beginning 
//of the array until a number is hit that is not less than 6
var firstNumbersLessThan6 = numbers3.TakeWhile(n => n < 6); 
Console.WriteLine("TakeWhile numbers less than 6 until number is not less than 6");
Console.WriteLine(firstNumbersLessThan6);

//Linq29
//var sortedWords = from w in words orderby w.Length desending select w;
var sortedWords = words.OrderByDescending(w=>w.Length);
Console.WriteLine("orderby to sort a list of words by length.");
Console.WriteLine(sortedWords);

//Linq39 Reverse to create a list of all digits in the array whose second letter 
//is 'i' that is reversed from the order in the original array.
var reversedIDigits = digits.Where(d=>d[1].ToString()=="i").Reverse();
Console.WriteLine("REVERSE() from the order in the original array.");
Console.WriteLine(reversedIDigits);


}

