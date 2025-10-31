using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace brazenhead.Core
{
    public class LoopManager
    {
        private Dictionary<Type, Loop> _loopByType = new();

        public Loop<T> Add<T>(in float tickInterval = 0f, in int executionOrder = 0)
        {
            var loop = new Loop<T>(executionOrder, tickInterval);
            _loopByType.Add(typeof(T), loop);
            _loopByType = new(_loopByType.OrderBy(x => x.Value.executionOrder));
            return loop;
        }

        public void Remove<T>() => _loopByType.Remove(typeof(T));

        public Loop<T> Get<T>() => _loopByType[typeof(T)] as Loop<T>;

        internal void OnUpdate(in float deltaTime)
        {
            foreach (var loop in _loopByType.Values)
                loop.OnUpdate(deltaTime);
        }

        public abstract class Loop
        {
            internal readonly int executionOrder;
            private float _timestep;
            private float _accumulator;

            internal Loop(in int executionOrder, in float timestep)
            {
                this.executionOrder = executionOrder;
                _timestep = timestep;
            }

            public void SetTimestep(float timestep) => _timestep = timestep;

            internal void OnUpdate(in float deltaTime)
            {
                if (_timestep <= 0f)
                {
                    Tick(deltaTime);
                    return;
                }
                _accumulator += deltaTime;
                while (_accumulator >= _timestep)
                {
                    _accumulator -= _timestep;
                    Tick(_timestep);
                }
            }

            private protected abstract void Tick(in float timeStep);
        }

        public class Loop<T> : Loop
        {
            private static readonly Comparison<(int executionOrder, ITickable<T> tickable)> _comparison = (x, y) => x.executionOrder.CompareTo(y.executionOrder);

            private readonly List<(int executionOrder, ITickable<T> tickable)> _tickables = new();

            internal Loop(in int executionOrder, in float tickInterval) : base(executionOrder, tickInterval) { }

            public void AddTickable(in ITickable<T> tickable, in int executionOrder = 0)
            {
                _tickables.Add((executionOrder, tickable));
                _tickables.Sort(_comparison);
            }

            public void RemoveTickable(in ITickable<T> tickable)
            {
                for (int i = 0; i < _tickables.Count; i++)
                    if (_tickables[i].tickable == tickable)
                    {
                        _tickables.RemoveAt(i);
                        break;
                    }
            }

            private protected override void Tick(in float deltaTime)
            {
                foreach (var (_, tickable) in _tickables)
                    tickable.OnTick(deltaTime);
            }
        }
    }
}
