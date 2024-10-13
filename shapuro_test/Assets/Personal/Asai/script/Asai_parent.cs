using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asai_parent : MonoBehaviour
{
    public Transform parent;
    public Transform parent2;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("candle"))
        {
            // 触れたobjの親を移動床にする
            other.transform.parent.SetParent(parent);
            //other.transform.localScale = new Vector3(1.0f, 1.3f, 6.0f);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("candle"))
        {
            // 触れたobjの親をなくす
            other.transform.parent.SetParent(parent2);
            //other.transform.parent.SetParent(null);
            //other.transform.localScale = new Vector3(1.0f, 1.3f, 6.0f);
        }
    }
}
