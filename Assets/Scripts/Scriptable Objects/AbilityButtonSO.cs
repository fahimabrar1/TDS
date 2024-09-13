using UnityEngine;

[CreateAssetMenu(fileName = "AbilityButtonSO", menuName = "TDS/Ability Buttons Data/AbilityButtonSO", order = 0)]
public class AbilityButtonSO : ScriptableObject
{

    public int cost;
    public Sprite sprite;
    public AbilityButton.AbilityButtonType abilityButtonType;
}