<Query Kind="Program">
  <Connection>
    <ID>0fa41ae9-7ee1-4732-b283-482818ef5d86</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Driver Assembly="(internal)" PublicKeyToken="no-strong-name">LINQPad.Drivers.EFCore.DynamicDriver</Driver>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Server>.</Server>
    <Database>etools2023</Database>
    <DisplayName>etools2023-entity</DisplayName>
    <DriverData>
      <EncryptSqlTraffic>True</EncryptSqlTraffic>
      <PreserveNumeric1>True</PreserveNumeric1>
      <EFProvider>Microsoft.EntityFrameworkCore.SqlServer</EFProvider>
    </DriverData>
  </Connection>
</Query>

// Driver is responsible for orchestrating the flow by calling
// various methods and classes that contain the actual business logic
// or data processing operations.
void Main()
{

	//(Required) Methods
	//GetCategories
	TestGetCategories().Dump();     //maybe for onintialized? // for index page UI //


	//(Required)GetItemsByCategoryID
	TestGetItemsByCategoryID(-3).Dump("invalid categoryid");   //users or employees selecting certain category for display
	TestGetItemsByCategoryID(0).Dump("invalid categoryid");   //users or employees selecting certain category for display
	TestGetItemsByCategoryID(4).Dump("Items of categoryID 4");   //users or employees selecting certain category for display

	//TestAddToShoppingLists
	TestAddToShoppingLists(34, 1).Dump("Add the first item to empty list");
	TestAddToShoppingLists(34, 2).Dump("duplicate item,quantity added to the existing item");
	TestAddToShoppingLists(82, 1).Dump("Add another item");
	TestAddToShoppingLists(83, 1).Dump("Add another item");
	TestAddToShoppingLists(84, 1).Dump("Add another item");
	TestAddToShoppingLists(5580, 1).Dump("Add another item");
	TestAddToShoppingLists(5580, 1).Dump("Duplicate Item Quantity added to the existing item");
	TestAddToShoppingLists(5580, 1).Dump("can't order more than the quantity on hand"); //quantity on hand for id5580 is 2 in the database.
	TestAddToShoppingLists(24, 0).Dump("Qty input should be greater than 0");
	TestAddToShoppingLists(24, 1).Dump("-----------No such product in the database");


	//TestEditQuantity of shoppingLists
	TestEditQuantity(5580, 0).Dump("Quantity should be greater than 0");
	TestEditQuantity(5580, 5).Dump("Quantity for Edit cannot be over Quantity On Hand");
	TestEditQuantity(5580, 1).Dump("Successfuly Edit For Quantity");


	//Test for Removing Item from the cart
	TestRemovingItem(0).Dump("productID can't be zero");
	TestRemovingItem(-3).Dump("no negative number for productID");
	TestRemovingItem(5580).Dump("The result of removing");


	shoppingListItems.Dump("The Current Shopping ListItem");
	//(Required)SaveSales


	var saleID = AddSales(shoppingListItems, 1, "", "M", 0); //new record saleID is always 0.
	Console.WriteLine($"{saleID} is new sale ID.");


	//(Required)GetSaleRefund


	//(Required)and SaveRefund



	// plan ahead
	// 1) get category needed(category id as parameter) (showing product name / price / etc)
	// 2) select a menu(UI) and choose quantity? -> keep CartView updated with that info by assigning values entered by users??




}

// This region contains methods used for testing the functionality
// of the application's business logic and ensuring correctness.
#region Test Methods


public List<CategoryView> TestGetCategories()
{
	try
	{
		return GetCategories();
	}

	#region catch all exceptions (define later)

	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();

		}
	}

	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	#endregion
	return null; //Ensures a valid return value even on failure

}



public List<StockItemView> TestGetItemsByCategoryID(int categoryID)
{
	try
	{
		return GetItemsByCategoryID(categoryID);
	}

	#region catch all exceptions (define later)

	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();

		}
	}

	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	#endregion
	return null; //Ensures a valid return value even on failure

}

public List<ShoppingItemsView> TestAddToShoppingLists(int productID, int qty)
{
	try
	{
		return AddToShoppingLists(productID, qty);
	}

	#region catch all exceptions (define later)

	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();

		}
	}

	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	#endregion
	return null; //Ensures a valid return value even on failure

}


public List<ShoppingItemsView> TestEditQuantity(int productID, int qty)
{
	try
	{
		return EditQuantity(productID, qty);
	}

	#region catch all exceptions (define later)

	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();

		}
	}

	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	#endregion
	return null; //Ensures a valid return value even on failure

}





public List<ShoppingItemsView> TestRemovingItem(int productID)
{
	try
	{
		return RemovingItem(productID);
	}

	#region catch all exceptions (define later)

	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();

		}
	}

	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}

	#endregion
	return null; //Ensures a valid return value even on failure

}



#endregion

// This region contains support methods for testing
#region Support Methods
public Exception GetInnerException(System.Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}
#endregion

// This region contains all methods responsible
// for executing business logic and operations.
#region Methods

//get all categories
public List<CategoryView> GetCategories()
{
	return Categories
					.Where(x => x.RemoveFromViewFlag == false)
					.Select(x => new CategoryView
					{
						//CategoryID = x.CategoryID, //maybe not needed to show user
						Description = x.Description,
						NumberOfItems = x.StockItems.Where(a => a.Discontinued == false)
													.Count()

						//ItemList = x.StockItems
						//						.Where(x => x.Discontinued == false)
						//						.Select(a => new StockItemView
						//						{
						//							StockItemID = a.StockItemID,
						//							Description = a.Description
						//						}).ToList()

					}).ToList();


}

//GetItemsByCategoryID
public List<StockItemView> GetItemsByCategoryID(int categoryID)
{

	#region Business Logic and Parameter Exceptions
	//create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();

	//Business Rules
	//These are processing rules that need to be satisfied
	// for valid data
	// rule : categoryID should be valid(greater than 0)

	if (categoryID <= 0)
	{
		throw new ArgumentException("Category ID is invalid(should be greater than 0)");
	}

	var matchingCategoryID = Categories
									.Where(x => x.CategoryID == categoryID)
									.FirstOrDefault();

	if (matchingCategoryID == null) //maybe this is not needed because customers only click categoryname they can see on the webpage, but just in case.
	{
		throw new ArgumentException("There is no matching category in the database.");
	}

	#endregion

	return StockItems
					.Where(x => x.RemoveFromViewFlag == false &&
								x.CategoryID == categoryID)
					.Select(x => new StockItemView
					{
						StockItemID = x.StockItemID,
						Description = x.Description,
						SellingPrice = x.SellingPrice

					}).ToList();

}


public List<ShoppingItemsView> AddToShoppingLists(int productID, int qty)
{

	if (productID == null)
	{
		throw new ArgumentNullException("Product ID should be supplied");
	}

	if (productID <= 0)
	{
		throw new ArgumentException("Product ID should be greater than 0");
	}

	if (qty <= 0)
	{
		throw new ArgumentNullException("Quantity should be greater than 0");
	}

	var productInDatabase = StockItems.Where(x => x.StockItemID == productID).FirstOrDefault();

	if (productInDatabase == null)
	{
		throw new ArgumentException("There is no such item in the database.");
	}


	if (productInDatabase.QuantityOnHand <= 0)
	{
		throw new ArgumentException("We don't have quantity on hand for this item now.");
	}



	var existingItems = shoppingListItems.Where(x => x.ProductID == productID).FirstOrDefault();
	if (existingItems != null)
	{
		if (existingItems.Quantity + qty > productInDatabase.QuantityOnHand)
		{
			throw new ArgumentException("You can't order more than the quantity on hand.");
		}

		Console.WriteLine("Duplicate item is already in the cart! quantity added to the existing Item");
		existingItems.Quantity += qty;

	}
	else
	{
		shoppingListItems.Add(new ShoppingItemsView
		{
			ProductID = productID,
			ProductName = productInDatabase.Description,
			//Quantity = productInDatabase.QuantityOnHand > 0 ? 1 : throw new ArgumentException("We don't have this item in the inventory"),
			Quantity = qty,
			UnitPrice = productInDatabase.SellingPrice,
		});
	}
	return shoppingListItems;

}


//public List<ShoppingItemsView> AddorEditShoppingLists (int quantity)



public List<ShoppingItemsView> EditQuantity(int productID, int qty)
{
	if (qty <= 0)
	{
		throw new ArgumentException("quantity should be greater than 0");
	}

	var productInDatabase = StockItems.Where(x => x.StockItemID == productID).FirstOrDefault();

	if (productInDatabase == null)
	{
		throw new ArgumentException("There is no such item in the record.");
	}

	var productInShoppingLists = shoppingListItems.Where(x => x.ProductID == productID).FirstOrDefault();
	if (productInShoppingLists == null)
	{
		throw new ArgumentNullException("there is no item with the productID in the shopping list item.");
	}


	if (qty > productInDatabase.QuantityOnHand)
	{
		throw new ArgumentException($"You can't order more than the quantity on hand. Quantity on hand : {productInDatabase.QuantityOnHand}.");
	}

	productInShoppingLists.Quantity = qty;
	return shoppingListItems;
}




public List<ShoppingItemsView> RemovingItem(int productID)
{
	if (productID == null)
	{
		throw new ArgumentNullException("Product ID should be supplied.");
	}

	if (productID <= 0)
	{
		throw new ArgumentNullException("Invalid ProductID for removing.");
	}

	var productInShoppingLists = shoppingListItems.Where(x => x.ProductID == productID).FirstOrDefault();

	if (productInShoppingLists == null)
	{
		throw new ArgumentNullException($"No item with the productID:{productID} in the shopping list");
	}
	else
	{
		shoppingListItems.Remove(productInShoppingLists);
	}


	return shoppingListItems;
}



//Separating Add and Edit..to make it simpler for now.

public int AddSales(List<ShoppingItemsView> shoppingListItems, int employeeID, string couponIDValue, string paymentType, int saleID)
{
	#region Business Logic and Parameter Exceptions
	//    create a list<Exception> to contain all discovered errors
	List<Exception> errorList = new List<Exception>();
	//  Business Rules
	//    These are processing rules that need to be satisfied
	//        for valid data
	//    rule:    shoppingItemView cannot be null
	if (shoppingListItems == null)
	{
		throw new ArgumentNullException("No shopping list was supplied");
	}

	if (shoppingListItems.Count == 0)
	{
		throw new ArgumentNullException("There should be at least one shopping item in the list.");
	}

	//    rule:    Only Associates or StoreManagers can do sales and returns
	if (employeeID == null)
	{
		throw new ArgumentNullException("Employee ID should be supplied");
	}

	var checkPosition = Employees
								.Where(x => x.EmployeeID == employeeID)
								.FirstOrDefault();

	if (checkPosition == null) //checking matching employee
	{
		throw new ArgumentException($"There is no matching employee with ID {employeeID}.");
	}

	if (!(checkPosition.Position.Description == "Store Manager" || checkPosition.Position.Description == "Associate"))
	{
		throw new ArgumentException("This Employee is not allowed to do sales and returns transaction.");
	}

	int discount = 0; //this is discount percentage, default to 0.

	var matchingCoupon = Coupons
								.Where(x => x.CouponIDValue == couponIDValue)
								.FirstOrDefault();

	if (!string.IsNullOrWhiteSpace(couponIDValue)) //only check coupon when it is supplied.
	{
		if (matchingCoupon == null)
		{
			throw new ArgumentException($"There is no matching coupon of {couponIDValue}.");
		}
		else
		{
			discount = matchingCoupon.CouponDiscount;
		}
	}
	
	#endregion

	// Retrieve the invoice from the database or create a new one if it doesn't exist.
	Sales sale = Sales
					.Where(x => x.SaleID == saleID)
					.FirstOrDefault();
	// If the invoice doesn't exist, initialize it.
	if (sale == null)
	{
		sale = new Sales();
		sale.SaleDate = DateTime.Now; // Set the current date for new invoices.
		sale.SaleDetails = new();
	}
	//else
	//{
	//This is for refund after sales probably........not coded yet.
	//// Update the date for existing invoices.
	//invoice.InvoiceDate = invoiceView.InvoiceDate;
	//}
	// Map attributes from the view model to the data model.
	sale.EmployeeID = employeeID;
	sale.PaymentType = paymentType;
	sale.CouponID = matchingCoupon == null ? null : matchingCoupon.CouponID == null ? null : matchingCoupon.CouponID;

	sale.SubTotal = shoppingListItems.Sum(a => a.Subtotal);
	sale.TaxAmount = sale.SubTotal * 0.05m;

	decimal Total = sale.SubTotal + sale.TaxAmount - discount; //this is for UI
	Console.WriteLine($"{Total} is total price."); //just for checking

	// Process each line item in the provided view model.

//Discount(when it is not null) also needs to be applied for the object before save changes.
//or??Discount is only used for UI to show total, and sale object doesn't even have 'total' property(field). so maybe that's all for discount to do. needs to think about this.

//StockItem should be changed before save changes. Not coded yet.
//StockItem should be changed before save changes. Not coded yet.
//StockItem should be changed before save changes. Not coded yet.
//StockItem should be changed before save changes. Not coded yet.
//StockItem should be changed before save changes. Not coded yet.
//StockItem should be changed before save changes. Not coded yet.


	foreach (var item in shoppingListItems)
	{
		SaleDetails saleDetails = new();
		saleDetails.StockItemID = item.ProductID;
		saleDetails.SellingPrice = item.UnitPrice;
		saleDetails.Quantity = item.Quantity;
		sale.SaleDetails.Add(saleDetails);
	}

		Sales.Add(sale);
		
		SaveChanges();
	
	return sale.SaleID;
	
	
}



#endregion

// This region includes the view models used to
// represent and structure data for the UI.
#region View Models

public class CategoryView
{
	//public int CategoryID { get; set; }
	public string Description { get; set; }
	public int NumberOfItems { get; set; }
	//public bool RemoveFromViewFlag { get; set; }
	//public List<StockItemView> ItemList { get; set; } = new();
	//public int CountOfList => ItemList.Count;
}

public class ShoppingItemsView
{
	public int ProductID { get; set; }
	public string ProductName { get; set; }
	public int Quantity { get; set; } = 1;
	public decimal UnitPrice { get; set; }
	public decimal Subtotal => UnitPrice * Quantity;
}

private List<ShoppingItemsView> shoppingListItems = new List<ShoppingItemsView>();
//see category
//select a category
//see items
//select certain item and add -> leads to shopping list



public class StockItemView
{
	public int StockItemID { get; set; }
	public string Description { get; set; }
	public decimal SellingPrice { get; set; }
	//public decimal PurchasePrice { get; set; }
	//public int QuantityOnHand { get; set; }
	//public int QuantityOnOrder { get; set; }
	//public int ReOrderLevel { get; set; }
	//public bool Discontinued { get; set; }
	//public int VendorID { get; set; }
	//public string VendorStockNumber { get; set; }
	//public int CategoryID { get; set; }
	//public bool RemoveFromViewFlag { get; set; }
}


public class SalesView
{

	public int SaleID { get; set; }
	public DateTime SaleDate { get; set; }
	public string PaymentType { get; set; }
	public int EmployeeID { get; set; }
	public decimal TaxAmount { get; set; }
	public decimal SubTotal { get; set; }
	public int CouponID { get; set; }

}

#endregion
