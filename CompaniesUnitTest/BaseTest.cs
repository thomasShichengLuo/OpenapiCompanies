using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.TestHost;


namespace CompaniesUnitTest
{
    public abstract class BaseTest
    {
        protected internal readonly TestServer _testServer;

        public readonly IFlurlClient flurlClient;

        protected readonly TestServerFixture _testServerFixture;

        protected BaseTest(TestServerFixture testServerFixture)
        {
            _testServerFixture = testServerFixture;
            _testServer = testServerFixture.Server;
            flurlClient = new FlurlClient(testServerFixture.CreateDefaultClient(new Uri("http://localhost:5265")));
        }

    }
}
