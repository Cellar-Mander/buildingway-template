using System;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using MyPlugin.Core;

namespace MyPlugin.Windows;

public sealed class ConfigWindow : Window, IDisposable
{
    // Dragging the slider changes the value every frame; save once it has settled
    private const long SaveDelayMs = 500;

    private readonly Configuration configuration;
    private readonly SaveDebouncer saveDebouncer = new(SaveDelayMs);

    public ConfigWindow(Plugin plugin) : base("My Plugin Display Name###MyPluginConfig")
    {
        Flags = ImGuiWindowFlags.AlwaysAutoResize;
        configuration = plugin.Configuration;
    }

    public void Dispose() => SaveUnsaved();

    public override void OnClose() => SaveUnsaved();

    public override void Draw()
    {
        var value = configuration.ExampleValue;
        if (ImGui.SliderInt("Example value", ref value, ConfigurationBase.MinExampleValue, ConfigurationBase.MaxExampleValue))
        {
            configuration.ExampleValue = value;
            saveDebouncer.Changed(Environment.TickCount64);
        }

        if (saveDebouncer.ShouldSave(Environment.TickCount64))
        {
            configuration.Save();
        }
    }

    private void SaveUnsaved()
    {
        if (saveDebouncer.Flush())
        {
            configuration.Save();
        }
    }
}
