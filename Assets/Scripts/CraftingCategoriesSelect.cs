using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCategoriesSelect : MonoBehaviour
{
    [SerializeField]
    GameObject selectPanel;

    GameObject activePanel;


    public void ShowPanel(GameObject categoryPanel)
    {
        categoryPanel.SetActive(true);
        selectPanel.SetActive(false);

        activePanel = categoryPanel;
    }

    public void HidePanel()
    {
        activePanel.SetActive(false);
        selectPanel.SetActive(true);
    }
}
