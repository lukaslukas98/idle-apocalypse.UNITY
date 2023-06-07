using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinesController : MonoBehaviour
{
    [SerializeField]
    public GameObject unitListContainer;
    [SerializeField]
    GameObject unitFieldPrefab;

    static MinesController mineController;

    ResourceController resourceController;

    List<Unit> units = new List<Unit>();
    RectTransform unitListRectTransform;

    public enum UnitType
    {
        Soldier,
        Miner
    }

    public class Unit
    {
        MinesController controller = mineController;
        UnitType type;
        public int tier;
        public int count;
        int incrementPerS;
        TextMeshProUGUI unitCountText;
        TextMeshProUGUI unitPerSText;
        public Button promoteUnitButton;

        public Unit(UnitType type, int tier, int count, int incrementPerS, TextMeshProUGUI unitCountText, TextMeshProUGUI unitPerSText, Button promoteUnitButton)
        {
            this.type = type;
            this.tier = tier;
            this.count = count;
            this.incrementPerS = incrementPerS;
            this.unitCountText = unitCountText;
            this.unitPerSText = unitPerSText;
            this.promoteUnitButton = promoteUnitButton;
            promoteUnitButton.onClick.AddListener(() => controller.PromoteSoldiers(tier));
        }

        public void AddCountPerS()
        {
            count += incrementPerS;
            UpdateCount();
            RecalculateIncrement(controller.units[tier+1].count);
            if(tier!=0)
            controller.CheckPromoteButton(tier);
        }
        public void AddCount(int amount)
        {
            count += amount;
            UpdateCount();
            if(tier == controller.units.Count+1)
            {
                controller.CheckForNextTier();
            }
            if (tier < controller.units.Count - 1)
            {
                RecalculateIncrement(controller.units[tier+1].count);
                controller.CheckPromoteButton(tier + 1);
            }
            if (tier != 0)
                controller.CheckPromoteButton(tier);

            controller.CheckForNextTier(); 
            ResourceController.resourceController.CheckForNextTier(ResourceController.resourceController.resources[ResourceController.resourceController.resources.Count - 1]);
        }

        private void UpdateCount()
        {
            unitCountText.text = count.ToString();
        }

        public void UpdatePromoteButton(bool state)
        {
            promoteUnitButton.interactable = state;
        }

        public void RecalculateIncrement(int higherTierCount)
        {
            incrementPerS = higherTierCount / 10;// (tier*2 + 1)/3;
            unitPerSText.text = "+" + incrementPerS.ToString() + "/s";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        resourceController = ResourceController.resourceController;
        unitListRectTransform = unitListContainer.GetComponent<RectTransform>();
        mineController = this;
        GameObject unitField = Instantiate(unitFieldPrefab,new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y+40), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new MinesController.Unit(UnitType.Miner,0,0,0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<Button>()));
        unitField.GetComponent<TextMeshProUGUI>().text = "T1 Miners:";
        units[0].promoteUnitButton.onClick.RemoveAllListeners();
        units[0].promoteUnitButton.onClick.AddListener(() => units[0].AddCount(1));
        units[0].promoteUnitButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Train";
        StartCoroutine("Increment");
    }

    public void InstantiateNewTier(int tier)
    {
        GameObject unitField = Instantiate(unitFieldPrefab, new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y-(30*tier)), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new MinesController.Unit(UnitType.Miner, tier, 0, 0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<Button>()));

        unitListRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120 + ((tier) * 70));
        unitField.GetComponent<TextMeshProUGUI>().text = "T"+(tier+1)+ " Miners:";
    }

    public void PromoteSoldiers(int index)
    {
        //units[index - 1].AddCount(-PromotionCost(index));
        units[index].AddCount(1);
    }

    IEnumerator Increment()
    {
        while (true)
        {
            for(int i=0; i<units.Count; i++)
            {
                //  units[i].AddCountPerS();
                //  units[i].RecalculateIncrement(units[i + 1].count);
                resourceController.resources[i].AddCount(units[i].count);
             //   resourceController.resources[i].RecalculateIncrement(resourceController.resources[i+1].count);
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void CheckPromoteButton(int index)
    {
        if (PromotionCost(index) - units[index-1].count <= 0)
        {
            units[index].UpdatePromoteButton(true);
        }
        else
        {
            units[index].UpdatePromoteButton(false);
        }
    }

    public void CheckForNextTier()
    {
        Unit unit = mineController.units[mineController.units.Count - 1];
        if (PromotionCost(unit.tier+1) - unit.count <= 0 && unit.tier < 100)
        {
            InstantiateNewTier(unit.tier+1);
            resourceController.InstantiateNewTier(unit.tier + 1);
        }
    }

    public int PromotionCost(int index)
    {
        return (int)Mathf.Pow(2, index + 1);
    }
}
