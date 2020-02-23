using ApiTestingDemoFramework;
using NUnit.Framework;
using Shouldly;

namespace ApiTestingDemo
{
    [TestFixture]
    public class HttpTestByFramework
    {
        [Test]
        public void GetAllTours_ValidData_ShouldReturnTours()
        {
            HttpService httpService = new HttpService();
            httpService.Login("user", "pass");
            var tours = httpService.GetAllTours();

            tours.Count.ShouldBe(2);
        }
    }
}
