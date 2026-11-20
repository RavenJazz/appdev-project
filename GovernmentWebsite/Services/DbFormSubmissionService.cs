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

    public async Task<IEnumerable<FormSubmissionDetailResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var submissions = await _db.FormSubmissions
            .AsNoTracking()
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync(ct);

        return submissions.Select(MapToDetailResponse);
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
            UpdatedAt = null,
            ReviewNotes = null,
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

    public async Task<FormSubmissionDetailResponse?> GetDetailByReferenceCodeAsync(string referenceCode, CancellationToken ct = default)
    {
        var submission = await _db.FormSubmissions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ReferenceCode == referenceCode.Trim().ToUpper(), ct);

        return submission is null ? null : MapToDetailResponse(submission);
    }

    public async Task<FormSubmissionDetailResponse?> UpdateAsync(string referenceCode, UpdateFormRequest request, CancellationToken ct = default)
    {
        var submission = await _db.FormSubmissions
            .FirstOrDefaultAsync(s => s.ReferenceCode == referenceCode.Trim().ToUpper(), ct);

        if (submission is null)
            return null;

        submission.ApplicantFullName = request.ApplicantFullName.Trim();
        submission.Email = request.Email.Trim();
        submission.ContactNumber = request.ContactNumber?.Trim();
        submission.FieldsJson = JsonSerializer.Serialize(request.Fields);
        submission.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(ct);

        return MapToDetailResponse(submission);
    }

    public async Task<FormSubmissionDetailResponse?> AdminUpdateStatusAsync(string referenceCode, AdminStatusUpdateRequest request, CancellationToken ct = default)
    {
        var submission = await _db.FormSubmissions
            .FirstOrDefaultAsync(s => s.ReferenceCode == referenceCode.Trim().ToUpper(), ct);

        if (submission is null)
            return null;

        submission.Status = request.Status.Trim();
        submission.ReviewNotes = request.ReviewNotes?.Trim();
        submission.UpdatedAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(ct);

        return MapToDetailResponse(submission);
    }

    public async Task<bool> DeleteAsync(string referenceCode, CancellationToken ct = default)
    {
        var submission = await _db.FormSubmissions
            .FirstOrDefaultAsync(s => s.ReferenceCode == referenceCode.Trim().ToUpper(), ct);

        if (submission is null)
            return false;

        _db.FormSubmissions.Remove(submission);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private static FormSubmissionDetailResponse MapToDetailResponse(FormSubmission submission)
    {
        Dictionary<string, string> fields;
        try
        {
            fields = JsonSerializer.Deserialize<Dictionary<string, string>>(submission.FieldsJson) ?? new();
        }
        catch
        {
            fields = new();
        }

        return new FormSubmissionDetailResponse(
            Id: submission.Id,
            ReferenceCode: submission.ReferenceCode,
            FormType: submission.FormType,
            ApplicantFullName: submission.ApplicantFullName,
            Email: submission.Email,
            ContactNumber: submission.ContactNumber,
            Status: submission.Status,
            SubmittedAt: submission.SubmittedAt,
            UpdatedAt: submission.UpdatedAt,
            ReviewNotes: submission.ReviewNotes,
            Fields: fields
        );
    }
}
