using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject fire; // 点火用の火
    [SerializeField] private GameState state;
    [SerializeField] private Fire player; // プレイヤー
    private bool IsBurning = false; // 点火済みか

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && IsBurning == false && player.GetIsCandle())
        {
            IsBurning = true;
            fire.GetComponent<SpriteRenderer>().enabled = true; // 点火する
            state.AddLampCount(); // カウントをインクリメントする
        }
    }

    public bool GetBurning => IsBurning;
}