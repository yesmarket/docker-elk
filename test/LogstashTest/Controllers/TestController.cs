using System;
using Microsoft.AspNetCore.Mvc;
using Prometheus;
using Serilog;
using Serilog.Context;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;

namespace LogstashTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public const string FlatFormat = "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Properties}{NewLine}";

        private static readonly string Host = Environment.MachineName;
        private static readonly Counter Counter = Metrics.CreateCounter("Logs", "Count", "Host", "Type", "Direct");

        // GET api/values
        [HttpGet]
        [Route("d")]
        public ActionResult<string> Default()
        {
            return "working";
        }

        // GET api/values
        [HttpGet]
        [Route("es")]
        public ActionResult ElasticsearchDirect()
        {
            Log(lc => lc.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200")) { IndexFormat = "elasticsearch-direct" }), "Logging directly to Elasticsearch");
            Counter.Labels(Host, "Elasticsearch", true.ToString()).Inc();
            return Ok();
        }

        [HttpGet]
        [Route("ls1")]
        public ActionResult LogstashDirect()
        {
            Log(lc => lc.WriteTo.LogstashHttp("http://logstash:5043"), "Logging directly to Logstash");
            Counter.Labels(Host, "Logstash", true.ToString()).Inc();
            return Ok();
        }

        [HttpGet]
        [Route("ls2")]
        public ActionResult LogstashViaDockerProvider()
        {
            Log(lc => lc.WriteTo.Console(new CompactJsonFormatter()), "Logging to Logstash via docker provider");
            Counter.Labels(Host, "Logstash", false.ToString()).Inc();
            return Ok();
        }

        private static void Log(Func<LoggerConfiguration, LoggerConfiguration> func, string message)
        {
            var loggerCoonfiguration = new LoggerConfiguration().Enrich.FromLogContext();
            using (var log = func.Invoke(loggerCoonfiguration).CreateLogger())
            {
                using (LogContext.PushProperty("conversationId", Guid.NewGuid()))
                {
                    log.Information(message);
                    log.Warning(message);
                    log.Error(message);
                }
            }
        }
    }
}
