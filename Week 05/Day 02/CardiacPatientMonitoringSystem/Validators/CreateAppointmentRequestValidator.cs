using FluentValidation;
using CardiacPatientMonitoringSystem.DTOs.Requests;

namespace CardiacPatientMonitoringSystem.Validators;

public class CreateAppointmentRequestValidator
    : AbstractValidator<CreateAppointmentRequest>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0)
            .WithMessage("PatientId must be greater than 0.");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty()
            .WithMessage("Appointment date is required.")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Appointment date must be in the future.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Appointment reason is required.");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Appointment status is required.");
    }
}