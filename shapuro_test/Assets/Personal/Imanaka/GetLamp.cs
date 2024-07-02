using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLamp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject FireImage;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("lamp"))
        {
            GameObject LampObject = FireImage;
            LampObject.transform.localScale = new Vector3(1, 1, 1);
            Instantiate(LampObject, other.transform.position, Quaternion.identity);
            other.gameObject.tag = "Untagged";
        }
    }
}
