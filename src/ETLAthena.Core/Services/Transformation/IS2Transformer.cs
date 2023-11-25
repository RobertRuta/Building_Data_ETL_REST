using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Transformation
{
    public interface IS2Transformer
    {
        BuildingModel Transform(S2Model data);
    }
}