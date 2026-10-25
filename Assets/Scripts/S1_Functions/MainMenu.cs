using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Unlocks the cursor so they can click buttons, just in case
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Loads your gameplay scene
        SceneManager.LoadScene("Assets/Scenes/Scene01_JY.unity");
    }
}
