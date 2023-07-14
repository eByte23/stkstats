using Newtonsoft.Json;

namespace StatSys.CoreStats.Models;

public class HittingData
{
    [JsonProperty("PA")]
    public int PA { get; set; }

    [JsonProperty("AB")]
    public int AB { get; set; }

    [JsonProperty("H")]
    public int H { get; set; }

    [JsonProperty("TB")]
    public int TB { get; set; }

    [JsonProperty("Singles")]
    public int Singles { get; set; }

    [JsonProperty("Doubles")]
    public int Doubles { get; set; }

    [JsonProperty("Triples")]
    public int Triples { get; set; }

    [JsonProperty("HR")]
    public int HR { get; set; }

    [JsonProperty("RBI")]
    public int RBI { get; set; }

    [JsonProperty("R")]
    public int R { get; set; }

    [JsonProperty("BB")]
    public int BB { get; set; }

    [JsonProperty("SO")]
    public int SO { get; set; }

    [JsonProperty("SOL")]
    public int KL { get; set; }

    [JsonProperty("SF")]
    public int SF { get; set; }

    [JsonProperty("HBP")]
    public int HBP { get; set; }

    // Percentages

    [JsonProperty("AVG")]
    public string? AVG { get; set; }

    [JsonProperty("SLG")]
    public string? SLG { get; set; }

    [JsonProperty("OPS")]
    public string? OPS { get; set; }

    [JsonProperty("OBP")]
    public string? OBP { get; set; }


    public void AddGame(HittingData game)
    {
        PA += game.PA;
        AB += game.AB;
        H += game.H;
        TB += game.TB;
        Singles += game.Singles;
        Doubles += game.Doubles;
        Triples += game.Triples;
        HR += game.HR;
        RBI += game.RBI;
        R += game.R;
        BB += game.BB;
        SO += game.SO;
        KL += game.KL;
        SF += game.SF;
        HBP += game.HBP;
        RecalculatePercentages();
    }

    public HittingData AddGame2(HittingData game)
    {

        var newData = new HittingData
        {
            PA = this.PA + game.PA,
            AB = this.AB + game.AB,
            H = this.H + game.H,
            TB = this.TB + game.TB,
            Singles = this.Singles + game.Singles,
            Doubles = this.Doubles + game.Doubles,
            Triples = this.Triples + game.Triples,
            HR = this.HR + game.HR,
            RBI = this.RBI + game.RBI,
            R = this.R + game.R,
            BB = this.BB + game.BB,
            SO = this.SO + game.SO,
            KL = this.KL + game.KL,
            SF = this.SF + game.SF,
            HBP = this.HBP + game.HBP,
        };

        newData.RecalculatePercentages();

        return newData;
    }

    private void RecalculatePercentages()
    {
        AVG = GetPercentageString(GetBattingAvg(this));

        double obp = GetBattingObpPercentage(this);
        OBP = GetPercentageString(obp);

        double slg = GetSluggingPercentage(this);
        SLG = GetPercentageString(slg);
        OPS = GetPercentageString(GetOpsPercentage(obp, slg));
    }

    public static double GetOpsPercentage(double onBasePercentage, double sluggingPercentage)
    {
        return onBasePercentage + sluggingPercentage;
    }

    public static double GetSluggingPercentage(HittingData hitting)
    {
        if (hitting.AB == 0)
        {
            return 0;
        }

        return ((double)hitting.Singles! + ((double)hitting.Doubles! * 2) + ((double)hitting.Triples! * 3) + ((double)hitting.HR! * 4)) / (double)hitting.AB!;
    }

    public static double GetBattingAvg(HittingData hitting)
    {
        if (hitting.AB == 0)
        {
            return 0;
        }

        return ((double)hitting.H!) / ((double)hitting.AB!);
    }

    public static double GetBattingObpPercentage(HittingData Hitting)
    {
        double leftSideEq = ((double)Hitting.H! + (double)Hitting.BB! + (double)Hitting.HBP!);
        double rightSideEq = ((double)Hitting.AB! + (double)Hitting.BB! + (double)Hitting.HBP! + (double)Hitting.SF!);

        if (rightSideEq == 0)
        {
            return 0;
        }

        double v = leftSideEq / rightSideEq;
        return v;
    }

    public static string GetPercentageString(double percentage)
    {
        return percentage.ToString("#.000");
    }
}