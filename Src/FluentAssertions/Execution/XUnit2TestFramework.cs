namespace FluentAssertions.Execution;

/// <summary>
/// Implements the xUnit v2 test framework adapter.
/// </summary>
internal class XUnit2TestFramework : XUnitTestFramework
{
    protected internal override string AssemblyName => "xunit.assert";
}
