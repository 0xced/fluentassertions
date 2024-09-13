namespace FluentAssertions.Execution;

/// <summary>
/// Implements the xUnit v3 test framework adapter.
/// </summary>
internal class XUnit3TestFramework : XUnitTestFramework
{
    protected internal override string AssemblyName => "xunit.v3.assert";

    protected override string TimeoutExceptionFullName => "Xunit.Sdk.TestTimeoutException";
}
