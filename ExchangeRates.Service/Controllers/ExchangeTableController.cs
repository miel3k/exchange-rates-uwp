using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRates.Model;

namespace ExchangeRates.Service.Controllers
{
    [Route("api/exchangeRates")]
    public class ExchangeTableController : Controller
    {
        private IExchangeTableRepository _repository;

        public ExchangeTableController(IExchangeTableRepository repository)
            => _repository = repository;

        [HttpGet("{date}")]
        public async Task<IActionResult> Get(DateTime date)
        {
            var exchangeTable = await _repository.GetAsync(date);
            if (exchangeTable == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(exchangeTable);
            }
        }
    }
}
