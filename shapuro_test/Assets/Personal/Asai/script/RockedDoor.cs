using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockedDoor : MonoBehaviour
{
    public float Asai_testx = 1f;
    public bool Rock = false;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && Rock == true)
        {
            transform.position += new Vector3(0, Asai_testx * Time.deltaTime, 0);
        }
    }
}