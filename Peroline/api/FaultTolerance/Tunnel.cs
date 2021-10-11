using System;
using System.Collections.Generic;
using System.Net;
using api.Helpers;

namespace api.FaultTolerance
{
    public class Tunnel
    {
        public DataContext dbContext;
        public HashSet<Type> errorsToRetry;
        public HashSet<HttpStatusCode> httpStatusToRetry;

        public Tunnel(DataContext context)
        {
            dbContext = context;
            errorsToRetry = new HashSet<Type>();
            httpStatusToRetry = new HashSet<HttpStatusCode>();
        }
    }
}
