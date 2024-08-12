using System;
using FluentAssertions;
using Machine.Specifications;

namespace MSpec.Specs;

[Subject("FrameworkSpecs")]
public class When_mspec_is_used
{
    Because of = () => Exception = Catch.Exception(() => 0.Should().Be(1));

    It should_fail = () => Exception.Should().BeAssignableTo<Exception>();
    It should_fail_with_a_specification_exception = () => Exception.GetType().FullName.Should().Be("Machine.Specifications.SpecificationException");

    private static Exception Exception;
}
