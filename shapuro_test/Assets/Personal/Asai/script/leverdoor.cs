using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leverdoor : MonoBehaviour
{
    public lever lever;
    public enum Test
    {
        Mode_TransF,
        Mode_TransS
    }
    public Test mode;
    public float FirstScale;
    public float FirstPosition;
    void Start()
    {
        Vector3 s = transform.localScale;
        FirstScale = s.y;
        Vector3 p = transform.position;
        FirstPosition = p.y;
        if (mode == Test.Mode_TransF && lever.mode == lever.Test.Mode_Second)
        {
            s.y = 0;
            p.y = 0.5f * FirstScale + FirstPosition;
            transform.localScale = s;
            transform.position = p;
            //Debug.Log("aho1");
        }
        if (mode == Test.Mode_TransS && lever.mode == lever.Test.Mode_First)
        {
            s.y = 0;
            p.y = 0.5f * FirstScale + FirstPosition;
            transform.localScale = s;
            transform.position = p;
            //Debug.Log("aho2");
        }
    }
    void FixedUpdate()
    {
        Vector3 s = transform.localScale;
        Vector3 p = transform.position;
        if (mode == Test.Mode_TransF && lever.mode == lever.Test.Mode_TransF)
        {
            s.y = 0;
            p.y = 0.5f * FirstScale + FirstPosition;
            transform.localScale = s;
            transform.position = p;
            //Debug.Log("aa");
        }
        if (mode == Test.Mode_TransS && lever.mode == lever.Test.Mode_TransS)
        {
            s.y = 0;
            p.y = 0.5f * FirstScale + FirstPosition;
            transform.localScale = s;
            transform.position = p;
            //Debug.Log("aa");
        }
        if (mode == Test.Mode_TransF && lever.mode == lever.Test.Mode_TransS)
        {
            s.y = FirstScale;
            p.y = FirstPosition;
            transform.localScale = s;
            transform.position = p;
        }
        if (mode == Test.Mode_TransS && lever.mode == lever.Test.Mode_TransF)
        {
            s.y = FirstScale;
            p.y = FirstPosition;
            transform.localScale = s;
            transform.position = p;
        }
    }
}
