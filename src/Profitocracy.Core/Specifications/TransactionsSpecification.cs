using ExpencesTracker.Core.Domain.Model.Transactions.ValueObjects;

namespace ExpencesTracker.Core.Specifications;

/// <summary>
/// Represents the specification for filtering
/// transactions based on various criteria
/// </summary>
public record TransactionsSpecification
{
    public Guid? ProfileId { get; init; }
    public Guid? CategoryId { get; init; }
    public SpendingType? SpendingType { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
}