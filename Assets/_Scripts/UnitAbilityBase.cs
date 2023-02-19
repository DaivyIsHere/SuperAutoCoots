using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAbilityBase
{
    public string description;
    public int requireCharge = 1;
    public int charge = 0;

    public void GainCharge()
    {
        charge += 1;
        if (charge >= requireCharge)
        {
            PerformAbility();
            charge = 0;
        }
    }

    public void PerformAbility()
    {
        
    }

    public virtual void IniAbility()
    {
        //sub to battleManager event
    }

    public virtual void OnDestroy()
    {
        //unsub to battleManager event
    }
}
