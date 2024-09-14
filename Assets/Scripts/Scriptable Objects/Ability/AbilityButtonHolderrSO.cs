using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityButtonHolderSO", menuName = "TDS/Ability Buttons Data/AbilityButtonHolderSO", order = 0)]
public class AbilityButtonHolderrSO : ScriptableObject
{
    public List<AbilityButtonSO> abilityList;
}