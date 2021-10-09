using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace peroline.FaultTolerance
{
    public interface IMemoriserAttribute
    {
        List<HttpStatusCode> HttpStatusCodes { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class MemoriserAttribute : Attribute, IMemoriserAttribute
    {
        public List<HttpStatusCode> HttpStatusCodes { get; set; } =
            new List<HttpStatusCode>();

        private Dictionary<int, HttpStatusCode> map =
            new Dictionary<int, HttpStatusCode>()
            {
                { 500, HttpStatusCode.InternalServerError },
                { 503, HttpStatusCode.ServiceUnavailable },
                { 404, HttpStatusCode.NotFound }
            };

        public MemoriserAttribute(params int[] httpStatus)
        {
            httpStatus.ToList().ForEach(
                x => {
                    if (map.TryGetValue(x, out HttpStatusCode v))
                    {
                        HttpStatusCodes.Add(v);
                    }
                });
        }
    }
}
