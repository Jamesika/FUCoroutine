# FUCoroutine
It's just fxxking same as Unity Coroutine.

# Features

## StartCoroutine & StopCoroutine
```csharp
public partial class ExampleNode : Node
{
    public void Example()
    {
        _coroutineHandler = CoroutineManager.StartCoroutine(CustomCoroutine());
        CoroutineManager.StopCoroutine(_coroutineHandler);
    }
}
```

## StartCoroutine & StopCoroutine from node
```csharp
public partial class ExampleNode : Node
{
    public void Example()
    {
        _coroutineHandler = this.StartCoroutine(CustomCoroutine());
        this.StopCoroutine(_coroutineHandler);
    }
}
```
- If the node is free or remove from tree, the coroutine will be automaticlly stopped.

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

    // Same as Unity, ignore Godot.Engine.TimeScale
    yield return new WaitForSecondsRealtime(1.0);

    // Same as Unity
    yield return new WaitUtil(() => Validate());

    // Same as Unity
    yield return new WaitWhile(() => Validate());
}
```

## TODO 
- Make coroutines on node pausable while the node is paused.