using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.NPC
{
    public class NpcModel : Model
    {
        public string Name { get; private set; }

        public NpcModel(string name)
        {
            Name = name;
        }
    }
}
