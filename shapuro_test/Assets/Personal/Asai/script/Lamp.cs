using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject fire; // 点火用の火
    [SerializeField] private GameState state;
    [SerializeField] private Candles_by_Fireplace CbF;
    [SerializeField] private Fire player; // プレイヤー
    private bool IsBurning = false; // 点火済みか
    [SerializeField] private int LampID;
    void Start()
    {
        GameObject Set_CbF = GameObject.Find("CbF");
        CbF = Set_CbF.GetComponent<Candles_by_Fireplace>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && IsBurning == false && player.GetIsCandle())
        {
            IsBurning = true;
            fire.GetComponent<SpriteRenderer>().enabled = true; // 点火する
            state.AddLampCount(LampID); // カウントをインクリメントする
            CbF.MiniCandlefire();
        }
    }
}