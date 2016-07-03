// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System.IO;
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
            var contents = await FileEx.ReadAllTextAsync(context.FileName);
            new Runner(context.FileName, contents, this.@out).Run();
        }
    }
}
