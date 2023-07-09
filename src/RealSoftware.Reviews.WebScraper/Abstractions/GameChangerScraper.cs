using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PuppeteerSharp;
using RealSoftware.Reviews.WebScraper.Models;
using RealSoftware.Reviews.WebScraper.Service;

namespace RealSoftware.Reviews.WebScraper.Abstractions
{
    public interface IGameChangerOptions
    {
        string Username { get; set; }
        string Password { get; set; }
        bool SkipLogin { get; set; }
    }

    public abstract class GameChangerBaseScraper<TLoadOpt, TData> : ScraperBase<TLoadOpt, TData>, IScraper<TLoadOpt, TData>
        where TLoadOpt : class, IGameChangerOptions
        where TData : class
    {
        protected GameChangerBaseScraper(IScraperCache cache, IPage page) : base(cache, page)
        {
        }


        protected override async Task BeforeLoadAsync(IPage page, TLoadOpt options)
        {
            var res = await page.GoToAsync("https://gc.com/login");

            if (!res.Ok)
            {
                throw new System.Exception("Could not load GameChanger Login Page");
            }

            await page.TypeAsync("#frm_login #email", options.Username);
            await page.TypeAsync("#frm_login #login_password", options.Password);

            await page.ClickAsync("#login");

            await page.WaitForNavigationAsync();

            var el = await page.WaitForSelectorAsync("#menu .teamsMenu .teamList li");
        }
    }

}