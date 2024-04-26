using System.Collections.Generic;

namespace ShadowFlareRemake.Events {
    public class AcceptBlock : System.IDisposable {

        private List<InternalBlock> _registeredEvents = new List<InternalBlock>();

        public void AcceptEvent<T>(System.Action<T> registrant) where T : IGameEvent {
            _registeredEvents.Add(new EventBlock<T>(registrant));
        }

        public void Dispose() {

            for(int i = 0; i < _registeredEvents.Count; i++) {
                _registeredEvents[i].Detach();
            }
            _registeredEvents.Clear();
        }

        private abstract class InternalBlock {
            public abstract void Detach();
        }

        private class EventBlock<T> : InternalBlock where T : IGameEvent {

            private System.Action<T> _action;

            public EventBlock(System.Action<T> action) {

                _action = action;
                Dispatcher.Subscribe(_action);
            }

            public override void Detach() {
                Dispatcher.Unsubscribe(_action);
            }
        }
    }
}
