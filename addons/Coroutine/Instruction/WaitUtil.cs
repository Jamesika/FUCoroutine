using System;
using System.Collections;

namespace FUCoroutine
{
    public class WaitUtil : YieldInstruction
    {
        public WaitUtil(Func<bool> condition)
        {
            _condition = condition;
        }

        private Func<bool> _condition { get; }

        public override bool IsComplete()
        {
            return _condition.Invoke();
        }

        public override void Reset()
        {
        }
    }
}
