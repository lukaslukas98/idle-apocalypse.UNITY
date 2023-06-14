using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject panel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        panel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        panel.SetActive(false);
    }
}
