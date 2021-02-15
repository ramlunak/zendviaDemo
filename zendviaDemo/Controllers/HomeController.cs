using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using zendviaDemo.Models;

namespace zendviaDemo.Controllers
{

    public class TSSRequest
    {
        public string numero_destino { get; set; }
        public string mensagem { get; set; }
        public string tipo_voz { get; set; }
        public string bina { get; set; }
        public bool resposta_usuario { get; set; }
        public bool gravar_audio { get; set; }
        public bool detecta_caixa { get; set; }
        public bool bina_inteligente { get; set; }
    }

    public class TSSDados
    {
        public int id { get; set; }
    }

    public class TSSResponse
    {
        public int status { get; set; }
        public bool sucesso { get; set; }
        public int motivo { get; set; }
        public string mensagem { get; set; }
        public TSSDados dados { get; set; }
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(string response = null)
        {
            ViewBag.Response = response;
            return View();
        }

        public async Task<IActionResult> Enviar([FromForm] TSSRequest request)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Access-Token", "75d55d5e3887ab48f574daec509dddbd");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var tSSRequest = new TSSRequest
                    {
                        numero_destino = request.numero_destino,
                        mensagem = request.mensagem,
                        resposta_usuario = true,
                        tipo_voz = "br-Vitoria",
                        bina = "4832830151",
                        gravar_audio = true,
                        detecta_caixa = true,
                        bina_inteligente = true
                    };

                    var url = "https://voice-app.zenvia.com/tts";
                    var response = await client.PostAsJsonAsync(url, tSSRequest);
                    var result = await response.Content.ReadAsStringAsync();
                    var objet = JsonConvert.DeserializeObject<TSSResponse>(result);
                    ViewBag.Response = JsonConvert.SerializeObject(objet);
                }
                catch (Exception ex)
                {
                    ;
                }
            }

            return RedirectToAction(nameof(Index), new { response = ViewBag.Response });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
