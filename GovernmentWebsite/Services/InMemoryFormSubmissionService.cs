using System.Collections.Concurrent;
using GovernmentWebsite.Contracts;

namespace GovernmentWebsite.Services;

public interface IFormSubmissionService
{
    Task<FormSubmissionResponse> SubmitAsync(SubmitFormRequest request, CancellationToken ct = default);
    Task<FormSubmissionResponse?> GetByReferenceCodeAsync(string referenceCode, CancellationToken ct = default);
}

public sealed class InMemoryFormSubmissionService : IFormSubmissionService
{
    private static readonly ConcurrentDictionary<string, FormSubmissionResponse> Submissions = new();

    public Task<FormSubmissionResponse> SubmitAsync(SubmitFormRequest request, CancellationToken ct = default)
    {
        var referenceCode = $"GOV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        var record = new FormSubmissionResponse(
            ReferenceCode: referenceCode,
            FormType: request.FormType.Trim(),
            ApplicantFullName: request.ApplicantFullName.Trim(),
            Status: "Submitted - Pending Review",
            SubmittedAt: DateTimeOffset.UtcNow
        );

        Submissions[referenceCode] = record;
        return Task.FromResult(record);
    }

    public Task<FormSubmissionResponse?> GetByReferenceCodeAsync(string referenceCode, CancellationToken ct = default)
    {
        Submissions.TryGetValue(referenceCode.Trim().ToUpperInvariant(), out var record);
        return Task.FromResult(record);
    }
}