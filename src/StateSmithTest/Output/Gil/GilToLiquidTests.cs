using StateSmith.Output.Gil.Liquid;
using StateSmith.Output.UserConfig;
using StateSmith.Output;
using StateSmith.Runner;
using StateSmithTest.Output;
using System.Text;
using Xunit;

#nullable enable

namespace StateSmithTest.Output.Gil;

public class GilToLiquidTests
{
    [Fact]
    public void Test_BasicLiquidOutput()
    {
        // Arrange
        var renderConfig = new RenderConfigBaseVars();
        var renderConfigLiquid = new RenderConfigLiquidVars();
        var codeFileWriter = new CapturingCodeFileWriter();
        var outputInfo = new OutputInfo()
        {
            OutputDirectory = "output/",
            BaseFileName = "TestSm"
        };
        var roslynCompiler = new RoslynCompiler();

        string gilCode = @"
        public class TestSm
        {
            public void Start()
            {
            }
        }";

        var transpiler = new GilToLiquid(outputInfo, renderConfig, renderConfigLiquid, codeFileWriter, roslynCompiler);

        // Act
        transpiler.TranspileAndOutputCode(gilCode);

        // Assert
        var result = codeFileWriter.GetSingleOutputFile();
        Assert.Equal("output/TestSm.liquid", result.FilePath);
        Assert.Contains("{% comment %}", result.Code);
        Assert.Contains("{{ current_state }}", result.Code);
        Assert.Contains("feeds['temperature']", result.Code);
        Assert.Contains("{% if event_triggered %}", result.Code);
        Assert.Contains("{% for transition in transitions %}", result.Code);
    }

    [Fact]
    public void Test_AdafruitIOFeedIntegration()
    {
        // Arrange
        var renderConfig = new RenderConfigBaseVars();
        var renderConfigLiquid = new RenderConfigLiquidVars();
        var codeFileWriter = new CapturingCodeFileWriter();
        var outputInfo = new OutputInfo()
        {
            OutputDirectory = "",
            BaseFileName = "IoSm"
        };
        var roslynCompiler = new RoslynCompiler();

        string gilCode = @"
        public class IoSm
        {
            public void ProcessTemperature()
            {
            }
        }";

        var transpiler = new GilToLiquid(outputInfo, renderConfig, renderConfigLiquid, codeFileWriter, roslynCompiler);

        // Act
        transpiler.TranspileAndOutputCode(gilCode);

        // Assert
        var result = codeFileWriter.GetSingleOutputFile();
        Assert.Contains("feeds['temperature']", result.Code);
        Assert.Contains("feeds['humidity']", result.Code);
        Assert.Contains("feeds['button-state']", result.Code);
        Assert.Contains("{% if feeds['temperature'] > 25 %}", result.Code);
    }

    [Fact]
    public void Test_CustomLiquidConfiguration()
    {
        // Arrange
        var renderConfig = new RenderConfigBaseVars();
        var renderConfigLiquid = new RenderConfigLiquidVars()
        {
            FeedPrefix = "sensor['",
            FeedSuffix = "'].value",
            StateVariablePrefix = "{{< ",
            StateVariableSuffix = " >}}"
        };
        var codeFileWriter = new CapturingCodeFileWriter();
        var outputInfo = new OutputInfo()
        {
            OutputDirectory = "",
            BaseFileName = "CustomSm"
        };
        var roslynCompiler = new RoslynCompiler();

        string gilCode = @"
        public class CustomSm
        {
        }";

        var transpiler = new GilToLiquid(outputInfo, renderConfig, renderConfigLiquid, codeFileWriter, roslynCompiler);

        // Act
        transpiler.TranspileAndOutputCode(gilCode);

        // Assert
        var result = codeFileWriter.GetSingleOutputFile();
        Assert.Contains("sensor['temperature'].value", result.Code);
        Assert.Contains("{{< current_state >}}", result.Code);
    }
}