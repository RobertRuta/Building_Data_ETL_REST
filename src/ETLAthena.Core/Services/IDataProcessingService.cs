using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services
{
    public interface IDataProcessingService
    {
        void ProcessDataFromSourceS1(S1Model data);
        void ProcessDataFromSourceS2(S2Model data);
    }
}