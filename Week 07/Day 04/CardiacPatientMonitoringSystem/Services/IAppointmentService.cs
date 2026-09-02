using CardiacPatientMonitoringSystem.DTOs.Requests;
using CardiacPatientMonitoringSystem.DTOs.Responses;

namespace CardiacPatientMonitoringSystem.Services;

public interface IAppointmentService
{
    Task<AppointmentResponse?> GetByIdAsync(
        int appointmentId,
        string userId);

    Task<IEnumerable<AppointmentResponse>> GetMyAppointmentsAsync(
        string userId);

    Task<AppointmentResponse?> CreateAsync(
        CreateAppointmentRequest request,
        string userId);

    Task<AppointmentResponse?> UpdateAsync(
        int appointmentId,
        UpdateAppointmentRequest request,
        string userId);

    Task<bool> DeleteAsync(
        int appointmentId,
        string userId);
}