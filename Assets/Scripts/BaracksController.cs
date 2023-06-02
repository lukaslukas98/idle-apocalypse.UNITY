using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaracksController : MonoBehaviour
{
    [SerializeField]
    GameObject baracksPanel;

    //[SerializeField]
    //TextMeshProUGUI T1SoldierCountText;
    //[SerializeField]
    //TextMeshProUGUI T1SoldiersPerSText;
    //[SerializeField]
    //TextMeshProUGUI T2SoldierCountText;
    //[SerializeField]
    //TextMeshProUGUI T2SoldiersPerSText;

    [SerializeField]
    public GameObject unitListContainer;
    [SerializeField]
    GameObject unitFieldPrefab;

    //float T1soldiers = 100;
    //float T2soldiers = 0;

    [SerializeField]
    Button promoteButton;

    public enum UnitType
    {
        Soldier,
        Miner
    }

    public class Unit
    {
        BaracksController controller = new BaracksController();
        UnitType type;
        public int tier;
        public int count;
        int incrementPerS;
        TextMeshProUGUI unitCountText;
        TextMeshProUGUI unitPerSText;
        GameObject promoteUnit;

        public Unit(UnitType type, int tier, int count, int incrementPerS, TextMeshProUGUI unitCountText, TextMeshProUGUI unitPerSText, GameObject promoteUnit)
        {
            this.type = type;
            this.tier = tier;
            this.count = count;
            this.incrementPerS = incrementPerS;
            this.unitCountText = unitCountText;
            this.unitPerSText = unitPerSText;
            this.promoteUnit = promoteUnit;
        }

        public void AddCount()
        {
            count += incrementPerS;
            UpdateCount();
        }
        public void AddCount(int amount)
        {
            count += amount;
            UpdateCount();
        }

        private void UpdateCount()
        {
            unitCountText.text = count.ToString();
        }

        public void RecalculateIncrement(int higherTierCount)
        {
            incrementPerS = higherTierCount / (tier*2 + 1)/3;
            unitPerSText.text = "+" + incrementPerS.ToString() + "/s";
        }

        public void PromoteSoldiers(int tier)
        {
            controller.AddSoldiers(-10, tier - 1);
            controller.AddSoldiers(1, tier);
            if ((tier * 2 + 1) * 3 - count < 0)
            {
               // ChangePromoteButton(true);
            }
            else
            {
              //  ChangePromoteButton(false);
            }
        }
    }

    [SerializeField]
    List<Unit> units = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject unitField = Instantiate(unitFieldPrefab,new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new BaracksController.Unit(UnitType.Soldier,1,0,0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).gameObject));
        StartCoroutine("Increment");
    }

    public void InstantiateNewTier(int tier)
    {
        GameObject unitField = Instantiate(unitFieldPrefab, new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y-(80*tier)), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new BaracksController.Unit(UnitType.Soldier, tier+1, 0, 0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).gameObject));
        unitListContainer.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100+(tier*80));
        //unitListContainer.GetComponent<RectTransform>().
        unitField.GetComponent<TextMeshProUGUI>().text = "T"+(tier+1)+" Soldiers:";
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

    public void PromoteSoldiers(int tier)
    {
        AddSoldiers(-10,tier-1);
        AddSoldiers(1, tier);
        if ((units[tier-1].tier * 2 + 1) * 3 - units[tier-1].count < 0)
        {
            ChangePromoteButton(true);
        }
        else
        {
            ChangePromoteButton(false);
        }
    }

    IEnumerator Increment()
    {
        while (true)
        {
            for(int i=0; i<units.Count-1; i++)
            {
                units[i].AddCount();
                units[i].RecalculateIncrement(units[i + 1].count);
            }
            CheckForNextTier(units[units.Count-1]);
            yield return new WaitForSeconds(1);
        }
    }


    public void AddSoldiers(int amount, int tier)
    {
        units[tier].AddCount(amount);

        if ((units[tier].tier * 2 + 1) * 3 - units[tier].count < 0)
        {
            ChangePromoteButton(true);
        }
        else
        {
            ChangePromoteButton(false);
        }
    }


    public void AddSoldiers(int amount)
    {
        units[0].AddCount(amount);

        if (units[0].count >= 10)
        {
            ChangePromoteButton(true);
        }
        else
        {
            ChangePromoteButton(false);
        }
    }

    private void ChangePromoteButton(bool state)
    {
        promoteButton.interactable = state;
    }

    public void CheckForNextTier(Unit unit)
    {
        if ((unit.tier * 2 + 1) * 3 - unit.count < 0)
        {
            InstantiateNewTier(unit.tier);
        }
    }
}
