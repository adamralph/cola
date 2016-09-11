// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    internal class App
    {
        private readonly TextWriter @out;

        public App(TextWriter @out)
        {
            this.@out = @out;
        }

        public async Task Run(AppContext context)
        {
            if (context.FileName == null)
            {
                await Console.Out.WriteLineAsync(
$@"
Usage: cola <file>

file:   Path to C# program.

Examples:
  cola TestScript.linq
  cola TestScript.linq > results.txt
  cola script1.linq | cola script2.linq

Go to https://github.com/adamralph/cola for detailed help.
Version={typeof(App).Assembly.GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault()?.InformationalVersion ?? "Unknown"}");

                return;
            }

            var contents = await FileEx.ReadAllTextAsync(context.FileName);
            new Runner(context.FileName, contents, this.@out).Run();
        }
    }
}
