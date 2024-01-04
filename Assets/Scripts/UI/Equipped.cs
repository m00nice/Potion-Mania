using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equipped : MonoBehaviour
{
    [SerializeField] private Image imagePotion;
    [SerializeField] private TextMeshProUGUI potionAmountText;
    private PotionObject currentPotionObject;

    void Start()
    {
        StartCoroutine(CheckPotion());
    }

    private IEnumerator CheckPotion()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(0.3f);
            if (currentPotionObject == null || currentPotionObject != Player.Instance.SelectedPotion)
            {
                currentPotionObject = Player.Instance.SelectedPotion;
                potionAmountText.text = currentPotionObject.PotionAmount.ToString();
                imagePotion.sprite = currentPotionObject.PotionPrefab.SpriteOfPotion;
            }
            else if(currentPotionObject != null)
            {
                potionAmountText.text = currentPotionObject.PotionAmount.ToString();
            }
        }
    }
}