using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField] public float buffDuration;
    [SerializeField] bool timeIsFrame;

    protected Character targetCharacter;
    [HideInInspector] public float buffTimer;


    public virtual void Enter(Character targetCharacter)
    {
        this.targetCharacter = targetCharacter;
        buffTimer = 0;
    }

    public virtual void LogicUpdate()
    {
        TimerUpdate();
    }

    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }

    void TimerUpdate()
    {
        buffTimer += !timeIsFrame ? Time.deltaTime : 1;

        if (buffTimer >= buffDuration)
        {
            BuffManager.RemoveBuff(targetCharacter, this);
        }
    }
}