using log4net.Layout;
using log4net.Core;
using System.IO;

namespace hrvster.Core
{
	public class JsonLayout : LayoutSkeleton
	{
		public override void ActivateOptions() { }

		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write("{");
			writer.Write("\"timestamp\":\"" + loggingEvent.TimeStamp.ToString("o") + "\",");
			writer.Write("\"level\":\"" + loggingEvent.Level.DisplayName + "\",");
			writer.Write("\"logger\":\"" + loggingEvent.LoggerName + "\",");
			writer.Write("\"message\":\"" + loggingEvent.RenderedMessage.Replace("\"", "\\\"") + "\",");
			writer.Write("\"thread\":\"" + loggingEvent.ThreadName + "\",");
			writer.Write("\"tags\":\"" + loggingEvent.LookupProperty("tags") + "\",");
			writer.Write("\"environment\":\"" + loggingEvent.LookupProperty("environment") + "\"");
			writer.Write("}");
			writer.WriteLine();
		}
	}
}
