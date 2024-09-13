using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace XUnit3.Specs;

public class FrameworkSpecs
{
    [Fact]
    public void When_xunit3_is_used_it_should_throw_xunit_exceptions_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;

        // Don't reference the exception type explicitly like this: act.Should().Throw<XunitException>()
        // It could cause this specs project to load the assembly containing the exception (this actually happens for xUnit)
        exception.GetType().GetInterfaces().Select(e => e.Name).Should().Contain("IAssertionException");
        exception.GetType().FullName.Should().Be("Xunit.Sdk.XunitException");
    }

    [Fact]
    public async Task When_xunit3_is_used_it_should_throw_xunit_timeout_exceptions_for_complete_within_failures()
    {
        // Act
        Func<Task> delay = () => Task.Delay(100, TestContext.Current.CancellationToken);
        Func<Task> act = async () => await delay.Should().CompleteWithinAsync(TimeSpan.FromMilliseconds(10));

        // Assert
        Type exceptionType = (await act.Should().ThrowAsync<Exception>()).Which.GetType();

        // Assert
        exceptionType.GetInterfaces().Select(e => e.Name).Should().Contain("ITestTimeoutException");
        exceptionType.FullName.Should().Be("Xunit.Sdk.TestTimeoutException");
    }
}
