using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace XUnit3Core.Specs;

public class FrameworkSpecs
{
    [Fact]
    public void When_xunit3_without_xunit_assert_is_used_it_should_throw_IAssertionException_for_assertion_failures()
    {
        // Act
        Action act = () => 0.Should().Be(1);

        // Assert
        Exception exception = act.Should().Throw<Exception>().Which;
        exception.GetType().GetInterfaces().Select(e => e.Name).Should().Contain("IAssertionException");
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
    }
}
