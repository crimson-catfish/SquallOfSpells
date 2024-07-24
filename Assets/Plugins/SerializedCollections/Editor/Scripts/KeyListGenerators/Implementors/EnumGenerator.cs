using System;
using System.Collections;

namespace AYellowpaper.SerializedCollections.KeysGenerators
{
    [KeyListGenerator("Populate Enum", typeof(Enum), false)]
    public class EnumGenerator : KeyListGenerator
    {
        public override IEnumerable GetKeys(Type type)
        {
            return Enum.GetValues(type);
        }
    }
}