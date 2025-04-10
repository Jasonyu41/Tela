using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    const string CloneText = "(Clone)";

    public static void AddBuff(GameObject targetGameObject, Buff addBuff)
    {
        Character targetCharacter = targetGameObject.GetComponent<Character>();
        
        AddBuff(targetCharacter, addBuff);
    }

    public static void AddBuff(Character targetCharacter, Buff addBuff)
    {
        if (!targetCharacter.buffsNameList.Contains(addBuff.name + CloneText))
        {
            Buff buff = PoolManager.Release(addBuff.gameObject).GetComponent<Buff>();

            targetCharacter.buffsList.Add(buff);
            targetCharacter.buffsNameList.Add(buff.name);
            
            buff.Enter(targetCharacter);
        }
        else
        {
            targetCharacter.buffsList[targetCharacter.buffsNameList.IndexOf(addBuff.name + CloneText)].buffTimer = 0;
        }
    }

    public static void RemoveBuff(GameObject targetGameObject, Buff removebuff)
    {
        Character targetCharacter = targetGameObject.GetComponent<Character>();

        RemoveBuff(targetCharacter, removebuff);
    }

    public static void RemoveBuff(Character targetCharacter, Buff removebuff)
    {
        targetCharacter.buffsList.Remove(removebuff);
        targetCharacter.buffsNameList.Remove(removebuff.name);
        
        removebuff.Exit();
    }
}
