using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool jump = true;
    public int HP = 0;
    public float Speed = 1;
    public float x = 0;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (jump == true)
        {
            x = Input.GetAxis("Horizontal");
            transform.position += this.transform.right * x * Speed;
        }
        else
        {
            transform.position += this.transform.right * x * Speed;
        }
        if (Input.GetKey(KeyCode.Space) && jump == true)
        {
            rigidbody.velocity = Vector3.up * 10;
            jump = false;
        }
    }
    void OnCollisionEnter(Collision other)
    {
        jump = true;
    }
}