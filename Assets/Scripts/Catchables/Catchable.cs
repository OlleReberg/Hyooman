using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catchable
{
    private int level;
    private CatchablesBaseSO catchableBase;
    public int HP { get; set; }
    public List<CombatAction> combatActions { get; set; }

    public Catchable(CatchablesBaseSO cBase, int cLevel)
    {
        catchableBase = cBase;
        level = cLevel;
        HP = cBase.MaxHp;

        //generate actions
        combatActions = new List<CombatAction>();
        foreach (var combatAction in cBase.LearnableCombatActions)
        {
            if (combatAction.Level <= level)
                combatActions.Add(new CombatAction(combatAction.CombatActionBase));

            if (combatActions.Count >= 4)
                break;
        }
    }

    public int MaxHp => Mathf.FloorToInt((catchableBase.MaxHp * level) / 100f) + 10;

    public int Attack => Mathf.FloorToInt((catchableBase.Attack * level) / 100f) + 5;

    public int Defense => Mathf.FloorToInt((catchableBase.Defense * level) / 100f) + 5;

    public int SpAttack => Mathf.FloorToInt((catchableBase.SpAttack * level) / 100f) + 5;

    public int SpDefense => Mathf.FloorToInt((catchableBase.SpDefense * level) / 100f) + 5;

    public int Speed => Mathf.FloorToInt((catchableBase.Speed * level) / 100f) + 5;
}
