using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycreate : MonoBehaviour
{
    public RockedDoor RockedDoor;
    public float CreateTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
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