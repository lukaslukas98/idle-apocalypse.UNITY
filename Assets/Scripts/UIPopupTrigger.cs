using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject baracksPanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        baracksPanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        baracksPanel.SetActive(false);
    }
}
