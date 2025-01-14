using ExpencesTracker.Core.Domain.SharedKernel;

namespace ExpencesTracker.Core.Domain.Model.Categories;

public class Category(Guid id): AggregateRoot<Guid>(id)
{
	public required Guid ProfileId { get; set; }
	public required string Name { get; set; }
	public decimal? PlannedAmount { get; set; }
}