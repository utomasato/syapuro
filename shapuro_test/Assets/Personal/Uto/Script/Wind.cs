using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] GameObject windArea;
    [SerializeField] ParticleSystem windParticle;
    [SerializeField] float range;
    [SerializeField] float particleMargin;
    ParticleSystem.MainModule main;
    Vector3 scale;

    void Start()
    {
        main = windParticle.main;
        scale = windArea.transform.localScale;
        scale.y = range;
        windArea.transform.localScale = scale;
        main.startLifetime = (range + particleMargin) / 100;
    }

    void Update()
    {
        //ChangeRange(range);
    }

    public void ChangeRange(float r)
    {
        range = r;
        scale.y = range;
        windArea.transform.localScale = scale;
        main.startLifetime = (range + particleMargin) / 100;
    }

}
