using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] Vector3 movingPosition;
    [SerializeField] [Range(0, 1)] float movingProgress;
    [SerializeField] float speed;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movingProgress = Mathf.PingPong(Time.time * speed, 1);
        Vector3 offset = movingPosition * movingProgress;
        transform.position = startPosition + offset;
    }
}
