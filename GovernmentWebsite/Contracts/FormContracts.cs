using System.ComponentModel.DataAnnotations;

namespace GovernmentWebsite.Contracts;

public sealed record SubmitFormRequest(
    [Required, StringLength(60)] string FormType,
    [Required, StringLength(150)] string ApplicantFullName,
    [Required, EmailAddress] string Email,
    [Phone] string? ContactNumber,
    [Required] Dictionary<string, string> Fields
);

public sealed record UpdateFormRequest(
    [Required, StringLength(150)] string ApplicantFullName,
    [Required, EmailAddress] string Email,
    [Phone] string? ContactNumber,
    [Required] Dictionary<string, string> Fields
);

public sealed record AdminStatusUpdateRequest(
    [Required, StringLength(50)] string Status,
    [StringLength(500)] string? ReviewNotes
);

public sealed record FormSubmissionResponse(
    string ReferenceCode,
    string FormType,
    string ApplicantFullName,
    string Status,
    DateTimeOffset SubmittedAt
);

public sealed record FormSubmissionDetailResponse(
    int Id,
    string ReferenceCode,
    string FormType,
    string ApplicantFullName,
    string Email,
    string? ContactNumber,
    string Status,
    DateTimeOffset SubmittedAt,
    DateTimeOffset? UpdatedAt,
    string? ReviewNotes,
    Dictionary<string, string> Fields
);
