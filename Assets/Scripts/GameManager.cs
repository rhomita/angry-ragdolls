using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    #endregion

    [SerializeField] private Camera cam;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameUI ui;
    [SerializeField] private List<LevelManager> levels;

    private int currentLevel = 0;

    public GameUI UI
    {
        get
        {
            return ui;
        }
    }

    public Camera Cam
    {
        get
        {
            return cam;
        }
    }

    public Transform CameraTransform
    {
        get
        {
            return cam.transform;
        }
    } 
    
    void Start()
    {
        foreach (LevelManager level in levels)
        {
            level.gameObject.SetActive(false);
        }
        ActivateLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneLoader.GoToScene("Menu");
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            sceneLoader.GoToScene("Play");
        }
    }
    
    public void NextLevel()
    {
        levels[currentLevel].gameObject.SetActive(false);
        currentLevel++;
        if (currentLevel >= levels.Count)
        {
            sceneLoader.GoToScene("Menu");
            return;
        }
        ActivateLevel();
    }

    private void ActivateLevel()
    {
        levels[currentLevel].gameObject.SetActive(true);
        levels[currentLevel].onFinished += sceneLoader.NextLevel;
    }
        
    protected void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}