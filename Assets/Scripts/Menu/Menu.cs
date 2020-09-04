using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public static bool newGame = true;

    public Button continueG;

    public void Start() {
        if (!DataHandler.hasLoadedFile()) {
            continueG.interactable = false;
        }
    }

    public void startNewGame() {
        newGame = true;
        SceneManager.LoadScene(1);
    }

    public void continueGame() {
        if (DataHandler.hasLoadedFile()) {
            newGame = false;
            SceneManager.LoadScene(1);
        } else {

        }
    }

    public void exitGame() {
        Application.Quit();
    }
}
