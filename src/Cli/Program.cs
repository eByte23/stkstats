namespace STKBC.Cli;


public class Program
{

    public static void Main(string[] args)
    {
        const string Path1 = "/workspace/stats/data/old-games/c res/St.Kilda 2019 MWBL C Resv Grade vs Doncaster 05_04_19 Stats.xml";
        var fileText = File.ReadAllText(Path1);
        var game = Stats.Parser.ParserUtil.Deserialize(fileText);

    }




    public cla
}