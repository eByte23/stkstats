import chromium from "chrome-aws-lambda"
import { Browser } from "puppeteer-core";
import { parse } from 'node-html-parser';

const handler = async (...params: any[]) => {
  var callback = params[2];
  let result = null;
  let browser: Browser | null = null;

  try {
    browser = await chromium.puppeteer.launch({
      args: chromium.args,
      defaultViewport: chromium.defaultViewport,
      executablePath: await chromium.executablePath,
      headless: chromium.headless,
      ignoreHTTPSErrors: true,
    });

    let page = await browser.newPage();

    const pageRes = await page.goto('https://gc.com/login');

    if (!pageRes?.ok()) {
      console.error(pageRes?.text());
      throw new Error('failed to load page');
    }


    await page.click("#frm_login #email")
    await page.type("#frm_login #email", "stkildabaseball@gmail.com");

    await page.click("#frm_login #password")
    await page.type("#frm_login #password", "Saints1879!");

    await page.click("#frm_login button[type=submit]");

    const dashboardPage = await page.waitForNavigation();

    if (!dashboardPage?.ok()) {
      console.error(pageRes?.text());
      throw new Error('failed to login page');
    }

    const source = await dashboardPage.frame()?.content();
    const root = parse(source!);

    console.log(root.querySelectorAll('#menu .teamsMenu .teamList li a'));



    // result = await page.title();
  } catch (error) {
    return callback(error);
  } finally {
    if (browser !== null) {
      await browser.close();
    }
  }

  return callback(null, result);
};


// fetch("https://gc.com/do-login", {
//   "headers": {
//     "accept": "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7",
//     "accept-language": "en",
//     "cache-control": "no-cache",
//     "content-type": "application/x-www-form-urlencoded",
//     "pragma": "no-cache",
//     "sec-ch-ua": "\"Not.A/Brand\";v=\"8\", \"Chromium\";v=\"114\", \"Google Chrome\";v=\"114\"",
//     "sec-ch-ua-mobile": "?0",
//     "sec-ch-ua-platform": "\"macOS\"",
//     "sec-fetch-dest": "document",
//     "sec-fetch-mode": "navigate",
//     "sec-fetch-site": "same-origin",
//     "sec-fetch-user": "?1",
//     "upgrade-insecure-requests": "1",
//     "cookie": "_hjSessionUser_2606438=eyJpZCI6ImQ2YzE3ZjU0LTEzNDItNWFhMS05NGQ2LTY2NDgzOTY5MzhlMyIsImNyZWF0ZWQiOjE2ODgyNDMzNDIwMTYsImV4aXN0aW5nIjpmYWxzZX0=; _hjFirstSeen=1; _hjIncludedInSessionSample_2606438=0; _hjSession_2606438=eyJpZCI6ImZiZDhmMjYwLWI5ODQtNGFiNy1iZWRmLTczYzNmMjUxMWMxOCIsImNyZWF0ZWQiOjE2ODgyNDMzNDIwMjIsImluU2FtcGxlIjpmYWxzZX0=; _hjAbsoluteSessionInProgress=0; _gcl_au=1.1.1473106716.1688243343; _ga=GA1.2.530356704.1688243343; _gid=GA1.2.404756379.1688243343; _gat_gtag_UA_12010494_1=1; _tt_enable_cookie=1; _ttp=d8e4ROKJ0yun-YL9gBnH9vRrqUV; csrftoken=DKeXrL4nbrV3qBfxr6fCsotxD1qYGvr0vQmQ3IVRNXI2aWqFQj59fgiHrCX0MWI5; _ga_TMKGW5WMV9=GS1.1.1688243342.1.0.1688243345.0.0.0; _gat=1; _sp_ses.9212=*; _sp_id.9212=7329e359-0da7-4955-804f-8a261500968d.1688243346.1.1688243350.1688243346.eb3f7934-b8e7-4280-bb34-ad45ede07fd9",
//     "Referer": "https://gc.com/login",
//     "Referrer-Policy": "strict-origin-when-cross-origin"
//   },
//   "body": "csrfmiddlewaretoken=gH5Io94H73pAV5TRqu750Hafw9WKIgLO8NdB06VbJzczFq4ZPHXCNzZpkKtMOH2T&email=stkildabaseball%40gmail.com&password=Saints1879%21",
//   "method": "POST"
// });
handler(null, null,(text) => console.log(text)).then(() => { });

exports.handler = handler;