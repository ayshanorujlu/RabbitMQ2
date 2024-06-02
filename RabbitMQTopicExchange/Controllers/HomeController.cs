using Microsoft.AspNetCore.Mvc;
using RabbitMQTopicExchange.Models;
using RabbitMQTopicExchange.Services;
using System.Diagnostics;

namespace RabbitMQTopicExchange.Controllers
{
    public class HomeController : Controller
    {
        private readonly EchangeTopicService _service;

        public HomeController(EchangeTopicService service)
        {
            _service=service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(List<string> selecteds, string message)
        {

            try
            {
                await _service.SendMessage(selecteds.ToArray(), message);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message); ;
            }

        }
    }
}
