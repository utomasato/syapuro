using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StagePlayData
{
    [SerializeField] private int id; // ステージID
    [SerializeField] private string name; // ステージ名
    [SerializeField] private int maxLamp; // ステージ内のランプの数
    [SerializeField] private int lampCount; // つけることができたLampの数

    public StagePlayData(int id, string name, int maxLamp, int lampCount)
    {
        this.id = id;
        this.name = name;
        this.maxLamp = maxLamp;
        this.lampCount = lampCount;
    }

    public int Id
    {
        get { return id; }
    }

    public string Name
    {
        get { return name; }
    }

    public int MaxLamp
    {
        get { return maxLamp; }
    }

    public int LampCount
    {
        set { if (value > this.lampCount) this.lampCount = value; }
        get { return lampCount; }
    }
}