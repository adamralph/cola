// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System;
    using System.Threading.Tasks;

    internal static class Program
    {
        public static async Task<int> MainAsync(string[] args)
        {
            try
            {
                await new App(Console.Out).Run(AppContext.Parse(args));
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.Message);
                return 1;
            }

            return 0;
        }

        public static int Main(string[] args)
        {
            return MainAsync(args).GetAwaiter().GetResult();
        }
    }
}
