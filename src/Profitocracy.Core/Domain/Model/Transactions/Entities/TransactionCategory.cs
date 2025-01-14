using ExpencesTracker.Core.Domain.SharedKernel;

namespace ExpencesTracker.Core.Domain.Model.Transactions.Entities;

public class TransactionCategory(Guid categoryId) : Entity<Guid>(categoryId)
{
	public required string Name { get; set; }
}