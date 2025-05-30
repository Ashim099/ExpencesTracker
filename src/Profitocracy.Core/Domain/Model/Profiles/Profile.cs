using ExpencesTracker.Core.Domain.Model.Profiles.Entities;
using ExpencesTracker.Core.Domain.Model.Profiles.ValueObjects;
using ExpencesTracker.Core.Domain.Model.Transactions;
using ExpencesTracker.Core.Domain.Model.Transactions.ValueObjects;
using ExpencesTracker.Core.Domain.SharedKernel;

namespace ExpencesTracker.Core.Domain.Model.Profiles;

/// <summary>
/// Profile aggregate root.
/// Contains data about settings of profile, balances
/// state and methods for calculation of transactions
/// </summary>
public class Profile : AggregateRoot<Guid>
{
	public Profile(
		Guid id,
		string name,
		AnchorDate startDate,
		decimal savedBalance,
		List<ProfileCategory> categoriesExpenses,
		ProfileSettings settings,
		bool isCurrent) : base(id)
	{
		Name = name;
		StartDate = startDate;
		Balance = StartDate.InitialBalance;
		SavedBalance = savedBalance;
		CategoriesExpenses = categoriesExpenses;
		Settings = settings;
		IsCurrent = isCurrent;
		Expenses = new ProfileExpenses
		{
			DailyFromActualBalance = new ProfileExpense
			{
				ActualAmount = 0,
				PlannedAmount = 0
			},
			DailyFromInitialBalance = new ProfileExpense
			{
				ActualAmount = 0,
				PlannedAmount = 0
			},
			Main = new ProfileExpense
			{
				ActualAmount = 0,
				PlannedAmount = 0
			},
			Secondary = new ProfileExpense
			{
				ActualAmount = 0,
				PlannedAmount = 0
			},
			Saved = new ProfileExpense
			{
				ActualAmount = 0,
				PlannedAmount = 0
			},
			TotalBalance = new ProfileExpense
			{
				ActualAmount = 0,
				PlannedAmount = 0
			}
		};

		BillingPeriod = new TimePeriod
		{
			DateFrom = new DateTime(
				StartDate.Timestamp.Year, 
				StartDate.Timestamp.Month, 
				day: 1,
				hour: 0,
				minute: 0,
				second: 0,
				millisecond: 0),
			DateTo = new DateTime(
				StartDate.Timestamp.Year,
				StartDate.Timestamp.Month,
				day: DateTime.DaysInMonth(StartDate.Timestamp.Year, StartDate.Timestamp.Month),
				hour: 23,
				minute: 59,
				second: 59,
				millisecond: 999)
		};

		_todayInitialBalance = Balance;
	}

	/// <summary>
	/// Is billing period of profile new
	/// </summary>
	public bool IsNewPeriod { get; private set; }
	
	/// <summary>
	/// Name of profile
	/// </summary>
	public string Name { get; }
	
	/// <summary>
	/// Initial balance from date when profile was created
	/// </summary>
	public AnchorDate StartDate { get; private set; }
	
	/// <summary>
	/// Start and end dates of billing period
	/// </summary>
	public TimePeriod BillingPeriod { get; private set; }
	
	/// <summary>
	/// Current balance
	/// </summary>
	public decimal Balance { get; private set; }
	
	/// <summary>
	/// Total saved amount
	/// </summary>
	public decimal SavedBalance { get; private set; }
	
	/// <summary>
	/// Calculations of expenses by types: main, secondary and saved
	/// </summary>
	public ProfileExpenses Expenses { get; }
	
	/// <summary>
	/// Result of expenses calculations for every category
	/// </summary>
	public List<ProfileCategory> CategoriesExpenses { get; }
	
	/// <summary>
	/// Profile settings
	/// </summary>
	public ProfileSettings Settings { get; }

	/// <summary>
	/// Is this profile currently using by user
	/// </summary>
	public bool IsCurrent { get; }

	/// <summary>
	/// This field is used to correctly calculate money for today.
	/// Without this field we will see decreasing amount of
	/// PlannedAmount of DailyFromActualBalance
	/// </summary>
	private decimal _todayInitialBalance;
	
	/// <summary>
	/// Process transaction and
	/// project results in profile
	/// </summary>
	/// <param name="transactions">Transactions to handle</param>
	/// <param name="currentDate">Current date</param>
	public void HandleTransactions(ICollection<Transaction> transactions, DateTime currentDate)
	{
		foreach (var transaction in transactions)
		{
			HandleTransaction(transaction, currentDate);
		}
		
		if (StartDate.Timestamp.Month != currentDate.Month)
		{
			IsNewPeriod = true;
			
			StartDate = new AnchorDate
			{
				InitialBalance = Balance,
				Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day)
			};
			
			BillingPeriod = new TimePeriod
			{
				DateFrom = new DateTime(currentDate.Year, currentDate.Month, 1),
				DateTo = new DateTime(
					currentDate.Year,
					currentDate.Month,
					day: DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
			};
		}
		
		RecalculateExpenses(currentDate);
	}
	
	/// <summary>
	/// Add profile categories to transaction processing
	/// </summary>
	/// <param name="categories">List of categories to add to profile</param>
	public void AddCategories(IEnumerable<ProfileCategory> categories)
	{
		CategoriesExpenses.AddRange(categories);
	}
	
	private void HandleTransaction(Transaction transaction, DateTime currentDate)
	{
		if (transaction.Type == TransactionType.Income)
		{
			HandleIncomeTransaction(transaction);
		}
		else
		{
			if (transaction.SpendingType == SpendingType.Saved && transaction.Timestamp < BillingPeriod.DateFrom)
			{
				HandleSavingSpendingTransaction(transaction, currentDate);
				return;
			}
			
			HandleExpenseTransaction(transaction, currentDate);
		}
	}
	
	private void RecalculateExpenses(DateTime currentDate)
	{
		var daysInInitialPeriod = BillingPeriod.DateTo.Day - BillingPeriod.DateFrom.Day + 1;
		var daysInActualPeriod = BillingPeriod.DateTo.Day - currentDate.Day + 1;
		
		// Used to prevent division by zero
		daysInInitialPeriod = daysInInitialPeriod == 0 ? 1 : daysInInitialPeriod;
		daysInActualPeriod = daysInActualPeriod == 0 ? 1 : daysInActualPeriod;

		Expenses.TotalBalance.PlannedAmount += StartDate.InitialBalance;
		Expenses.DailyFromActualBalance.PlannedAmount = _todayInitialBalance / daysInActualPeriod;
		Expenses.DailyFromInitialBalance.PlannedAmount = Expenses.TotalBalance.PlannedAmount / daysInInitialPeriod;
		
		Expenses.Main.PlannedAmount = Expenses.TotalBalance.PlannedAmount * 0.5m;
		Expenses.Secondary.PlannedAmount = Expenses.TotalBalance.PlannedAmount * 0.3m;
		Expenses.Saved.PlannedAmount = Expenses.TotalBalance.PlannedAmount * 0.2m;
	}
	
	private void HandleIncomeTransaction(Transaction transaction)
	{
		_todayInitialBalance += transaction.Amount;
		Balance += transaction.Amount;
		Expenses.TotalBalance.PlannedAmount += transaction.Amount;
	}

	private void HandleExpenseTransaction(Transaction transaction, DateTime currentDate)
	{
		Balance -= transaction.Amount;

		if (transaction.Timestamp.Day == currentDate.Day)
		{
			Expenses.DailyFromInitialBalance.ActualAmount += transaction.Amount;
			Expenses.DailyFromActualBalance.ActualAmount += transaction.Amount;
		}
		else
		{
			_todayInitialBalance -= transaction.Amount;
		}

		Expenses.TotalBalance.ActualAmount += transaction.Amount;
		
		switch (transaction.SpendingType)
		{
			case SpendingType.Main:
				HandleMainSpendingTransaction(transaction);
				break;
			case SpendingType.Secondary:
				HandleSecondarySpendingTransaction(transaction);
				break;
			case SpendingType.Saved:
				HandleSavingSpendingTransaction(transaction, currentDate);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(transaction.SpendingType));
		}

		if (transaction.Category is not null)
		{
			HandleCategoryTransaction(transaction);
		}
	}

	private void HandleMainSpendingTransaction(Transaction transaction)
	{
		Expenses.Main.ActualAmount += transaction.Amount;
	}
	
	private void HandleSecondarySpendingTransaction(Transaction transaction)
	{
		Expenses.Secondary.ActualAmount += transaction.Amount;
	}
	
	private void HandleSavingSpendingTransaction(Transaction transaction, DateTime currentDate)
	{
		if (transaction.Timestamp.Month == currentDate.Month)
		{
			Expenses.Saved.ActualAmount += transaction.Amount;	
		}
		
		SavedBalance += transaction.Amount;
	}

	private void HandleCategoryTransaction(Transaction transaction)
	{
		var category = CategoriesExpenses
			.Find(c => c.Id.Equals(transaction.Category!.Id));

		if (category is null)
		{
			return;
		}

		category.ActualAmount += transaction.Amount;
	}
}