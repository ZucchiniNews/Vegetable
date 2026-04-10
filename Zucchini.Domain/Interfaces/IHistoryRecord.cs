namespace Domain.Interfaces;

public interface IHistoryRecord
{
    string PartitionKey { get; set; }
    string RowKey { get; set; }
}