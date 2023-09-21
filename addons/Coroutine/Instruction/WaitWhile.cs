using System;
using System.Collections;

namespace FUCoroutine
{
    public class WaitWhile : YieldInstruction
    {
        public WaitWhile(Func<bool> condition)
        {
            _condition = condition;
        }

        private Func<bool> _condition;

        public override bool IsComplete()
        {
            return !_condition.Invoke();
        }

        public override void Reset()
        {
        }
    }
}
