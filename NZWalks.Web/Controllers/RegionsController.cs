using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using NZWalks.Web.Models.DTO;
using System.Collections;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NZWalks.Web.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<RegionsDTO> regions = new List<RegionsDTO>();
            try
            {
                //Get the data from api 

                var client = httpClientFactory.CreateClient();

                var httpResonseMessage = await client.GetAsync("https://localhost:7109/api/regions");

                httpResonseMessage.EnsureSuccessStatusCode();
                regions.AddRange(await httpResonseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionsDTO>>());

                //ViewBag.Response = responseInString;
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(regions);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateRegionDTO createRegionDTO)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7109/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(createRegionDTO), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<CreateRegionDTO>();
            if (response is not null)
                return RedirectToAction("Index", "Regions");

            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionsDTO>($"https://localhost:7109/api/regions/{id.ToString()}");

            if (response is not null)
                return View(response);

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionsDTO regionsDTO)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7109/api/regions/{regionsDTO.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(regionsDTO), Encoding.UTF8, "application/json")
            };
            var responseMessage = await client.SendAsync(httpRequestMessage);
            responseMessage.EnsureSuccessStatusCode();

            if (responseMessage is not null)
                return RedirectToAction("Index", "Regions");

            return View();
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var httpResponseMessage = await client.DeleteAsync($"https://localhost:7109/api/regions/{id}");
            httpResponseMessage.EnsureSuccessStatusCode();

            return RedirectToAction("Index", "Regions");
        }
    }
}
