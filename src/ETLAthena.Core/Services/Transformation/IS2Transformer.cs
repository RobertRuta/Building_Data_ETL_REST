using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Transformation
{
    public interface IS2Transformer
    {
        BuildingModel Transform(S2Model data);
        List<BuildingModel> Transform(List<S2Model> data);
    }
}