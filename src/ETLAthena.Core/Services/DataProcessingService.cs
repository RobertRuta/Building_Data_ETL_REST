using ETLAthena.Core.Models;
using ETLAthena.Core.Services.Validation;
using ETLAthena.Core.Services.Transformation;
using ETLAthena.Core.Services.Merging;
using ETLAthena.Core.DataStorage;

namespace ETLAthena.Core.Services
{
    public class DataProcessingService : IDataProcessingService
    {
        private readonly IDataStorageService _dataStorageService;
        private readonly IS1Validator _s1Validator;
        private readonly IS2Validator _s2Validator;
        private readonly IS1Transformer _s1Transformer;
        private readonly IS2Transformer _s2Transformer;
        private readonly IMerger _merger;

        public DataProcessingService(IS1Validator s1Validator, IS2Validator s2Validator, IS1Transformer s1Transformer, IS2Transformer s2Transformer, IMerger merger)
        {
            _s1Validator = s1Validator;
            _s2Validator = s2Validator;
            _s1Transformer = s1Transformer;
            _s2Transformer = s2Transformer;
            _merger = merger;
        }

        public void ProcessDataFromSourceS1(S1Model data)
        {
            if (_s1Validator.Validate(data))
            {
                var transformedData = _s1Transformer.Transform(data);
                _merger.Merge(transformedData);
            }
        }

        public void ProcessDataFromSourceS2(S2Model data)
        {
            if (_s2Validator.Validate(data))
            {
                var transformedData = _s2Transformer.Transform(data);
                _merger.Merge(transformedData);
            }
        }
    }
}