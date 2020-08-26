using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

    private Animator anim;
    private GameManager gameManager;
    void Start() {
        anim = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update() {
        if (anim.GetBool("fadeOut") || anim.GetBool("fadeIn")) {
            gameManager.transition = true;
        } else {
            gameManager.transition = false;
        }
    }

    public void fadeIn() {
        anim.SetBool("fadeIn", true);
        anim.SetBool("fadeOut", false);
    }

    public void fadeOut() {
        anim.SetBool("fadeOut", true);
        anim.SetBool("fadeIn", false);
    }
}
