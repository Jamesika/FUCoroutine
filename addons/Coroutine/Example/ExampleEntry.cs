using System.Collections;
using Godot;

namespace FUCoroutine.Example;

/// <summary>
/// Press space to stop coroutine;
/// Press 1~8 to run examples;
/// </summary>
public partial class ExampleEntry : Node
{
    private ICoroutineHandler _coroutineHandler;
    
    public override void _UnhandledKeyInput(InputEvent @event)
    {
        base._UnhandledKeyInput(@event);
        if (@event is InputEventKey eventKey && eventKey.IsPressed() && !eventKey.Echo)
        {
            switch (eventKey.Keycode)
            {
                case Key.Space:
                    // Press space to stop coroutine
                    CoroutineManager.StopCoroutine(_coroutineHandler);
                    _coroutineHandler = null;
                    break;
                case Key.Key1:
                    CoroutineManager.StopCoroutine(_coroutineHandler);
                    _coroutineHandler = CoroutineManager.StartCoroutine(ExampleWaitForFrames());
                    break;
                case Key.Key2:
                    CoroutineManager.StopCoroutine(_coroutineHandler);
                    _coroutineHandler = CoroutineManager.StartCoroutine(ExampleNestedCoroutines());
                    break;
                case Key.Key3:
                    CoroutineManager.StopCoroutine(_coroutineHandler);
                    _coroutineHandler = CoroutineManager.StartCoroutine(ExampleWaitUntil());
                    break;
                case Key.Key4:
                    CoroutineManager.StopCoroutine(_coroutineHandler);
                    _coroutineHandler = CoroutineManager.StartCoroutine(ExampleWaitForSeconds());
                    break;
                case Key.Key5:
                    CoroutineManager.StopCoroutine(_coroutineHandler);
                    _coroutineHandler = CoroutineManager.StartCoroutine(ExampleWaitForSecondsReuseObject());
                    break;
                case Key.Key6:
                    var node6 = new CoroutineBindNodeExample();
                    AddChild(node6);
                    CoroutineManager.StartCoroutine(node6.TestStartAndStop());
                    break;
                case Key.Key7:
                    var node7 = new CoroutineBindNodeExample();
                    AddChild(node7);
                    CoroutineManager.StartCoroutine(node7.TestFreeNode());
                    break;
                case Key.Key8:
                    var node8 = new CoroutineBindNodeExample();
                    AddChild(node8);
                    CoroutineManager.StartCoroutine(node8.TestRemoveFromTree());
                    break;
            }
        }
    }
    
    #region [Examples]

    public IEnumerator ExampleWaitForFrames()
    {
        Log("ExampleWaitForFrames");
        Log("WaitOneFrame Begin");
        yield return null;
        Log("WaitOneFrame End");
        
        Log("Wait 10 Frames Begin");
        yield return new WaitForFrames(10);
        Log("Wait 10 Frames End");
        
        Log("Wait End Of Frame Begin");
        yield return new WaitForEndOfProcess();
        Log("Wait End Of Frame End");
        
        Log("Wait Physics Process Begin");
        yield return new WaitForPhysicsProcess();
        Log("PhysicsProcess");
        yield return new WaitForPhysicsProcess();
        Log("PhysicsProcess");
        yield return new WaitForPhysicsProcess();
        Log("PhysicsProcess");
        Log("Wait Physics Process End");
    }

    public IEnumerator ExampleNestedCoroutines()
    {
        Log("ExampleNestedCoroutines Begin");
        Log("ExampleNestedCoroutine1");
        yield return ExampleNestedCoroutine();
        Log("ExampleNestedCoroutine2");
        yield return CoroutineManager.StartCoroutine(ExampleNestedCoroutine());
        Log("ExampleNestedCoroutines End");
    }
    
    private IEnumerator ExampleNestedCoroutine()
    {
        yield return null;
    }

    public IEnumerator ExampleWaitUntil()
    {
        Log("ExampleWaitUntil Begin");
        // wait util passed 10 frames 
        var frame = Engine.GetProcessFrames();
        yield return new WaitUtil(() => Engine.GetProcessFrames() - frame > 10);
        Log("ExampleWaitUntil End");
    }

    public IEnumerator ExampleWaitForSeconds()
    {
        Log("ExampleWaitSeconds Begin");

        Log("Set timescale 0.2f");
        Engine.TimeScale = 0.2f;
        
        Log("Wait 1s begin");
        yield return new WaitForSeconds(1f);
        Log("Wait 1s end");
        
        Log("Wait 1s realtime begin");
        yield return new WaitForSecondsRealtime(1f);
        Log("Wait 1s realtime end");
        
        Log("Set timescale 1f");
        Engine.TimeScale = 1f;
        
        Log("ExampleWaitSeconds End");
    }

    public IEnumerator ExampleWaitForSecondsReuseObject()
    {
        var waitForOneSecond = new WaitForSeconds(1f);
        Log("ExampleWaitForSecondsReuseObject Begin");
        Log("wait 1s");
        yield return waitForOneSecond;
        Log("wait 1s");
        yield return waitForOneSecond;
        Log("wait 1s");
        yield return waitForOneSecond;
        Log("ExampleWaitForSecondsReuseObject End");
    }
    
    private void Log(string info)
    {
        GD.Print($"[{Engine.GetProcessFrames()}][{Engine.GetPhysicsFrames()}] {info}");
    }

    #endregion
}