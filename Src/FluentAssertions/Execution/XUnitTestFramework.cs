namespace FluentAssertions.Execution;

/// <summary>
/// Implements the xUnit (version 2 and 3) test framework adapter.
/// </summary>
internal abstract class XUnitTestFramework() : LateBoundTestFramework(loadAssembly: true)
{
    protected override string ExceptionFullName => "Xunit.Sdk.XunitException";
}
