namespace ETLAthena.Core.Services
{
    public interface IDataPullService
    {
        Task PullS1DataFromSourceAsync(string s1ApiEndpoint);
        Task PullS2DataFromSourceAsync(string s2ApiEndpoint);
    }    
}
