using FluentValidation;

namespace CleanArchitecture.Application.Api.Samples.Queries;

/// <summary>Validates <see cref="GetSampleCacheQuery"/>.</summary>
public sealed class GetSampleCacheQueryValidator : AbstractValidator<GetSampleCacheQuery>
{
    public GetSampleCacheQueryValidator()
    {
        RuleFor(query => query.Key)
            .NotEmpty()
            .WithMessage("A cache key must be provided.")
            .MaximumLength(128)
            .WithMessage("The cache key cannot exceed 128 characters.");
    }
}
