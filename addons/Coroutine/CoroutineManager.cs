using System;
using System.Collections;
using System.Collections.Generic;
using Godot;

namespace FUCoroutine
{
    public partial class CoroutineManager : Node
    {
        private static CoroutineManager _instance;
        private static CoroutineManager Instance
        {
            get
            {
                if (!GodotObject.IsInstanceValid(_instance))
                {
                    _instance = new CoroutineManager();
                    _instance.Name = nameof(CoroutineManager);
                    _instance.CallDeferred(nameof(AddToSceneTree));
                }
                return _instance;
            }
        }

        private List<Coroutine> _newCoroutines = new List<Coroutine>();
        private List<Coroutine> _coroutines = new List<Coroutine>();

        private void AddToSceneTree()
        {
            ((SceneTree)Engine.GetMainLoop()).Root.AddChild(_instance);
        }

        private CoroutineManager()
        {
            ProcessMode = ProcessModeEnum.Always;
            
            // Make sure coroutines run after node process
            ProcessPriority = int.MaxValue;
            ProcessPhysicsPriority = int.MaxValue;
        }
        
        public override void _ExitTree()
        {
            base._ExitTree();
            _coroutines.Clear();
        }

        #region [Handle Process]

        public override void _Process(double delta)
        {
            base._Process(delta);
            ResetCoroutines(false);
            TickCoroutines(CoroutineTickOrder.Process, delta);
            
            ResetCoroutines(false);
            TickCoroutines(CoroutineTickOrder.EndOfProcess, delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            ResetCoroutines(true);
            TickCoroutines(CoroutineTickOrder.PhysicsProcess, delta);
        }

        private void ResetCoroutines(bool isPhysics)
        {
            // Add New
            _coroutines.AddRange(_newCoroutines);
            _newCoroutines.Clear();

            // Remove Completed & Invalid 
            for (var i = _coroutines.Count - 1; i >= 0; i--)
            {
                var coroutine = _coroutines[i];
                if (coroutine.IsCompleted || (coroutine.IsBindingNode && !coroutine.IsBindingNodeValid))
                {
                    _coroutines.RemoveAt(i);
                }
            }
        }

        private void TickCoroutines(CoroutineTickOrder tickOrder, double delta)
        {
            foreach (var tempCoroutine in _coroutines)
                tempCoroutine.Tick(tickOrder, delta);
        }

        #endregion
        
        #region [Manage Coroutines]

        /// <summary>
        /// When node if off SceneTree, stop the coroutine. 
        /// </summary>
        public static ICoroutineHandler StartCoroutineFromNode(Node node, IEnumerator routine)
        {
            if (!GodotObject.IsInstanceValid(node))
            {
                LogError("node is not valid!");
                return null;
            }

            return StartCoroutineImpl(node, routine);
        }

        public static ICoroutineHandler StartCoroutine(IEnumerator routine)
        {
            return StartCoroutineImpl(null, routine);
        }
        
        public static ICoroutineHandler StartCoroutineImpl(Node node, IEnumerator routine)
        {
            var coroutine = new Coroutine(routine, false, node);
            Instance._newCoroutines.Add(coroutine);
            return coroutine;
        }

        public static void StopCoroutineFromNode(Node node, ICoroutineHandler coroutineHandler)
        {
            if (!GodotObject.IsInstanceValid(node))
            {
                LogError("node is not valid!");
                return;
            }

            var coroutine = coroutineHandler as Coroutine;
            if(coroutine == null)
                return;

            if (coroutine.Node != node)
            {
                LogError("Coroutine doesn't match the node : " + node.Name);
                return;
            }

            StopCoroutineImpl(coroutine);
        }

        public static void StopCoroutine(ICoroutineHandler coroutineHandler)
        {
            StopCoroutineImpl(coroutineHandler as Coroutine);
        }

        private static void StopCoroutineImpl(Coroutine coroutine)
        {
            coroutine?.Complete();
        }

        #endregion

        private static void LogError(string info)
        {
            GD.PrintErr(info);
        }
    }
}
