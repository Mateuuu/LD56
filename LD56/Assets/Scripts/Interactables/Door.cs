using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] Transform doorHinge;
    [SerializeField] float doorOpenTime = 1f;
    [SerializeField] float doorCloseTime = 1f;


    Coroutine doorOpenRoutine;
    Coroutine doorCloseRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            if(doorOpenRoutine != null)
            {
                StopCoroutine(doorOpenRoutine);
            }
            if (doorCloseRoutine != null)
            {
                StopCoroutine(doorCloseRoutine);
            }
            doorOpenRoutine = StartCoroutine(OpenDoor());
        }

    }



    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("PlayerBody"))
        {
            if (doorOpenRoutine != null)
            {
                StopCoroutine(doorOpenRoutine);
            }
            if (doorCloseRoutine != null)
            {
                StopCoroutine(doorCloseRoutine);
            }
            doorCloseRoutine = StartCoroutine(CloseDoor());
        }
    }




    IEnumerator OpenDoor()
    {
        float time = 0;

        while(time < doorOpenTime)
        {

            float yRot = Mathf.Lerp(doorHinge.localEulerAngles.y, 110f, time / doorOpenTime);
            doorHinge.localEulerAngles = new Vector3(0, yRot, 0);

            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CloseDoor()
    {
        float time = 0;

        while (time < doorCloseTime)
        {

            float yRot = Mathf.Lerp(doorHinge.localEulerAngles.y, 0f, time / doorCloseTime);
            doorHinge.localEulerAngles = new Vector3(0, yRot, 0);

            time += Time.deltaTime;
            yield return null;
        }
    }

}
