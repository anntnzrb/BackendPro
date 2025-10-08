namespace BackendPro.Web.Models;

public class EmptyStateViewModel
{
    public string Icon { get; init; } = "";
    public string Title { get; init; } = "";
    public string Description { get; init; } = "";
    public string? ActionText { get; init; }
    public string? ActionUrl { get; init; }
}
