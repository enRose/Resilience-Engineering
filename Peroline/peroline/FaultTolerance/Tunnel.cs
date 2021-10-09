using System;
using System.Collections.Generic;
using System.Net;

namespace peroline.FaultTolerance
{
    public class Tunnel
    {
        public HashSet<Type> errorsToRetry;
        public HashSet<HttpStatusCode> httpStatusToRetry;

        public Tunnel()
        {
            errorsToRetry = new HashSet<Type>();
            httpStatusToRetry = new HashSet<HttpStatusCode>();
        }
    }
}
