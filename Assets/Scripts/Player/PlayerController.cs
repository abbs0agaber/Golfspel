using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pair<T, U> {
    public Pair() {
    }

    public Pair(T first, U second) {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};

public class PlayerController : MonoBehaviour{

    [SerializeField] private float playerSpeed;
    [SerializeField] private float ShootRotateSpeed;
    [SerializeField] private float camAutoRotateSpeed;
    [SerializeField] private GameObject ShootingCam;
    [SerializeField] private Transform camOffsetTransform;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float cameraDelay;
    private Queue<Pair<Vector3, float>> positionHistory = new();
    private bool lastStill = false;
    public bool isShooting = true;

    private void Update() {
        // Debug.Log(rb.velocity.magnitude);
        isShooting = (0.8f > rb.velocity.magnitude && lastStill);
        lastStill = (0.8f > rb.velocity.magnitude);
        CameraUpdate();

        if(isShooting) ShootUpdate();
        // rb.velocity = new Vector3(Input.GetAxis("Horizontal")*playerSpeed, rb.velocity.y, Input.GetAxis("Vertical")*playerSpeed);
    }


    private void ShootUpdate() {
        rb.velocity = Vector3.zero;
        ShootingCam.transform.RotateAround(transform.position, new Vector3(0, 1, 0), Input.GetAxis("Horizontal")* ShootRotateSpeed * Time.deltaTime);
    }
    private void CameraUpdate() {
        //positionHistory.Enqueue(new Pair<Vector3, float>(transform.position, Time.time));
        //while(positionHistory.Count > 1 && (positionHistory.Peek().Second + cameraDelay < Time.time)) {
        //    positionHistory.Dequeue();
        //}

        Vector3 look = transform.position - ShootingCam.transform.position;
        float angle = Mathf.Atan2(look.z, look.x) * Mathf.Rad2Deg;
        float angle2 = Mathf.Atan2(rb.velocity.z, rb.velocity.x) * Mathf.Rad2Deg;
        //Debug.Log(angle-angle2);

        // Debug.Log(Time.time-positionHistory.Peek().Second);
        camOffsetTransform.position = transform.position;
        if(isShooting) camOffsetTransform.position = transform.position;
        else {
            // camOffsetTransform.position = Vector3.Lerp(camOffsetTransform.position, positionHistory.Peek().First, 1f);

            ShootingCam.transform.RotateAround(transform.position, new Vector3(0, 1, 0), Mathf.Lerp(0f, angle - angle2, camAutoRotateSpeed * Time.deltaTime) * Mathf.Pow(rb.velocity.magnitude, 0.8f));
        }
    }

}
