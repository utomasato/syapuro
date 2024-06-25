using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycreate : MonoBehaviour
{
    public RockedDoor RockedDoor;
    public float CreateTime;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Createkey();
            }
        }
    }
    void Createkey()
    {
        CreateTime += Time.deltaTime;
        if (CreateTime > 2f)
        {
            RockedDoor.Rock = true;
        }
    }
}
