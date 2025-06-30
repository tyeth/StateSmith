#nullable enable

using Xunit;
using StateSmith.Output.UserConfig;
using StateSmith.Output;
using StateSmith.Output.Gil.Liquid;
using FluentAssertions;

namespace StateSmithTest.Output.Gil;

public class GilToLiquidTests
{
    [Fact]
    public void Test_BasicLiquidOutput()
    {
        string gilCode = GilFileTestHelper.BuildExampleGilFile(skipIndentation: false, out var sm).ToString();

        RenderConfigLiquidVars renderConfig = new();

        OutputInfo outputInfo = new()
        {
            outputDirectory = TestHelper.GetThisDir()
        };

        CapturingCodeFileWriter capturingWriter = new();
        GilToLiquid transpiler = new(outputInfo, new(), renderConfig, capturingWriter, new());

        transpiler.TranspileAndOutputCode(gilCode);

        capturingWriter.LastCode.Should().Contain("{% comment %}");
        capturingWriter.LastCode.Should().Contain("{{ current_state }}");
        capturingWriter.LastCode.Should().Contain("feeds['temperature']");
    }

    [Fact]
    public void Test_CustomFeedConfiguration()
    {
        string gilCode = GilFileTestHelper.BuildExampleGilFile(skipIndentation: false, out var sm).ToString();

        RenderConfigLiquidVars renderConfig = new()
        {
            FeedPrefix = "sensor['",
            FeedSuffix = "'].value"
        };

        OutputInfo outputInfo = new()
        {
            outputDirectory = TestHelper.GetThisDir()
        };

        CapturingCodeFileWriter capturingWriter = new();
        GilToLiquid transpiler = new(outputInfo, new(), renderConfig, capturingWriter, new());

        transpiler.TranspileAndOutputCode(gilCode);

        capturingWriter.LastCode.Should().Contain("sensor['temperature'].value");
        capturingWriter.LastCode.Should().NotContain("feeds['temperature']");
    }

}