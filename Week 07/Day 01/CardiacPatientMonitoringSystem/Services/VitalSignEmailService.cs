using CardiacPatientMonitoringSystem.DTOs.Responses;

namespace CardiacPatientMonitoringSystem.Services;

public class VitalSignEmailService : IVitalSignEmailService
{
    private readonly IEmailService _emailService;

    public VitalSignEmailService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendCriticalAlertAsync(
        string email,
        VitalSignAnalysisResponse analysis,
        bool isUpdate = false)
    {
        var alertsHtml = string.Join(
            "",
            analysis.Alerts.Select(alert =>
                $"<li style='margin-bottom: 8px;'>{alert}</li>")
        );

        var messageType = isUpdate
            ? "updated vital sign reading"
            : "latest vital sign reading";

        var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
</head>

<body style='margin:0; padding:0; background-color:#f4f6f8; font-family:Arial, sans-serif;'>

    <div style='max-width:600px; margin:40px auto; background:white; border-radius:12px; overflow:hidden; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>

        <div style='background-color:#c62828; padding:25px; text-align:center;'>
            <h1 style='color:white; margin:0; font-size:24px;'>
                ⚠ Critical Vital Sign Alert
            </h1>
        </div>

        <div style='padding:30px;'>

            <p style='font-size:16px; color:#333;'>
                Your $messageType requires attention.
            </p>

            <div style='background-color:#fff3f3; border-left:4px solid #c62828; padding:15px; margin:20px 0;'>
                <strong style='color:#c62828;'>
                    Status: Critical
                </strong>
            </div>

            <h3 style='color:#333;'>
                Detected Alerts
            </h3>

            <ul style='color:#555; padding-left:20px;'>
                {alertsHtml}
            </ul>

            <p style='font-size:14px; color:#666; margin-top:25px;'>
                Please review your vital signs and contact your healthcare
                provider if necessary.
            </p>

            <p style='font-size:12px; color:#999; margin-top:30px;'>
                This notification is generated automatically by the
                Cardiac Patient Monitoring System.
            </p>

        </div>
    </div>

</body>
</html>";

        await _emailService.SendEmailAsync(
            email,
            "⚠ Critical Vital Sign Alert",
            emailBody);
    }
}