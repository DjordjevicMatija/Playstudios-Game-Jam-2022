using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDemo : MonoBehaviour
{
    private IEnumerator RetrunToMenu()
    {
        yield return new WaitForSeconds(31);
        SceneManager.LoadScene("MainMenu");
    }
    private void Start()
    {
        StartCoroutine(RetrunToMenu());
    }

}
