using SQLite;

namespace ExpencesTracker.Infrastructure.Persistence.Sqlite.Models.Transaction;

/// <summary>
/// Persistence representation for
/// <see cref="ExpencesTracker.Core.Domain.Model.Transactions.Transaction"/> domain model 
/// </summary>
internal class TransactionModel
{
	[PrimaryKey]
	public Guid Id { get; set; }
	public decimal Amount { get; set; }
	public Guid ProfileId { get; set; }
	public short Type { get; set; }
	public short? SpendingType { get; set; }
	public DateTime Timestamp { get; set; }
	public string? Description { get; set; }
	public double? GeoTagLongitude { get; set; }
	public double? GeoTagLatitude { get; set; }
	public Guid? CategoryId { get; set; }
	public string? CategoryName { get; set; }
}