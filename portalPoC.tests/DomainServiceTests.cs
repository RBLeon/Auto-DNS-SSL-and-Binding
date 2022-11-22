using Moq;
using portalPoC.lib.Models;
using portalPoC.lib.Services;
using portalPoC.lib.Services.Interfaces;

namespace portalPoC.tests
{
    //Always 1. Arrange, 2. Act, 3. Assert
    //test functions that (don't have obvious returns || have multiple return paths)
    public class DomainServiceTests
    {
        [Fact]
        public async Task DoesNotThrowOnException()
        {
            var TransipMock = new Mock<ITransipService>();
            var LesslMock = new Mock<ILesslService>();
            var BindingMock = new Mock<IBindingService>();
            TransipMock.Setup<DnsEntryResult>(m => m.AddDnsEntryAsync(It.Is<DomainModel>((d) => d == null)).Result)
                .Throws(() => new ArgumentNullException("WHYYYYYY"));

            var sut = new DomainService(TransipMock.Object, LesslMock.Object, BindingMock.Object);

            await sut.CreateDomainAsync(null);
        }

        [Fact]
        public async Task ReturnsSuccessAsExpected()
        {
            var TransipMock = new Mock<ITransipService>();
            var LesslMock = new Mock<ILesslService>();
            var BindingMock = new Mock<IBindingService>();
            TransipMock.Setup<DnsEntryResult>(m => m.AddDnsEntryAsync(It.IsNotNull<DomainModel>()).Result)
                .Returns(new DnsEntryResult { IsSuccess = true, Message = "test1" });

            var sut = new DomainService(TransipMock.Object, LesslMock.Object, BindingMock.Object);

            var result = await sut.CreateDomainAsync(new DomainModel());

            Assert.True(result.IsSuccess);
            Assert.Equal("registration successful", result.Message);
        }

        [Fact]
        public async Task ReturnsErrorAsExpected()
        {
            var TransipMock = new Mock<ITransipService>();
            var LesslMock = new Mock<ILesslService>();
            var BindingMock = new Mock<IBindingService>();
            TransipMock.Setup<DnsEntryResult>(m => m.AddDnsEntryAsync(It.IsNotNull<DomainModel>()).Result)
                .Returns(new DnsEntryResult { IsSuccess = false, Message = "THIS IS KAPUT!" });

            var sut = new DomainService(TransipMock.Object, LesslMock.Object, BindingMock.Object);

            var result = await sut.CreateDomainAsync(new DomainModel());

            //$"registration failed with errorcode: {transipResult.Message}";

            Assert.False(result.IsSuccess);
            Assert.Equal("registration failed with errorcode: THIS IS KAPUT!", result.Message);
        }
    }
}