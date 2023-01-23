using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Animator uiAnimator;
    public Image healthBar;
    public TMP_Text coinsText;
    public Image inventoryItemImage;

    // Singleton instantiation.
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        // If the original game manager is here, the scene game manager is self-destroyed.
        // (Otherwise, the Start function will be called and this will become the Original Game Manager).
        if (GameObject.Find("OriginalGameManager"))
            Destroy(this.gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        this.gameObject.name = "OriginalGameManager";
    }
}
