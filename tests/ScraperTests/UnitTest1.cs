using HtmlAgilityPack;
using Newtonsoft.Json;
using RealSoftware.Reviews.WebScraper.Scraper;
using RealSoftware.Reviews.WebScraper.Service;

namespace ScraperTests;

public class UnitTest1
{
    [Fact]
    public async void TestGetTeamScheduleFromInlineJson()
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.Load("/Users/elijahbate/Personal/Dev/stats/tests/ScraperTests/Data/Schedule.html");


        var scraper = new GameChangerGameScheduleScraper(null, null);

        var b = scraper.GetTeamScheduleFromInlineJson(htmlDoc);

        var data = JsonConvert.DeserializeObject<dynamic>(b);

        Assert.Equal("64420d933709d41baa98925c", b);


    }



    [Fact]
    public async void TestGetGameSchedule()
    {
        var scraper = new GameChangerGameScheduleScraper(null, null);

        var htmlDoc = new HtmlDocument();
        htmlDoc.Load("/Users/elijahbate/Personal/Dev/stats/tests/ScraperTests/Data/Schedule.html");


        var gameIds = await scraper.GetGameSchedule(htmlDoc);
        Assert.Collection(gameIds,
            item => Assert.Equal("64420d933709d41baa98925c", item),
            item => Assert.Equal("6455d2a47cf200d699000002", item),
            item => Assert.Equal("645fd2a47cf2001403000136", item),
            item => Assert.Equal("6468d2a47ce9004c17000267", item),
            item => Assert.Equal("6471d2a47ce9008b00000393", item),
            item => Assert.Equal("647ad2a47ce900c56d0004df", item),
            item => Assert.Equal("648dd2a47c0001279e0005fa", item),
            item => Assert.Equal("6496d2a47c0001710c00071f", item),
            item => Assert.Equal("649fd2a47c8f0154c60000b9", item)
        );
    }


    [Fact]
    public async void Test1()
    {
        var scraper = new GameChangerTeamsScraper(null, null);

        var htmlDoc = new HtmlDocument();
        htmlDoc.Load("/Users/elijahbate/Personal/Dev/stats/tests/ScraperTests/Data/Dashboard.html");


        var a = await scraper.GetTeams(htmlDoc);

        Assert.Collection(a,
            // 1
            item =>
            {
                Assert.Equal("MWBL ST KILDA B GRADE", item.Name);
                Assert.Equal("6442041d8ed57a0914e16648", item.Id);
                Assert.Equal("/t/winter-2023/mwbl-st-kilda-b-grade-6442041d8ed57a0914e16648", item.Link);
            },
            // 2
            item =>
            {
                Assert.Equal("MWBL ST KILDA B RESERVES", item.Name);
                Assert.Equal("644204e069473147ce33dfd5", item.Id);
                Assert.Equal("/t/winter-2023/mwbl-st-kilda-b-reserves-644204e069473147ce33dfd5", item.Link);
            },
            // 3
            item =>
            {
                Assert.Equal("MWBL ST KILDA E GRADE", item.Name);
                Assert.Equal("64420520e1fa981ab898929b", item.Id);
                Assert.Equal("/t/winter-2023/mwbl-st-kilda-e-grade-64420520e1fa981ab898929b", item.Link);
            },
            // 4
            item =>
            {
                Assert.Equal("MWBL ST KILDA WOMENS", item.Name);
                Assert.Equal("64420644e1fa981ab898929f", item.Id);
                Assert.Equal("/t/winter-2023/mwbl-st-kilda-womens-64420644e1fa981ab898929f", item.Link);
            },
            // 5
            item =>
            {
                Assert.Equal("Saints D Grade", item.Name);
                Assert.Equal("648dc462f8eb00129b000006", item.Id);
                Assert.Equal("/t/winter-2023/saints-d-grade-648dc462f8eb00129b000006", item.Link);
            },
            // 6
            item =>
            {
                Assert.Equal("Saints D Res", item.Name);
                Assert.Equal("648dc462f8eb001230000002", item.Id);
                Assert.Equal("/t/winter-2023/saints-d-res-648dc462f8eb001230000002", item.Link);
            },
            item =>
            {
                Assert.Equal("St Kilda  B Reserves", item.Name);
                Assert.Equal("62629441359b006eee00016e", item.Id);
                Assert.Equal("/t/winter-2022/st-kilda-b-reserves-62629441359b006eee00016e", item.Link);
            },
            item =>
            {
                Assert.Equal("St Kilda B Grade", item.Name);
                Assert.Equal("62629441359b006e4e00016b", item.Id);
                Assert.Equal("/t/winter-2022/st-kilda-b-grade-62629441359b006e4e00016b", item.Link);
            },
            item =>
            {
                Assert.Equal("VSBL ST KILDA SAINTS DIVISION 2 1sts", item.Name);
                Assert.Equal("6327d3ab9386313cf8083fb5", item.Id);
                Assert.Equal("/t/summer-2022/vsbl-st-kilda-saints-division-2-1sts-6327d3ab9386313cf8083fb5", item.Link);
            },
            item =>
            {
                Assert.Equal("VSBL ST KILDA SAINTS DIVISION 2 2nds", item.Name);
                Assert.Equal("6327d8f7ef49121ec6a406a5", item.Id);
                Assert.Equal("/t/summer-2022/vsbl-st-kilda-saints-division-2-2nds-6327d8f7ef49121ec6a406a5", item.Link);
            },
            item =>
            {
                Assert.Equal("VSBL ST KILDA SAINTS DIVISION 2 3rds", item.Name);
                Assert.Equal("6327d7ca3838a3b0d408407a", item.Id);
                Assert.Equal("/t/summer-2022/vsbl-st-kilda-saints-division-2-3rds-6327d7ca3838a3b0d408407a", item.Link);
            },
            item =>
            {
                Assert.Equal("VSBL ST KILDA SAINTS DIVISION 2 4ths", item.Name);
                Assert.Equal("6327d887ecee3d3b69a40665", item.Id);
                Assert.Equal("/t/summer-2022/vsbl-st-kilda-saints-division-2-4ths-6327d887ecee3d3b69a40665", item.Link);
            },
            item =>
            {
                Assert.Equal("VSBL ST KILDA SAINTS DIVISION 3 SOUTH", item.Name);
                Assert.Equal("6327da68ecee3d3b69a4066b", item.Id);
                Assert.Equal("/t/summer-2022/vsbl-st-kilda-saints-division-3-south-6327da68ecee3d3b69a4066b", item.Link);
            },
            item =>
            {
                Assert.Equal("VSBL ST KILDA SAINTS DIVISION 4 SOUTH", item.Name);
                Assert.Equal("6327d93e27e3a1836aa9b32a", item.Id);
                Assert.Equal("/t/summer-2022/vsbl-st-kilda-saints-division-4-south-6327d93e27e3a1836aa9b32a", item.Link);
            }

        );
    }
}