using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScroll : MonoBehaviour
{

    Vector2 opos;

    public float test = 1;
    public float test2 = 1;

    public float ycheck = 1;

    public bool useUnscaledTime;

    // Start is called before the first frame update
    void Start()
    {
        opos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 npos = Vector2.zero;

        //npos = (Vector2)transform.localPosition + (Vector2.right * 1) * Time.deltaTime;
        //npos = (Vector2)transform.localPosition + (Vector2.up * 1) * Time.deltaTime;

        float delta = Time.deltaTime;
        if (useUnscaledTime)
            delta = Time.unscaledDeltaTime;

        npos = (Vector2)transform.localPosition + (Vector2.right * test2 * delta) + (Vector2.up * test * delta);

        if (transform.localPosition.x <= -1)
        {
            npos.x = opos.x;
        }

        if (transform.localPosition.x >= 1)
        {
            npos.x = opos.x;
        }

        if (transform.localPosition.y >= ycheck)
        {
            npos.y = opos.y;
        }

        transform.localPosition = npos;
    }
}
