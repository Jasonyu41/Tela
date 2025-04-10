using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    [SerializeField] TextAsset attributeRestraintText;
    float[,] attributeRestraintTable = new float[7, 7];

    protected override void Awake()
    {
        base.Awake();

        AttributeRestraintTableUpdate();
    }

    public static void NormalAttack(Character character, AttackData attackData)
    {
        CharacterData characterData = character.characterData;

        float damage = 0;
        switch (attackData.damageType)
        {
            case DamageType.PhysicalDamage:
                damage = attackData.damage * ((attackData.damageAddition + 100) / 100) * CriticalHit(attackData) * (AbsorptionCalculate(characterData.physicalDefense, characterData.physicalAbsorptionAddition) / 100);
                break;
            case DamageType.MagicDamage:
                damage = attackData.damage * ((attackData.damageAddition + 100) / 100) * CriticalHit(attackData) * (AbsorptionCalculate(characterData.magicDefense, characterData.magicAbsorptionAddition) / 100);
                break;
            case DamageType.AttributeDamage:
                damage = (attackData.damage * AttributeMagnification(attackData.attributeType, characterData.selfAttribute) - AttributeDefenseCalculate(attackData.attributeType, characterData)) * ((attackData.damageAddition + 100) / 100);
                break;
        }
        if (damage < 0) damage = 0;
        character.TakeDamege(damage);

        if (attackData.buff != null)
        {
            BuffManager.AddBuff(character, attackData.buff);
        }
    }

    static float CriticalHit(AttackData attackData) => Random.Range(1, 100) <= attackData.criticalHit ? (attackData.criticalHitDamage + 100) / 100 : 1;

    public static float AbsorptionCalculate(float defense, float absorptionAddition)
    {
        float absorption;
        switch (defense)
        {
            case <= 215f:
                absorption = defense * 0.093f;
                break;
            case <= 440f:
                absorption = 20 + (defense - 215) * 0.088f;
                break;
            case <= 690f:
                absorption = 40 + (defense - 440) * 0.08f;
                break;
            case < 1000f:
                absorption = 60 + (defense - 690) * 0.064f;
                break;
            default:
                return 20;
        }
        return 100 - Mathf.Clamp(absorption + absorptionAddition, 0, 80);
    }
    
    void AttributeRestraintTableUpdate()
    {
        string[] textRow = attributeRestraintText.text.Split('\n');

        for (int i = 1; i < textRow.Length; i++)
        {
            string[] textCol = textRow[i].Split(',');

            for (int j = 1; j < textCol.Length; j++)
            {
                attributeRestraintTable[i - 1, j - 1] = float.Parse(textCol[j]);
            }
        }
    }

    static float AttributeMagnification(AttributeType attackAttribute, AttributeType defenseAttribute)
    {
        return Instance.attributeRestraintTable[(int)attackAttribute, (int)defenseAttribute];
    }

    static float AttributeDefenseCalculate(AttributeType attribute, CharacterData characterData)
    {
        switch (attribute)
        {
            case AttributeType.None:
                return 0;
            case AttributeType.Fire:
                return characterData.attribute.fireDefense * ((characterData.attribute.fireDefenseAddition + 100) / 100);
            case AttributeType.Wooden:
                return characterData.attribute.woodenDefense * ((characterData.attribute.woodenDefenseAddition + 100) / 100);
            case AttributeType.Water:
                return characterData.attribute.waterDefense * ((characterData.attribute.waterDefenseAddition + 100) / 100);
            case AttributeType.Rock:
                return characterData.attribute.rockDefense * ((characterData.attribute.rockDefenseAddition + 100) / 100);
            case AttributeType.Electric:
                return characterData.attribute.electricDefense * ((characterData.attribute.electricDefenseAddition + 100) / 100);
            case AttributeType.Dark:
                return characterData.attribute.darkDefense * ((characterData.attribute.darkDefenseAddition + 100) / 100);
        }
        return 0;
    }
}
