using GovernmentWebsite.Contracts;
using GovernmentWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace GovernmentWebsite.Controllers;

[ApiController]
[Route("api/forms/submissions")]
public sealed class FormSubmissionsController(IFormSubmissionService submissionService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<FormSubmissionResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FormSubmissionResponse>> Create(
        [FromBody] SubmitFormRequest request,
        CancellationToken ct)
    {
        if (request.Fields.Count == 0)
        {
            ModelState.AddModelError(nameof(request.Fields), "At least one form field is required.");
            return ValidationProblem(ModelState);
        }

        FormSubmissionResponse created = await submissionService.SubmitAsync(request, ct);

        return CreatedAtAction(
            nameof(GetByReferenceCode),
            new { referenceCode = created.ReferenceCode },
            created);
    }

    [HttpGet("{referenceCode}")]
    [ProducesResponseType<FormSubmissionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormSubmissionResponse>> GetByReferenceCode(
        string referenceCode,
        CancellationToken ct)
    {
        FormSubmissionResponse? submission = await submissionService.GetByReferenceCodeAsync(referenceCode, ct);
        return submission is null ? NotFound() : Ok(submission);
    }
}