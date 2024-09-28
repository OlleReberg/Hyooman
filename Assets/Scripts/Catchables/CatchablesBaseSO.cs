using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Catchable", menuName = "Catchable/Create new Catchable")]
public class CatchablesBaseSO : ScriptableObject
{
    [SerializeField] private string catchableName;
    [TextArea]
    [SerializeField] private string description;

    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private CatchableType catchableType1;
    [SerializeField] private CatchableType catchableType2;
    [SerializeField] private int maxHp;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int spAttack;
    [SerializeField] private int spDefense;
    [SerializeField] private int speed;

    [SerializeField] private List<LearnableCombatAction> learnableCombatActions;

    public string CatchableName => catchableName;
    public string Description => description;
    public Sprite FrontSprite => frontSprite;
    public Sprite BackSprite => backSprite;
    public CatchableType CatchableType1 => catchableType1;
    public CatchableType CatchableType2 => catchableType2;
    public int MaxHp => maxHp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    public List<LearnableCombatAction> LearnableCombatActions => learnableCombatActions;
}

public enum CatchableType
{
    None,
    Average, //normal
    Science, //steel
    Strong, //fighting
    Psychological, //psychic
    Edgy, //dark
    Engineer, //Electric
    Annoying, //insectbug
    Alchemist, //poison
    Built, //Rock
    Gross, //ghost
    Cool, //Ice
    Passionate, //dragon
    Angry, //fire
    Sneaky, //flying
    Adaptable, //water
    Outdoorsy, //grass
    Rich
}

[System.Serializable]
public class LearnableCombatAction
{
    [SerializeField] private CombatActionBaseSO combatActionBase;
    [SerializeField] private int level;

    public CombatActionBaseSO CombatActionBase => combatActionBase;
    public int Level => level;
}
