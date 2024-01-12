
using FluentValidation.Results;
using System.Collections.Concurrent;

namespace Ordering.Application.Exeptions
{
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; set; }

        public ValidationException() : base("Validation failed. One or more validation failures have occurred.")
        {
            Errors = new ConcurrentDictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            PopulateErrors(failures);
        }

        private void PopulateErrors(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures
                .GroupBy(failure => failure.PropertyName, failure => failure.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
