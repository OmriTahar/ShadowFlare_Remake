using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : MonoBehaviour, I_Item
{
    public string Name { get; set; }
    public float Weight { get; set; }
    public ItemType Type { get; set; }

    protected int HealthRestore; 
    protected int ManaRestore;

    public virtual void Use(Unit unit)
    {
        if (unit != null)
            unit.Heal(HealthRestore, ManaRestore);
    }
}
