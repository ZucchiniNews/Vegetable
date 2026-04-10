namespace Domain.Interfaces;

public interface IHistoryRecord
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
}