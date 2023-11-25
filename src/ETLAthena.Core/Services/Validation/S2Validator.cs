using ETLAthena.Core.Models;

namespace ETLAthena.Core.Services.Validation
{
    public class S2Validator : IS2Validator
    {
        public bool Validate(S2Model data)
        {
            // Example validation logic for S2 data
            if (data == null) return false;
            if (data.Id <= 0) return false;
            if (string.IsNullOrWhiteSpace(data.Name)) return false;
            if (data.Coordinates == null || data.Coordinates.Length != 2) return false;

            return true;
        }
    }
}