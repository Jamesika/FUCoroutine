using System.Collections;

namespace FUCoroutine
{
    public abstract class YieldInstruction : IEnumerator
    {
        public object Current => null;

        public bool MoveNext() => !IsComplete();
        
        public virtual CoroutineTickOrder TickOrder => CoroutineTickOrder.Process;
        
        /// <summary>
        /// reset to reuse same instruction
        /// </summary>
        public virtual void Reset()
        {
        }
        
        public virtual void Tick(double delta)
        {
        }
        
        public abstract bool IsComplete();
    }
}
