using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TransitionToScene(string sceneToTransitionTo)
    {
        animator.SetBool("Transition", true);
        StartCoroutine(Transition(sceneToTransitionTo));
    }
    IEnumerator Transition(string sceneToTransitionTo)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneToTransitionTo);
    }
}
