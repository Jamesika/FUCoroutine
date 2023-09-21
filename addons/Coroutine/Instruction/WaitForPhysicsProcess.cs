using System.Collections;

namespace FUCoroutine
{
    public class WaitForPhysicsProcess : YieldInstruction
    {
        public WaitForPhysicsProcess()
        {
        }

        public override CoroutineTickOrder TickOrder => CoroutineTickOrder.PhysicsProcess;

        public override bool IsComplete()
        {
            return true;
        }
    }
}