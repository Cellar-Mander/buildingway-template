using System;
using Dalamud.Configuration;
using MyPlugin.Core;

namespace MyPlugin;

[Serializable]
public class Configuration : ConfigurationBase, IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public void Save() => Plugin.PluginInterface.SavePluginConfig(this);
}
