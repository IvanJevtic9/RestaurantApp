using System.Collections.Generic;

namespace RestaurantApp.Core.Model
{
    public class OperationResult
    {
        public bool Succeeded { get; set; }
        public List<OperationError> Errors { get; }

        public OperationResult()
        {
            Errors = new List<OperationError>();
        }
        public Dictionary<string, List<string>> TransformToDict()
        {
            var dict = new Dictionary<string, List<string>>();

            foreach (var error in Errors)
            {
                if (!dict.ContainsKey(error.PropertyKey)) dict.Add(error.PropertyKey, new List<string>() { error.Message });
                else dict[error.PropertyKey].Add(error.Message);
            }

            return dict;
        }

        public void AddError(string key, string message)
        {
            Errors.Add(new OperationError() { PropertyKey = key, Message = message });
        }
    }

    public class OperationError
    {
        public string PropertyKey { get; set; }
        public string Message { get; set; }
    }
}
