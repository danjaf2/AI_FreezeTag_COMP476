using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public float score;
    void Start()
    {
        score= 0;
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale= 1.0f;
        
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

   
}
