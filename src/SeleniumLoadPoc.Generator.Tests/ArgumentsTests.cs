using System;
using Xunit;

namespace SeleniumLoadPoc.Generator.Tests
{
    public class ArgumentsTests
    {
        [Fact]
        public void GivenWrongNumberOfArgumentsGeneratesExample()
        {
            var arguments = Arguments.Parse(new string[] { });
            Assert.False(arguments.Valid);
        }
    }
}
