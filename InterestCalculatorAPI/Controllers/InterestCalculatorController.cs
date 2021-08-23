using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace InterestCalculatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InterestCalculatorController : Controller
    {
        IConfiguration _configuration;
        private readonly HttpClient _client;

        public InterestCalculatorController(IConfiguration configuration, HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        /// <summary>
        /// Calcula taxa de juros.
        /// </summary>
        /// <returns>Retorna cálculo de juro baseado no Valor Inicial, taxa de Juro e tempo</returns>
        /// <response code="200">Retorna cálculo de juro baseado no Valor Inicial, taxa de Juro e tempo</response>
        /// <response code="500">Retorna erro com descrição da falha ao processar</response>
        /// <param name="initialValue">Valor Inicial</param>
        /// <param name="time">Tempo (Quantidade de Meses)</param>
        [HttpGet]
        [Route("/calculaJuros")]
        public async Task<IActionResult> GetAsync(decimal initialValue, int time)
        {
            string baseURL = _configuration.GetSection("RateFinderAPIConfig:BaseURL").Value;
            if (baseURL == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi configurado a chave 'BaseURL' para a API RateFinder");

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _client.GetAsync(baseURL + "taxaJuros");
            
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                var interest = decimal.Parse(result, CultureInfo.InvariantCulture);

                decimal finalValue = CalculateFinalValue(initialValue, time, interest);

                return Ok(finalValue.ToString("F"));
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível comunicar com API RateFinder");
        }

        private static decimal CalculateFinalValue(decimal initialValue, int time, decimal interest)
        {
            var finalValue = initialValue;

            for (int i = 0; i < time; i++)
                finalValue += (finalValue * interest);
            // Aqui poderia usar o método Math.Pow(), porém acho mais legivel da forma que eu fiz

            return finalValue;
        }
    }
}
