using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = transform.GetComponent<Animator>();
    }

    public void GoToScene(string scene)
    {
        StartCoroutine(GoScene(scene));
    }

    public void NextLevel()
    {
        StartCoroutine(GoNextLevel());
    }

    IEnumerator GoNextLevel()
    {
        animator.SetTrigger("ChangeLevel");
        yield return new WaitForSeconds(1);
        GameManager.instance.NextLevel();
    }
    
    IEnumerator GoScene(string scene)
    {
        animator.SetTrigger("ChangeScene");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}