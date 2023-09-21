using System.Collections;
using Godot;

namespace FUCoroutine;

public static class NodeCoroutineExtensions
{
    public static ICoroutineHandler StartCoroutine(this Node node, IEnumerator enumerator)
    {
        return CoroutineManager.StartCoroutineFromNode(node, enumerator);
    }

    public static void StopCoroutine(this Node node, ICoroutineHandler coroutineHandler)
    {
        CoroutineManager.StopCoroutineFromNode(node, coroutineHandler);
    }
}