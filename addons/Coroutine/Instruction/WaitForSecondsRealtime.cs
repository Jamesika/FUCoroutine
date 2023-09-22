using System;
using System.Collections;
using System.Threading;
using Godot;

namespace FUCoroutine
{
    /// <summary>
    /// If the binding node is not paused, WaitForSecondsRealtime will be paused only when TimeScale == 0!
    /// </summary>
    public class WaitForSecondsRealtime : YieldInstruction
    {
        private double _originSeconds;
        private double _leftSeconds;
        
        public WaitForSecondsRealtime(double seconds)
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
            {
                if (!Mathf.IsZeroApprox(Engine.TimeScale))
                    _leftSeconds -= delta / Engine.TimeScale;
            }
        }

        public override bool IsComplete()
        {
            return _leftSeconds <= 0;
        }
    }
}