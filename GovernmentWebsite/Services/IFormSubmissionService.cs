using GovernmentWebsite.Contracts;

namespace GovernmentWebsite.Services;

public interface IFormSubmissionService
{
    Task<IEnumerable<FormSubmissionDetailResponse>> GetAllAsync(CancellationToken ct = default);
    Task<FormSubmissionDetailResponse?> GetDetailByReferenceCodeAsync(string referenceCode, CancellationToken ct = default);
    Task<FormSubmissionResponse?> GetByReferenceCodeAsync(string referenceCode, CancellationToken ct = default);
    Task<FormSubmissionResponse> SubmitAsync(SubmitFormRequest request, CancellationToken ct = default);
    Task<FormSubmissionDetailResponse?> UpdateAsync(string referenceCode, UpdateFormRequest request, CancellationToken ct = default);
    Task<FormSubmissionDetailResponse?> AdminUpdateStatusAsync(string referenceCode, AdminStatusUpdateRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(string referenceCode, CancellationToken ct = default);
}
