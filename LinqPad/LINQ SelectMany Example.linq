<Query Kind="Statements" />

//http://www.codethinked.com/a-visual-look-at-the-linq-selectmany-operator
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
    new Order { Id=1, CustomerId=1, Description="Order 1"},
    new Order { Id=2, CustomerId=1, Description="Order 2"},
    new Order { Id=3, CustomerId=1, Description="Order 3"},
    new Order { Id=4, CustomerId=1, Description="Order 4"},
    new Order { Id=5, CustomerId=2, Description="Order 5"},
    new Order { Id=6, CustomerId=2, Description="Order 6"},
    new Order { Id=7, CustomerId=3, Description="Order 7"},
    new Order { Id=8, CustomerId=3, Description="Order 8"},
    new Order { Id=9, CustomerId=3, Description="Order 9"}
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
  