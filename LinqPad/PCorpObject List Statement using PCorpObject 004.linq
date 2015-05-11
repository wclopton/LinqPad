<Query Kind="Statements">
  <Connection>
    <ID>81a66682-25de-464b-84db-cbd6322a7349</ID>
    <Persist>true</Persist>
    <Server>rudy.miner.com\MSSQL3</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>Electric</Database>
    <UserName>pcorpadmin</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAA2FSfev9QAUGOnooUyba/EgAAAAACAAAAAAADZgAAwAAAABAAAACR7Fthgf2LUAnfh2jCXjiMAAAAAASAAACgAAAAEAAAAEzekajkPfoY50FRAEi7Ce0QAAAAm8/vSS/8MGAzcFqFGJXx4BQAAABg54kJ2oFeCGOImzn4VkFu3IsUZQ==</Password>
    <IsProduction>true</IsProduction>
  </Connection>
</Query>


var pCorpDataList = from pco in PCorpObjects_VW
//where pco.ControlCenter == "WCC" && pco.District == 563
//let districts = new {pco.District.Where(d => d.District == 563)}
//where districts.Any() 
orderby pco.ControlCenter, pco.District, pco.Substation, pco.Circuit
select new { pco.ControlCenter, pco.District, pco.Substation, pco.Circuit };


IQueryable<PCorpObject> controlCenterFilter = //(List<PCorpObject>)
										pCorpDataList
                //.OrderBy(cc => cc.ControlCenter)
                .Where(cc => cc.ControlCenter == "WCC")  //test
                .Select(cc => new PCorpObject(cc.ControlCenter)
                {   //Control Center
                    Type = PCorpType.ControlCenter,
                    Childs =// (List<PCorpObject>)
                    ( //District
                     pCorpDataList
                        //.OrderBy(d => d.District)
                    .Where(d => d.ControlCenter == cc.ControlCenter
                        && d.District == 564 //"CASPER 564"
                        ) //Test
                    .Select(d => new PCorpObject(d.District.ToString())
                    {
                        Type = PCorpType.District,
//                        Childs =
//                        ( //Substation
//                        pCorpDataList
//                            // .OrderBy(ss => ss.Substation)
//                        .Where(ss => ss.District == d.District && ss.ControlCenter == cc.ControlCenter
//                             && ss.District == 564 && ss.Substation == "BUFFALO" 
//                            )
//                        .Select(ss => new PCorpObject(ss.Substation)
//                        {
//                            Type = PCorpType.Substation,
//                            Childs =
//                            ( //Circuit
//                            pCorpDataList
//                                //.OrderBy(c => c.Circuit)
//                            .Where(c => c.Substation == ss.Substation && c.District == d.District && c.ControlCenter == cc.ControlCenter)
//                            .Select(c => new PCorpObject(c.Circuit)
//                            {
//                                Type = PCorpType.Circuit
//                            }).Distinct().ToList()
//                            //.Distinct(new PCorpObject()).ToList()
//                            )
//                        }).Distinct().ToList()
//						//.Distinct(new PCorpObject()).ToList()
//                        )
                    })
					.Distinct()
                   // .Distinct(new PCorpObject()).ToList()
                    )
                })
				.Distinct();
                //.Distinct(new PCorpObject()).ToList();
				
controlCenterFilter.Dump();
}// close bracket to end the method block

// create new class
public enum PCorpType
{
   ControlCenter,
   District,
   Substation,
   Circuit
}
class PCorpObject: IEqualityComparer<PCorpObject>
{
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