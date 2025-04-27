namespace Banned.Logger.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        var logger = new Logger(option =>
        {
            option.Name           = "TestName";
            option.WriteOnConsole = true;
            option.BaseDirectory  = "log";
        });
        await logger.InfoAsync("test info");
    }
}