using FluentValidation;
using CardiacPatientMonitoringSystem.DTOs.Requests;

namespace CardiacPatientMonitoringSystem.Validators;

public class CreateMedicationRequestValidator
    : AbstractValidator<CreateMedicationRequest>
{
    public CreateMedicationRequestValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage("PatientId must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Medication name is required.");

        RuleFor(x => x.Dosage)
            .NotEmpty()
            .WithMessage("Dosage is required.");

        RuleFor(x => x.Frequency)
            .NotEmpty()
            .WithMessage("Frequency is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("End date must be on or after the start date.");
    }
}