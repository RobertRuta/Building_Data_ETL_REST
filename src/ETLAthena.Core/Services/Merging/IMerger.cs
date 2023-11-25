using ETLAthena.Core.Models;
namespace ETLAthena.Core.Services.Merging
{
    public interface IMerger
    {
        void Merge(BuildingModel data);
    }
}