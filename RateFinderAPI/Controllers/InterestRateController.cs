using Microsoft.AspNetCore.Mvc;

namespace RateFinderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InterestRateController : Controller
    {
        /// <summary>
        /// Busca taxa de juros.
        /// </summary>
        /// <returns>Retorna taxa de juro fixa em 0.01</returns>
        /// <response code="200">Retorna taxa de juro fixa em 0.01</response>
        [HttpGet]
        [Route("/taxaJuros")]
        public decimal Get()
        {
            return new decimal(0.01);
        }
    }
}
