using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private float savespeed;//初期値のスピードを保存
    // Start is called before the first frame update

    void Start()
    {
        savespeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-speed * Time.deltaTime, 0, 0));

        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(speed * Time.deltaTime, 0, 0));

        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("RecoveryItem"))
        {
            Changeobj(transform.position);
            transform.position = other.transform.position;
            Changecolor(this.gameObject, other.gameObject);
            ChangeSize(this.gameObject, other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    void Changeobj(Vector3 initialpos)//プレイヤー転生
    {
        GameObject copy = Instantiate(this.gameObject, initialpos, Quaternion.identity);
        copy.GetComponent<PlayerControll>().enabled = false;

    }

    void Changecolor(GameObject Target/*変更するオブジェクト*/, GameObject newColor /*変更後の色*/)
    {
        Color objColor = newColor.gameObject.GetComponent<Renderer>().material.color;
        Target.GetComponent<Renderer>().material.color = objColor;
    }

    void ChangeSize(GameObject target, GameObject NewSize)
    {
        Vector3 objSize = NewSize.transform.localScale;
        target.transform.localScale = objSize;
    }


}
