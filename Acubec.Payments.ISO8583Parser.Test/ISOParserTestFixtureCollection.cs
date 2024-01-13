using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Test;

[Collection("ISO8583ParserTestCollection")]
public class ISO8583ParserTestFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; private set; }
    public ISO8583ParserTestFixture()
    {
        //_serviceProvider.UseUt
        var provider = new ISOParserTestFixtureCollection();
        this.ServiceProvider = provider.ServiceProvider;
    }
    // Setup logic: You can initialize objects, set up dependencies, etc. in the constructor or in other methods

    // Cleanup logic: Implement IDisposable.Dispose() to clean up any resources used by the test fixture
    public void Dispose()
    {
        // Cleanup code here
        GC.SuppressFinalize(this);
    }
}

[Collection("ISO8583ParserTestCollection")]
[CollectionDefinition("ServiceProvider")]
public class ISOParserTestFixtureCollection: ICollectionFixture<ISO8583ParserTestFixture>
{
    private readonly IServiceCollection _services;
    private readonly IServiceProvider serviceProvider;

    public ISOParserTestFixtureCollection()
    {
        _services = new ServiceCollection();
        _services.AddISO8583Parser();
        _services.AddSingleton<IServiceProvider>((c) => ServiceProvider);
        serviceProvider = _services.BuildServiceProvider();
    }


    public IServiceProvider ServiceProvider => serviceProvider;
}
