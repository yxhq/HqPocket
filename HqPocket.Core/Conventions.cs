namespace HqPocket;

/// <summary>
/// 一些约定，一个View只能对应一个ViewModel，一个ViewModel可以有多个View
/// </summary>
public static class Conventions
{
    public static string ViewSuffix { get; set; } = "View";
    public static string ModelSuffix { get; set; } = "Model";
    public static string ViewModelSuffix { get; set; } = "ViewModel";
    public static string WindowViewSuffix { get; set; } = "Window";
    public static string ViewDirectory { get; set; } = "Views";
    public static string ViewModelDirectory { get; set; } = "ViewModels";
    public static string ViewDotFolder { get; } = $".{ViewDirectory}.";
    public static string ViewModelDotFolder { get; } = $".{ViewModelDirectory}.";
    public static string ResourceDirectory { get; } = "Resources";
    public static string SharedResourceName { get; } = "SharedResource";
    public static string ShellName { get; set; } = "MainWindow";
    public static string PluginsDirectory { get; set; } = "Plugins";
    public static string WritableJsonFile { get; set; } = "appsettings.json";
}
