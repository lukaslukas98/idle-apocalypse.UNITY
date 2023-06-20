using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDetailsPanelScript : MonoBehaviour
{
    public void ToggleDetailsPanel(GameObject panel)
    {
        if(panel.active)
        panel.SetActive(false);
        else
            panel.SetActive(true);

    }
}
