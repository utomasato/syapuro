using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private float interval;
    int selectNumber = 0;
    bool IsMoving = false;
    float t = 0.0f;
    int p0, p1;
    Vector3 startPos;

    void Start()
    {
        p0 = 0;
        p1 = p0;
        IsMoving = false;
        startPos = transform.position;
    }

    void Update()
    {
        if (!IsMoving)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                p1 += 1;
                IsMoving = true;
                t = 0.0f;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                p1 -= 1;
                IsMoving = true;
                t = 0.0f;
            }
        }
        else
        {
            t += Time.deltaTime;
            if (t >= 1.0f)
            {
                t = 1.0f;
                IsMoving = false;
                p0 = p1;
            }
            Vector3 pos = transform.position;
            pos.x = startPos.x + Mathf.Lerp(p0 * interval, p1 * interval, t);
            transform.position = pos;
        }
    }
}
