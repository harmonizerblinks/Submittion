using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace XOProject.Controller
{
    [Route("api/Trade/")]
    public class TradeController : ControllerBase
    {
        private IShareRepository _shareRepository { get; set; }
        private ITradeRepository _tradeRepository { get; set; }
        private IPortfolioRepository _portfolioRepository { get; set; }

        public TradeController(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var trades = _tradeRepository.Query();

            return Ok(trades);
        }
        
        [HttpGet("{tradeId}")]
        public async Task<IActionResult> GetPortfolioInfo([FromRoute]int tradeId)
        {
            var trade = _tradeRepository.Query().Where(x => x.Id.Equals(tradeId));

            return Ok(trade);
        }

        [HttpGet("{portfolioid}")]
        public async Task<IActionResult> GetAllTradings([FromRoute]int portFolioid)
        {
            var trade = _tradeRepository.Query().Where(x => x.PortfolioId.Equals(portFolioid));
            return Ok(trade);
        }

        /// <summary>
        /// For a given symbol of share, get the statistics for that particular share calculating the maximum, minimum, 
        /// average and Sum of all the trades for that share individually grouped into Buy trade and Sell trade.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>

        [HttpGet("Analysis/{symbol}")]
        public async Task<IActionResult> GetAnalysis([FromRoute]string symbol)
        {
            var result = new List<TradeAnalysis>();
            var value = _tradeRepository.Query().Where(s => s.Symbol.Equals(symbol)).OrderBy(n => n.NoOfShares).ToList();

            string type = "BUY";

            for (int i = 0; i < 2; i++)
            {
                var check = value.Where(a => a.Action.Equals(type)).Count();
                if(check > 0)
                {
                    result.Add(new TradeAnalysis
                    {
                        Sum = value.Where(a => a.Action.Equals(type)).Select(n => n.NoOfShares).Sum(),
                        Maximum = value.Where(a => a.Action.Equals(type)).Select(n => n.NoOfShares).Max(),
                        Minimum = value.Where(a => a.Action.Equals(type)).Select(n => n.NoOfShares).Min(),
                        Average = value.Where(a => a.Action.Equals(type)).Select(n => n.NoOfShares).Average(),
                        Action = type
                    });
                }
                type = "SELL";
            }
            
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Trade value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tradeRepository.InsertAsync(value);

            return Created($"Trade/{value.Id}", value);
        }
    }
}
