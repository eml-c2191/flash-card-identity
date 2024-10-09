using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace Identity.API.Models.Validations
{
    public class ValidationError
    {
        public string Field { get; }
        public string Message { get; }
        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }
    }
    public class ValidationResultModel
    {
        public string Message { get; }
        public IEnumerable<ValidationError> Errors { get; }
        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation Request Input Failed";

            Errors = modelState?.Keys.SelectMany(selector: key
                        => modelState.GetValueOrDefault(key)?
                            .Errors.Select(x => new ValidationError(key, x.ErrorMessage))
                            ?? Enumerable.Empty<ValidationError>())
                    ?? Enumerable.Empty<ValidationError>();
        }
    }
}
