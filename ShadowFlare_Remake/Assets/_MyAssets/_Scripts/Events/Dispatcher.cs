using System.Collections.Generic;
using System;
using UnityEngine;

namespace ShadowFlareRemake.Events {
    public static class Dispatcher {

        private static Dictionary<Type, ListenerBlock> _blocks = new Dictionary<Type, ListenerBlock>();

        public static void Dispatch(IGameEvent l8Event) {
            DispatchInternal(l8Event);
        }

        public static void Subscribe<T>(Action<T> listener) where T : IGameEvent {

            var type = typeof(T);

            if(_blocks.ContainsKey(type)) {
                ((ListenerBlock<T>)_blocks[type]).AddListener(listener);

            } else {
                _blocks.Add(type, new ListenerBlock<T>(listener));
            }
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : IGameEvent {

            var type = typeof(T);

            if(_blocks.ContainsKey(type)) {
                ((ListenerBlock<T>)_blocks[type]).RemoveListener(listener);
            }
        }

        private static void DispatchInternal(object e) {

            var type = e.GetType();

            while(type != null) {

                if(_blocks.TryGetValue(type, out var block)) {
                    block.Dispatch(e);
                }

                type = type.BaseType;
            }
        }

        private abstract class ListenerBlock {
            public abstract void Dispatch(object l8Event);
        }

        private class ListenerBlock<T> : ListenerBlock {

            public List<Action<T>> _delegates = new List<Action<T>>();

            public ListenerBlock(Action<T> listener) {
                _delegates.Add(listener);
            }

            public void AddListener(Action<T> listener) {
                _delegates.Add(listener);
            }

            public void RemoveListener(Action<T> listener) {
                _delegates.Remove(listener);
            }

            public void Dispatch(T e) {
                for(int i = 0; i < _delegates.Count; ++i) {
                    try {
                        _delegates[i](e);
                    } catch(Exception ex) {
                        Debug.LogException(ex);
                    }
                }
            }

            public override void Dispatch(object l8Event) {
                Dispatch((T)l8Event);
            }
        }
    }
}


