using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_sub : MonoBehaviour
{
    [SerializeField] private Fire parent;

    private void OnTriggerEnter(Collider other)
    {
        if (!parent.JudgeCandle && other.gameObject.CompareTag("FirePosition")) // 火の玉状態で蝋燭に当たったら
        {
            parent.Transfer(other.gameObject.GetComponent<Candlewick>());
        }
        if (other.gameObject.CompareTag("water"))
        {
            parent.Deletefire();
        }
        if (other.gameObject.CompareTag("wind"))
        {
            if (parent.getIsNormal()) parent.Deletefire();
        }
    }
}
