using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityButtonSO", menuName = "TDS/AbilityButtonsData/AbilityButtonSO", order = 0)]
[Serializable]
public class AbilityButtonSO : ScriptableObject
{

    public int cost;
    public int coolDown;
    public Sprite sprite;
    public AbilityButton.AbilityButtonType abilityButtonType;
}