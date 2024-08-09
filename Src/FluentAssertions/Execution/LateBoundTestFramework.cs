using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAssertions.Execution;

internal class LateBoundTestFramework(string assemblyName, string exceptionFullName, bool loadAssembly = false) : ITestFramework
{
    private Func<string, Exception> exceptionFactory;

    public string AssemblyName => assemblyName;

    [DoesNotReturn]
    public void Throw(string message) => throw exceptionFactory(message);

    public bool IsAvailable
    {
        get
        {
            var assembly = GetAssembly();
            var exceptionType = assembly?.GetType(exceptionFullName);
            exceptionFactory = GetExceptionFactory(exceptionType);
            return exceptionFactory is not null;
        }
    }

    private Assembly GetAssembly()
    {
        var assembly = Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.GetName().Name == assemblyName);
        if (assembly is null && loadAssembly)
        {
            try
            {
                return Assembly.Load(new AssemblyName(assemblyName));
            }
            catch
            {
                return null;
            }
        }

        return assembly;
    }

    private static Func<string, Exception> GetExceptionFactory(Type exceptionType)
    {
        if (exceptionType is null)
        {
            return null;
        }

        var constructor = exceptionType.GetConstructor([typeof(string)])
            ?? throw new MissingMemberException(exceptionType.FullName, ".ctor");
        var parameter = Expression.Parameter(typeof(string), "m");
        var expression = Expression.Lambda<Func<string, Exception>>(Expression.New(constructor, parameter), parameter);

        try
        {
            return expression.Compile();
        }
        catch
        {
            return message => (Exception)Activator.CreateInstance(exceptionType, message);
        }
    }
}
