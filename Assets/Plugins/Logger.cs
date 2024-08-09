using UnityEngine;

namespace SquallOfSpells.Plugins
{
    public class Logger
    {
        private readonly Object _owner;
        private readonly bool   _enabled;

        public Logger(Object owner, bool enabled = true)
        {
            _owner = owner;
            _enabled = enabled;
        }

        public void Log(object message)
        {
            if (_enabled)
                Debug.Log(message, _owner);
        }
    }
}