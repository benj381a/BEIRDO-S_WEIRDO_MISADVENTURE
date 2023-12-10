using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] int levelID = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SmallBeardoManager.Instance.animator.SetTrigger("Restart");
        SceneManager.LoadSceneAsync(levelID);
    }
}
