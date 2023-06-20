using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    public enum UnitType
    {
        Soldier,
        Miner
    }

    
    int promotionCost;
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
        promotionCost = (int)Mathf.Pow(2, tier + 1);
       // promoteUnitButton.onClick.AddListener(() => PromoteSoldiers(tier));
    }

    //public void PromoteSoldiers(int index)
    //{
    //    units[index - 1].AddCount(-PromotionCost(index), units[index].count, units.Count);
    //    units[index].AddCount(1, units[index + 1].count, units.Count);
    //}
    public void AddCountPerS(int higherTierCount)
    {
        count += incrementPerS;
        UpdateCount();
        RecalculateIncrement(higherTierCount);
        if (tier != 0)
            CheckPromoteButton(tier);
    }
    public void AddCount(int amount, int higherTierCount, int lastTierUnlocked)
    {
        count += amount;
        UpdateCount();
        if (tier < lastTierUnlocked - 1)
        {
            RecalculateIncrement(higherTierCount);
            CheckPromoteButton(tier + 1);
        }
        if (tier != 0)
            CheckPromoteButton(tier);
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


    private void CheckPromoteButton(int previousTierCount)
    {
        if (promotionCost - previousTierCount <= 0)
        {
            UpdatePromoteButton(true);
        }
        else
        {
            UpdatePromoteButton(false);
        }
    }

}
