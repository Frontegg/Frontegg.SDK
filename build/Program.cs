using System;
using System.IO;
using static Bullseye.Targets;
using static SimpleExec.Command;
using Target = Bullseye.Internal.Target;

namespace Frontegg.SDK.Build
{
    class Program
    {
        private const string ArtifactsDir = "artifacts";
        private const string Clean = "clean";
        private const string Build = "build";
        private const string Test = "test";
        private const string Pack = "pack";
        private const string Publish = "publish";
        
        private static readonly string LogsDirectory = Path.Combine(ArtifactsDir, "logs");
        private static readonly string OutputDirectory = Path.Combine(ArtifactsDir, "output");
        
        static void Main(string[] args)
        {
            Target(Clean, () =>
            {
                if (Directory.Exists(ArtifactsDir))
                {
                    Directory.Delete(ArtifactsDir, true);
                }

                Directory.CreateDirectory(ArtifactsDir);
            });

            Target("logsDirectory", () => Directory.CreateDirectory(LogsDirectory));
            Target("outputDirectory", () => Directory.CreateDirectory(OutputDirectory));
            
            Target(Build, () => Run("dotnet", "build ./src/Frontegg.SDK.sln -c Release  /maxcpucount /nr:false /nologo"));
            
            Target(Test,
                DependsOn(Build),
                () => Run("dotnet", $"test ./src/aspnet/Frontegg.SDK.AspNet.Tests/Frontegg.SDK.AspNet.Tests.csproj -c Release -r {ArtifactsDir} --no-build -l trx;LogFileName=Frontegg.SDK.Aspney.Tests.xml --verbosity=normal")
                );
            
            Target(
                Pack,
                DependsOn(Build),
                () => Run("dotnet", $"pack src/aspnet/Frontegg.SDK.AspNet/Frontegg.SDK.AspNet.csproj -c Release -o {ArtifactsDir} --no-build")
                );
            
            Target(Publish, DependsOn(Pack), () =>
            {
                // var packagesToPush = Directory.GetFiles(ArtifactsDir, "*.nupkg", SearchOption.TopDirectoryOnly);
                // Console.WriteLine($"Found packages to publish: {string.Join("; ", packagesToPush)}");
                //
                // var apiKey = Environment.GetEnvironmentVariable("FRONTEGG_API_KEY");
                // if (string.IsNullOrWhiteSpace(apiKey))
                // {
                //     Console.WriteLine("Frontegg API Key not available. No packages will be pushed.");
                //     return;
                // }
                // Console.WriteLine($"Feedz API Key ({apiKey.Substring(0,5)}) available. Pushing packages to Nuget...");
                // foreach (var packageToPush in packagesToPush)
                // {
                //     Run("dotnet", $"nuget push {packageToPush} -k {apiKey} --skip-duplicate", noEcho: true);
                // }
            });
            
            Target("default", DependsOn(Clean, Test));
            
            RunTargetsAndExit(args);
        }
    }
}