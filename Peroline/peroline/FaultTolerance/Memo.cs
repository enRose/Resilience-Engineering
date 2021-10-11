using System;
namespace peroline.FaultTolerance
{
    public static class Memo
    {
        public static Tunnel SaveMemoWhenErrors(
            this Tunnel tunnel,
            ErrorMemo errorMemo)
        {


            return tunnel;
        }
    }
}
