// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using static System.FormattableString;

    public class Runner
    {
        private readonly string fileName;
        private readonly string source;
        private readonly TextWriter @out;

        public Runner(string fileName, string source, TextWriter @out)
        {
            this.fileName = Path.GetFullPath(fileName);
            this.source = source;
            this.@out = @out;
        }

        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1303:Do not pass literals as localized parameters",
            MessageId = "Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText(System.String,Microsoft.CodeAnalysis.CSharp.CSharpParseOptions,System.String,System.Text.Encoding,System.Threading.CancellationToken)",
            Justification = "I don't want to retrieve this string from a resource table. It's culture invariant.")]
        public void Run()
        {
            var parts = this.source.Split(new[] { "\r\n\r\n", "\n\n" }, 2, StringSplitOptions.None);
            var userCode = parts.Length == 1 ? null : parts[1];

            var code = Invariant(
$@"using System;
using System.IO;
using LINQPad;

public class UserQuery
{{
{userCode}
}}

namespace LINQPad
{{
    public static class Extensions
    {{
        private static TextWriter @out = Console.Out;

        public static void SetOut(TextWriter @out) => Extensions.@out = @out;

        public static T Dump<T>(this T o)
        {{
            @out.WriteLine(o?.ToString());
            return o;
        }}
    }}
}}
");

            var assemblyFileName = Path.Combine(
                Path.GetDirectoryName(this.fileName),
                ".cola",
                Path.GetFileName(this.fileName) + ".dll");

            Directory.CreateDirectory(Path.GetDirectoryName(assemblyFileName));

            var assemblyName = Path.GetFileNameWithoutExtension(assemblyFileName);
            var tree = CSharpSyntaxTree.ParseText(code);
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Uri).GetTypeInfo().Assembly.Location),
            };

            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(assemblyName, new[] { tree }, references, options);

            ////var emitResult = compilation.Emit(assemblyFileName, Path.ChangeExtension(assemblyFileName, "pdb"));
            var emitResult = compilation.Emit(assemblyFileName);
            if (!emitResult.Success)
            {
                TryDeleteFile(assemblyFileName);

                var message = string.Join(
                    Environment.NewLine, emitResult.Diagnostics.Select(diagnostic => diagnostic.ToString()));

                throw new InvalidOperationException(message);
            }

            var assembly = LoadAssembly(assemblyFileName);
            assembly.GetType("LINQPad.Extensions").GetMethod("SetOut").Invoke(null, new[] { this.@out });

            var userQueryType = assembly.GetType("UserQuery");
            var main = userQueryType.GetMethod(
                "Main", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            if (main == null)
            {
                throw new InvalidOperationException("Cannot find Main method"); // NOTE (adamralph). Mimicking LINQPad message.
            }

            main.Invoke(Activator.CreateInstance(userQueryType), null);
        }

        [SuppressMessage(
            "Microsoft.Reliability",
            "CA2001:AvoidCallingProblematicMethods",
            MessageId = "System.Reflection.Assembly.LoadFile",
            Justification = "YOLO")]
        private static Assembly LoadAssembly(string assemblyFileName)
        {
            return Assembly.LoadFile(assemblyFileName);
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "We don't care if the attempt to delete the file failed.")]
        private static void TryDeleteFile(string assemblyFileName)
        {
            try
            {
                File.Delete(assemblyFileName);
            }
            catch (Exception)
            {
            }
        }
    }
}
