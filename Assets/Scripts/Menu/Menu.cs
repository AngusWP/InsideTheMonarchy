using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public static int soundSetting = 75;
    public static int musicSetting = 75; // percents, this is the only way I could think to transfer it across scenes.

    public void playGame() {
        SceneManager.LoadScene(1);
    }

    public void exitGame() {
        Application.Quit();
    }
}
