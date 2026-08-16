using FluentValidation;
using CardiacPatientMonitoringSystem.DTOs.Requests;

namespace CardiacPatientMonitoringSystem.Validators;

public class CreateVitalSignRequestValidator
    : AbstractValidator<CreateVitalSignRequest>
{
    public CreateVitalSignRequestValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage("PatientId must be greater than 0.");

        RuleFor(x => x.HeartRate)
            .InclusiveBetween(30, 250)
            .WithMessage("Heart rate must be between 30 and 250.");

        RuleFor(x => x.SystolicBloodPressure)
            .InclusiveBetween(50, 250)
            .WithMessage("Systolic blood pressure must be between 50 and 250.");

        RuleFor(x => x.DiastolicBloodPressure)
            .InclusiveBetween(30, 150)
            .WithMessage("Diastolic blood pressure must be between 30 and 150.")
            .LessThan(x => x.SystolicBloodPressure)
            .WithMessage("Diastolic blood pressure must be lower than systolic blood pressure.");

        RuleFor(x => x.Temperature)
            .InclusiveBetween(30m, 45m)
            .WithMessage("Temperature must be between 30 and 45.");

        RuleFor(x => x.OxygenSaturation)
            .InclusiveBetween(50, 100)
            .WithMessage("Oxygen saturation must be between 50 and 100.");

        RuleFor(x => x.RecordedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("RecordedAt cannot be in the future.");
    }
}