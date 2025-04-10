using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using KinematicCharacterController;
using UnityEngine;

public class PartyManager : Singleton<PartyManager>
{
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] VoidEventChannel switchPartyMemverEventChannel;
    [SerializeField] Vector3 offset;
    [SerializeField] float waitToDisableTime;
    public static Player[] party = new Player[3];

    Animator backAnimator;
    Animator goAheadAnimator;
    Player tempPlayer;

    protected override void Awake()
    {
        base.Awake();

        party[0] = transform.GetChild(0).GetComponent<Player>();
        party[1] = transform.GetChild(1).GetComponent<Player>();
        party[2] = transform.GetChild(2).GetComponent<Player>();
    }

    private void Start()
    {
        party[0].gameObject.SetActive(true);
        party[1].gameObject.SetActive(false);
        party[2].gameObject.SetActive(false);
    }

    public static void SwitchPartyMember(int index)
    {
        if (party[index].characterData.health != 0 && party[index] != Instance.tempPlayer)
        {
            Instance.backAnimator = party[0].gameObject.GetComponent<Animator>();
            Instance.goAheadAnimator = party[index].gameObject.GetComponent<Animator>();

            party[0].GetComponent<PlayerCharacterController>().motor.Capsule.enabled = false;
            Vector3 tempPosition = party[0].transform.TransformPoint(Instance.offset);
            KinematicCharacterMotor tempMotor = party[index].GetComponent<PlayerCharacterController>().motor;
            tempMotor.Capsule.enabled = true;
            tempMotor.SetPosition(tempPosition);
            tempMotor.SetRotation(party[0].transform.rotation);

            party[index].gameObject.SetActive(true);
            Instance.StartCoroutine(Instance.PlayAnimation(party[0]));

            // party[index].cinemachineFreeLook.m_YAxis = party[0].cinemachineFreeLook.m_YAxis;
            // party[index].cinemachineFreeLook.m_XAxis = party[0].cinemachineFreeLook.m_XAxis;
            party[0].cinemachineFreeLook.Priority = 9;
            party[0].cinLookAt.Priority = 5;
            party[index].cinemachineFreeLook.Priority = 11;
            party[index].cinLookAt.Priority = 5;
            if (GameManager.Instance.isLookAtEnemy)
            {
                (party[index].cinemachineFreeLook.Priority, party[index].cinLookAt.Priority) = (party[index].cinLookAt.Priority, party[index].cinemachineFreeLook.Priority);
            }

            (party[0], party[index]) = (party[index], party[0]);
            UI_HUD.SwitchCharacterStats(index);
            Instance.switchPartyMemverEventChannel.Broadcast();

            // Instance.cinemachineFreeLook.Follow = party[0].transform;
            // Instance.cinemachineFreeLook.LookAt = party[0].transform.Find("Camera Follow Point");
        }
    }

    IEnumerator PlayAnimation(Player player)
    {
        Instance.tempPlayer = player;
        Instance.backAnimator.CrossFade("Dodge_B", 0.1f);
        Instance.goAheadAnimator.CrossFade("Dodge_F", 0.1f);

        yield return new WaitForSeconds(waitToDisableTime);

        player.gameObject.SetActive(false);
        Instance.tempPlayer = null;
    }
}
