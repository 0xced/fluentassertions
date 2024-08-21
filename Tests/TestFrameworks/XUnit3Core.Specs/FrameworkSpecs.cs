using System;
using FluentAssertions;
using Xunit;

namespace XUnit3Core.Specs;

public class FrameworkSpecs
{
    [Fact]
    public void When_xunit3_is_used_it_should_throw_xunit_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;
        exception.GetType().GetInterfaces().Should().ContainSingle(i => i.Name == "IAssertionException");
    }
}
