using DotNetExtract.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace DotNetExtract
{
    public static class Extractor
    {
        private static bool HasExtension(string name, string[] extensions, out string extension)
        {
            extension = "";
            if (extensions == null || extensions.Length == 0) return false;
            foreach (var ext in extensions)
            {
                if (name.EndsWith(ext))
                {
                    extension = ext;
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<AssemblyInfo> GetEmbeddedResources(string folderPath, string regexPattern = "",
            string[] customExtensions = null)
        {
            var result = new List<AssemblyInfo>();
            foreach (var file in Directory.EnumerateFiles(folderPath, "*.dll", SearchOption.AllDirectories).ToList())
            {
                try
                {
                    string hash;
                    using (var sha256Hash = SHA256.Create())
                    {
                        var checksum = sha256Hash.ComputeHash(File.ReadAllBytes(file));
                        hash = BitConverter.ToString(checksum).Replace("-", string.Empty).ToLower();
                    }

                    var hashExist = result.Any(x => x.Hash == hash);
                    if (hashExist) continue;

                    var assembly = Assembly.LoadFrom(file);
                    var assemblyName = assembly.GetName().Name;
                    var resources = new List<EmbeddedResourceInfo>();
                    var assemblyNames = (string[]) assembly.GetManifestResourceNames();

                    if (assemblyNames.Length == 0) continue;

                    foreach (var name in assemblyNames)
                    {
                        var stream = assembly.GetManifestResourceStream(name);
                        var bytes = stream.ToBytes();

                        var ext = "";
                        var fileName = "";
                        var folder = new List<string>();
                        var hasExtension = HasExtension(name, customExtensions, out var extension);
                        if (hasExtension)
                        {
                            ext = extension[0] == '.' ? extension : $".{extension}";
                            var fileWithoutExtension = name.Substring(0, name.LastIndexOf(ext));
                            var fileWithName = fileWithoutExtension.Remove(0,assemblyName.Length + 1);
                            var split = fileWithName.Split('.');
                            fileName = split.LastOrDefault();
                            folder = split.Take(split.Length - 1).ToList();
                        }
                        else
                        {
                            var fileWithName = name.Remove(0,assemblyName.Length + 1);
                            var split = fileWithName.Split('.');
                            ext = $".{split.LastOrDefault()}";
                            fileName = split[split.Length - 2];
                            folder = split.Take(split.Length - 2).ToList();
                        }
                        
                        if (string.IsNullOrEmpty(regexPattern) || name.IsMatch(regexPattern))
                        {
                            resources.Add(new EmbeddedResourceInfo()
                            {
                                Stream = bytes,
                                FullName = name,
                                Name = fileName,
                                Extension = ext,
                                Folders = folder.ToArray()
                            });
                        }
                    }

                    if (resources.Any())
                    {
                        result.Add(new AssemblyInfo
                        {
                            Name = assemblyName + ".dll",
                            Path = file,
                            Hash = hash,
                            EmbeddedResources = resources
                        });
                    }
                }
                catch
                {
                }
            }

            return result;
        }
    }
}