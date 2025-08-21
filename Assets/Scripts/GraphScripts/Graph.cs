using UnityEngine;

public class Graph : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    Transform pointPrefab;

    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {

            Transform point = Instantiate(pointPrefab);
            point.localPosition = Vector3.right * i/5f;
            point.localScale = Vector3.one/5f;

        }



    }
}
