using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_SlowDown : Buff_BaseStatusEffect
{
    [SerializeField] float effectValue;

    public override void Enter(Character character)
    {
        base.Enter(character);
        
        character.AddAnimationSpeed(effectValue / 100);
    }

    public override void Exit()
    {
        base.Exit();

        targetCharacter.AddAnimationSpeed(-effectValue / 100);
    }
}
