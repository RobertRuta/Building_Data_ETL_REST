using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services
{
    public interface IDataIngestionService
    {
        S1Model IngestDataFromSourceS1(string jsonData);
        S2Model IngestDataFromSourceS2(string jsonData);
        List<S1Model> IngestBulkDataFromSourceS1(string jsonData);
        List<S2Model> IngestBulkDataFromSourceS2(string jsonData);
    }
}