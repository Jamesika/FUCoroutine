using System;
using Godot;

namespace FUCoroutine
{
    public class WaitForSecondsRealtime : YieldInstruction
    {
        private double _originSeconds;
        private ulong _beginTimeMsec;
        
        public WaitForSecondsRealtime(double seconds)
        {
            _originSeconds = seconds;
            _beginTimeMsec = Time.GetTicksMsec();
        }

        public override void Reset()
        {
            base.Reset();
            _beginTimeMsec = Time.GetTicksMsec();
        }
 
        public override bool IsComplete()
        {
            var passedTime = Time.GetTicksMsec() - _beginTimeMsec;
            return passedTime > _originSeconds * 1000;
        }
    }
}