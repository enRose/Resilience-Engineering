using System;
using System.Linq;
using System.Net;

namespace api.FaultTolerance
{
    public static class Picker
    {
        public static Tunnel When<T>(this Tunnel tunnel)
            where T : Exception
        {
            tunnel.errorsToRetry.Add(typeof(T));

            return tunnel;
        }

        public static Tunnel When(
            this Tunnel tunnel,
            params HttpStatusCode[] statusCodes
            )
        {
            statusCodes?.ToList()?.ForEach(s =>
                tunnel.httpStatusToRetry.Add(s));

            return tunnel;
        }
    }
}
