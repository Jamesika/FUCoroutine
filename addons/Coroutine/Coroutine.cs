using System;
using System.Collections;
using System.Diagnostics;
using Godot;

namespace FUCoroutine
{
    public enum CoroutineTickOrder
    {
        Process,
        EndOfProcess,

        PhysicsProcess,
    }

    public interface ICoroutineHandler
    {
    }

    public partial class CoroutineManager
    {
        private class Coroutine : ICoroutineHandler
        {
            private IEnumerator _routine;
            private Coroutine _innerCoroutine;

            private CoroutineTickOrder _tickOrder = CoroutineTickOrder.Process;

            /// <summary>
            /// Is this coroutine nested in another coroutine, it means this coroutine isn't managed by CoroutineManager
            /// </summary>
            private bool _isNested;

            public bool IsCompleted { get; private set; }

            public bool IsBindingNodeValid => IsBindingNode && GodotObject.IsInstanceValid(Node) && Node.IsInsideTree();
            public bool IsBindingNode { get; private set; }
            public Node Node { get; private set; }

            /// <summary>
            /// Create coroutine
            /// </summary>
            /// <param name="routine"></param>
            /// <param name="isNested">is this coroutine nested in another coroutine</param>
            internal Coroutine(IEnumerator routine, bool isNested, Node node = null)
            {
                _routine = routine;
                _isNested = isNested;
                Node = node;
                if (Node != null)
                    IsBindingNode = true;

                if (routine is YieldInstruction yieldInstruction)
                    _tickOrder = yieldInstruction.TickOrder;
            }

            public void Complete()
            {
                IsCompleted = true;
            }

            public void Tick(CoroutineTickOrder tickOrder, double delta)
            {
                if (IsCompleted)
                    return;

                if (IsBindingNode)
                {
                    if (!IsBindingNodeValid)
                    {
                        GD.PrintErr("Node is not valid!");
                        Complete();
                        return;
                    }

                    // Pause coroutine when node is paused
                    if (IsNodePaused(Node))
                        return;
                }
                
                // Tick Inner Coroutine
                if (_innerCoroutine != null && _innerCoroutine._isNested)
                {
                    _tickOrder = _innerCoroutine._tickOrder;
                    _innerCoroutine.Tick(tickOrder, delta);
                }

                if (_innerCoroutine != null && _innerCoroutine.IsCompleted)
                    _innerCoroutine = null;

                // Check Tick Order
                if (_tickOrder != tickOrder)
                    return;

                if (_innerCoroutine == null)
                {
                    if (_routine is YieldInstruction yieldInstruction)
                        yieldInstruction.Tick(delta);

                    if (!_routine.MoveNext())
                    {
                        Complete();
                        return;
                    }

                    var routineCurrent = _routine.Current;
                    if (routineCurrent != null)
                    {
                        if (routineCurrent is IEnumerator enumerator)
                        {
                            if (routineCurrent is YieldInstruction currentYieldInstruction)
                            {
                                currentYieldInstruction.Reset();
                                _tickOrder = currentYieldInstruction.TickOrder;
                                _innerCoroutine = new Coroutine(enumerator, true);
                            }
                            else
                            {
                                _tickOrder = CoroutineTickOrder.Process;
                                _innerCoroutine = new Coroutine(enumerator, true);
                            }
                        }
                        else if (routineCurrent is Coroutine coroutine)
                        {
                            _innerCoroutine = coroutine;
                        }
                        else
                        {
                            _tickOrder = CoroutineTickOrder.Process;
                        }
                    }
                    else
                    {
                        _tickOrder = CoroutineTickOrder.Process;
                    }
                }
            }

            private bool IsNodePaused(Node node)
            {
                var treePaused = Node.GetTree().Paused;
                
                // get parent util ProcessMode is not Inherit
                while (node.ProcessMode == ProcessModeEnum.Inherit)
                {
                    var parentNode = node.GetParent();
                    if (parentNode != null)
                        node = parentNode;
                    else
                        break;
                }

                var processMode = node.ProcessMode;
                if (processMode == ProcessModeEnum.Inherit)
                    processMode = ProcessModeEnum.Pausable;

                return treePaused && processMode == ProcessModeEnum.Pausable ||
                       !treePaused && processMode == ProcessModeEnum.WhenPaused ||
                       processMode == ProcessModeEnum.Disabled;
            }
        }
    }
}
