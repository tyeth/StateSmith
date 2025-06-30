using System.Text;
using StateSmith.Output.UserConfig;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace StateSmith.Output.Gil.Liquid;

public class LiquidGilVisitor
{
    private readonly StringBuilder sb;
    private readonly RenderConfigBaseVars renderConfig;
    private readonly RenderConfigLiquidVars renderConfigLiquid;
    private readonly RoslynCompiler roslynCompiler;
    private readonly SyntaxTree syntaxTree;

    public LiquidGilVisitor(string gilCode, StringBuilder sb, RenderConfigBaseVars renderConfig, RenderConfigLiquidVars renderConfigLiquid, RoslynCompiler roslynCompiler)
    {
        this.sb = sb;
        this.renderConfig = renderConfig;
        this.renderConfigLiquid = renderConfigLiquid;
        this.roslynCompiler = roslynCompiler;
        this.syntaxTree = CSharpSyntaxTree.ParseText(gilCode);
    }

    public void Process()
    {
        var root = syntaxTree.GetRoot();
        
        // Generate Liquid template header
        AppendCommentHeader();
        
        // Process the state machine structure
        ProcessStateMachine(root);
    }

    private void AppendCommentHeader()
    {
        sb.AppendLine($"{renderConfigLiquid.CommentPrefix}");
        sb.AppendLine($"Generated Liquid Template for {renderConfig.SmName}");
        sb.AppendLine($"StateSmith Version: {renderConfig.StateSmithVersion}");
        sb.AppendLine($"Compatible with Adafruit IO Blockly Actions");
        sb.AppendLine($"{renderConfigLiquid.CommentSuffix}");
        sb.AppendLine();
    }

    private void ProcessStateMachine(SyntaxNode root)
    {
        // Find state machine class
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
        if (classDeclaration == null) return;

        // Generate state machine template structure
        GenerateStateMachineTemplate(classDeclaration);
    }

    private void GenerateStateMachineTemplate(ClassDeclarationSyntax classDeclaration)
    {
        // Generate state machine initialization
        sb.AppendLine($"{renderConfigLiquid.CommentPrefix} State Machine: {classDeclaration.Identifier.ValueText} {renderConfigLiquid.CommentSuffix}");
        sb.AppendLine();

        // Generate state variable output
        sb.AppendLine($"{renderConfigLiquid.StateVariablePrefix}current_state{renderConfigLiquid.StateVariableSuffix}");
        sb.AppendLine();

        // Generate event handling template
        GenerateEventHandling();

        // Generate state transitions
        GenerateStateTransitions();

        // Generate feed integration examples
        GenerateFeedIntegration();
    }

    private void GenerateEventHandling()
    {
        sb.AppendLine($"{renderConfigLiquid.CommentPrefix} Event Handling {renderConfigLiquid.CommentSuffix}");
        sb.AppendLine($"{renderConfigLiquid.EventPrefix}event_triggered{renderConfigLiquid.EventSuffix}");
        sb.AppendLine($"  Event: {renderConfigLiquid.StateVariablePrefix}event_name{renderConfigLiquid.StateVariableSuffix}");
        sb.AppendLine($"{renderConfigLiquid.EventEndTag}");
        sb.AppendLine();
    }

    private void GenerateStateTransitions()
    {
        sb.AppendLine($"{renderConfigLiquid.CommentPrefix} State Transitions {renderConfigLiquid.CommentSuffix}");
        sb.AppendLine($"{renderConfigLiquid.LoopPrefix}transition in transitions{renderConfigLiquid.LoopSuffix}");
        sb.AppendLine($"  From: {renderConfigLiquid.StateVariablePrefix}transition.from{renderConfigLiquid.StateVariableSuffix}");
        sb.AppendLine($"  To: {renderConfigLiquid.StateVariablePrefix}transition.to{renderConfigLiquid.StateVariableSuffix}");
        sb.AppendLine($"  Event: {renderConfigLiquid.StateVariablePrefix}transition.event{renderConfigLiquid.StateVariableSuffix}");
        sb.AppendLine($"{renderConfigLiquid.LoopEndTag}");
        sb.AppendLine();
    }

    private void GenerateFeedIntegration()
    {
        sb.AppendLine($"{renderConfigLiquid.CommentPrefix} Adafruit IO Feed Integration {renderConfigLiquid.CommentSuffix}");
        sb.AppendLine($"Temperature: {renderConfigLiquid.FeedPrefix}temperature{renderConfigLiquid.FeedSuffix}");
        sb.AppendLine($"Humidity: {renderConfigLiquid.FeedPrefix}humidity{renderConfigLiquid.FeedSuffix}");
        sb.AppendLine($"Button State: {renderConfigLiquid.FeedPrefix}button-state{renderConfigLiquid.FeedSuffix}");
        sb.AppendLine();
        
        // Conditional feed handling
        sb.AppendLine($"{renderConfigLiquid.EventPrefix}{renderConfigLiquid.FeedPrefix}temperature{renderConfigLiquid.FeedSuffix} > 25{renderConfigLiquid.EventSuffix}");
        sb.AppendLine($"  High temperature detected: {renderConfigLiquid.StateVariablePrefix}{renderConfigLiquid.FeedPrefix}temperature{renderConfigLiquid.FeedSuffix}{renderConfigLiquid.StateVariableSuffix}");
        sb.AppendLine($"{renderConfigLiquid.EventEndTag}");
    }
}