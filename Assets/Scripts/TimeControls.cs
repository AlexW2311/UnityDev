using UnityEngine;

public class TimeControls : MonoBehaviour
{
    public bool timeStop = false;
    int stopped = 0;
    int resumed = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !timeStop)
        {
            Time.timeScale = stopped;
            timeStop = true;
        }
        else
        {
            Time.timeScale = resumed;
            timeStop = false;
        }
    }
}
