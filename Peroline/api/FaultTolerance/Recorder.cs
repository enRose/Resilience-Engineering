using System;
namespace api.FaultTolerance
{
    public static class Recorder
    {
        public static Tunnel Then(
            this Tunnel tunnel)
        {
            //tunnel.dbContext.ErrorMemo.Add();

            return tunnel;
        }
    }
}
