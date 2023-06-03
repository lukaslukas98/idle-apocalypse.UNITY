using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaracksController : MonoBehaviour
{
    [SerializeField]
    public GameObject unitListContainer;
    [SerializeField]
    GameObject unitFieldPrefab;

    static BaracksController controller1;

    public enum UnitType
    {
        Soldier,
        Miner
    }

    public class Unit
    {
        BaracksController controller = controller1;
        UnitType type;
        public int tier;
        public int count;
        int incrementPerS;
        TextMeshProUGUI unitCountText;
        TextMeshProUGUI unitPerSText;
        public Button promoteUnit;

        public Unit(UnitType type, int tier, int count, int incrementPerS, TextMeshProUGUI unitCountText, TextMeshProUGUI unitPerSText, Button promoteUnit)
        {
            this.type = type;
            this.tier = tier;
            this.count = count;
            this.incrementPerS = incrementPerS;
            this.unitCountText = unitCountText;
            this.unitPerSText = unitPerSText;
            this.promoteUnit = promoteUnit;
            promoteUnit.onClick.AddListener(() => controller.PromoteSoldiers(tier));
        }

        public void AddCount()
        {
            count += incrementPerS;
            UpdateCount();
            controller.CheckPromoteButton(tier - 1);
        }
        public void AddCount(int amount)
        {
            count += amount;
            UpdateCount();
            if(tier<controller.units.Count)
            controller.CheckPromoteButton(tier - 1);
        }

        private void UpdateCount()
        {
            unitCountText.text = count.ToString();
        }

        public void UpdatePromoteButton(bool state)
        {
            promoteUnit.interactable = state;
        }

        public void RecalculateIncrement(int higherTierCount)
        {
            incrementPerS = higherTierCount / 10;// (tier*2 + 1)/3;
            unitPerSText.text = "+" + incrementPerS.ToString() + "/s";
        }
    }

    [SerializeField]
    List<Unit> units = new List<Unit>();
    RectTransform unitListRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        unitListRectTransform = unitListContainer.GetComponent<RectTransform>();
        controller1 = this;
        GameObject unitField = Instantiate(unitFieldPrefab,new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y+40), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new BaracksController.Unit(UnitType.Soldier,1,0,0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<Button>()));
        units[0].promoteUnit.onClick.RemoveAllListeners();
        units[0].promoteUnit.onClick.AddListener(() => units[0].AddCount(1));
       // InstantiateNewTier(1);
        StartCoroutine("Increment");
    }

    public void InstantiateNewTier(int tier)
    {
        GameObject unitField = Instantiate(unitFieldPrefab, new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y-(30*tier+1)), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new BaracksController.Unit(UnitType.Soldier, tier+1, 0, 0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<Button>()));

        unitListRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120 + (tier * 70));
        unitField.GetComponent<TextMeshProUGUI>().text = "T"+(tier+1)+" Soldiers:";
    }

    public void PromoteSoldiers(int tier)
    {
        AddSoldiers(-(int)Mathf.Pow(2, tier), tier-1);
        AddSoldiers(1, tier);
    }

    IEnumerator Increment()
    {
        while (true)
        {
            for(int i=0; i<units.Count-1; i++)
            {
                units[i].AddCount();
                units[i].RecalculateIncrement(units[i + 1].count);
                CheckPromoteButton(i);

            }
            CheckForNextTier(units[units.Count-1]);
            yield return new WaitForSeconds(1);
        }
    }


    public void AddSoldiers(int amount, int tier)
    {
        units[tier-1].AddCount(amount);
    }

    private void CheckPromoteButton(int tier)
    {
        if (Mathf.Pow(2, tier+2) - units[tier].count <= 0)
        {
            units[tier + 1].UpdatePromoteButton(true);
        }
        else
        {
            units[tier + 1].UpdatePromoteButton(false);
        }
    }

    public void CheckForNextTier(Unit unit)
    {
        if ((int)Mathf.Pow(2, unit.tier + 1) - unit.count <= 0 && unit.tier < 100)
        {
            InstantiateNewTier(unit.tier);
        }
    }

    public void debug()
    {
        Debug.Log(units.Count);
    }
}
