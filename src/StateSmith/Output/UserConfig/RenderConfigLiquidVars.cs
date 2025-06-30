using StateSmith.Output.UserConfig.AutoVars;

#nullable enable

namespace StateSmith.Output.UserConfig;

public class RenderConfigLiquidVars : RenderConfigBaseVars, IRenderConfigLiquid
{
    public RenderConfigLiquidVars()
    {
        AutoExpandedVars = new();
    }

    public AutoExpandedVarsProcessor AutoExpandedVars { get; set; }

    public string FeedPrefix { get; set; } = "feeds['";
    public string FeedSuffix { get; set; } = "']";
    public string StateVariablePrefix { get; set; } = "{{ ";
    public string StateVariableSuffix { get; set; } = " }}";
    public string EventPrefix { get; set; } = "{% if ";
    public string EventSuffix { get; set; } = " %}";
    public string EventEndTag { get; set; } = "{% endif %}";
    public string LoopPrefix { get; set; } = "{% for ";
    public string LoopSuffix { get; set; } = " %}";
    public string LoopEndTag { get; set; } = "{% endfor %}";
    public string CommentPrefix { get; set; } = "{% comment %}";
    public string CommentSuffix { get; set; } = "{% endcomment %}";
}