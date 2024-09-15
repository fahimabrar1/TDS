using UnityEngine;

[CreateAssetMenu(fileName = "AbilituButtonDataNewSO", menuName = "TDS/Ability Buttons Data/AbilituButtonDataNewSO", order = 0)]
public class AbilituButtonDataNewSO : ScriptableObject
{

    public int cost;
    public int coolDown;
    public Sprite sprite;
    public AbilityButton.AbilityButtonType abilityButtonType;
}