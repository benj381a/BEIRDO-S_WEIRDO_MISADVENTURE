using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SmallBeardoManager.Instance.animator.SetTrigger("Restart");
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
