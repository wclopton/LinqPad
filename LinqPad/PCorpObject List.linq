<Query Kind="Expression">
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

from pco in PCorpObjects_VW
where pco.ControlCenter == "WCC"
orderby pco.ControlCenter, pco.District, pco.Substation, pco.Circuit
select new { pco.ControlCenter, pco.District, pco.Substation, pco.Circuit }
//group by ControlCenter, District, Substation, Circuit 
//select new 
//{ 
//pco.ControlCenter, pco.District, pco.SubStation, pco.Circuit 
//}

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