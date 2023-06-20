using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarracksController : MonoBehaviour
{
    [SerializeField]
    public GameObject unitListContainer;
    [SerializeField]
    GameObject unitFieldPrefab;

    static BarracksController controller1;

    List<Unit> units = new List<Unit>();
    RectTransform unitListRectTransform;

    public class Soldier : Unit
    {
        public Soldier(UnitType type, int tier, int count, int incrementPerS, TextMeshProUGUI unitCountText, TextMeshProUGUI unitPerSText, Button promoteUnitButton) : base(type, tier, count, incrementPerS, unitCountText, unitPerSText, promoteUnitButton)
        {
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        unitListRectTransform = unitListContainer.GetComponent<RectTransform>();
        controller1 = this;
        GameObject unitField = Instantiate(unitFieldPrefab,new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y+40), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new Unit(Unit.UnitType.Soldier,0,0,0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<Button>()));
        unitField.GetComponent<TextMeshProUGUI>().text = "T1 Soldiers:";
        units[0].promoteUnitButton.onClick.RemoveAllListeners();
        units[0].promoteUnitButton.onClick.AddListener(() => units[0].AddCount(1,1,1));
        units[0].promoteUnitButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Train";
        StartCoroutine("Increment");
    }

    public void InstantiateNewTier(int tier)
    {
        GameObject unitField = Instantiate(unitFieldPrefab, new Vector2(unitListContainer.transform.position.x, unitListContainer.transform.position.y-(30*tier)), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new Unit(Unit.UnitType.Soldier, tier, 0, 0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<Button>()));

        unitListRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120 + ((tier) * 70));
        unitField.GetComponent<TextMeshProUGUI>().text = "T"+(tier+1)+" Soldiers:";
    }

    public void PromoteSoldiers(int index)
    {
        units[index - 1].AddCount(-PromotionCost(index), units[index].count,units.Count);
        units[index].AddCount(1, units[index+1].count, units.Count);
    }

    IEnumerator Increment()
    {
        while (true)
        {
            for(int i=0; i<units.Count-2; i++)
            {
                units[i].AddCountPerS(units[i+1].count);
                units[i].RecalculateIncrement(units[i + 1].count);

            }
            CheckForNextTier(units[units.Count-1]);
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

    public void CheckForNextTier(Unit unit)
    {
        if (PromotionCost(unit.tier+1) - unit.count <= 0 && unit.tier < 100)
        {
            InstantiateNewTier(unit.tier+1);
        }
    }

    public int PromotionCost(int index)
    {
        return (int)Mathf.Pow(2, index + 1);
    }
}
