using RateFinderAPI.Controllers;
using Xunit;

namespace RateFinderAPITest.Controllers
{
    public class InterestRateControllerTest
    {
        InterestRateController _interestRateController;

        public InterestRateControllerTest()
        {
            _interestRateController = new InterestRateController();
        }

        [Fact]
        public void ShouldReturnRateRate001()
        {
            // Act
            var resultRate = _interestRateController.Get();

            // Assert
            Assert.Equal(new decimal(0.01), resultRate);
        }
    }
}
