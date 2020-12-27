
using Microsoft.WindowsAzure.Storage.Table;

namespace api.models.entities
{
    public class HistoryEntity : TableEntity
    {
        public HistoryEntity()
        {
        }

        public HistoryEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }
    }
}