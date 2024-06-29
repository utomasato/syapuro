using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StageSelect : MonoBehaviour
{
    [SerializeField] private float interval;
    int selectNumber = 0;
    bool IsMoving = false;
    float t = 0.0f;
    int p0;
    [SerializeField] Vector3 startPos;
    [SerializeField] private List<string> Stagelist;

    void Start()
    {
        if (SceneSelectionState.selectedIndex == -1)
        {
            p0 = 0;
            transform.position = startPos;
        }
        else
        {
            p0 = SceneSelectionState.selectedIndex;
            Vector3 pos = startPos;
            pos.x = startPos.x + p0 * interval;
            transform.position = pos;
        }
        selectNumber = p0;
        IsMoving = false;
        //Debug.Log(SceneSelectionState.selectedIndex);
    }

    void Update()
    {
        if (!IsMoving)
        {
            if (Input.GetKeyDown(KeyCode.D) && selectNumber + 1 < Stagelist.Count)
            {
                selectNumber += 1;
                IsMoving = true;
                t = 0.0f;
            }
            if (Input.GetKeyDown(KeyCode.A) && 0 < selectNumber)
            {
                selectNumber -= 1;
                IsMoving = true;
                t = 0.0f;
            }

            if (Input.GetKeyDown(KeyCode.Return) && selectNumber < Stagelist.Count && Stagelist[selectNumber] != null)
            {
                SceneSelectionState.selectedIndex = selectNumber;
                SceneManager.LoadScene(Stagelist[selectNumber]);
            }
        }
        else
        {
            t += Time.deltaTime;
            if (t >= 1.0f)
            {
                t = 1.0f;
                IsMoving = false;
                p0 = selectNumber;
            }
            Vector3 pos = transform.position;
            pos.x = startPos.x + Mathf.Lerp(p0 * interval, selectNumber * interval, t);
            transform.position = pos;
        }
    }
}
