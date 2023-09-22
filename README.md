# FUCoroutine
It's just fxxking same as Unity Coroutine.  
It's a plugin for Godot Engine.

# Features

## StartCoroutine & StopCoroutine
```csharp
public partial class ExampleNode : Node
{
    public void Example()
    {
        var coroutineHandler = CoroutineManager.StartCoroutine(CustomCoroutine());
        CoroutineManager.StopCoroutine(coroutineHandler);
    }
}
```

## StartCoroutine & StopCoroutine from node
```csharp
public partial class ExampleNode : Node
{
    public void Example()
    {
        var coroutineHandler = this.StartCoroutine(CustomCoroutine());
        this.StopCoroutine(coroutineHandler);
    }
}
```
- If the node is free or remove from tree, the coroutine will be automaticlly stopped.
- If the node is paused (`GetTree().Paused = true`), the coroutine will be paused, too.

## Wait For XXX
```csharp
public IEnumerator Example()
{
    // Same as Unity : WaitForEndOfFrame()
    yield return new WaitForEndOfProcess();
    
    // Same as Unity, Wait one frame
    yield return null;
    
    yield return new WaitForFrames(10);

    // Same as Unity ：WaitForFixedUpdate()
    yield return new WaitForPhysicsProcess();

    // Same as Unity
    yield return new WaitForSeconds(1.0);

    // Same as Unity, ignore Godot.Engine.TimeScale. **but if TimeScale is zero, WaitForSecondsRealtime will be paused.**
    yield return new WaitForSecondsRealtime(1.0);

    // Same as Unity
    yield return new WaitUtil(Validate());

    // Same as Unity
    yield return new WaitWhile(Validate());
}
```

## Custom Wait For Something

```csharp
public class WaitForSomething : YieldInstruction
{
    public override bool IsComplete()
    {
        // Is it completed ?
    }
}
```