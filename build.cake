#tool "nuget:?package=NuGet.CommandLine&version=4.9.2"
#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"
//#addin "Cake.FileHelpers"

#load "./build/types.cake"

var target = Argument("Target", "Full");

Setup<BuildState>(_ => 
{
    var state = new BuildState
    {
        Paths = new BuildPaths
        {
            SolutionFile = MakeAbsolute(File("./Templates.sln")),
            NuSpecFile = MakeAbsolute(File("./EMG.Templates.nuspec"))
        }
    };

    CleanDirectory(state.Paths.OutputFolder);

    return state;
});

Task("Version")
    .Does<BuildState>(state =>
{
    var version = GitVersion();

    var packageVersion = version.SemVer;
    var buildVersion = $"{version.FullSemVer}+{DateTimeOffset.UtcNow:yyyyMMddHHmmss}";

    state.Version = new VersionInfo
    {
        PackageVersion = packageVersion,
        BuildVersion = buildVersion
    };

    Information($"Package version: {state.Version.PackageVersion}");
    Information($"Build version: {state.Version.BuildVersion}");

    if (BuildSystem.IsRunningOnTeamCity)
    {
        TeamCity.SetBuildNumber(state.Version.BuildVersion);
    }
});

Task("Pack")
    .IsDependentOn("Version")
    .Does<BuildState>(state => 
{
    var settings = new NuGetPackSettings
    {
        Version = state.Version.PackageVersion,
        OutputDirectory = state.Paths.OutputFolder
    };

    NuGetPack(state.Paths.NuSpecFile, settings);
});

Task("UploadPackagesToTeamCity")
    .IsDependentOn("Pack")
    .WithCriteria(BuildSystem.IsRunningOnTeamCity)
    .Does<BuildState>(state => 
{
    Information("Uploading packages to TeamCity");
    var files = GetFiles($"{state.Paths.OutputFolder}/*.nukpg");

    foreach (var file in files)
    {
        Information($"\tUploading {file.GetFilename()}");

        TeamCity.PublishArtifacts(file.FullPath);
    }
});

Task("UploadPackagesToMyGet")
    .IsDependentOn("Pack")
    .WithCriteria(BuildSystem.IsRunningOnTeamCity)
    .Does<BuildState>(state => 
{
    var apiKey = EnvironmentVariable("EMGPrivateApiKey");

    Information("Uploading packages to MyGet");
    var files = GetFiles($"{state.Paths.OutputFolder}/*.nukpg");

    foreach (var file in files)
    {
        Information($"\tUploading {file.GetFilename()}");

        NuGetPush(file, new NuGetPushSettings
        {
            ApiKey = apiKey,
            Source = "https://www.myget.org/F/emgprivate/api/v3/index.json"
        });
    }
});

Task("Push")
    .IsDependentOn("UploadPackagesToMyGet");

Task("Full")
    .IsDependentOn("Version")
    .IsDependentOn("Pack")
    .IsDependentOn("Push");

RunTarget(target);