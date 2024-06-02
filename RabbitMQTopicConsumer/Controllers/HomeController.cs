using Microsoft.AspNetCore.Mvc;
using RabbitMQTopicConsumer.Models;
using RabbitMQTopicConsumer.Services;
using System.Diagnostics;

namespace RabbitMQTopicConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly TopicConsumerService _consumerService;

        public HomeController(TopicConsumerService consumerService)
        {
            _consumerService=consumerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveSelects(string[] selects)
        {
            try
            {
                _consumerService.ConnectPatterns(selects);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
