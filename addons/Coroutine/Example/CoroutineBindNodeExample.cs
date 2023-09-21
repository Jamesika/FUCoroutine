using System.Collections;
using Godot;

namespace FUCoroutine.Example;
 
public partial class CoroutineBindNodeExample : Node
{
    public IEnumerator TestRemoveFromTree()
    {
        Log("TestRemoveFromTree Begin");
        Log("Start infinite coroutine from node");
        this.StartCoroutine(ExampleCoroutine());
        
        yield return new WaitForSeconds(2f);
        
        Log("Remove node from tree");
        this.GetParent().RemoveChild(this);
    }

    public IEnumerator TestFreeNode()
    {
        Log("TestFreeNode Begin");
        Log("Start infinite coroutine from node");
        this.StartCoroutine(ExampleCoroutine());
        
        yield return new WaitForSeconds(2f);
        
        Log("QueueFree node");
        this.QueueFree();
    }
    
    public IEnumerator TestStartAndStop()
    {
        Log("TestStartAndStop Begin");
        Log("Start infinite coroutine from node");
        var handler = this.StartCoroutine(ExampleCoroutine());

        yield return new WaitForSeconds(2f);
        
        Log("Stop coroutine from node");
        this.StopCoroutine(handler);
    }

    private IEnumerator ExampleCoroutine()
    {
        while (true)
        {
            Log("Loop");
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private void Log(string info)
    {
        GD.Print($"[{Engine.GetProcessFrames()}][{Engine.GetPhysicsFrames()}] {info}");
    }
}