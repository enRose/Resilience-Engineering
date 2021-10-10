using System;
using System.Net;
using peroline.FaultTolerance;
using Xunit;

namespace peroline.Tests
{
    public class HttpGetRetryTest
    {
        [Fact]
        public void Should()
        {
            var t = new Tunnel();

            t.RetryOn(HttpStatusCode.InternalServerError);

           

            Assert.False(true, "Customer age 16");
        }
    }
}
