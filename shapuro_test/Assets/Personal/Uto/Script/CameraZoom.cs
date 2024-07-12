using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float startSize;
    [SerializeField] private float endSize;
    private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    //private Camera camera;
    [SerializeField] private float time;
    private float t;

    private bool IsZoomNow = false;

    void Start()
    {
        startSize = Camera.main.orthographicSize;
        StartZoom();
    }

    void Update()
    {
        if (IsZoomNow && t < time)
        {
            t += Time.deltaTime; // 経過時間を更新
            Camera.main.orthographicSize = Mathf.Lerp(startSize, endSize, t / time); // サイズを線形補間
            transform.position = Vector3.Lerp(startPosition, endPosition, t / time); // 位置を線形補完
            if (t >= time)
            {
                Camera.main.orthographicSize = endSize;
                transform.position = endPosition;
                IsZoomNow = false;
            }
        }
    }

    public void StartZoom()
    {
        t = 0.0f;
        startSize = Camera.main.orthographicSize;
        startPosition = transform.position;
        IsZoomNow = true;
    }
}
