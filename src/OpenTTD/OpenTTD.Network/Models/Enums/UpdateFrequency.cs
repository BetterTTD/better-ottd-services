namespace OpenTTD.Network.Models.Enums;

public enum UpdateFrequency
{
	AdminFrequencyPoll      = 0x01, /// The admin can poll this.
	AdminFrequencyDaily     = 0x02, /// The admin gets information about this on a daily basis.
	AdminFrequencyWeekly    = 0x04, /// The admin gets information about this on a weekly basis.
	AdminFrequencyMonthly   = 0x08, /// The admin gets information about this on a monthly basis.
	AdminFrequencyQuarterly = 0x10, /// The admin gets information about this on a quarterly basis.
	AdminFrequencyManually  = 0x20, /// The admin gets information about this on a yearly basis.
	AdminFrequencyAutomatic = 0x40, /// The admin gets information about this when it changes.
}