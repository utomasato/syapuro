using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_sub : MonoBehaviour
{
    [SerializeField] private Fire parent;

    private void OnTriggerStay(Collider other)
    {
        if (!parent.GetIsOnCandle() && other.gameObject.CompareTag("FirePosition")) // 火の玉状態で蝋燭に当たったら
        {
            parent.Transfer(other.gameObject.GetComponent<Candlewick>());
        }
        if (!parent.GetIsOnCandle() && other.gameObject.CompareTag("candle")) // 火の玉状態で蝋燭に当たったら
        {
            parent.Transfer(other.gameObject.GetComponent<Candle>());
        }
        if (other.gameObject.CompareTag("water")) // 水に当たったら
        {
            parent.Deletefire();
        }
        if (other.gameObject.CompareTag("wind"))
        {
            if (parent.getIsNormal()) parent.Deletefire();
        }
    }
}
