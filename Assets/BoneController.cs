using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoneController : MonoBehaviour
{
    private Rigidbody2D hand;
    public GameObject body;
    public Vector2 direction;
    private Vector2 mousePosition;
    public GameObject handStart;
    public float speed = 10f;
    public float distanceAllow;
    //public Vector2 screenCenter = new Vector2(1024 / 2, 768 / 2);
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        hand = GetComponent<Rigidbody2D>();
        //var from = cam.ScreenToWorldPoint(screenCenter);
        //Debug.DrawLine(from, from - Vector3.up, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        direction = (Vector2)Input.mousePosition - hand.position;
    }

    private void FixedUpdate()
    {
        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        if ((mousePosition - (Vector2)body.transform.position).magnitude > 5)
        {
            hand.position = mousePosition.normalized * 5;
        }
        else hand.position = mousePosition;
        //distanceAllow = ((Vector2)handStart.transform.position - ((Vector2)Input.mousePosition - screenCenter)).magnitude;
        //if (distanceAllow > 5)
        //{
        //    hand.position = ((Vector2)Input.mousePosition - screenCenter).normalized * distanceAllow;
        //}
        //else hand.position = (Vector2)Input.mousePosition - screenCenter;
        Debug.Log(mousePosition);
        //var from = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
        //Debug.DrawLine(from, from - Vector3.up, Color.red);
    }
}
