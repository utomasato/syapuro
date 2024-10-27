using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balloon : MonoBehaviour
{
    public GameObject fire;
    public GameObject parent;
    private bool isRise = false;
    private float t;
    private Vector3 rise;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t >= 0.1)
        {
            t = 0F;
            if (isRise)
            {
                rise = parent.gameObject.transform.localPosition;
                parent.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 2000);
            }
            else
            {
                parent.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * -400);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("何かにぶつかった");
        if (other.gameObject == fire)
        {
            Debug.Log("プレイヤー");
            if (!fire.GetComponent<Fire_sub>().getIsNormal())
            {
                Debug.Log("上昇");
                isRise = true;
            }
            else
            {
                isRise = false;
            }
        }
    }
}
