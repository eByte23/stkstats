using System.Security.Cryptography.X509Certificates;
using Be.Vlaanderen.Basisregisters.Generators.Guid;
using GameChanger.Parser;
using Newtonsoft.Json;

namespace Web.Tests.StatsWebOutput;


public class UniquePlayersListGenerator
{
    private const string PLAYER_ID_NAMESPACE = "7bddb795-8973-4308-8f6c-a9badccf605c";
    public class StreamParsedPlayer
    {
        public Guid UniqueId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? TeamId { get; set; }

        public List<string> GameChangerIds = new List<string>();
    }



    public static List<string> GetLinupPlayerIds(GameChangerStream stream)
    {
        var playerIds = new List<string>();
        var allEvents = stream.Events.SelectMany(x => x);


        foreach (var item in allEvents)
        {

            if (item.EventData.Code == "fill_lineup_index")
            {
                if (item.EventData.Attributes is null) continue;

                if (item.EventData.Attributes.ContainsKey("playerId"))
                {
                    playerIds.Add(item.EventData.Attributes["playerId"].ToString());
                }
            }

            if (item.EventData?.Events is null) continue;

            foreach (var evt in item.EventData.Events)
            {
                if (evt.Code != "fill_lineup_index")
                {
                    continue;
                }

                if (evt.Attributes is null) continue;

                if (evt.Attributes.ContainsKey("playerId"))
                {
                    playerIds.Add(evt.Attributes["playerId"].ToString());
                }
            }

        }

        return playerIds;
    }
    private Guid GetUniqueId(string firstName, string lastName, string firstGameChangerId)
    {
        var parts = new string[]{
                firstName,
                lastName,
                firstGameChangerId
            };

        return Deterministic.Create(Guid.Parse(PLAYER_ID_NAMESPACE), string.Join("-", parts).ToLower());
    }

}


public class GeneratePlayersListTests
{
    private const string PLAYER_ID_NAMESPACE = "7bddb795-8973-4308-8f6c-a9badccf605c";


    [Fact]
    public void TestGenerateNewPlayersList()
    {
        // Arrange

        var gameStats = ParserUtil.ParseStatsFile("test_files/full-stats-game.json");
        var stream = ParserUtil.ParseStreamFile("test_files/full-stream-data.json");


        var players = gameStats.Teams[0].Players;
        var lineupPlayerIds = UniquePlayersListGenerator.GetLinupPlayerIds(stream);
        var actualPlayers = players.Where(x => lineupPlayerIds.Contains(x.Id)).Select(item =>
        {

            // Guid guid = GetUniqueId(item.Fname, item.Lname, item.Id);
            // var last12 = guid.ToString().Substring(guid.ToString().Length - 12);
            return new UniquePlayersListGenerator.StreamParsedPlayer
            {
                FirstName = item.Fname,
                LastName = item.Lname,
                // Id = $"{item.Fname}-{item.Lname}-{last12}".ToLower(),
                GameChangerIds = new List<string> { item.Id },
                Name = $"{item.Fname} {item.Lname}",
                // UniqueId = guid
            };
        }).OrderBy(x => x.Name).ToList();



        //     // Assert

        var expectedPlayers = new List<UniquePlayersListGenerator.StreamParsedPlayer>
        {
            GetPlayer("Mat", "Manning", new List<string>{"648dc462f84f013676000008"}),
            GetPlayer("Elijah", "Bate", new List<string>{"648dc462f84f0136bb00000f"}),
            GetPlayer("Chris", "Bordin", new List<string>{"6493c462f8e900fb7700016e"}),
            GetPlayer("Pete", "McCourt", new List<string>{"648dc462f8eb0013fd00001b"}),
            GetPlayer("Sean", "O’Halloran", new List<string>{"6496c462f8c90070200002ab"}),
            GetPlayer("Carlos", "Mena", new List<string>{"648dc462f8eb00143a000020"}),
            GetPlayer("Adam", "Cauley", new List<string>{"648dc462f8eb0013de000019"}),
            GetPlayer("Glenn", "Wimetal", new List<string>{"648dc462f8eb00142300001e"}),
            GetPlayer("George", "Armstrong", new List<string>{"648dc462f8eb00140900001c"}),
            GetPlayer("Avi", "Lewis", new List<string>{"648dc462f8eb0013f000001a"}),
            GetPlayer("Jared", "Drever", new List<string>{"648dc462f8eb001442000021"}),
        }.OrderBy(x => x.Name).ToList();

        Assert.Equal(JsonConvert.SerializeObject(expectedPlayers), JsonConvert.SerializeObject(actualPlayers));
    }

    private UniquePlayersListGenerator.StreamParsedPlayer GetPlayer(string firstName, string lastName, List<string> gameChangerIds)
    {
        // Guid uniqueId = GetUniqueId(firstName, lastName, gameChangerIds[0]);
        // var last12 = uniqueId.ToString().Substring(uniqueId.ToString().Length - 12);

        return new UniquePlayersListGenerator.StreamParsedPlayer
        {
            // UniqueId = uniqueId,
            // Id = $"{firstName}-{lastName}-{last12}".ToLower().Replace("`", "").Replace("'", ""),
            Name = $"{firstName} {lastName}",
            FirstName = firstName,
            LastName = lastName,
            GameChangerIds = gameChangerIds
        };
    }
}