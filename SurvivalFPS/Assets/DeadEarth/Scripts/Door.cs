using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum DoorState { Open, Animating, Closed };

public class Door : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float openDistance = 4.0f;
    [SerializeField] float timeToOpen = 2.0f;
    [SerializeField] AnimationCurve animationCurve;

    // state parameters
    DoorState doorState = DoorState.Closed; 

    // Update is called once per frame
    void Update()
    {
        CycleDoor();
    }

    private void CycleDoor()
    {
        if (Input.GetKeyDown("space"))
        {
            switch (doorState)
            {
                case DoorState.Open:
                {
                    // close the door
                    StartCoroutine(MoveDoor(-openDistance));
                    break;
                }
                case DoorState.Closed:
                {
                    // open the door
                    StartCoroutine(MoveDoor(openDistance));
                    break;
                }
            }

        }
    }

    IEnumerator MoveDoor(float distance)
    {
        doorState = DoorState.Animating;

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + Vector3.left * distance;

        float time = 0;
        while (time <= timeToOpen)
        {
            transform.position = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(time / timeToOpen));
            time += Time.deltaTime;
            yield return null;
        }

        if (distance > 0)
        {
            doorState = DoorState.Open;
        }
        else
        {
            doorState = DoorState.Closed;
        }
    }
}
