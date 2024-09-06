// using System;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using Sirenix.OdinInspector;
// public class CurrencyButton : MonoBehaviour
// {


//     [BoxGroup("Components")]
//     public TextMeshProUGUI costText;
//     [BoxGroup("Components")]
//     public Button button;
//     [BoxGroup("Components")]
//     public CurrencyManager currencyManager;
//     [BoxGroup("Components")]
//     public Image character;

//     [BoxGroup("Data")]
//     public bool canBuy;

//     [BoxGroup("Data")]
//     public int cost;

//     [BoxGroup("Data")]
//     public SoldierType soldierType;

//     /// <summary>
//     /// This function is called when the object becomes enabled and active.
//     /// </summary>
//     public void SetOnUpdateButtonsAction()
//     {
//         currencyManager.OnUpdateButtonsAction += OnUpdateButtons;
//     }


//     /// <summary>
//     /// This function is called when the behaviour becomes disabled or inactive.
//     /// </summary>
//     void OnDisable()
//     {
//         currencyManager.OnUpdateButtonsAction -= OnUpdateButtons;
//     }

//     /// <summary>
//     /// Start is called on the frame when a script is enabled just before
//     /// any of the Update methods is called the first time.
//     /// </summary>
//     void Start()
//     {
//         canBuy = false;
//         costText.text = cost.ToString();
//     }




//     private void OnUpdateButtons(int currencyValue)
//     {
//         if (currencyValue >= cost)
//         {
//             button.interactable = true;
//             //Todo: make is purchasable
//         }
//         else
//         {
//             button.interactable = false;
//         }
//     }



//     public void OnClickButton()
//     {
//         currencyManager.OnClickCurrencyButton(cost);
//         GameManager.instance.battleManager.playerbase.SpawnSoldier();
//     }
// }