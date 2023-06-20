using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCrafting : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI crafterCountUI;
    [SerializeField]
    Slider crafterCountSlider;

    int currentItems = 0;
    [SerializeField]
    TextMeshProUGUI currentItemsUI;

    [SerializeField]
    Input craftAmountUI;
    int craftAmount;

    float craftDuration=1;
    float craftResourcesRequired=1;

    public void Start()
    {
      //  craftAmountUI.
    }

    public IEnumerator Increment()
    {
        while (true)
        {


                //resourceController.resources[i].AddCount(units[i].count);

            yield return new WaitForSeconds(1);
        }
    }
}
