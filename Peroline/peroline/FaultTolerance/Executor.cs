using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace peroline.FaultTolerance
{
    public static class Executor
    {
        public static async Task<T> ExecuteAsyn<T>(
            this Tunnel tunnel,
            Func<Task<T>> f)
        {
            try
            {
                T r = await f();

                if (r is HttpResponseMessage message)
                {
                    if (tunnel.httpStatusToRetry.Contains(message.StatusCode) ||
                        tunnel.errorsToRetry.Contains(message.GetType())
                        )
                    {


                    }
                }

                return r;
            }
            catch(Exception)
            {
                // add error logging here

                throw;
            }
        }
    }
}
