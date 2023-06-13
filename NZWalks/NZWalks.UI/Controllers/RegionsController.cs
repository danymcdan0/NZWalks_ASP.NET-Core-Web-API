using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new List<RegionDTO>();

            try
            {
                //Get all regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7096/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDTO>>());

            }
            catch (Exception ex)
            {
                //Log the exception
                throw;
            }
            return View(response);
        }

        [HttpGet]
        public IActionResult Add() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel addRegionViewModel) 
        {
			var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7096/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(addRegionViewModel), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

            if (response != null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
		}

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id) 
        {
			var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDTO>($"https://localhost:7096/api/regions/{Id.ToString()}");

            if (response != null)
            {
                return View(response);
            }

			return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDTO regionDTO) 
        {
			var client = httpClientFactory.CreateClient();

			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:7096/api/regions/{regionDTO.Id}"),
				Content = new StringContent(JsonSerializer.Serialize(regionDTO), Encoding.UTF8, "application/json")
			};

			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDTO>();

            if (response != null)
            {
                return RedirectToAction("Edit", "Regions");
            }

            return View();
		}

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDTO regionDTO) 
        {
            try
            {
				var client = httpClientFactory.CreateClient();

				var httpResponseMessage = await client.DeleteAsync($"https://localhost:7096/api/regions/{regionDTO.Id}");

				httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
			}
            catch (Exception ex)
            {
                //Log or console the error
            }

            return View("Edit");
		}
	}
}
