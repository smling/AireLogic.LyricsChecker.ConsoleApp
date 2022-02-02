using System;
using System.Collections.Generic;
using System.Text;

namespace AireLogic.LyricsChecker.ConsoleApp.Exceptions
{
    internal class DataSourceException : Exception
    {
        public DataSourceException(string message, Exception exception) : base(message, exception)
        {
        }
    }
}
