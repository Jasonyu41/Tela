using UnityEngine;

public class Buff_BaseStatusEffect : Buff
{
    [SerializeField] protected StatusEffectEnum statusEffect;

    public override void Enter(Character character)
    {
        base.Enter(character);

        SwitchStatusEffect(statusEffect, true);
    }

    public override void Exit()
    {
        base.Exit();

        SwitchStatusEffect(statusEffect, false);
    }

    void SwitchStatusEffect(StatusEffectEnum statusEffect, bool isActive)
    {
        switch(statusEffect)
        {
            case StatusEffectEnum.BeAttack:
                targetCharacter.statusEffect.beAttack = isActive;
                break;
            case StatusEffectEnum.SlowDown:
                targetCharacter.statusEffect.slowDown = isActive;
                break;
            case StatusEffectEnum.test3:
                targetCharacter.statusEffect.test3 = isActive;
                break;
            case StatusEffectEnum.test4:
                targetCharacter.statusEffect.test4 = isActive;
                break;
            case StatusEffectEnum.test5:
                targetCharacter.statusEffect.test5 = isActive;
                break;
            case StatusEffectEnum.test6:
                targetCharacter.statusEffect.test6 = isActive;
                break;
            case StatusEffectEnum.test7:
                targetCharacter.statusEffect.test7 = isActive;
                break;
            case StatusEffectEnum.test8:
                targetCharacter.statusEffect.test8 = isActive;
                break;
        }
    }
}
