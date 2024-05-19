namespace FIRST.Utilities.Options;

public class FirstOptions
{
    public const string OptionName = "FIRST";
    public FtcOptions FtcOptions { get; set; } = null!;
}

public class FtcOptions
{
    public const string OptionName = "FTC";
    public int CurrentSeason { get; set; }
    public string ProgramCode { get; set; }
}