namespace CardiacPatientMonitoringSystem.Services;

public class VitalSignService
{
    public bool IsHeartRateNormal(int heartRate)
    {
        return heartRate >= 60 && heartRate <= 100;
    }

    public bool IsOxygenSaturationNormal(decimal oxygenSaturation)
    {
        return oxygenSaturation >= 95;
    }
}