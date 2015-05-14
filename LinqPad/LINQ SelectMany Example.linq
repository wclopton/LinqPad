<Query Kind="Statements" />

//http://www.codethinked.com/a-visual-look-at-the-linq-selectmany-operator
//http://community.bartdesmet.net/blogs/bart/archive/2008/08/30/c-3-0-query-expression-translation-cheat-sheet.aspx
  var customers = new Customer[]
  {
    new Customer() { Id=1, Name ="A"},
    new Customer() { Id=2, Name ="B"},
    new Customer() { Id=3, Name ="C"}
  };

  var customers2 = new CustomerWithOrders[]
  {
    new CustomerWithOrders() { Id=1, Name ="A", Orders=new List<OrderWithFK>(){ new OrderWithFK{ Id=1, CustomerId=1, Description="Order 1"},new OrderWithFK{ Id=2, CustomerId=1, Description="Order 2"}}},
    new CustomerWithOrders() { Id=2, Name ="B", Orders=new List<OrderWithFK>(){ new OrderWithFK{ Id=3, CustomerId=2, Description="Order 3"},new OrderWithFK{ Id=4, CustomerId=2, Description="Order 4"}}},
    new CustomerWithOrders() { Id=3, Name ="C", Orders=new List<OrderWithFK>(){ new OrderWithFK{ Id=5, CustomerId=3, Description="Order 5"}}}
  };
  
  var orders = new Order[]
  {
    new Order { Id=1, CustomerId=1, Description="Order 1", Total =330.00, OrderDate= new DateTime(2015, 5, 1)},
    new Order { Id=2, CustomerId=1, Description="Order 2", Total =471.20, OrderDate= new DateTime(2015, 5, 2)},
    new Order { Id=3, CustomerId=1, Description="Order 3", Total =88.80, OrderDate= new DateTime(2015, 5, 3)},
    new Order { Id=4, CustomerId=1, Description="Order 4", Total =479.75, OrderDate= new DateTime(2015, 5, 4)},
    new Order { Id=5, CustomerId=2, Description="Order 5", Total =320.00, OrderDate= new DateTime(2015, 5, 4)},
    new Order { Id=6, CustomerId=2, Description="Order 6", Total =403.20, OrderDate= new DateTime(2015, 5, 5)},
    new Order { Id=7, CustomerId=3, Description="Order 7", Total =375.50, OrderDate= new DateTime(2015, 5, 5)},
    new Order { Id=8, CustomerId=3, Description="Order 8", Total =480.00, OrderDate= new DateTime(2015, 5, 6)},
    new Order { Id=9, CustomerId=3, Description="Order 9", Total =407.70, OrderDate= new DateTime(2015, 5, 7)}
  };

//this is a cross-join (cartesian product of two sets).  There is no explicit join operator for it.
//SELECT * FROM [TABLE 1] CROSS JOIN [TABLE 2]
// OR SELECT * FROM [TABLE 1], [TABLE 2]
  var customerOrders = from c in customers
                       from o in orders
                       where o.CustomerId == c.Id
                       select new 
                              { 
                                 CustomerId = c.Id
                                 , OrderDescription = o.Description 
                              };

  foreach (var item in customerOrders)
    Console.WriteLine(item.CustomerId + ": " + item.OrderDescription);

//use SelectMany with WHERE clause inside the SelectMany
//The first argument maps each customer to a collection of orders (completely analagous to the 'where' clause you already have).
//The second argument transforms each matched pair {(c1, o1), (c1, o2) .. (c3, o9)} into a new type, which I've made the same as your example
//The resulting collection is flat like you'd expect in your original example.
var customerOrders2 = customers.SelectMany(
            c => orders.Where(o => o.CustomerId == c.Id),
            (c, o) => new { CustomerId = c.Id, OrderDescription = o.Description });
Console.WriteLine(customerOrders2);

//create derived table and do where on derived table
var customerOrders3 = customers.SelectMany(
            c => orders,
            (c, o) => new { CustomerId = c.Id, OrderCustId = o.CustomerId, OrderDescription = o.Description })
			.Where(x => x.CustomerId == x.OrderCustId);
Console.WriteLine(customerOrders3);

//which will produce the same result without needing the flat collection of Orders. 
//The SelectMany takes each Customer's Orders collection and iterates through that to 
//produce an IEnumerable<Order> from an IEnumerable<Customer>.

var customerOrders4 = customers2
                        .SelectMany(c=>c.Orders)
                        .Select(o=> new { CustomerId = o.CustomerId, 
                                           OrderDescription = o.Description });
Console.WriteLine(customerOrders4);	

var names = new string[] { "Ana", "Raz", "John" };
var numbers = new int[] { 1, 2, 3 };
var newList=names.SelectMany(
    x => numbers,
    (y, z) => { return y + z + " test "; });
    Console.WriteLine(newList);

//Linq15() uses a compound from clause to select all orders where the order total is less than 500.00.
var orders15 = customers.SelectMany(c=>orders
	.Where(o=> o.Total < 400.00D && o.CustomerId == c.Id),
	(c, o) => new { CustomerId = c.Id, OrderId = o.Id, o.Total });
	Console.WriteLine(orders15);

//Linq18() uses a compound from clause to select all orders where the order total is less than 500.00.
DateTime cutoffDate = new DateTime(2015, 5, 4);
var orders18 = customers.SelectMany(c=>orders
	.Where(o=> o.OrderDate >=cutoffDate && o.CustomerId == c.Id),
	(c, o) => new { CustomerId = c.Id, OrderId = o.Id, o.Total, o.OrderDate });
	Console.WriteLine(orders18);

//Linq42 group words by their first letter.
var numberGroups42 = orders.GroupBy(w=>w.OrderDate).Select(w=> new{ OrderDate = w.Key, Words = w});
Console.WriteLine("Group words by OrderDate.");
Console.WriteLine(numberGroups42);
	
}// close bracket to end the method block

public class Customer
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
  
  class Order
  {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; }
	public double Total { get; set; }
	public DateTime OrderDate {get;set;}
  }
  
  class CustomerWithOrders
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<OrderWithFK> Orders { get; set; }
  }
  class OrderWithFK
  {
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Description { get; set; }
  //}