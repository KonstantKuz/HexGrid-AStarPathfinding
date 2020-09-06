using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        float horizontalOffset = Input.GetAxis("Horizontal") * speed;
        float verticalOffset = Input.GetAxis("Vertical") * speed;
        transform.Translate(horizontalOffset, 0 ,verticalOffset);
    }
}
