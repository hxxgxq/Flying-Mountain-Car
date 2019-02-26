using UnityEngine;

public class Reborn : MonoBehaviour {

    private Transform[] Waypoints;
    private Transform rebornpoint;
    private Vector3 Lastpos = Vector3.zero;

    void Awake()
    {
        Waypoints = GetComponentsInChildren<Transform>();
    }

    public void reborn(GameObject car)
    {
        if ((car.transform.position - Lastpos).magnitude > 2)
        {
            float mindistance = float.MaxValue;
            for (int i = 1; i < Waypoints.Length; i++)
            {
                float dist = Vector3.Distance(car.transform.position, Waypoints[i].position);
                if (dist < mindistance && Mathf.Abs(Waypoints[i].position.y - car.transform.position.y)<5)
                {
                    mindistance = dist;
                    rebornpoint = Waypoints[i];
                }
                Debug.Log("hhhhhhhhhhhh   " + i);
            }
            Lastpos = rebornpoint.transform.position;
            car.transform.position = Lastpos;
            car.transform.rotation = rebornpoint.transform.rotation;
        }

    }
}