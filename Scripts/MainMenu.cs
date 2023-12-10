using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    private List<AudioSource> audioSources;
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        audioSources = GameObject.Find("Music Manager").GetComponents<AudioSource>().ToList();
        audioSources[1].clip = clip;
        audioSources[1].loop = true;
        audioSources[1].Play();
    }

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
