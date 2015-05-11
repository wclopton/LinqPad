<Query Kind="Program" />

void Main()
{
	Linq1();
}

// Define other methods and classes here
public void Linq1() 
{ 
//add 1 to returned result
int?[] numbers1 = { null, 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
int[] numbers2 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
int[] numbers3 = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 }; 
var lowNums = numbers1.Where(x => x < 5).Select(x=>x+1);
Console.WriteLine(lowNums); 

//use index to get text name from an array
string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }; 
var shortDigits = digits.Where((digitStr, indexInt) => digitStr.Length < indexInt); 
foreach (var x in shortDigits) 
{ 
	 Console.WriteLine("The word {0} is shorter than its value.", x); 
} 

//select to produce a sequence of strings representing the text version of a sequence of ints.
var textNums = numbers2.Select(x=>digits[x].ToUpper());
Console.WriteLine(textNums); 

//produce a sequence containing text representations of digits and whether their length is even or odd.
//rename column
var digitOddEvens = numbers3.Select(x=> new{ Digit = digits[x], Even = (x % 2 == 0) });
Console.WriteLine(digitOddEvens);

// indexed Select clause to determine if the value of ints in an array match their position in the array.
var numsInPlace = numbers3.Select((num, index)=> new{Num=num, InPlace =(num==index)});
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
Console.WriteLine(pairs2);

} 