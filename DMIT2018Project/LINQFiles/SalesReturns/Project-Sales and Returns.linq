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

	//Required Methods
	//GetCategories
	TestGetCategories().Dump();     //maybe for onintialized?


	//GetItemsByCategoryID
	TestGetItemsByCategoryID(-3).Dump();   //users or employees selecting certain category for display
	TestGetItemsByCategoryID(0).Dump();   //users or employees selecting certain category for display
	TestGetItemsByCategoryID(4).Dump();   //users or employees selecting certain category for display


	//SaveSales
	//GetSaleRefund
	//and SaveRefund



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
						CategoryID = x.CategoryID,
						Description = x.Description,
						ItemList = x.StockItems
												.Where(x => x.Discontinued == false)
												.Select(a => new StockItemView
												{
													StockItemID = a.StockItemID,
													Description = a.Description
												}).ToList()

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




#endregion

// This region includes the view models used to
// represent and structure data for the UI.
#region View Models

public class CategoryView
{
	public int CategoryID { get; set; }
	public string Description { get; set; }
	public bool RemoveFromViewFlag { get; set; }
	public List<StockItemView> ItemList { get; set; } = new();
	public int CountOfList => ItemList.Count;

}

public class StockItemView
{
	public int StockItemID { get; set; }
	public string Description { get; set; }
	public decimal SellingPrice { get; set; }
	public decimal PurchasePrice { get; set; }
	public int QuantityOnHand { get; set; }
	public int QuantityOnOrder { get; set; }
	public int ReOrderLevel { get; set; }
	public bool Discontinued { get; set; }
	public int VendorID { get; set; }
	public string VendorStockNumber { get; set; }
	public int CategoryID { get; set; }
	public bool RemoveFromViewFlag { get; set; }
}


#endregion
