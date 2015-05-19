<Query Kind="Statements" />


//Then you can use nested GroupBy as many times as you need (depending on the depth of the hierarchy). It would be also relatively easy to rewrite this into a recursive method (that would work for arbitrarily deep hierarchies):
// pseudo code which describes structure:
//new List<PCorpObject>(){new PCorpObject { Name="MOAB"}}.AsQueryable()
 object[][] rawData = new object[][] 
{ 
  new object[]{ "A1", "B1", "C1" }, 
  new object[]{ "A1", "B1", "C2" },
  new object[]{ "A2", "B2", "C3" }, 
  new object[]{ "A2", "B2", "C4" }
  // .. more 
};

rawData.Dump();
// Group by 'A'
var list = rawData.GroupBy(aels => aels[0]).Select(a => 
  // Group by 'B'
  a.GroupBy(bels => bels[1]).Select(b =>
    // Generate result of type 'X' for the current grouping
    new X { A = a.Key.ToString(), B = b.Key.ToString(), 
            // Take the third element 
            Cs = b.Select(c => c[2].ToString()).ToList() }));

list.Dump();
	}// close bracket to end the method block		
class X {
    public string A { get; set; }
    public string B { get; set; }
    public List<string> Cs { get; set; }
//} 