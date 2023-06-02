using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaracksController : MonoBehaviour
{
    [SerializeField]
    GameObject baracksPanel;

    [SerializeField]
    TextMeshProUGUI T1SoldierCountText;
    [SerializeField]
    TextMeshProUGUI T2SoldierCountText;

    float T1soldiers = 100;
    float T2soldiers = 0;

    [SerializeField]
    Button promoteButton;

    struct Unit
    {
        string name;
        int tier;
        int count;
        int incrementPerS;
    }

    List<Unit> units = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Increment");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        baracksPanel.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        baracksPanel.SetActive(false);
    }

    public void AddSoldiers(int amount)
    {
        T1soldiers += amount;

        UpdateSoldierCountT1();
        if(T1soldiers >= 10) {
            ChangePromoteButton(true);
        }
        else
        {
            ChangePromoteButton(false);
        }
    }

    private void UpdateSoldierCountT1()
    {
        T1SoldierCountText.text = T1soldiers.ToString();
    }

    private void ChangePromoteButton(bool state)
    {
        promoteButton.interactable = state;
    }

    public void PromoteSoldiers()
    {
        AddSoldiers(-10);
        AddSoldiersT2(1);
        UpdateSoldierCountT2();
    }

    public void AddSoldiersT2(int amount)
    {
        T2soldiers += amount;
    }

    IEnumerator Increment()
    {
        while (true)
        {
            AddSoldiers(Mathf.FloorToInt(T2soldiers / 10));

            yield return new WaitForSeconds(1);
        }
    }
    private void UpdateSoldierCountT2()
    {
        T2SoldierCountText.text = T2soldiers.ToString();
    }
}
