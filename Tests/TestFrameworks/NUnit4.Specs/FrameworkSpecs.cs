using System;
using FluentAssertions;
using NUnit.Framework;

namespace NUnit4.Specs;

[TestFixture]
public class FrameworkSpecs
{
    [Test]
    public void Throw_nunit_framework_exception_for_nunit4_tests()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;
        exception.GetType().FullName.Should().Be("NUnit.Framework.AssertionException");
    }
}
