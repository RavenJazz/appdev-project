using GovernmentWebsite.Contracts;
using GovernmentWebsite.Services;
using Microsoft.AspNetCore.Mvc;

namespace GovernmentWebsite.Controllers;

[ApiController]
[Route("api/forms/submissions")]
public sealed class FormSubmissionsController(IFormSubmissionService submissionService) : ControllerBase
{
    // GET: api/forms/submissions (List all submissions - Admin)
    [HttpGet]
    [ProducesResponseType<IEnumerable<FormSubmissionDetailResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FormSubmissionDetailResponse>>> GetAll(CancellationToken ct)
    {
        var submissions = await submissionService.GetAllAsync(ct);
        return Ok(submissions);
    }

    // POST: api/forms/submissions (Create submission - Citizen)
    [HttpPost]
    [ProducesResponseType<FormSubmissionResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FormSubmissionResponse>> Create(
        [FromBody] SubmitFormRequest request,
        CancellationToken ct)
    {
        if (request.Fields == null || request.Fields.Count == 0)
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

    // GET: api/forms/submissions/{referenceCode} (Read one submission)
    [HttpGet("{referenceCode}")]
    [ProducesResponseType<FormSubmissionDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormSubmissionDetailResponse>> GetByReferenceCode(
        string referenceCode,
        CancellationToken ct)
    {
        FormSubmissionDetailResponse? submission = await submissionService.GetDetailByReferenceCodeAsync(referenceCode, ct);
        return submission is null ? NotFound(new { message = $"Submission '{referenceCode}' not found." }) : Ok(submission);
    }

    // PUT: api/forms/submissions/{referenceCode} (Update details - Citizen)
    [HttpPut("{referenceCode}")]
    [ProducesResponseType<FormSubmissionDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormSubmissionDetailResponse>> Update(
        string referenceCode,
        [FromBody] UpdateFormRequest request,
        CancellationToken ct)
    {
        if (request.Fields == null || request.Fields.Count == 0)
        {
            ModelState.AddModelError(nameof(request.Fields), "At least one form field is required.");
            return ValidationProblem(ModelState);
        }

        FormSubmissionDetailResponse? updated = await submissionService.UpdateAsync(referenceCode, request, ct);
        return updated is null ? NotFound(new { message = $"Submission '{referenceCode}' not found." }) : Ok(updated);
    }

    // PATCH: api/forms/submissions/{referenceCode}/status (Update status & notes - Admin)
    [HttpPatch("{referenceCode}/status")]
    [ProducesResponseType<FormSubmissionDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormSubmissionDetailResponse>> UpdateStatus(
        string referenceCode,
        [FromBody] AdminStatusUpdateRequest request,
        CancellationToken ct)
    {
        FormSubmissionDetailResponse? updated = await submissionService.AdminUpdateStatusAsync(referenceCode, request, ct);
        return updated is null ? NotFound(new { message = $"Submission '{referenceCode}' not found." }) : Ok(updated);
    }

    // DELETE: api/forms/submissions/{referenceCode} (Delete submission)
    [HttpDelete("{referenceCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        string referenceCode,
        CancellationToken ct)
    {
        bool deleted = await submissionService.DeleteAsync(referenceCode, ct);
        return deleted ? NoContent() : NotFound(new { message = $"Submission '{referenceCode}' not found." });
    }
}
