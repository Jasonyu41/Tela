using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponConfig", menuName = "ComboSystem/CreatNewWeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    [SerializeField] public List<ComboConfig> lightComboConfigs = new List<ComboConfig>();
    [SerializeField] public List<ComboConfig> midComboConfigs = new List<ComboConfig>();
    [SerializeField] public List<ComboConfig> heavyComboConfigs = new List<ComboConfig>();
}
