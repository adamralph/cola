// Copyright (c) Adam Ralph. (adam@adamralph.com)
namespace System
{
    using System.Globalization;

    public class FormattableString
    {
        private readonly string messageFormat;
        private readonly object[] args;

        public FormattableString(string messageFormat, object[] args)
        {
            this.messageFormat = messageFormat;
            this.args = args;
        }

        public static string Invariant(FormattableString formattable)
        {
            if (formattable == null)
            {
                throw new ArgumentNullException("formattable");
            }

            return formattable.ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            return string.Format(provider, this.messageFormat, this.args);
        }
    }
}
