﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XOProject.Controller;

namespace XOProject.Tests
{
    class TradeControllerTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock = new Mock<ITradeRepository>();
        private readonly TradeController _tradeController;

        public TradeControllerTests()
        {
            _tradeController = new TradeController(_tradeRepositoryMock.Object);
        }

        [Test]
        public async Task Post_InsertNewTrade()
        {
            var newTrade = new Trade
            {
                NoOfShares = 100, PortfolioId = 1, Price = 10000,
                Symbol = "CBI", Action = "BUY"
            };
            
            var trade = await _tradeController.Post(newTrade);
            
            Assert.NotNull(trade);

            var createdResult = trade as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchAllTrade()
        {

            var trade = await _tradeController.Get();

            Assert.NotNull(trade);

            var foundResult = trade as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchAllPortfolioTrade()
        {
            int portfolioId = 1;
            var trade = await _tradeController.GetAllTradings(portfolioId);

            Assert.NotNull(trade);

            var foundResult = trade as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }


        [Test]
        public async Task Get_TradeAnalysis()
        {
            string symbol = "CBI";
            var trade = await _tradeController.GetAnalysis(symbol);

            Assert.NotNull(trade);

            var foundResult = trade as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

    }
}
