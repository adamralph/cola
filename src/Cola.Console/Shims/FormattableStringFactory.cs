// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace System.Runtime.CompilerServices
{
    using System;

    public static class FormattableStringFactory
    {
        public static FormattableString Create(string messageFormat, params object[] args)
        {
            return new FormattableString(messageFormat, args);
        }
    }
}
