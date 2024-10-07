using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hehehehe : MonoBehaviour
{
    [SerializeField] GameObject heheheheButton;

    private void Start()
    {
        if (GameManager.Instance.GameWon)
        {

            heheheheButton.SetActive(true);

        }
        else
        {
            heheheheButton.SetActive(false);
        }
    }

}
