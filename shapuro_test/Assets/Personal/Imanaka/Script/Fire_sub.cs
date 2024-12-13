using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_sub : MonoBehaviour
{
    [SerializeField] private Fire parent;
    [SerializeField] private LayerMask layerMask;

    private void OnTriggerStay(Collider other)
    {
        if (!parent.GetIsOnCandle() && other.gameObject.CompareTag("FirePosition")) // 火の玉状態で蝋燭に当たったら
        {
            parent.Transfer(other.gameObject.GetComponent<Candlewick>());
        }
        if (!parent.GetIsOnCandle() && other.gameObject.CompareTag("candle")) // 火の玉状態で蝋燭に当たったら
        {
            parent.Transfer(other.gameObject.GetComponent<Candle>());
        }
        if (other.gameObject.CompareTag("water")) // 水に当たったら
        {
            SetDeathCause("honoo ga mizu ni haittesimatta"/*炎が水にはいってしまった*/);
            parent.Deletefire();
        }
        if (other.gameObject.CompareTag("wind")) // 風エリアに入ったら
        {
            if (parent.getIsNormal()) // ダッシュ中でない
            {

                Ray[] rays = {
                    new Ray(transform.position+new Vector3(transform.localScale.x*0.5f,transform.localScale.y*0.5f,0f), -other.gameObject.transform.up),
                    new Ray(transform.position+new Vector3(-transform.localScale.x*0.5f,transform.localScale.y*0.5f,0f), -other.gameObject.transform.up),
                    new Ray(transform.position+new Vector3(transform.localScale.x*0.5f,-transform.localScale.y*0.5f,0f), -other.gameObject.transform.up),
                    new Ray(transform.position+new Vector3(-transform.localScale.x*0.5f,-transform.localScale.y*0.5f,0f), -other.gameObject.transform.up)
                };
                RaycastHit hit;
                foreach (Ray ray in rays)
                {
                    if (Physics.Raycast(ray, out hit, 20f, layerMask)) // 風が来る方向にレイを飛ばす
                    {
                        if (hit.collider.gameObject.layer == 8) // 扇風機に例が当たったら
                        {
                            Debug.Log("Raycast:" + hit.collider.name);
                            SetDeathCause("炎が風に消されてしまった...\n火が弱かったようだ..."/*炎が風に消されてしまった*/);
                            parent.Deletefire();
                            break;
                        }
                    }
                }
            }
        }
    }
    public bool getIsNormal()
    {
        return parent.getIsNormal();
    }
    public void SetDeathCause(string Cause)
    {
        parent.SetDeathCause(Cause);
    }
}

