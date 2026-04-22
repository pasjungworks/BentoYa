using TMPro;
using UnityEngine;

public class ShopHeader : MonoBehaviour
{
    public TextMeshProUGUI walletText;

    void OnEnable()
    {
        RefreshCurrency();
    }

    public void RefreshCurrency()
    {
        // Load the PlayerData which contains satisfiesCurrency
        PlayerData data = SaveSystem.Load();

        // Update the text to match the saved wallet balance
        if (walletText != null)
        {
            walletText.text = data.satisfiesCurrency.ToString();
        }
    }
}
