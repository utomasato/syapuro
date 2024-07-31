using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{
    public int score;
    public void addscores(int x)
    {
        score += x;
    }
    public int getscores()
    {
        return score;
    }
}
