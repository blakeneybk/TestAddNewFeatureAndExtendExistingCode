var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

var slnFile = "./Litmus.Interview.sln";

Task("Clean")
    .Does(() =>
{
    DotNetCoreClean(slnFile);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild(slnFile, new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest(slnFile, new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

RunTarget(target);