using System.Text.Json;
using GovernmentWebsite.Contracts;
using GovernmentWebsite.Data;
using GovernmentWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace GovernmentWebsite.Services;

public sealed class DbFormSubmissionService : IFormSubmissionService
{
    private readonly ApplicationDbContext _db;

    public DbFormSubmissionService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<FormSubmissionResponse> SubmitAsync(SubmitFormRequest request, CancellationToken ct = default)
    {
        var referenceCode = $"GOV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";

        var submission = new FormSubmission
        {
            ReferenceCode = referenceCode,
            FormType = request.FormType.Trim(),
            ApplicantFullName = request.ApplicantFullName.Trim(),
            Email = request.Email.Trim(),
            ContactNumber = request.ContactNumber?.Trim(),
            Status = "Submitted - Pending Review",
            SubmittedAt = DateTimeOffset.UtcNow,
            FieldsJson = JsonSerializer.Serialize(request.Fields)
        };

        _db.FormSubmissions.Add(submission);
        await _db.SaveChangesAsync(ct);

        return new FormSubmissionResponse(
            ReferenceCode: submission.ReferenceCode,
            FormType: submission.FormType,
            ApplicantFullName: submission.ApplicantFullName,
            Status: submission.Status,
            SubmittedAt: submission.SubmittedAt
        );
    }

    public async Task<FormSubmissionResponse?> GetByReferenceCodeAsync(string referenceCode, CancellationToken ct = default)
    {
        var submission = await _db.FormSubmissions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ReferenceCode == referenceCode.Trim().ToUpper(), ct);

        if (submission is null)
            return null;

        return new FormSubmissionResponse(
            ReferenceCode: submission.ReferenceCode,
            FormType: submission.FormType,
            ApplicantFullName: submission.ApplicantFullName,
            Status: submission.Status,
            SubmittedAt: submission.SubmittedAt
        );
    }
}
