using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Linq;

namespace MS.IConstruye
{
    public class TelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }
        public TelemetryProcessor(ITelemetryProcessor next) => Next = next;
        public void Process(ITelemetry item)
        {
            var namesToExclude = "/health|localhost";
            RequestTelemetry request = item as RequestTelemetry;
            if (request != null)
            {
                foreach (string excludedUrl in namesToExclude.Split('|').Select(x => x.ToLower()))
                {
                    if (request.Url.AbsolutePath.ToLower() == excludedUrl)
                        return;
                    if (request.Url.Host == excludedUrl)
                        return;
                }
            }
            Next.Process(item);
        }
    }
}
