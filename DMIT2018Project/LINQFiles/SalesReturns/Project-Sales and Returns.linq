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


	TestAddToShoppingLists(34).Dump();
	TestAddToShoppingLists(5580).Dump();
	TestAddToShoppingLists(5580).Dump("Duplicate Item Quantity +1");
	TestAddToShoppingLists(5580).Dump("can't order more than the quantity on hand"); //quantity on hand for id5580 is 2 in the database.
	TestAddToShoppingLists(24).Dump("No such product in the database");


	//(Required)SaveSales

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

public List<ShoppingItemsView> TestAddToShoppingLists(int productID)
{
	try
	{
		return AddToShoppingLists(productID);
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


public List<ShoppingItemsView> AddToShoppingLists(int productID)
{

	if (productID == null)
	{
		throw new ArgumentNullException("Product ID should be supplied");
	}

	if (productID <= 0)
	{
		throw new ArgumentException("Product ID should be greater than 0");
	}

	var productInDatabase = StockItems.Where(x => x.StockItemID == productID).FirstOrDefault();

	if (productInDatabase == null)
	{
		throw new ArgumentException("There is no such item in the database.");
	}


	var existingItems = shoppingListItems.Where(x => x.ProductID == productID).FirstOrDefault();
	if (existingItems != null)
	{
		if (existingItems.Quantity == productInDatabase.QuantityOnHand)
		{
			throw new ArgumentException("You can't order more than the quantity on hand.");
		}
		
		Console.WriteLine("Duplicate item is already in the cart! Quantity 1 added to the existing Item");
		existingItems.Quantity += 1;

	}
	else
	{
		shoppingListItems.Add(new ShoppingItemsView
		{
			ProductID = productID,
			ProductName = productInDatabase.Description,
			Quantity = productInDatabase.QuantityOnHand > 0 ? 1 : throw new ArgumentException("We don't have this item in the inventory"),
			UnitPrice = productInDatabase.SellingPrice,
		});
	}
	return shoppingListItems;

}


//public List<ShoppingItemsView> AddorEditShoppingLists (int quantity)





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


#endregion
