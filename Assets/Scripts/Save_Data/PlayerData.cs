using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int totalStars;
    public int satisfiesCurrency;

    public List<string> ClearedLevel = new List<string>();
    public List<int> levelHighestScore = new List<int>();
    public List<int> levelMaxStars = new List<int>();
    public List<int> levelSatisfiesCurrencyHighest = new List<int>();

    public List<string> unlockedItems = new List<string>();
}
