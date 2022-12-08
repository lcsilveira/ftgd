using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private string loadScene;

    private void Start()
    {
        SceneManager.sceneLoaded += (scene, mode) => { NewPlayer.Instance.SetSpawnPosition(); };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == NewPlayer.Instance.gameObject)
            SceneManager.LoadScene(loadScene);
    }
}
