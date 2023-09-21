using System.Collections;

namespace FUCoroutine
{
    public class WaitForFrames : YieldInstruction
    {
        private int _originCount;
        private int _leftCount;
        
        public WaitForFrames(int count)
        {
            _originCount = count;
            _leftCount = count;
        }

        public override void Reset()
        {
            base.Reset();
            _leftCount = _originCount;
        }

        public override void Tick(double delta)
        {
            base.Tick(delta);
            if (_leftCount > 0)
                _leftCount--;
        }

        public override bool IsComplete()
        {
            return _leftCount <= 0;
        }
    }
}
