using FluentValidation;
using MediatR;
using MS.IConstruye.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MS.IConstruye.Application
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest>[] _validators;
        public ValidatorBehavior(IValidator<TRequest>[] validators) => _validators = validators;
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                string errors = string.Join(",", failures.Select(x => x.ErrorMessage).ToArray());
                throw new IConstruyeBaseException(
                    $"Comando {typeof(TRequest).Name} => {errors}", new ValidationException("Excepción lanzada por validaciones", failures));
            }
            var response = await next();
            return response;
        }
    }
}
