using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using XOProject.Controller;

namespace XOProject.Tests
{
    class PortfolioControllerTests
    {
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

        private readonly PortfolioController _portfolioController;

        public PortfolioControllerTests()
        {
            _portfolioController = new PortfolioController(_portfolioRepositoryMock.Object);
        }

        [Test]
        public async Task Post_InsertNewPortfolio()
        {
            var newPortfolio = new Portfolio
            {
                Name = "HARMONY",
            };
            var portfolio = await _portfolioController.Post(newPortfolio);

            Assert.NotNull(portfolio);

            var createdResult = portfolio as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }



        [Test]
        public async Task Get_FetchAllPortfolio()
        {

            var portfolio = await _portfolioController.Get();

            Assert.NotNull(portfolio);

            var foundResult = portfolio as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchSinglePortfolio()
        {
            int portfolioId = 1;
            var portfolio = await _portfolioController.GetPortfolioInfo(portfolioId);

            Assert.NotNull(portfolio);

            var foundResult = portfolio as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }
        

    }
}
