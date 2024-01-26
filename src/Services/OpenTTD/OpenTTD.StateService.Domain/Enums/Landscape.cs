using System.ComponentModel;

namespace OpenTTD.StateService.Domain.Enums;

public enum Landscape
{
    [Description("Temperate")]
    TEMPERATE = 0,
    [Description("Arctic")]
    ARCTIC = 1,
    [Description("Tropic")]
    TROPIC = 2,
    [Description("Toyland")]
    TOYLAND = 3,
}