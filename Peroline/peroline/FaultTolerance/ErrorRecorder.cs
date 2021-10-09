using System;
using System.Net;

namespace peroline.FaultTolerance
{
    public static class ErrorPicker
    {
        public static Tunnel Pick<T>(this Tunnel tunnel)
            where T : Exception
        {
            tunnel.errorsToRetry.Add(typeof(T));

            return tunnel;
        }

        public static Tunnel Pick(
            this Tunnel tunnel,
            HttpStatusCode statusCode)
        {
            tunnel.httpStatusToRetry.Add(statusCode);

            return tunnel;
        }
    }
}
