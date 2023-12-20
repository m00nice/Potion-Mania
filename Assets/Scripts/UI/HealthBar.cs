using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject heart;
    [SerializeField] private Player player;

    private void Start()
    {
        StartCoroutine(CheckHealth());
    }

    private IEnumerator CheckHealth()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(0.2f);
            if (transform.childCount != player.CurrentHealth)
            {
                foreach (Transform heartChild in transform)
                {
                    Destroy(heartChild.gameObject);
                }
                for (int i = 0; i < player.CurrentHealth; i++)
                {
                    Instantiate(heart, transform, false);
                }
            }
        }
    }
}