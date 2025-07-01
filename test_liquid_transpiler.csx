#!/usr/bin/env dotnet-script
#r "nuget: StateSmith, 0.9.9-alpha"

using StateSmith.Runner;
using StateSmith.Output.UserConfig;
using StateSmith;

// Test the Liquid transpiler with simple BlankTemplate state machine
Console.WriteLine("Testing StateSmith Liquid Transpiler...");

// Configure the transpiler
var renderConfig = new RenderConfigLiquidVars();
Console.WriteLine($"Using feed syntax: {renderConfig.FeedPrefix}feed-name{renderConfig.FeedSuffix}");

// Run the transpiler
SmRunner runner = new(
    diagramPath: "src/test-misc/examples/BlankTemplate2/BlankTemplateSm.plantuml", 
    renderConfig: renderConfig, 
    transpilerId: TranspilerId.Liquid
);

try 
{
    runner.Run();
    Console.WriteLine("✅ Liquid transpiler completed successfully!");
    Console.WriteLine("Check output directory for generated .liquid file");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}