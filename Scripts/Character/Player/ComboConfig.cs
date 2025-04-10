using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComboConfig", menuName = "ComboSystem/CreatNewComboConfig")]
public class ComboConfig : ScriptableObject
{
    [SerializeField] public string animationName;
    [SerializeField] public float animationFadeTime = 0.1f;
    [SerializeField] public float releaseTime = 1f;
    [SerializeField] public float durationTime = 0.4f;
}
