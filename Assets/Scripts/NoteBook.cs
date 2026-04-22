using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ShopCategory
{
    public string categoryName;
    public GameObject[] pages;
}
public class NoteBook : MonoBehaviour
{
    public List<ShopCategory> categories;

    private int currentCategoryIndex = 0;
    private int currentPageIndex = 0;

    void Start()
    {
        SwitchCategory(0);
    }

    public void SwitchCategory(int categoryIndex)
    {
        currentCategoryIndex = categoryIndex;
        currentPageIndex = 0;
        UpdateUI();
    }

    public void NextPage()
    {
        currentPageIndex++;

        if (currentPageIndex >= categories[currentCategoryIndex].pages.Length)
        {
            currentPageIndex = 0;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (var cat in categories)
        {
            foreach (var page in cat.pages)
            {
                page.SetActive(false);
            }
        }

        categories[currentCategoryIndex].pages[currentPageIndex].SetActive(true);
    }
}
