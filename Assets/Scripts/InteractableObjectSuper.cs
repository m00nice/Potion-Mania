using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObjectSuper : MonoBehaviour
{
    [SerializeField] private GameObject pressE;

    public void ShowE()
    {
        pressE.gameObject.SetActive(true);
    }

    public void HideE()
    {
        pressE.gameObject.SetActive(false);
    }

    public abstract void Interact(Player player);

}
