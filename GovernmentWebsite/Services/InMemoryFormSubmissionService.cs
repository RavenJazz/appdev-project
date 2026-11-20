using System.Collections.Concurrent;
using System.Text.Json;
using GovernmentWebsite.Contracts;

namespace GovernmentWebsite.Services;

public sealed class InMemoryFormSubmissionService : IFormSubmissionService
{
    private static readonly ConcurrentDictionary<string, FormSubmissionDetailResponse> Submissions = new();

    public Task<IEnumerable<FormSubmissionDetailResponse>> GetAllAsync(CancellationToken ct = default)
    {
        return Task.FromResult<IEnumerable<FormSubmissionDetailResponse>>(Submissions.Values.OrderByDescending(s => s.SubmittedAt).ToList());
    }

    public Task<FormSubmissionResponse> SubmitAsync(SubmitFormRequest request, CancellationToken ct = default)
    {
        var referenceCode = $"GOV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        var detail = new FormSubmissionDetailResponse(
            Id: Submissions.Count + 1,
            ReferenceCode: referenceCode,
            FormType: request.FormType.Trim(),
            ApplicantFullName: request.ApplicantFullName.Trim(),
            Email: request.Email.Trim(),
            ContactNumber: request.ContactNumber?.Trim(),
            Status: "Submitted - Pending Review",
            SubmittedAt: DateTimeOffset.UtcNow,
            UpdatedAt: null,
            ReviewNotes: null,
            Fields: request.Fields
        );

        Submissions[referenceCode] = detail;

        return Task.FromResult(new FormSubmissionResponse(
            detail.ReferenceCode,
            detail.FormType,
            detail.ApplicantFullName,
            detail.Status,
            detail.SubmittedAt
        ));
    }

    public Task<FormSubmissionDetailResponse?> GetDetailByReferenceCodeAsync(string referenceCode, CancellationToken ct = default)
    {
        Submissions.TryGetValue(referenceCode.Trim().ToUpperInvariant(), out var record);
        return Task.FromResult(record);
    }

    public Task<FormSubmissionResponse?> GetByReferenceCodeAsync(string referenceCode, CancellationToken ct = default)
    {
        if (Submissions.TryGetValue(referenceCode.Trim().ToUpperInvariant(), out var record))
        {
            return Task.FromResult<FormSubmissionResponse?>(new FormSubmissionResponse(
                record.ReferenceCode,
                record.FormType,
                record.ApplicantFullName,
                record.Status,
                record.SubmittedAt
            ));
        }
        return Task.FromResult<FormSubmissionResponse?>(null);
    }

    public Task<FormSubmissionDetailResponse?> UpdateAsync(string referenceCode, UpdateFormRequest request, CancellationToken ct = default)
    {
        var key = referenceCode.Trim().ToUpperInvariant();
        if (Submissions.TryGetValue(key, out var existing))
        {
            var updated = existing with
            {
                ApplicantFullName = request.ApplicantFullName.Trim(),
                Email = request.Email.Trim(),
                ContactNumber = request.ContactNumber?.Trim(),
                Fields = request.Fields,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            Submissions[key] = updated;
            return Task.FromResult<FormSubmissionDetailResponse?>(updated);
        }
        return Task.FromResult<FormSubmissionDetailResponse?>(null);
    }

    public Task<FormSubmissionDetailResponse?> AdminUpdateStatusAsync(string referenceCode, AdminStatusUpdateRequest request, CancellationToken ct = default)
    {
        var key = referenceCode.Trim().ToUpperInvariant();
        if (Submissions.TryGetValue(key, out var existing))
        {
            var updated = existing with
            {
                Status = request.Status.Trim(),
                ReviewNotes = request.ReviewNotes?.Trim(),
                UpdatedAt = DateTimeOffset.UtcNow
            };
            Submissions[key] = updated;
            return Task.FromResult<FormSubmissionDetailResponse?>(updated);
        }
        return Task.FromResult<FormSubmissionDetailResponse?>(null);
    }

    public Task<bool> DeleteAsync(string referenceCode, CancellationToken ct = default)
    {
        return Task.FromResult(Submissions.TryRemove(referenceCode.Trim().ToUpperInvariant(), out _));
    }
}
