using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction
{
    public CombatActionBaseSO CombatActionBase { get; set; }
    public int Mana { get; set; }

    public CombatAction(CombatActionBaseSO cCombatActionBase)
    {
        CombatActionBase = cCombatActionBase;
        Mana = cCombatActionBase.Mana;
    }
}
