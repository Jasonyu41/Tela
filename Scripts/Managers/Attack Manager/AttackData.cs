using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackData
{
    [SerializeField] public float damage;
    [SerializeField] public float damageAddition;
    [SerializeField] public DamageType damageType;
    [SerializeField] public AttributeType attributeType;
    [SerializeField, Range(0, 100)] public float criticalHit;
    [SerializeField] public float criticalHitDamage;
    [SerializeField] public Buff buff;

    [HideInInspector] public Transform damageSource;
}

[System.Serializable]
public struct AttackDatas
{
    [SerializeField] public AttackData[] attackDatas;
    [SerializeField] public float attackIntervalsTime;
}

public enum DamageType
{
    PhysicalDamage,
    MagicDamage,
    AttributeDamage
}

public enum AttributeType
{
    None,
    Fire,
    Wooden,
    Water,
    Rock,
    Electric,
    Dark
}