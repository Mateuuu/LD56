using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSlimeCount : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.ResetSlimeCount();
    }

}
