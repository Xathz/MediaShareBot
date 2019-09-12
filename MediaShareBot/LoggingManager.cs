using System.IO;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace MediaShareBot {

    /// <summary>
    /// Custom configuration and logging.
    /// </summary>
    public static class LoggingManager {

        /// <summary>
        /// Write a log.
        /// </summary>
        public static Logger Log = LogManager.GetLogger(Constants.ApplicationName);

        /// <summary>
        /// Setup all the logging targets and rules. Call only once, usually at the start of the program.
        /// </summary>
        public static void Initialize() {
            Directory.CreateDirectory(Constants.LogDirectory);
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration();

            // Setup and layout formatting for the console
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget() {
                Name = Constants.ApplicationName,
                //Layout = "${message}"
                // Match the console target format with file targets. It looks cluttered on small terminals
                Layout = "${longdate} [${pad:padCharacter= :padding=5:fixedLength=true:alignmentOnTruncation=Right:${uppercase:${level}}}] [${callsite:includeNamespace=false:cleanNamesOfAnonymousDelegates=true:cleanNamesOfAsyncContinuations=true}] ${message}"
            };

            // Override the trace color
            ConsoleRowHighlightingRule consoleTarget_RowRules_Trace = new ConsoleRowHighlightingRule() {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Trace"),
                ForegroundColor = ConsoleOutputColor.Gray
            };
            consoleTarget.RowHighlightingRules.Add(consoleTarget_RowRules_Trace);

            // Override the debug color
            ConsoleRowHighlightingRule consoleTarget_RowRules_Debug = new ConsoleRowHighlightingRule() {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Debug"),
                ForegroundColor = ConsoleOutputColor.Gray
            };
            consoleTarget.RowHighlightingRules.Add(consoleTarget_RowRules_Debug);

            // Override the info color
            ConsoleRowHighlightingRule consoleTarget_RowRules_Info = new ConsoleRowHighlightingRule() {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Info"),
                ForegroundColor = ConsoleOutputColor.White
            };
            consoleTarget.RowHighlightingRules.Add(consoleTarget_RowRules_Info);

            // Override the warn color
            ConsoleRowHighlightingRule consoleTarget_RowRules_Warn = new ConsoleRowHighlightingRule() {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Warn"),
                ForegroundColor = ConsoleOutputColor.Yellow
            };
            consoleTarget.RowHighlightingRules.Add(consoleTarget_RowRules_Warn);

            // Override the error color
            ConsoleRowHighlightingRule consoleTarget_RowRules_Error = new ConsoleRowHighlightingRule() {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Error"),
                ForegroundColor = ConsoleOutputColor.Red
            };
            consoleTarget.RowHighlightingRules.Add(consoleTarget_RowRules_Error);

            // Override the fatal color
            ConsoleRowHighlightingRule consoleTarget_RowRules_Fatal = new ConsoleRowHighlightingRule() {
                Condition = ConditionParser.ParseExpression("level == LogLevel.Fatal"),
                ForegroundColor = ConsoleOutputColor.Red
            };
            consoleTarget.RowHighlightingRules.Add(consoleTarget_RowRules_Fatal);

            // Add consoleTarget to the overall configuration
            loggingConfiguration.AddTarget(consoleTarget);
            loggingConfiguration.AddRule(LogLevel.Trace, LogLevel.Fatal, Constants.ApplicationName);

            // =================================

            // All messages from Trace to Warn levels write to the general file
            FileTarget fileTarget_General = new FileTarget() {
                Name = Constants.ApplicationName,
                FileName = Path.Combine(Constants.LogDirectory, "General.log"),
                Layout = "${longdate} [${pad:padCharacter= :padding=5:fixedLength=true:alignmentOnTruncation=Right:${uppercase:${level}}}] [${callsite:includeNamespace=false:cleanNamesOfAnonymousDelegates=true:cleanNamesOfAsyncContinuations=true}] ${message}",
                ArchiveFileName = Path.Combine(Constants.LogDirectory, "General{#}.Archive.log"),
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 7,
                ConcurrentWrites = false
            };
            // Limit how often the file will get written to disk.
            // Default: BufferSize = 50 (log events), FlushTimeout = 5000 (milliseconds)
            BufferingTargetWrapper fileAsyncTargetWrapper_General = new BufferingTargetWrapper {
                Name = Constants.ApplicationName,
                WrappedTarget = fileTarget_General,
                BufferSize = 50,
                FlushTimeout = 5000,
                SlidingTimeout = false
            };
            loggingConfiguration.AddTarget(fileAsyncTargetWrapper_General);
            loggingConfiguration.AddRule(LogLevel.Trace, LogLevel.Warn, Constants.ApplicationName);

            // All messages from Warn to Fatal levels write to the error file with advanced trace information
            FileTarget fileTarget_Error = new FileTarget() {
                Name = Constants.ApplicationName,
                FileName = Path.Combine(Constants.LogDirectory, "Error.log"),
                Layout = "${longdate} [${pad:padCharacter= :padding=5:fixedLength=true:alignmentOnTruncation=Right:${uppercase:${level}}}] [${callsite:includeSourcePath=true:cleanNamesOfAnonymousDelegates=true:cleanNamesOfAsyncContinuations=true}:${callsite-linenumber}; ${stacktrace}] ${message}${exception:format=ToString,StackTrace}",
                ArchiveFileName = Path.Combine(Constants.LogDirectory, "Error{#}.Archive.log"),
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 7,
                ConcurrentWrites = false
            };
            loggingConfiguration.AddTarget(fileTarget_Error);
            loggingConfiguration.AddRule(LogLevel.Error, LogLevel.Fatal, Constants.ApplicationName);

            // Apply all the custom configurations to the LogManager
            LogManager.Configuration = loggingConfiguration;

            Log.Info("Logging initialization finished.");
        }

        /// <summary>
        /// Flush any pending log messages.
        /// </summary>
        public static void Flush() {
            LogManager.Flush();
        }

    }

}
