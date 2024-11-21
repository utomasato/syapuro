using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asaitest : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        Debug.Log("bb");
        if (other.gameObject.name == "candlecon")
        {
            Debug.Log("aaa");
        }
    }
}
