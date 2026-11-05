using System;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using MyPlugin.Core;

namespace MyPlugin.Windows;

public sealed class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;

    public ConfigWindow(Plugin plugin) : base("My Plugin Display Name###MyPluginConfig")
    {
        Flags = ImGuiWindowFlags.AlwaysAutoResize;
        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        var value = configuration.ExampleValue;
        if (ImGui.SliderInt("Example value", ref value, ConfigurationBase.MinExampleValue, ConfigurationBase.MaxExampleValue))
        {
            configuration.ExampleValue = value;
            configuration.Save();
        }
    }
}
