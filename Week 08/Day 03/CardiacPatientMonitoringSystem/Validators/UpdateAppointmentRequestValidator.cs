using FluentValidation;
using CardiacPatientMonitoringSystem.DTOs.Requests;

namespace CardiacPatientMonitoringSystem.Validators;

public class UpdateAppointmentRequestValidator
    : AbstractValidator<UpdateAppointmentRequest>
{
    public UpdateAppointmentRequestValidator()
    {
        RuleFor(x => x.AppointmentDate)
            .NotEmpty()
            .WithMessage("Appointment date is required.")
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Appointment date must be in the future.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Appointment reason is required.")
            .MaximumLength(500)
            .WithMessage("Appointment reason cannot exceed 500 characters.");
    }
}