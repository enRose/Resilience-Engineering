using System;
namespace api.FaultTolerance
{
    public class Memo
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }

        public string ErrorType { get; set; }

        public string RetriedByBotId { get; set; }
        public bool HasBeenRetried { get; set; }
        public int NumOfRetry { get; set; }

        // 1. in-flight app recovery
        // 2. submit recovery 
        public string RecoveryFor { get; set; }

        public string DataForRetry { get; set; }
    }
}
