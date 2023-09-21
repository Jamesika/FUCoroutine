using System.Collections;

namespace FUCoroutine
{
    public class WaitForEndOfProcess : YieldInstruction
    {
        public WaitForEndOfProcess()
        {
        }

        public override CoroutineTickOrder TickOrder => CoroutineTickOrder.EndOfProcess;
        
        public override bool IsComplete()
        {
            return true;
        }
    }
}