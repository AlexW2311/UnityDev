using UnityEngine;

public class FriendCube : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string cubeName = "Friend Cube :) ";
    public int health = 100;
    public float speed = 5;
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
