using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private float introSeconds;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("IntroSequence");
    }

    // Update is called once per frame
    void Update()
    {

    }

   IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(introSeconds);
        SceneManager.LoadSceneAsync("Login");
    }


}
