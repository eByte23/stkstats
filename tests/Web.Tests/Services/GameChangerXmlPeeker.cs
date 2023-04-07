using System;
using STKBC.Stats.Services;

namespace STKBC.Tests.Services;

public class GameChangerXmlPeekerTests
{

    const string SABERTOOTH_FILE_EXAMPLE_OVERVIEW = @"<?xml version=""1.0"" encoding=""utf-8""?>
<bsgame generated=""05/05/19"" source=""GameChanger"" source_format=""Sabertooth"" version=""10.0.0.1"">
  <venue date=""05/04/19"" gameid=""5cccbb850cd201f5ec000008"" homeid=""SNTS"" homename=""St.Kilda 2019 MWBL C Resv Grade"" leaguegame=""Y"" location="""" schedinn=""7"" start=""12:30 PM"" visid=""DNCS"" visname=""Doncaster"" />
  <status complete=""Y"" />
</bsgame>";

    const string CHELSEA_FILE_EXAMPLE_OVERVIEW = @"<?xml version=""1.0"" encoding=""utf-8""?>
<bsgame generated=""04/06/23"" source=""GameChanger"" source_format=""Chelsea"" version=""8.17.0.1"">
  <venue date=""08/13/22"" gameid=""62f5944135d90073e5000002"" homeid=""STKL"" homename=""St Kilda  B Reserves"" leaguegame=""Y"" location="""" schedinn=""9"" start=""12:15 PM"" visid=""CRYD"" visname=""Croydon B Reserves"" />
  <status complete=""Y"" />
</bsgame>";

    [Fact]
    public void GameChangerXmlPeeker_Should_ReturnSabertoohFormatOverview()
    {

        var peeker = new GameChangerXmlPeeker();

        var overview = peeker.GetFileOverviewFromXml(SABERTOOTH_FILE_EXAMPLE_OVERVIEW);

        Assert.Equivalent(new GameChangerFileOverview
        {
            HomeTeam = "St.Kilda 2019 MWBL C Resv Grade",
            AwayTeam = "Doncaster",
            GameId = "5cccbb850cd201f5ec000008",
            GameDate = new DateTime(2019, 5, 4),
            Format = GameChangerFormat.Sabertooth,
        }, overview);

    }

    [Fact]
    public void GameChangerXmlPeeker_Should_ReturnChelseaFormatOverview()
    {
        var peeker = new GameChangerXmlPeeker();

        var overview = peeker.GetFileOverviewFromXml(CHELSEA_FILE_EXAMPLE_OVERVIEW);

        Assert.Equivalent(new GameChangerFileOverview
        {
            HomeTeam = "St Kilda  B Reserves",
            AwayTeam = "Croydon B Reserves",
            GameId = "62f5944135d90073e5000002",
            GameDate = new DateTime(2022, 8, 13),
            Format = GameChangerFormat.Chelsea,
        }, overview);

    }
}