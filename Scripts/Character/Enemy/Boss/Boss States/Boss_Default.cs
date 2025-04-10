using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BossAttackMode/Default", fileName = "Boss_0_Default")]
public class Boss_Default : BossState
{
    [Header("State Setting")]
    [SerializeField] float[] switchToNextStateTime = {3f, 3.5f};

    BossAndPlayerDistance bossAndPlayerDistance = BossAndPlayerDistance.None;
    Vector3 currentMoveInput;
    float timer;
    bool isSecondStage;
    bool isOutLineEnd;

    protected override void OnEnable()
    {
        base.OnEnable();

        isSecondStage = false;
        bossAndPlayerDistance = BossAndPlayerDistance.None;
    }

    public override void Enter()
    {
        if (boss.characterData.health / boss.characterData.maxHealth <= boss.enterSecondStageHP && !isSecondStage)
        {
            isSecondStage = true;
            bossController.SwitchStateToSecondStage();
            return;
        }

        base.Enter();

        UpdateMoveInput();
        
        timer = Random.Range(switchToNextStateTime[0], switchToNextStateTime[1]);
    }

    public override void Exit()
    {
        base.Exit();

        bossController.SetInputs(Vector3.zero);
        bossAndPlayerDistance = BossAndPlayerDistance.None;
    }

    public override void Update()
    {
        base.Update();

        if (timer <= 0)
        {
            bossController.SwitchStateToRND();
            return;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (Time.frameCount % 30 == 0)
        {
            UpdateMoveInput();
        }
    }
    
    void UpdateMoveInput()
    {
        if (bossController.isOutLine)
        {
            isOutLineEnd = true;
            return;
        }
        if (bossController.bossAndPlayerDistance == bossAndPlayerDistance && !isOutLineEnd) return;

        switch (bossController.bossAndPlayerDistance)
        {
            case BossAndPlayerDistance.Close:
                currentMoveInput = bossController.moveBackDirection[Random.Range(0, bossController.moveBackDirection.Length)];
                bossAndPlayerDistance = BossAndPlayerDistance.Close;
                break;
            case BossAndPlayerDistance.Medium:
                switch (Random.Range(0, 1))
                {
                    case 0:
                        currentMoveInput = bossController.moveBackDirection[Random.Range(0, bossController.moveBackDirection.Length)];
                        break;
                    case 1:
                        currentMoveInput = bossController.moveFrontDirection[Random.Range(0, bossController.moveFrontDirection.Length)];
                        break;
                }
                bossAndPlayerDistance = BossAndPlayerDistance.Medium;
                break;
            case BossAndPlayerDistance.Long:
                currentMoveInput = bossController.moveFrontDirection[Random.Range(0, bossController.moveFrontDirection.Length)];
                bossAndPlayerDistance = BossAndPlayerDistance.Long;
                break;
        }
        isOutLineEnd = false;
        bossController.SetInputs(currentMoveInput);
    }
}
