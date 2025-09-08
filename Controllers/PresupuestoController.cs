using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace miprimerproyecto.Controllers
{
    public class PresupuestoController : Controller
    {
        private const string GeminiApiKey = "AIzaSyAJmbunmglS2y3BTeyMhalSxL-ULuyJiRc";
        private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=";

        [HttpPost]
        public async Task<IActionResult> Generar(string prompt)
        {
            using (var httpClient = new HttpClient())
            {
                var requestBody = new
                {
                    contents = new[] {
                        new {
                            parts = new[] {
                                new { text = prompt }
                            }
                        }
                    }
                };
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var response = await httpClient.PostAsync(
                    GeminiApiUrl + GeminiApiKey,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json")
                );
                var result = await response.Content.ReadAsStringAsync();
                var obj = JObject.Parse(result);
                var texto = obj["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "No se pudo generar el presupuesto.";
                return Json(new { presupuesto = texto });
            }
        }
    }
}
