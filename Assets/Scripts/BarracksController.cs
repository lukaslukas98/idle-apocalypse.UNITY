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

    public static List<Unit> units = new List<Unit>();
    RectTransform unitListRectTransform;

    //public class Soldier : Unit
    //{
    //    public Soldier(UnitType type, int tier, int count, int incrementPerS, TextMeshProUGUI unitCountText, TextMeshProUGUI unitPerSText, Button promoteUnitButton) : base(type, tier, count, incrementPerS, unitCountText, unitPerSText, promoteUnitButton)
    //    {
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        unitListRectTransform = unitListContainer.GetComponent<RectTransform>();
        controller1 = this;
        GameObject unitField = Instantiate(unitFieldPrefab,new Vector2(0, 0), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new Unit(Unit.UnitType.Soldier,0,0,0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(3).GetComponent<Button>()));
        unitField.transform.localPosition = unitListRectTransform.localPosition;
        units[0].unitText.text = "T1 Soldiers:";
        units[0].promoteUnitButton.onClick.RemoveAllListeners();
        units[0].promoteUnitButton.onClick.AddListener(() => units[0].AddCount(1,1));
        units[0].promoteUnitButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Train";
        StartCoroutine("Increment");
    }

    public void InstantiateNewTier(int tier)
    {
        GameObject unitField = Instantiate(unitFieldPrefab, new Vector2(0, 0), unitListContainer.transform.rotation, unitListContainer.transform);
        units.Add(new Unit(Unit.UnitType.Soldier, tier, 0, 0, unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(1).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(2).GetComponent<TextMeshProUGUI>(), unitField.transform.GetChild(3).GetComponent<Button>()));
        unitField.transform.localPosition = new Vector2(unitListRectTransform.localPosition.x, unitListRectTransform.localPosition.x - (120 * (tier))+30);
        unitField.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => PromoteSoldiers(tier));
        //unitListRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical)
        //unitListRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120+(70 * tier));
        //Resize(70, new Vector3(0, 1, 0), unitListRectTransform);
        unitField.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "T"+(tier+1)+" Soldiers:";
    }

    public void PromoteSoldiers(int index)
    {
        units[index - 1].AddCount(-PromotionCost(index), units.Count);
        units[index].AddCount(1, units.Count);
        Debug.Log(index + " total count" + units.Count);
        if(units.Count > index+1)
        {
            Debug.Log("checked");
            units[index + 1].CheckPromoteButton(index);
        }
    }

    IEnumerator Increment()
    {
        while (true)
        {
            for(int i=0; i<units.Count-1; i++)
            {
                units[i].AddCountPerS(units[i+1].count);
                units[i].RecalculateIncrement(units[i + 1].count);
                units[i+1].CheckPromoteButton(i-1);

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
