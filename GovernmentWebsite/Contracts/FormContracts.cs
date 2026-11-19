using System.ComponentModel.DataAnnotations;

namespace GovernmentWebsite.Contracts;

public sealed record SubmitFormRequest(
    [Required, StringLength(60)] string FormType,
    [Required, StringLength(150)] string ApplicantFullName,
    [Required, EmailAddress] string Email,
    [Phone] string? ContactNumber,
    [Required] Dictionary<string, string> Fields
);

public sealed record FormSubmissionResponse(
    string ReferenceCode,
    string FormType,
    string ApplicantFullName,
    string Status,
    DateTimeOffset SubmittedAt
);