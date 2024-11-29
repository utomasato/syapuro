using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloon : MonoBehaviour
{
    public GameObject fire;
    public GameObject parent;
    public GameObject floor;
    private bool isRise = false;
    private float t;
    private bool p = false;
    private Vector3 rise;
    // Start is called before the first frame update

    void Update()
    {
        t += Time.deltaTime;
        if (t >= 0.1)
        {
            t = 0F;
            if (isRise)
            {
                rise = parent.gameObject.transform.localPosition;
                parent.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 1000);
            }
            else
            {
                parent.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * -200);
            }
        }
        floor.GetComponent<BoxCollider>().enabled = p;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == fire)
        {
            p = true;
            if (!fire.GetComponent<Fire_sub>().getIsNormal())
            {
                isRise = true;
            }
            else
            {
                isRise = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == fire)
        {
            isRise = false;
            p = false;
        }
    }
}
