using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityButtonHolderrSO", menuName = "TDS/Ability Buttons Data/AbilityButtonHolderrSO", order = 0)]
public class AbilityButtonHolderSO : ScriptableObject
{
    public List<AbilityButtonSO> abilityList;
}