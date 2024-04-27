using ShadowFlareRemake.Enums;
using System;
using UnityEngine;

namespace ShadowFlareRemake.Player {

    public interface IPlayerStats : IUnitStats {

        public Vocation Vcocation { get; }
        public int Strength { get; }
        public int CurrentExp { get;  }
        public int ExpToLevelUp { get;  }
    }
}
