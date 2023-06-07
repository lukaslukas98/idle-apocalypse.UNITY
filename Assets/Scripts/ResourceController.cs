using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    [SerializeField]
    public GameObject resourceListContainer;
    [SerializeField]
    GameObject resourceFieldPrefab;

    public static ResourceController resourceController;

    public List<Resource> resources = new List<Resource>();
    RectTransform resourceListRectTransform;

    public class Resource
    {
        ResourceController controller = resourceController;
        public int tier;
        public int count;
        int incrementPerS;
        TextMeshProUGUI resourceCountText;
        TextMeshProUGUI resourcePerSText;

        public Resource(int tier, int count, int incrementPerS, TextMeshProUGUI resourceCountText, TextMeshProUGUI resourcePerSText)
        {
            this.tier = tier;
            this.count = count;
            this.incrementPerS = incrementPerS;
            this.resourceCountText = resourceCountText;
            this.resourcePerSText = resourcePerSText;
        }

        //public void AddCountPerS()
        //{
        //    count += incrementPerS;
        //    UpdateCount();
        //    RecalculateIncrement(controller.resources[tier + 1].count);
        //}
        public void AddCount(int amount)
        {
            count += amount;
            UpdateCount();
            if (tier == controller.resources.Count + 1)
            {
                controller.CheckForNextTier(this);
            }
            if (tier < controller.resources.Count - 1)
            {
             //   RecalculateIncrement(controller.resources[tier + 1].count);
            }
        }

        private void UpdateCount()
        {
            resourceCountText.text = count.ToString();
        }

        //public void RecalculateIncrement(int higherTierCount)
        //{
        //    incrementPerS = higherTierCount / 10;// (tier*2 + 1)/3;
        //    resourcePerSText.text = "+" + incrementPerS.ToString() + "/s";
        //}
    }


    // Start is called before the first frame update
    void Start()
    {
        resourceListRectTransform = resourceListContainer.GetComponent<RectTransform>();
        resourceController = this;
        GameObject resourceField = Instantiate(resourceFieldPrefab, new Vector2(resourceListContainer.transform.position.x, resourceListContainer.transform.position.y + 40), resourceListContainer.transform.rotation, resourceListContainer.transform);
        resources.Add(new ResourceController.Resource(0, 0, 0, resourceField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), resourceField.transform.GetChild(1).GetComponent<TextMeshProUGUI>()));
        resourceField.GetComponent<TextMeshProUGUI>().text = "T1 Resources:";
        //StartCoroutine("Increment");
    }

    public void InstantiateNewTier(int tier)
    {
        GameObject resourceField = Instantiate(resourceFieldPrefab, new Vector2(resourceListContainer.transform.position.x, resourceListContainer.transform.position.y - (30 * tier)), resourceListContainer.transform.rotation, resourceListContainer.transform);
        resources.Add(new ResourceController.Resource(tier, 0, 0, resourceField.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), resourceField.transform.GetChild(1).GetComponent<TextMeshProUGUI>()));

        resourceListRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 120 + ((tier) * 70));
        resourceField.GetComponent<TextMeshProUGUI>().text = "T" + (tier + 1) + " Miners:";
    }

    //IEnumerator Increment()
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < resources.Count - 2; i++)
    //        {
    //         //   resources[i].AddCountPerS();
    //          //  resources[i].RecalculateIncrement(resources[i + 1].count);

    //        }
    //        CheckForNextTier(resources[resources.Count - 1]);
    //        yield return new WaitForSeconds(1);
    //    }
    //}

    public void CheckForNextTier(Resource resource)
    {
        if (PromotionCost(resource.tier + 1) - resource.count <= 0 && resource.tier < 100)
        {
          //  InstantiateNewTier(resource.tier + 1);
        }
    }

    public int PromotionCost(int index)
    {
        return (int)Mathf.Pow(2, index + 1);
    }
}
