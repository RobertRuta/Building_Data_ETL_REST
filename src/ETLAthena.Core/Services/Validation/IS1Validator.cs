using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Validation
{
    public interface IS1Validator
    {
        bool Validate(S1Model data);
    }   
}