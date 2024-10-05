using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeCountDisplay : MonoBehaviour
{

    TMP_Text displayCount;

    private void Awake()
    {
        displayCount = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        GameManager.Instance.slimeCaptured += UpdateCounter;

        displayCount.text = GameManager.Instance.SlimesCaptured + "/" + GameManager.Instance.NumSlimes;

    }

    private void OnDisable()
    {
        GameManager.Instance.slimeCaptured -= UpdateCounter;
    }


    private void UpdateCounter(int numSlimes)
    {
        displayCount.text = numSlimes + "/" + GameManager.Instance.NumSlimes;
    }
}
