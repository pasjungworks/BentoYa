using System.Collections.Generic;
using UnityEngine;

public class RiceSwap : MonoBehaviour
{
    [Header("Bento Settings")]
    public List<GameObject> bentoList;
    private string lastSelectedRiceTag = "WhiteRice";
    public Transform currentBento;

    public void SwapRice(string targetTag)
    {
        lastSelectedRiceTag = targetTag;

        GameObject activeBento = null;
        foreach (GameObject bento in bentoList)
        {
            if (bento.activeSelf)
            {
                activeBento = bento;
                break;
            }
        }

        if (activeBento != null)
        {
            Transform[] allChildren = activeBento.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in allChildren)
            {
                if (child.CompareTag(targetTag))
                {
                    child.gameObject.SetActive(true);
                }
                else if (IsARiceTag(child.tag))
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("No active Bento found! Make sure one is SetActive(true).");
        }
    }
    bool IsARiceTag(string t)
    {
        return t == "WhiteRice" || t == "RabbitRice" || t == "CircleRice" || t == "CatRice";
    }
}
