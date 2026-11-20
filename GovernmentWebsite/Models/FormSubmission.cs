using System.ComponentModel.DataAnnotations;

namespace GovernmentWebsite.Models;

public class FormSubmission
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string ReferenceCode { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string FormType { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string ApplicantFullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(20)]
    public string? ContactNumber { get; set; }

    [Required, StringLength(50)]
    public string Status { get; set; } = "Submitted - Pending Review";

    public DateTimeOffset SubmittedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }

    [StringLength(500)]
    public string? ReviewNotes { get; set; }

    // Store dynamic fields as JSON
    public string FieldsJson { get; set; } = "{}";
}
