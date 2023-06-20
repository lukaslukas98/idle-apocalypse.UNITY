using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingAmountUI : MonoBehaviour
{

    //Input input;
    TMP_InputField input;
    int amount=0;

    //[SerializeField]
   // TextMeshProUGUI queueUI;

    public void CraftAmount(TextMeshProUGUI queueUI)
    {
        queueUI.text = "Queue: " + amount;
        amount = 0;
        input.text = "";
    }


    public void SetAmount()
    {
        if(input.text != "")
        amount = int.Parse(input.text);
    }

    public void AddAmount(int amountToAdd)
    {
        amount += amountToAdd;
        input.text = amount.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
