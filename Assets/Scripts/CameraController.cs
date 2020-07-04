using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed = 27.5f;
    private float zoomSpeed = 50.5f;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position += new Vector3(speed * Input.GetAxis("Horizontal") * Time.deltaTime, speed * Input.GetAxis("Vertical") * Time.deltaTime, 0);
        mainCamera.orthographicSize += -1 * zoomSpeed * Input.mouseScrollDelta.y * Time.deltaTime;
    }
}
