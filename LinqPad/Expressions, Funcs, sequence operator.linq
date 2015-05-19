<Query Kind="Statements" />

//Linq98 This sample calculates the dot product of two integer vectors. 
//It uses a user-created sequence operator, Combine, to calculate the dot product, 
//passing it a lambda function to multiply two arrays, element by element, and sum the result.            
    int[] vectorA = { 0, 2, 4, 5, 6 }; 
    int[] vectorB = { 1, 3, 5, 7, 8 }; 
     
    int dotProduct = vectorA.Combine(vectorB, (a, b) => a * b).Sum(); 
     
    Console.WriteLine("Dot product: {0}", dotProduct); 
}// close bracket to end the method block

public static class CustomSequenceOperators 
{ 
    public static IEnumerable<int> Combine(this IEnumerable<int> first, IEnumerable<int> second, Func<int, int, int> func)
   {
       using (IEnumerator<int> e1 = first.GetEnumerator(), e2 = second.GetEnumerator())
       {
           while (e1.MoveNext() && e2.MoveNext())
           {
               yield return func(e1.Current, e2.Current);
           }
       }
   } 
//} // notice how no close bracket is added!