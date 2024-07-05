using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycreate : MonoBehaviour
{
    public Fire_test fire_test;
    public RockedDoor RockedDoor;
    public float CreateTime;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Fire" && Input.GetKey(KeyCode.Space) && RockedDoor.Rock == false && fire_test.GetIsOnCandle())
        {
            Createkey();
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
