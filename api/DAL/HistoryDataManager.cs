using api.models.entities;

namespace api.DAL
{
    public class HistoryDataManager
    {
        private readonly TableStorageDataManager<HistoryEntity> _tableStorageDataManager;

        public HistoryDataManager()
        {
            _tableStorageDataManager = new TableStorageDataManager<HistoryEntity>("History");
        }
    }
}