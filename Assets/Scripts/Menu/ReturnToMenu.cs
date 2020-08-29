using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour {

    private GameManager gameManager;
    private ActionMenu actionMenu;

    public GameObject returnToMenu;

    private bool open = false;

    void Start() {
        gameManager = GetComponent<GameManager>();
        actionMenu = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionMenu>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            
            if (open) {
                return;
            }

            open = true;

            List<GameObject> obj = new List<GameObject>();
            
            foreach (GameObject o in actionMenu.openObjects) {
                obj.Add(o);
            }

            foreach (GameObject ob in obj) {
                ob.SetActive(false);
                actionMenu.openObjects.Remove(ob);
            }

            actionMenu.setTask(false);
            actionMenu.removeActionMenu();
            returnToMenu.SetActive(true);
        }
    }

    public void yes() {
        //saveGame();
        SceneManager.LoadScene(0);
    }

    public void no() {
        returnToMenu.SetActive(false);
        open = false;
    }
}
