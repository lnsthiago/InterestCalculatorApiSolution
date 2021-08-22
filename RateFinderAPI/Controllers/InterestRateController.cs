using Microsoft.AspNetCore.Mvc;

namespace RateFinderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InterestRateController : Controller
    {
        [HttpGet]
        [Route("/taxaJuros")]
        public decimal Get()
        {
            return new decimal(0.01);
        }
    }
}
