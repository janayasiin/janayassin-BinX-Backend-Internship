using CardiacPatientMonitoringSystem.DTOs.Responses;
using CardiacPatientMonitoringSystem.Models;

namespace CardiacPatientMonitoringSystem.Services
{
    public class VitalSignAnalysisService : IVitalSignAnalysisService
    {
        public VitalSignAnalysisResponse Analyze(VitalSign vitalSign)
        {
            var alerts = new List<string>();
            var status = "Normal";

            // Heart Rate
            if (vitalSign.HeartRate > 120)
            {
                alerts.Add("Heart rate is critically high.");
                status = "Critical";
            }
            else if (vitalSign.HeartRate < 50)
            {
                alerts.Add("Heart rate is critically low.");
                status = "Critical";
            }

            // Blood Pressure
            if (vitalSign.SystolicBloodPressure > 140 ||
                vitalSign.DiastolicBloodPressure > 90)
            {
                alerts.Add("Blood pressure is high.");

                if (status != "Critical")
                    status = "Warning";
            }

            if (vitalSign.SystolicBloodPressure < 90 ||
                vitalSign.DiastolicBloodPressure < 60)
            {
                alerts.Add("Blood pressure is low.");

                if (status != "Critical")
                    status = "Warning";
            }

            // Oxygen Saturation
            if (vitalSign.OxygenSaturation < 90)
            {
                alerts.Add("Oxygen saturation is critically low.");
                status = "Critical";
            }

            // Temperature
            if (vitalSign.Temperature > 38.0m)
            {
                alerts.Add("Temperature is high.");

                if (status != "Critical")
                    status = "Warning";
            }

            if (vitalSign.Temperature < 36.0m)
            {
                alerts.Add("Temperature is low.");

                if (status != "Critical")
                    status = "Warning";
            }

            return new VitalSignAnalysisResponse
            {
                Status = status,
                Alerts = alerts
            };
        }
    }
}