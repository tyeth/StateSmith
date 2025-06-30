#nullable enable

namespace StateSmith.Output.UserConfig;

public interface IRenderConfigLiquid : IRenderConfig
{
    string FeedPrefix { get; set; }
    string FeedSuffix { get; set; }
    string StateVariablePrefix { get; set; }
    string StateVariableSuffix { get; set; }
    string EventPrefix { get; set; }
    string EventSuffix { get; set; }
    string EventEndTag { get; set; }
    string LoopPrefix { get; set; }
    string LoopSuffix { get; set; }
    string LoopEndTag { get; set; }
    string CommentPrefix { get; set; }
    string CommentSuffix { get; set; }
}