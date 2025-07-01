#!/usr/bin/env dotnet-script
#r "src/StateSmith/bin/Debug/net8.0/StateSmith.dll"

using StateSmith.Runner;
using StateSmith.Output.UserConfig;
using StateSmith;

Console.WriteLine("StateSmith Garden Watering Liquid Transpiler");
Console.WriteLine("===========================================");

try 
{
    // Configure Liquid output settings
    var renderConfig = new RenderConfigLiquidVars();
    renderConfig.FeedPrefix = "feeds['";
    renderConfig.FeedSuffix = "']";
    
    Console.WriteLine($"Feed syntax: {renderConfig.FeedPrefix}feed-name{renderConfig.FeedSuffix}");
    
    // Create output directory
    var outputDir = "examples/blockly-tool/generated/";
    Directory.CreateDirectory(outputDir);
    
    // Run StateSmith transpiler
    var smRunner = new SmRunner(
        diagramPath: "examples/blockly-tool/garden_watering.plantuml",
        renderConfig: renderConfig,
        transpilerId: TranspilerId.Liquid
    );
    
    smRunner.Settings.outputDirectory = outputDir;
    smRunner.Settings.outputCodeGenDirectory = outputDir;
    
    Console.WriteLine("Starting transpilation...");
    smRunner.Run();
    
    Console.WriteLine("✅ SUCCESS: Garden watering state machine transpiled to Liquid!");
    Console.WriteLine($"Output directory: {outputDir}");
    
    // List generated files
    var files = Directory.GetFiles(outputDir, "*.liquid");
    if (files.Length > 0)
    {
        Console.WriteLine("\nGenerated files:");
        foreach (var file in files)
        {
            Console.WriteLine($"  - {Path.GetFileName(file)}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ ERROR: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    Environment.Exit(1);
}