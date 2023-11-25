using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Transformation
{
    public interface IS1Transformer
    {
        BuildingModel Transform(S1Model data);
        List<BuildingModel> Transform(List<S1Model> data);
    }
}