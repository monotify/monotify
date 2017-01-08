using System;
using System.Collections.Generic;

namespace Monotify.Models
{
    public class MonoLog
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string User { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Version { get; set; }
        public string App { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string CustomLevel { get; set; }

        public int StatusCode { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Dictionary<string, object> Data { get; set; }
        public Dictionary<string, object> Form { get; set; }
        public Dictionary<string, object> Cookie { get; set; }
        public Dictionary<string, object> ServerVariables { get; set; }
    }
}