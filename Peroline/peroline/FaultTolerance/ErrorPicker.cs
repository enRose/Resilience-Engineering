using System;
using System.Net;
using System.Linq;

namespace peroline.FaultTolerance
{
    public static class ErrorPicker
    {
        public static Tunnel RetryOn<T>(this Tunnel tunnel)
            where T : Exception
        {
            tunnel.errorsToRetry.Add(typeof(T));

            return tunnel;
        }

        public static Tunnel RetryOn(
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
