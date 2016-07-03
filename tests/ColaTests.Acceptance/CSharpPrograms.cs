// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace ColaTests.Acceptance
{
    using System.IO;
    using Cola;
    using ColaTests.Acceptance.Support;
    using FluentAssertions;
    using Xbehave;

    public static class CSharpPrograms
    {
        [Scenario]
        [Example("")]
        [Example("public static ")]
        public static void HelloWorld(string methodPrefix, string statement, TextWriter @out)
        {
            "Given a hello world statement"
                .x(() => statement =
$@"<Query Kind=""Program"" />

{methodPrefix}void Main()
{{
    ""Hello, world!"".Dump();
}}
");

            "When I run the source"
                .x(c => new Runner(c.Step.Scenario.DisplayName.ToPath(), statement, @out = new StringWriter().Using(c)).Run());

            "Then I see 'Hello, world!'"
                .x(() => @out.ToString().Should().ContainEquivalentOf("Hello, world!"));
        }
    }
}
