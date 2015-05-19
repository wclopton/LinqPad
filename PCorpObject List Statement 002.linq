<Query Kind="Statements" />

var pCorpDataList1 = new List<PCorpObject>(){
	new PCorpObject { Name = "SCC", Type=PCorpType.ControlCenter, HasChild=true, 
		Childs = new List<PCorpObject>(){new PCorpObject { Name="MOAB"}}.AsQueryable()
	}
};

//Console.WriteLine(pCorpDataList1);

var pCorpDataList2 = new List<PCorpData>() {
	new PCorpData{ControlCenter = "WCC", District = "CASPER 564", Substation = "BUFFALO", Circuit = "BUF11"},
	new PCorpData{ControlCenter = "WCC", District = "CASPER 564", Substation = "BUFFALO", Circuit = "BUF12"},
	new PCorpData{ControlCenter = "WCC", District = "CLEAR WATER", Substation = "CLEAR WATER", Circuit = "CLW03"},
	new PCorpData{ControlCenter = "WCC", District = "CLEAR WATER", Substation = "CLEAR WATER", Circuit = "CLW04"},
	new PCorpData{ControlCenter = "SCC", District = "MOAB", Substation = "MOAB CITY", Circuit = "MOA14"},
	new PCorpData{ControlCenter = "SCC", District = "MOAB", Substation = "MOAB CITY", Circuit = "MOA12"},
	new PCorpData{ControlCenter = "SCC", District = "MOAB", Substation = "MOAB CITY", Circuit = "MOA13"},
	new PCorpData{ControlCenter = "SCC", District = "MOAB", Substation = "MOAB CITY", Circuit = "MOA11"},
	new PCorpData{ControlCenter = "SCC", District = "MOAB", Substation = "CANYONLANDS", Circuit = "CAN02"},
	new PCorpData{ControlCenter = "SCC", District = "MOAB", Substation = "CANYONLANDS", Circuit = "CAN01"},
	new PCorpData{ControlCenter = "SCC", District = "CLEAR WATER", Substation = "CLEAR WATER", Circuit = "CLW01"},
	new PCorpData{ControlCenter = "SCC", District = "CLEAR WATER", Substation = "CLEAR WATER", Circuit = "CLW02"}

}.AsQueryable();
//Console.WriteLine(pCorpDataList2);

var pCorpGroupBy = pCorpDataList2.GroupBy(cc=>cc.ControlCenter)
	.Select(cc=> 
		new PCorpObject  { Name=cc.Key, Type=PCorpType.ControlCenter, Childs =
		cc.GroupBy(d=>d.District)
			.Select(d=>
				new PCorpObject { Name=d.Key, Type=PCorpType.District, Childs =
				d.GroupBy(ss=>ss.Substation)
				.Select(ss=>
					new PCorpObject { Name=ss.Key, Type= PCorpType.Substation, Childs = 
					ss.GroupBy(c=>c.Circuit)
					
					.Select(
						c=>new PCorpObject {Name=c.Key, Type=PCorpType.Circuit}
						).OrderBy(c => c.Name).AsQueryable()
						}
					).OrderBy(ss => ss.Name).AsQueryable()
				}
			).OrderBy(d => d.Name).AsQueryable()
		}
	).OrderBy(cc => cc.Name);
pCorpGroupBy.Dump();

//var pCorpList = pCorpDataList2
//   .Select (
//      cc => 
//         new PCorpObject (cc.ControlCenter)
//         {
//            Type = PCorpType.ControlCenter, 
//            Childs = //District Childrene
//				pCorpDataList2
//               .Where (d => ((d.ControlCenter == cc.ControlCenter) ))
//               .Select (
//                  d => 
//                     new PCorpObject (d.District.ToString())
//                     {
//                        Type = PCorpType.District,
//						Childs = //Substation Children
//							pCorpDataList2
//							.Where (s => s.ControlCenter == cc.ControlCenter 
//											&& s.District == d.District)
//							.Select (
//								s => 
//									new PCorpObject (s.Substation.ToString())
//									{
//										Type = PCorpType.Substation,
//										Childs =//Circuit Children
//												pCorpDataList2
//												.OrderBy(c => c.Circuit)
//												.Where(c => c.Substation == s.Substation 
//															&& c.District == d.District 
//															&& c.ControlCenter == cc.ControlCenter)
//												.Select(c => new PCorpObject(c.Circuit.ToString())
//												{
//													Name = c.Circuit.ToString(),
//													Type = PCorpType.Circuit
//												})
//									}
//							)
//							.Distinct (new PCorpObject())
//                     }
//               )
//               .Distinct (new PCorpObject())
//         }
//   )
//   .Distinct (new PCorpObject());
//
//
//pCorpList.Dump();

}// close bracket to end the method block

// create new class
public enum PCorpType
{
   ControlCenter,
   District,
   Substation,
   Circuit
}
class PCorpData{
	public string ControlCenter{get;set;}
	public string District{get;set;}
	public string Substation{get;set;}
	public string Circuit{get;set;}
}
class PCorpObject: IEqualityComparer<PCorpObject>
{
    private bool _hasChild;
	public PCorpObject()  { }
   public PCorpObject(string name)
   {
       Name = name;
   }
	public string Name{get;set;}
	public PCorpType Type { get; set; }
	public bool HasChild
			{
		       get
				{
					
					if (this.Childs == null) return false;
					//if (this.Childs.Count <= 0) return false;
	
					return true;
				}
				set{_hasChild = value;}
			}
			//Cannot implicitly convert type 'System.Linq.IQueryable<UserQuery.PCorpObject>' to 
			//'System.Collections.Generic.List<UserQuery.PCorpObject>'. An explicit conversion exists (are you missing a cast?)
	public IQueryable<PCorpObject> Childs { get; set; }
	
	public bool Equals(PCorpObject x, PCorpObject y)
	{
			return x.Name == y.Name && x.Type == y.Type;
	}
	
	public int GetHashCode(PCorpObject obj)
	{
	return (obj.Name + obj.Type).GetHashCode();
	}
	
// notice how no close bracket is added!

//from p in Products
//let spanishOrders = p.OrderDetails.Where (o => o.Order.ShipCountry == "Spain")
//where spanishOrders.Any()
//orderby p.ProductName
//select new
//{
//	p.ProductName,
//	p.Category.CategoryName,
//	Orders = spanishOrders.Count(),	
//	TotalValue = spanishOrders.Sum (o => o.UnitPrice * o.Quantity)
//}