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

var objectList = PCorpObjects_VW
   .OrderBy (pco => pco.ControlCenter)
   .ThenBy (pco => pco.District)
   .ThenBy (pco => pco.Substation)
   .ThenBy (pco => pco.Circuit)
   .Select (
      pco => 
         new  
         {
            ControlCenter = pco.ControlCenter, 
            District = pco.District, 
            Substation = pco.Substation, 
            Circuit = pco.Circuit
         }
   )
   .Where (cc => (cc.ControlCenter == "WCC" 
   					&& cc.District == 564 ))
					;
var query = objectList
   .Select (
      cc => 
         new  
         {
            Name = cc.ControlCenter, 
            Type = PCorpType.ControlCenter, 
            children = objectList
               .Where (d => ((d.ControlCenter == cc.ControlCenter) 
			   					//&& (d.District == cc.District)
							)
						)
               .Select (
                  d => 
                     new  
                     {
                        Name = d.District, 
                        Type = PCorpType.District, 
                        children = objectList
                           .Where (ss => ((ss.ControlCenter == cc.ControlCenter) 
						   					&& (ss.District == cc.District)))
                           .Select (
                              ss => 
                                 new  
                                 {
                                    Name = ss.Substation, 
                                    Type = PCorpType.Substation, 
                                    children = objectList
                                       .Where (
                                          c => 
                                                (((c.ControlCenter == cc.ControlCenter) 
												&& (c.District == cc.District)) 
												&& (c.Substation == cc.Substation)
												//&& (c.Circuit == cc.Circuit)
                                                )
                                       )
                                       .Select (
                                          c => 
                                             new  
                                             {
                                                Name = c.Circuit, 
                                                Type = PCorpType.Circuit
                                             }
                                       )
                                 }
                           )
                     }
               )
         }
   )

 ;
   //.Distinct(new PCorpObject())
   
   //return x.Name == y.Name && x.Type == y.Type;
   //.GroupBy(x => new{x.Name, x.Type}).Select(g => g.First());
objectList.Dump();
query.Dump();
//controlCenterFilter.Dump();
}// close bracket to end the method block

// create new class
public enum PCorpType
{
   ControlCenter,
   District,
   Substation,
   Circuit

