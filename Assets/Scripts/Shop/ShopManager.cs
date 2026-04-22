using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public int price;
        public Button buyButton;
        public TextMeshProUGUI statusText;
    }

    [Header("Shop Settings")]
    public List<ShopItem> shopItems;
    public TextMeshProUGUI currencyDisplayText;

    [Header("Visuals")]
    public string ownedText = "เป็นเจ้าของแล้ว";
    public Color ownedColor = Color.gray;

    void Start()
    {
        RefreshShop();
    }

    public void RefreshShop()
    {
        PlayerData data = SaveSystem.Load();

        if (currencyDisplayText != null)
            currencyDisplayText.text = data.satisfiesCurrency.ToString();

        foreach (var item in shopItems)
        {
            bool isOwned = data.unlockedItems.Contains(item.itemName);

            if (isOwned)
            {
                item.buyButton.interactable = false;
                if (item.statusText != null) item.statusText.text = ownedText;
                item.buyButton.GetComponent<Image>().color = ownedColor;
            }
            else
            {
                item.buyButton.interactable = (data.satisfiesCurrency >= item.price);
            }
        }
    }

    public void BuyItem(string nameToBuy)
    {
        PlayerData data = SaveSystem.Load();

        ShopItem selectedItem = shopItems.Find(i => i.itemName == nameToBuy);

        if (selectedItem != null)
        {
            if (data.satisfiesCurrency >= selectedItem.price)
            {
                data.satisfiesCurrency -= selectedItem.price;

                data.unlockedItems.Add(selectedItem.itemName);

                SaveSystem.save(data);

                Debug.Log("Purchased: " + selectedItem.itemName);

                RefreshShop();
            }
            else
            {
                Debug.Log("Not enough satisfiesCurrency!");
            }
        }
    }
}
