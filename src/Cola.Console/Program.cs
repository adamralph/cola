// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System;
    using System.Threading.Tasks;

    internal static class Program
    {
        public static async Task<int> MainAsync(string[] args)
        {
#if DEBUG
            await new App(Console.Out).Run(AppContext.Parse(args));
#else
            try
            {
                await new App(Console.Out).Run(AppContext.Parse(args));
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.Message);
                return 1;
            }
#endif

            return 0;
        }

        public static int Main(string[] args)
        {
            return MainAsync(args).GetAwaiter().GetResult();
        }
    }
}
