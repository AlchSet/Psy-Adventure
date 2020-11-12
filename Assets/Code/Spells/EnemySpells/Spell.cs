using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public GameObject projectile;
    SpriteRenderer pentacle;

    // Start is called before the first frame update
    void Start()
    {
        pentacle = GetComponent<SpriteRenderer>();
        pentacle.enabled = false;
    }


    public void Fire(Vector2 dir)
    {
        GameObject g = Instantiate(projectile, transform.position, Quaternion.identity);
        g.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 10, ForceMode2D.Impulse);
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        pentacle.enabled = true;

        pentacle.color = Color.red;
        float maxTime = 0.85f;
        float ctime = 0;
        

        while(true)
        {
            ctime += Time.deltaTime;

            pentacle.color = Color.Lerp(Color.red, Color.clear, ctime / maxTime);


            if(ctime/maxTime>=1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        
        pentacle.enabled = false;
    }


}
