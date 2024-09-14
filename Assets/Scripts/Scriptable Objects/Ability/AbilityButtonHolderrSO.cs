using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityButtonHolderSO", menuName = "TDS/AbilityButtonsData/AbilityButtonHolderSO", order = 0)]
[Serializable]

public class AbilityButtonHolderrSO : ScriptableObject
{
    public List<AbilityButtonSO> abilityList;
}