using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions.Common;

namespace FluentAssertions.Execution;

/// <summary>
/// Implements a wrapper around all supported test frameworks to throw the correct assertion exception.
/// </summary>
internal class TestFrameworkProvider(Configuration configuration)
{
    #region Private Definitions

    private static readonly Dictionary<string, ITestFramework> Frameworks = new(StringComparer.OrdinalIgnoreCase)
    {
        ["mspec"] = new LateBoundTestFramework("Machine.Specifications", "Machine.Specifications.SpecificationException"),
        ["nunit"] = new LateBoundTestFramework("nunit.framework", "NUnit.Framework.AssertionException"),
        ["mstestv2"] = new LateBoundTestFramework("Microsoft.VisualStudio.TestPlatform.TestFramework", "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"),

        // Keep xUnit last as it's likely to go through the catch path in the IsAvailable implementation
        ["xunit2"] = new LateBoundTestFramework("xunit.assert", "Xunit.Sdk.XunitException", loadAssembly: true),
        ["xunit3"] = new LateBoundTestFramework("xunit.v3.assert", "Xunit.Sdk.XunitException", loadAssembly: true),
    };

    private ITestFramework testFramework;

    #endregion

    [DoesNotReturn]
    public void Throw(string message)
    {
        testFramework ??= DetectFramework();
        testFramework.Throw(message);
    }

    private ITestFramework DetectFramework()
    {
        ITestFramework detectedFramework = AttemptToDetectUsingAppSetting()
            ?? AttemptToDetectUsingDynamicScanning()
            ?? new FallbackTestFramework();

        return detectedFramework;
    }

    private ITestFramework AttemptToDetectUsingAppSetting()
    {
        string frameworkName = configuration.TestFrameworkName;

        if (string.IsNullOrEmpty(frameworkName))
        {
            return null;
        }

        if (!Frameworks.TryGetValue(frameworkName, out ITestFramework framework))
        {
            string frameworks = string.Join(", ", Frameworks.Keys);

            var message =
                $"FluentAssertions was configured to use the test framework '{frameworkName}' but this is not supported. " +
                $"Please use one of the supported frameworks: {frameworks}.";

            throw new InvalidOperationException(message);
        }

        if (!framework.IsAvailable)
        {
            string frameworks = string.Join(", ", Frameworks.Keys);

            var innerMessage = framework is LateBoundTestFramework lateBoundTestFramework
                ? $"the required assembly '{lateBoundTestFramework.AssemblyName}' could not be found"
                : "it could not be found";

            var message =
                $"FluentAssertions was configured to use the test framework '{frameworkName}' but {innerMessage}. " +
                $"Please use one of the supported frameworks: {frameworks}.";

            throw new InvalidOperationException(message);
        }

        return framework;
    }

    private static ITestFramework AttemptToDetectUsingDynamicScanning()
    {
        return Frameworks.Values.FirstOrDefault(framework => framework.IsAvailable);
    }
}
