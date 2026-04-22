using System;
using UnityEngine;

public class ChangeBento : MonoBehaviour
{
    public GameObject[] bentoModel;
    public GameObject[] decorationList;

    void Start()
    {
        foreach (var item in decorationList)
        {
            item.SetActive(false);
        }
    }
    public void SelectBento(int index)
    {
        if (index < 0 || index >= bentoModel.Length) return;

        for (int i = 0; i < bentoModel.Length; i++)
        {
            bentoModel[i].SetActive(i == index);
        }
    }
    public void ToggleDecoration(int index)
    {
        if (index >= 0 && index < decorationList.Length)
        {
            GameObject item = decorationList[index];

            // Toggle: If it's on, turn it off. If it's off, turn it on.
            item.SetActive(!item.activeSelf);
        }
        else
        {
            Debug.LogWarning("Index " + index + " is not in the decoration list!");
        }
    }
}
