using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiBehavior : MonoBehaviour
{
    public Vector2 targetPosition = new Vector2(0f,0f);
    [SerializeField] float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2.Lerp(this.transform.position, targetPosition, speed * Time.deltaTime);
    }

}
