using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Validation
{
    public class S1Validator : IS1Validator
    {
        public bool Validate(S1Model data)
        {
            if (data == null) return false;
            if (data.Id <= 0) return false;
            if (string.IsNullOrWhiteSpace(data.Address1)) return false;

            return true;
        }
    }
}