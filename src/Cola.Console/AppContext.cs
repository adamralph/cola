// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System;

    internal class AppContext
    {
        public string FileName { get; set; }

        public static AppContext Parse(string[] args)
        {
            if (args.Length > 1)
            {
                throw new ArgumentException("I can only run one file at a time.", "args");
            }

            var appContext = new AppContext();

            if (args.Length > 0)
            {
                appContext.FileName = args[0];
            }

            return appContext;
        }
    }
}
