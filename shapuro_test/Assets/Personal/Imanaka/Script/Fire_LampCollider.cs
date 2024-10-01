using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_LampCollider : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        Fire fire = GetComponentInParent<Fire>();
        if (fire.GetIsOnCandle())
        {
            if (other.gameObject.CompareTag("lamp"))
            {
                other.gameObject.GetComponent<LampON>().Ignition();
            }
        }

    }
}
