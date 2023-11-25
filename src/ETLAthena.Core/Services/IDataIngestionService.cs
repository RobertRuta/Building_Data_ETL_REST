namespace ETLAthena.Core.Services
{
    public interface IDataIngestionService
    {
        void IngestDataFromSourceS1(string jsonData);
        void IngestDataFromSourceS2(string jsonData);
        void IngestBulkDataFromSourceS1(string jsonData);
        void IngestBulkDataFromSourceS2(string jsonData);
    }
}