using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Validation 
{
    public interface IS2Validator
    {
        bool Validate(S2Model data);
    }
}