using FluentAssertions;
using System;
using System.Threading.Tasks;

namespace GasWeb.Server.Tests
{
    public static class AssertionExtensions
    {
        public static Task ShouldThrow<TException>(this Task task) where TException : Exception
        {
            var action = new Func<Task>(() => task);
            return action.Should().ThrowAsync<TException>();
        }
    }
}
