// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace Cola
{
    using System;

    internal class AppContext
    {
        public string FileName { get; set; }

        public static AppContext Parse(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("I need a file name.", "args");
            }

            if (args.Length > 1)
            {
                throw new ArgumentException("I can only run one file at a time.", "args");
            }

            return new AppContext { FileName = args[0] };
        }
    }
}
