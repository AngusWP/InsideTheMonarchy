using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public float smoothing;

    private bool up = false, down = false;

    void Start() {

    }

    private void Update() {

        if (down) {
            Vector3 toPos = new Vector3(1.04f, -1f, -10);
            Vector3 newPosition = Vector3.Lerp(transform.position, toPos, smoothing * Time.deltaTime);
            transform.position = newPosition;
            StartCoroutine(updateDownBool());
        }

        if (up) {
            Vector3 toPos = new Vector3(1.04f, 0, -10);
            Vector3 newPos = Vector3.Lerp(transform.position, toPos, smoothing * Time.deltaTime);
            transform.position = newPos;
            StartCoroutine(updateUpBool());
        }
    }

    public IEnumerator updateUpBool() {
        yield return new WaitForSeconds(1f);
        up = false;
    }

    public IEnumerator updateDownBool() {
        yield return new WaitForSeconds(1f);
        down = false;
    }

    public void moveDown() {
        down = true;
    }

    public void moveUp() {
        up = true;
    }

}
