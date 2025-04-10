using UnityEngine;

public class BossEnterBattleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.GetComponent<Boss>().EnterBattle();
            gameObject.SetActive(false);
        }
    }
}
