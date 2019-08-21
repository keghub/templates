#tool "nuget:?package=NuGet.CommandLine&version=5.1.0"

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

Task("Pack")
    .Does<BuildState>(state => 
{
    var settings = new NuGetPackSettings
    {
        OutputDirectory = state.Paths.OutputFolder
    };

    NuGetPack(state.Paths.NuSpecFile, settings);
});

Task("Full")
    .IsDependentOn("Pack");

RunTarget(target);


public class BuildState
{
    public BuildPaths Paths { get; set; }
}

public class BuildPaths
{
    public FilePath SolutionFile { get; set; }

    public FilePath NuSpecFile { get; set; }

    public DirectoryPath SolutionFolder => SolutionFile.GetDirectory();

    public DirectoryPath OutputFolder => SolutionFolder.Combine("outputs");
}