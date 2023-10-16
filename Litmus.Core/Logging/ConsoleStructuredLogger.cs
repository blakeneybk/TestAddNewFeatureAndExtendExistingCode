using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace Litmus.Core.Logging
{
    /// <summary>
    /// This implementation of structured logger writes to Console.WriteLine.
    /// It is typically only used in test and debugging at Litmus.
    ///
    /// For other systems we typically send our logs to Seq in a structured format
    /// via the Serilog library.  We have a wrapper around Serilog to expose it
    /// to our applications using this same interface
    /// </summary>
    /// <seealso cref="https://serilog.net/"/>
    /// <seealso cref="https://datalust.co/seq"/>
    public class ConsoleStructuredLogger : IStructuredLogger
    {
        protected enum LogLevel
        {
            Verbose,
            Debug,
            Information,
            Warning,
            Error,
            Fatal
        }
        
        private const string PreambleFormat = "{0} [P{1}:T{2}] [{3}] ";

        private static readonly object SyncObject = new object();

        private int? currentProcessId;

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Verbose, exception, messageTemplate, propertyValues);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Verbose, null, messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Debug, exception, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Debug, null, messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Information, exception, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Information, null, messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Warning, exception, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Warning, null, messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Error, exception, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Error, null, messageTemplate, propertyValues);
        }

        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Fatal, exception, messageTemplate, propertyValues);
        }

        public void Fatal(string messageTemplate, params object[] propertyValues)
        {
            WriteLogEntry(LogLevel.Fatal, null, messageTemplate, propertyValues);
        }

        protected virtual void WriteLogEntry(LogLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            lock (SyncObject)
            {
                currentProcessId = currentProcessId ?? Process.GetCurrentProcess().Id;
                var currentThreadId = Thread.CurrentThread.ManagedThreadId;

                Console.Write(PreambleFormat, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff", CultureInfo.InvariantCulture), currentProcessId, currentThreadId, level);
                Console.WriteLine(ConvertStructureFormatToStringFormat(messageTemplate), propertyValues);
                if (exception != null)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        private static readonly Regex structuredLogFormatRegex = new Regex(@"{(\$?[^0-9][\w\.]+(\:\d+)?)}", RegexOptions.Compiled);

        [DebuggerStepThrough]
        private static string ConvertStructureFormatToStringFormat(string messageTemplate)
        {
            messageTemplate = messageTemplate.Replace(@"{}", @"{{}}");

            // Fixes an issue where "+() => { }" in a stack trace caused Console.WriteLine to throw a format exception.
            messageTemplate = messageTemplate.Replace(@"{ }", @"{{ }}");

            int currentFormatValue = 0;
            var matches = structuredLogFormatRegex.Matches(messageTemplate);

            foreach (Match match in matches)
            {
                var captures = match.Captures;
                for (int i = 0; i < captures.Count; i++)
                {
                    messageTemplate = messageTemplate.Replace(captures[i].Value, "{" + currentFormatValue + "}");

                    currentFormatValue++;
                }
            }

            return messageTemplate;
        }
    }
}
