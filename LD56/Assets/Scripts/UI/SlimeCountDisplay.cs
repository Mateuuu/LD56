using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlimeCountDisplay : MonoBehaviour
{

    [SerializeField] GameObject winText;

    TMP_Text displayCount;

    private void Awake()
    {
        if(winText != null)
        {
            winText.SetActive(false);
        }
        displayCount = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        GameManager.Instance.slimeCaptured += UpdateCounter;

        displayCount.text = GameManager.Instance.SlimesCaptured.ToString() + "/" + GameManager.Instance.NumSlimes.ToString();

    }

    private void OnDisable()
    {
        GameManager.Instance.slimeCaptured -= UpdateCounter;
    }


    private void UpdateCounter(int numSlimes)
    {
        displayCount.text = numSlimes + "/" + GameManager.Instance.NumSlimes;

        if (GameManager.Instance.GameWon)
        {
            if(winText != null)
            {
                winText.SetActive(true);
            }
        }
    }
}
