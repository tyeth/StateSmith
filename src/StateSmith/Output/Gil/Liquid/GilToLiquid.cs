using System.Text;
using StateSmith.Output.UserConfig;
using StateSmith.Runner;

#nullable enable

namespace StateSmith.Output.Gil.Liquid;

public class GilToLiquid : IGilTranspiler
{
    private readonly StringBuilder fileSb = new();

    private readonly ICodeFileWriter codeFileWriter;
    private readonly RenderConfigBaseVars renderConfig;
    private readonly RenderConfigLiquidVars renderConfigLiquid;
    private readonly IOutputInfo outputInfo;
    private readonly RoslynCompiler roslynCompiler;

    public GilToLiquid(IOutputInfo outputInfo, RenderConfigBaseVars renderConfig, RenderConfigLiquidVars renderConfigLiquid, ICodeFileWriter codeFileWriter, RoslynCompiler roslynCompiler)
    {
        this.outputInfo = outputInfo;
        this.renderConfig = renderConfig;
        this.codeFileWriter = codeFileWriter;
        this.renderConfigLiquid = renderConfigLiquid;
        this.roslynCompiler = roslynCompiler;
    }

    public void TranspileAndOutputCode(string gilCode)
    {
        LiquidGilVisitor visitor = new(gilCode, fileSb, renderConfig, renderConfigLiquid, roslynCompiler);
        visitor.Process();

        PostProcessor.PostProcess(fileSb);

        codeFileWriter.WriteFile($"{outputInfo.OutputDirectory}{outputInfo.BaseFileName}.liquid", code: fileSb.ToString());
    }
}