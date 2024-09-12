using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{

    private Button button;

    public TMP_Text levelText;
    public TMP_Text costText;
    public TMP_Text valueText;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (TryGetComponent(out Button btn))
            button = btn;
    }




}
