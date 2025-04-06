using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using XBank.FileMonitor.Core.Interfaces;
using Serilog;

namespace XBank.FileMonitor.App.Services
{
    public static class PluginLoader
    {
        public static IEnumerable<ILoader> LoadPlugins(string pluginsDirectory, IEnumerable<string> activeLoaders)
        {
            var result = new List<ILoader>();

            if (!Directory.Exists(pluginsDirectory))
                return result;

            var dllFiles = Directory.GetFiles(pluginsDirectory, "*.dll", SearchOption.TopDirectoryOnly);

            foreach (var dll in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dll);

                    var loaderTypes = assembly.GetTypes()
                        .Where(t => typeof(ILoader).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in loaderTypes)
                    {
                        var instance = Activator.CreateInstance(type) as ILoader;

                        if (instance != null && activeLoaders.Contains(instance.FileExtension, StringComparer.OrdinalIgnoreCase))
                        {
                            result.Add(instance);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to load plugin from file: {PluginFile}", dll);
                    continue;
                }
            }

            return result;
        }
    }
}
