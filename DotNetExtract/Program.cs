using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetExtract
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        private static void CreateDirectory(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static void WriteFile(string filePath, byte[] content)
        {
            CreateDirectory(filePath);
            File.WriteAllBytes(filePath, content);
        }

        public static int Main(string[] args)
        {
            var app = new CommandLineApplication();

            app.HelpOption();
            var optionSubject = app.Option("-s|--source <SOURCE>", "The assemblies folder source path. (Required)",
                CommandOptionType.SingleValue, x => x.IsRequired());
            var optionDestination = app.Option("-d|--destination <DESTINATION>",
                "The destination folder path. If you do not specify a path, the files are extracted in the 'current directory'.",
                CommandOptionType.SingleValue);
            var optionPattern = app.Option("-p|--pattern <PATTERN>",
                "Pattern of the names of the resources you want to extract so you should write regex on full resource name like 'RazorClassLibrary.Areas.Library.Pages.Shared._Message.cshtml'.",
                CommandOptionType.SingleValue);
            var optionReplace = app.Option("-r|--replace <REPLACE>",
                "Strategy how to replace files that already exist at the destination. default is 'replace all'. Other available options are 'ask|a' and 'skip|s'",
                CommandOptionType.SingleValue);
            var listOption = app.Option("-l|--list",
                "Get list of embedded resources. You can you regex pattern with this too.",
                CommandOptionType.NoValue);
            var extOption = app.Option("-e|--extension",
                "You can define your custom undetectable file extensions. The value is comma separated like '.txt.dat,.mkv,.cshtml.cs'",
                CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var replaceStrategy = ReplaceMode.Replace;
                var destination = optionDestination.HasValue()
                    ? optionDestination.Value()
                    : Environment.CurrentDirectory;
                var pattern = optionPattern.HasValue()
                    ? optionPattern.Value()
                    : "";
                var replaceMode = optionReplace.HasValue()
                    ? optionReplace.Value()
                    : "";
                var extensionValue = extOption.HasValue()
                    ? extOption.Value()
                    : "";
                var isInListMode = listOption.Values.Count > 0;

                var defaultExtensions = new List<string> {".cshtml.cs"};

                if (!string.IsNullOrEmpty(extensionValue))
                {
                    var exts = extensionValue.Split(',').Select(x => x.RemoveWhitespace());
                    foreach (var ext in exts)
                    {
                        if (!defaultExtensions.Contains(ext))
                        {
                            defaultExtensions.Add(ext);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(replaceMode))
                {
                    if (new[] {"ask", "a"}.Contains(replaceMode.ToLowerInvariant()))
                    {
                        replaceStrategy = ReplaceMode.Ask;
                    }

                    if (new[] {"skip", "s"}.Contains(replaceMode.ToLowerInvariant()))
                    {
                        replaceStrategy = ReplaceMode.Skip;
                    }
                }

                var results = Extractor.GetEmbeddedResources(optionSubject.Value(), pattern, defaultExtensions.ToArray());

                if (isInListMode)
                {
                    Console.Clear();
                    foreach (var result in results)
                    {
                        if (!string.IsNullOrEmpty(pattern))
                        {
                            Console.WriteLine($"- Pattern: {pattern}");
                        }

                        Console.WriteLine($"- Assembly: {result.Name}");
                        foreach (var resource in result.EmbeddedResources)
                        {
                            Console.WriteLine($" |- {resource.FullName}");
                        }

                        Console.WriteLine();
                    }

                    return 0;
                }

                foreach (var result in results)
                {
                    foreach (var resource in result.EmbeddedResources)
                    {
                        var folders = string.Join(Path.DirectorySeparatorChar, resource.Folders);
                        var resourcePath = string.Join(Path.DirectorySeparatorChar, folders,
                            $"{resource.Name}{resource.Extension}");
                        var path = string.Join(Path.DirectorySeparatorChar, destination, resourcePath);
                        var exists = File.Exists(path);
                        if (exists && replaceStrategy == ReplaceMode.Ask)
                        {
                            Console.Clear();
                            Console.WriteLine($"Assembly Name: {result.Name}");
                            Console.WriteLine($"Assembly Path: {result.Path}");
                            Console.WriteLine($"Resource Name: {resource.Name}{resource.Extension}");
                            Console.WriteLine($"Resource Path: {path}");
                            Console.Write($"Do you want to replace it? [yes (y), no (N), exit (e)] ");
                            var key = Console.ReadKey().KeyChar.ToString().ToLowerInvariant();
                            switch (key)
                            {
                                case "y":
                                    WriteFile(path, resource.Stream);
                                    break;
                                case "e":
                                    Environment.Exit(0);
                                    break;
                            }
                        }
                        else if (exists && replaceStrategy == ReplaceMode.Skip)
                        {
                            // ReSharper disable once RedundantJumpStatement
                            continue;
                        }
                        else
                        {
                            WriteFile(path, resource.Stream);
                        }
                    }
                }

                return 0;
            });

            return app.Execute(args);
        }
    }
}