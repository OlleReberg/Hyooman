using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "Catchable/Create New Action")]
public class CombatActionBaseSO : ScriptableObject
{
    [SerializeField] private string combatActionName;
    [TextArea]
    [SerializeField] private string description;
    [SerializeField] private CatchableType type;
    [SerializeField] private int power;
    [SerializeField] private int accuracy;
    [SerializeField] private int mana;

    public string CombatActionName => combatActionName;
    public string Description => description;
    public CatchableType Type => type;
    public int Power => power;
    public int Accuracy => accuracy;
    public int Mana => mana;
}
