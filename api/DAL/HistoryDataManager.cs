namespace api.DAL
{
    public class HistoryDataManager
    {
        private readonly TableStorageDataManager _tableStorageDataManager;

        public HistoryDataManager(TableStorageDataManager tableStorageDataManager)
        {
            _tableStorageDataManager = new TableStorageDataManager("History");
        }
    }
}