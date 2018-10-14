using System;
using System.Threading.Tasks;
using XOProject.Controller;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace XOProject.Tests
{
    public class ShareControllerTests
    {
        private readonly Mock<IShareRepository> _shareRepositoryMock = new Mock<IShareRepository>();

        private readonly ShareController _shareController;

        public ShareControllerTests()
        {
            _shareController = new ShareController(_shareRepositoryMock.Object);
        }

        [Test]
        public async Task Post_ShouldInsertHourlySharePrice()
        {
            var hourRate = new HourlyShareRate
            {
                Symbol = "CBI",
                Rate = 330.0M,
                TimeStamp = new DateTime(2018, 08, 17, 5, 0, 0)
            };

            // Arrange

            // Act
            var result = await _shareController.Post(hourRate);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchAllShares()
        {

            var share = await _shareController.Get();

            Assert.NotNull(share);

            var foundResult = share as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }

        [Test]
        public async Task Get_FetchShareBySymbol()
        {
            string symbol = "CBI";
            var share = await _shareController.GetShareBySymbol(symbol);

            Assert.NotNull(share);

            var foundResult = share as OkObjectResult;
            Assert.NotNull(foundResult);
            Assert.AreEqual(200, foundResult.StatusCode);
        }
        
    }
}
