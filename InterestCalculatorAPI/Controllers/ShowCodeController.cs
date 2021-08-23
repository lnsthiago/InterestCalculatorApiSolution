using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterestCalculatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowCodeController : Controller
    {
        /// <summary>
        /// Busca URL do GitHub do projeto.
        /// </summary>
        /// <returns>Retorna com URL do GitHub do projeto</returns>
        /// <response code="200">Retorna com URL do GitHub do projeto</response>
        [HttpGet]
        [Route("/showmethecode")]
        public string Get()
        {
            return "https://github.com/lnsthiago/InterestCalculatorApiSolution/";
        }
    }
}
