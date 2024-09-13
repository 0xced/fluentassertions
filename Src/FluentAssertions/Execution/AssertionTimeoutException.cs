using System;
using System.Runtime.Serialization;

namespace FluentAssertions.Execution;

#pragma warning disable CA1032, RCS1194 // AssertionTimeoutException should never be constructed with an empty message
public class AssertionTimeoutException : Exception, ITestTimeoutException
#pragma warning restore CA1032, RCS1194
{
    public AssertionTimeoutException(string message)
        : base(message)
    {
    }

    protected AssertionTimeoutException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
