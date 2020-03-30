using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryPuzzle : MonoBehaviour
{
    public List<Tier> Tiers = new List<Tier>();

    void Update()
    {
        Solved();
    }

    private void Solved()
    {
        foreach (var tier in Tiers)
        {
            tier.CheckSolved();
        }
    }
}
