using System.Collections;
using UnityEngine;

namespace SquallOfSpells.Plugins
{
    public class Logger
    {
        private readonly bool   _enabled;
        private readonly Object _owner;

        public Logger(Object owner, bool enabled = true)
        {
            _owner = owner;
            _enabled = enabled;
        }

        public void Log(object message)
        {
            if (!_enabled)
                return;

            if (!(message is string) && message is IEnumerable enumerable)
            {
                Debug.Log("===============================================");

                Debug.Log("enumerable collection: " + message, _owner);

                foreach (var element in enumerable)
                {
                    Debug.Log(element, _owner);
                }

                Debug.Log("===============================================");
            }
            else
            {
                Debug.Log(message, _owner);
            }
        }
    }
}