namespace OpenTTD.API.Network.AdminPort;

public enum AdminCompanyRemoveReason
{
    /// <summary>
    /// The company is manually removed.
    /// </summary>
    AdminCrrManual,
    /// <summary>
    /// The company is removed due to autoclean.
    /// </summary>
    AdminCrrAutoclean,
    /// <summary>
    /// The company went belly-up.
    /// </summary>
    AdminCrrBankrupt,
    /// <summary>
    /// Sentinel for end.
    /// </summary>
    AdminCrrEnd,
}