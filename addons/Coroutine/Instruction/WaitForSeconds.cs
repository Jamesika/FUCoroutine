using System;

namespace FUCoroutine
{
    public class WaitForSeconds : YieldInstruction
    {
        private double _originSeconds;
        private double _leftSeconds;
        
        public WaitForSeconds(double seconds)
        {
            _originSeconds = seconds;
            _leftSeconds = seconds;
        }

        public override void Reset()
        {
            base.Reset();
            _leftSeconds = _originSeconds;
        }

        public override void Tick(double delta)
        {
            base.Tick(delta);
            if (_leftSeconds > 0)
                _leftSeconds -= delta;
        }

        public override bool IsComplete()
        {
            return _leftSeconds <= 0;
        }
    }
}
