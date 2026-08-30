using FluentValidation;
using CardiacPatientMonitoringSystem.DTOs.Requests;

namespace CardiacPatientMonitoringSystem.Validators;

public class CreateVitalSignRequestValidator
    : AbstractValidator<CreateVitalSignRequest>
{
    public CreateVitalSignRequestValidator()
    {
        RuleFor(x => x.HeartRate)
            .InclusiveBetween(30, 220)
            .WithMessage("Heart rate must be between 30 and 220 bpm.");

        RuleFor(x => x.SystolicBloodPressure)
            .InclusiveBetween(60, 250)
            .WithMessage("Systolic blood pressure must be between 60 and 250 mmHg.");

        RuleFor(x => x.DiastolicBloodPressure)
            .InclusiveBetween(30, 150)
            .WithMessage("Diastolic blood pressure must be between 30 and 150 mmHg.");

        RuleFor(x => x.DiastolicBloodPressure)
            .LessThan(x => x.SystolicBloodPressure)
            .WithMessage("Diastolic blood pressure must be lower than systolic blood pressure.");

        RuleFor(x => x.Temperature)
            .InclusiveBetween(30m, 45m)
            .WithMessage("Temperature must be between 30 and 45 °C.");

        RuleFor(x => x.OxygenSaturation)
            .InclusiveBetween(50, 100)
            .WithMessage("Oxygen saturation must be between 50% and 100%.");

        RuleFor(x => x.RecordedAt)
            .NotEmpty()
            .WithMessage("Recorded date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Recorded date cannot be in the future.");
    }
}