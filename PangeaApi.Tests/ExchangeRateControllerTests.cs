using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pangea.Api.Controllers;
using Pangea.Models;

namespace Pangea.Api.Tests;


// TODO if I had more time I'd write tests for the ingest service as well

public class ExchangeRateControllerTests
{
    [Fact]
    public void GetByCountry_Invalid_Country_ReturnsBadRequest()
    {
        var mockSet = new Mock<DbSet<PartnerRate>>();
        var mockContext = new Mock<PangeaContext>();
        mockContext.Setup(m => m.PartnerRates).Returns(mockSet.Object);

        var _controller = new ExchangeRateController(mockContext.Object);

        var response = _controller.Get("BLAH");
        Assert.IsType<BadRequestObjectResult>(response.Result);
    }

    // if you pass in a legit country code, endpoint should return only the Exchange Rates
    // that match the country code
    [Fact]
    public void GetByCountry_ValidCountry_ReturnsMatchingExchangeRates()
    {
        // TODO put this in a reusable helper method
        var data = GetMockPartnerRatesAllCountries().AsQueryable();
        var mockSet = new Mock<DbSet<PartnerRate>>();
        mockSet.As<IQueryable<PartnerRate>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<PartnerRate>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<PartnerRate>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<PartnerRate>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
        var mockContext = new Mock<PangeaContext>();
        mockContext.Setup(c => c.PartnerRates).Returns(mockSet.Object);
        var _controller = new ExchangeRateController(mockContext.Object);

        var validCountryCode = "MEX";
        var response = _controller.Get(validCountryCode);

        Assert.Single(response.Value!);
    }

    // TODO add test for enforcing the rounding behavior in the Partner -> Exchange conversion logic 
    private static List<PartnerRate> GetMockPartnerRatesAllCountries()
    {
        return new List<PartnerRate>()
        {
            new PartnerRate
            {
                Currency = "MXN",
                PaymentMethod = "debit",
                DeliveryMethod = "cash",
                Rate = 12.34m,
                AcquiredDate = new DateTime()
            },
            new PartnerRate
            {
                Currency = "PHP",
                PaymentMethod = "debit",
                DeliveryMethod = "cash",
                Rate = 23.45m,
                AcquiredDate = new DateTime()
            },
            new PartnerRate
            {
                Currency = "GTQ",
                PaymentMethod = "debit",
                DeliveryMethod = "cash",
                Rate = 34.56m,
                AcquiredDate = new DateTime()
            },
            new PartnerRate
            {
                Currency = "INR",
                PaymentMethod = "debit",
                DeliveryMethod = "cash",
                Rate = 45.67m,
                AcquiredDate = new DateTime()
            },
        };
    }
}