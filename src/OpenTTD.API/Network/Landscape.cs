using System.ComponentModel;

namespace OpenTTD.API.Network;

public enum Landscape
{
    [Description("Temperate")]
    LtTemperate = 0,
    [Description("Arctic")]
    LtArctic = 1,
    [Description("Tropic")]
    LtTropic = 2,
    [Description("Toyland")]
    LtToyland = 3,
}