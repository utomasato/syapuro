using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment : MonoBehaviour
{

    [SerializeField]
    private FIRE1 FireScript;
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
        if (other.gameObject.CompareTag("RecoveryItem"))
        {

            FireScript.JudgeIsChange = true;
            FireScript.SetObject = other.gameObject;
        }
    }
}
