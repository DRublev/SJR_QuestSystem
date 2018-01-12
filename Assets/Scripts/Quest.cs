using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public string Name { get; set; }
    public int ExpReward = 0;

    public Quest(string name, int expReward)
    {
        Name = name;
        ExpReward = expReward;
    }
}
