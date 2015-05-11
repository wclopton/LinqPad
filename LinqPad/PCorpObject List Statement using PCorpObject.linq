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


var query = from pco in PCorpObjects_VW
where pco.ControlCenter == "WCC" && pco.District == 563
//let districts = new {pco.District.Where(d => d.District == 563)}
//where districts.Any() 
orderby pco.ControlCenter, pco.District, pco.Substation, pco.Circuit
select new { pco.ControlCenter, pco.District, pco.Substation, pco.Circuit };


List<PCorpObject> controlCenterList = query
                //.OrderBy(cc => cc.ControlCenter)
                .Where(cc => cc.ControlCenter == "WCC")  //test
                .Select(cc => new PCorpObject(cc.ControlCenter)
                {   //Control Center
                    Type = PCorpType.ControlCenter,
                    //Childs = (d1 => pCorpDataList.Where(d1.District == cc.District))

                })
                .Distinct(new PCorpObject()).ToList();
				
query.Dump();
}

public enum PCorpType
{
   ControlCenter,
   District,
   Substation,
   Circuit
}
class PCorpObject: IEqualityComparer<PCorpObject>
{
public PCorpObject()
        {
        }
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
                if (this.Childs.Count <= 0) return false;

                return true;
            }
        }
public List<PCorpObject> Childs { get; set; }

public bool Equals(PCorpObject x, PCorpObject y)
{
  return x.Name == y.Name &&
          x.Type == y.Type;
}

public int GetHashCode(PCorpObject obj)
{
  return (obj.Name + obj.Type).GetHashCode();
}
		
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