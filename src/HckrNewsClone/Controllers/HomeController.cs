using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HckrNewsClone.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HckrNewsClone.Controllers
{
	public class HomeController : Controller
    {
		private string BaseUrl = "http://hckrnews.com/data/";
		// GET: /<controller>/
		public async Task<IActionResult> Index()
        {		
			string nextDayDt = DateTime.Today.AddDays(-3).ToString("yyyyMMdd");
			string todayDt = DateTime.Today.AddDays(-4).ToString("yyyyMMdd");
			//HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + todayDt + ".js");

			//HttpClient client = new HttpClient();
			//var todayResult = await client.GetAsync(BaseUrl + todayDt + ".js");
			//var yestResult = await client.GetAsync(BaseUrl + yesterdayDt + ".js");

			//SearchModel model = new SearchModel();
			//if(todayResult.IsSuccessStatusCode && yestResult.IsSuccessStatusCode)
			//{
			//	string todayData = await todayResult.Content.ReadAsStringAsync();
			//	List<NewsItem> todayList = JsonConvert.DeserializeObject<List<NewsItem>>(todayData);

			//	string yestData = await yestResult.Content.ReadAsStringAsync();
			//	List<NewsItem> yestList = JsonConvert.DeserializeObject<List<NewsItem>>(yestData);

			//	todayList.RemoveAll(c => c.Datetime.Date != DateTime.Today.AddDays(-4));
			//	yestList.RemoveAll(c => c.Datetime.Date != DateTime.Today.AddDays(-4));

			//	todayList.AddRange(yestList);

			//	//todayList = todayList.OrderByDescending(c => c.Points + c.Comments).Take((int)Math.Round(0.20 * todayList.Count, 0)).OrderByDescending(c => c.Datetime).ToList();

			//	var topByPts = todayList.OrderByDescending(c => c.Points).Take((int)Math.Round(0.2 * todayList.Count, 0));
			//	var topByComments = todayList.OrderByDescending(c => c.Comments).Take((int)Math.Round(0.2 * todayList.Count, 0));

			//	todayList = topByPts.Union(topByComments).ToList();				
			//}

			SearchModel model = new SearchModel();
			model.NewsDate = DateTime.Today.AddDays(-4);
			model.SearchResults = await SearchNews(DateTime.Today.AddDays(-4), todayDt, nextDayDt);

			return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> Index(SearchModel model)
		{
			string dtFormat = "yyyyMMdd";

			string nextDay = model.NewsDate.AddDays(1).ToString(dtFormat);

			//HttpClient client = new HttpClient();
			//var nextDayResult = await client.GetAsync(BaseUrl + nextDay + ".js");
			//var todayResult = await client.GetAsync(BaseUrl + model.NewsDate.ToString(dtFormat) + ".js");

			//if (todayResult.IsSuccessStatusCode && nextDayResult.IsSuccessStatusCode)
			//{
			//	string todayData = await todayResult.Content.ReadAsStringAsync();
			//	List<NewsItem> todayList = JsonConvert.DeserializeObject<List<NewsItem>>(todayData);

			//	string nextDayData = await nextDayResult.Content.ReadAsStringAsync();
			//	List<NewsItem> nextDayList = JsonConvert.DeserializeObject<List<NewsItem>>(nextDayData);

			//	todayList.RemoveAll(c => c.Datetime.Date != model.NewsDate);
			//	nextDayList.RemoveAll(c => c.Datetime.Date != model.NewsDate);

			//	todayList.AddRange(nextDayList);

			//	//todayList = todayList.OrderByDescending(c => c.Points + c.Comments).Take((int)Math.Floor(0.2 * todayList.Count)).OrderByDescending(c => c.Datetime).ToList();

			//    var topByPts = todayList.OrderByDescending(c => c.Points).Take((int)Math.Floor(0.2 * todayList.Count));
			//	var topByComments = todayList.OrderByDescending(c => c.Comments).Take((int)Math.Floor(0.2 * todayList.Count));

			//	todayList = topByPts.Union(topByComments).OrderByDescending(c => c.Datetime).ToList();


			//}

			model.SearchResults = await SearchNews(model.NewsDate, model.NewsDate.ToString(dtFormat), nextDay);

			return View(model);
		}

		private async Task<List<NewsItem>> SearchNews(DateTime selectedDt, string todayDt, string tmrwDt)
		{
			HttpClient client = new HttpClient();

			var nextDayResult = await client.GetAsync(BaseUrl + tmrwDt + ".js");
			var todayResult = await client.GetAsync(BaseUrl + todayDt + ".js");

			List<NewsItem> searchResults = null;

			if (todayResult.IsSuccessStatusCode && nextDayResult.IsSuccessStatusCode)
			{
				string todayData = await todayResult.Content.ReadAsStringAsync();
				List<NewsItem> todayList = JsonConvert.DeserializeObject<List<NewsItem>>(todayData);

				string nextDayData = await nextDayResult.Content.ReadAsStringAsync();
				List<NewsItem> nextDayList = JsonConvert.DeserializeObject<List<NewsItem>>(nextDayData);

				todayList.RemoveAll(c => c.Datetime.Date != selectedDt);
				nextDayList.RemoveAll(c => c.Datetime.Date != selectedDt);

				todayList.AddRange(nextDayList);

				//todayList = todayList.OrderByDescending(c => c.Points + c.Comments).Take((int)Math.Floor(0.2 * todayList.Count)).OrderByDescending(c => c.Datetime).ToList();

				var topByPts = todayList.OrderByDescending(c => c.Points).Take((int)Math.Floor(0.2 * todayList.Count));
				var topByComments = todayList.OrderByDescending(c => c.Comments).Take((int)Math.Floor(0.2 * todayList.Count));

				searchResults = topByPts.Union(topByComments).OrderByDescending(c => c.Points + c.Comments).ToList();

				//model.SearchResults = todayList;				
			}

			return searchResults;
		}
	}
}
