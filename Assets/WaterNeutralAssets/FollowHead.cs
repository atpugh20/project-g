using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FollowHead : MonoBehaviour
{
    //public GameObject target;
    public float distance;
    public int length;
    public GameObject particle;
    private GameObject[] particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            float x = 0 + i;
            float y = 0 + i;
            particles[i] = Instantiate(particle, new Vector2(x, y), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < length; i++)
        {
            distance = Vector2.Distance(particles[i].transform.position, particles[i - 1].transform.position);

            if (distance >= .2)
            {
                float speed = 37f * distance;
                particles[i].transform.position = Vector2.MoveTowards(particles[i].transform.position, particles[i - 1].transform.position, speed * Time.deltaTime);
                //transform.position = Vector2.Lerp(transform.position, target.transform.position, Time.deltaTime);
            }
        }
    }
}
