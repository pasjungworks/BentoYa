using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FoodRequirement
{
    public string foodName;
    public int amountNeeded;
}
[System.Serializable]
public class SpecialObjectBonus
{
    public GameObject objectToCheck;
    public string bonusName;
    public int currencyValue;
}
public class CheckFood : MonoBehaviour
{
    public static CheckFood instance;

    [Header("Level Setup")]
    public List<FoodRequirement> requiredFoods;
    public Transform bentoParent;
    public GameObject finishButoon;

    [Header("Special Object Settings")]
    public List<SpecialObjectBonus> specialObjects;

    [HideInInspector]
    public float currentFoodBonusScore;
    public int totalSatisfiesEarned;
    private void Awake()
    {
        instance = this;
        if (finishButoon != null) finishButoon.SetActive(false);
    }
    public void CheckBentoComplete()
    {
        int TotalCurrency = 0;
        HashSet<string> uniqueBonusTypes = new HashSet<string>();
        Dictionary<string, int> currentFoodCounts = new Dictionary<string, int>();

        foreach (Transform child in bentoParent)
        {
            FoodItem food = child.GetComponent<FoodItem>();
            if ((food != null))
            {
                TotalCurrency += food.satisfiesValue;

                if(food.isBonusFood)
                {
                    uniqueBonusTypes.Add(food.foodName);
                }
                if (currentFoodCounts.ContainsKey(food.foodName))
                    currentFoodCounts[food.foodName]++;
                else currentFoodCounts.Add(food.foodName, 1);
            }
        }
        foreach (SpecialObjectBonus special in specialObjects)
        {
            if (special.objectToCheck != null && special.objectToCheck.activeInHierarchy)
            {
                TotalCurrency += special.currencyValue;
                if (!string.IsNullOrEmpty(special.bonusName))
                {
                    uniqueBonusTypes.Add(special.bonusName);
                }
            }
        }
        Debug.Log($"Total Currency: {TotalCurrency}, Unique Bonus Types: {uniqueBonusTypes.Count}");
        float finalBonusScore = CalculateBonus(uniqueBonusTypes.Count);
        totalSatisfiesEarned = TotalCurrency;
        currentFoodBonusScore = finalBonusScore;
        bool isComplete = true;

        foreach (FoodRequirement req in requiredFoods)
        {
            if (!currentFoodCounts.ContainsKey(req.foodName) || currentFoodCounts[req.foodName] < req.amountNeeded)
            {
                isComplete = false; break;
            }
        }
        finishButoon.SetActive(isComplete);
    }
    private float CalculateBonus(int count)
    {
        if(count == 1) return 120f;
        if(count == 2) return 380f;
        if(count == 3) return 770f;
        if(count == 4) return 1290f;
        return 0f;
    }
}
