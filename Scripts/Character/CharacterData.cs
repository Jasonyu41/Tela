using UnityEngine;

[System.Serializable]
public struct CharacterData
{
    [Header("Base")]
    [SerializeField] public int level;
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float manaPoint;
    [SerializeField] public float maxManaPoint;

    [Header("Attack")]
    [SerializeField] public float physicalAttack;
    [SerializeField] public float physicalAttackAddition;
    [SerializeField] public float magicAttack;
    [SerializeField] public float magicAttackAddition;

    [Header("Defense")]
    [SerializeField] public float physicalDefense;
    [SerializeField] public float physicalDefenseAddition;
    [SerializeField, Range(0f, 100f)] public float physicalAbsorptionAddition;
    [SerializeField] public float magicDefense;
    [SerializeField] public float magicDefenseAddition;
    [SerializeField, Range(0f, 100f)] public float magicAbsorptionAddition;

    [Header("Attribute")]
    [SerializeField] public AttributeType selfAttribute;
    [SerializeField] public Attribute attribute;
    
    [Header("CriticalHit")]
    [SerializeField, Range(0f, 100f)] public float criticalHit;
    [SerializeField] public float criticalHitDamage;

    [Header("Other")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float coolDown;
    [SerializeField] public float luck;
}

[System.Serializable]
public struct Attribute
{
    [Header("Fire")]
    [SerializeField] public float fireAttack;
    [SerializeField] public float fireAttackAddition;
    [SerializeField] public float fireDefense;
    [SerializeField] public float fireDefenseAddition;
    
    [Header("Wooden")]
    [SerializeField] public float woodenAttack;
    [SerializeField] public float woodenAttackAddition;
    [SerializeField] public float woodenDefense;
    [SerializeField] public float woodenDefenseAddition;
    
    [Header("Water")]
    [SerializeField] public float waterAttack;
    [SerializeField] public float waterAttackAddition;
    [SerializeField] public float waterDefense;
    [SerializeField] public float waterDefenseAddition;
    
    [Header("Rock")]
    [SerializeField] public float rockAttack;
    [SerializeField] public float rockAttackAddition;
    [SerializeField] public float rockDefense;
    [SerializeField] public float rockDefenseAddition;
    
    [Header("Electric")]
    [SerializeField] public float electricAttack;
    [SerializeField] public float electricAttackAddition;
    [SerializeField] public float electricDefense;
    [SerializeField] public float electricDefenseAddition;
    
    [Header("Dark")]
    [SerializeField] public float darkAttack;
    [SerializeField] public float darkAttackAddition;
    [SerializeField] public float darkDefense;
    [SerializeField] public float darkDefenseAddition;
}