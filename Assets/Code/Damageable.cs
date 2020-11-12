using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public int life = 1;
    public int maxLife;

    public UnityEvent OnDeath;
    public UnityEvent OnHit;
    public UnityEvent OnStunEnd;

    public UnityEvent OnHealed;

    public bool isHitStun;
    public bool isImmune;

    public bool immortal;


    public CollisionInfo info;


    public bool usePhysics = false;

    public bool isDead;

    public float hitStunSeconds = 0.5f;
    public float immunitySeconds = 0.5f;

    public SpriteRenderer sprite;


    public bool flashSprite;
    private void Awake()
    {
        maxLife = life;
    }
    public void DealDamage(int dmg)
    {
        usePhysics = false;

        info = null;
        if(isDead)
        {
            return;
        }
        if (!isHitStun)
        {


            if (!immortal)
                life -= dmg;

            //OnHit.Invoke();
            StartCoroutine(HitStun());
            if (life <= 0)
            {
                isDead=true;
                OnDeath.Invoke();
            }
        }
        else
        {
            return;
        }



    }


    public void DealDamage(CollisionInfo i)
    {
        if(isDead)
        {
            return;
        }
        usePhysics = true;
        if (!isImmune)
        {
            info = i;


            if (!immortal)
                life -= info.totalDmg;

            if (life <= 0)
            {
                isDead = true;
                OnDeath.Invoke();
            }
            else
            {
                StartCoroutine(HitStun());
                StartCoroutine(HitImmune());

            }

            //StartCoroutine(HitStun());
            //StartCoroutine(HitImmune());
            //if (life <= 0)
            //{

            //    OnDeath.Invoke();
            //}
        }
        else
        {
            return;
        }





    }

    public void DealDamage(int totalDmg,float force,Vector2 position)
    {
        if(isDead)
        {
            return;
        }
        CollisionInfo info = new CollisionInfo();

        info.totalDmg = totalDmg;
        info.knockbackForce = force;
        info.position = position;

        usePhysics = true;
        if (!isImmune)
        {
            this.info = info;
            //info = i;


            if (!immortal)
                life -= info.totalDmg;

            if (life <= 0)
            {
                isDead = true;
                OnDeath.Invoke();
            }
            else
            {
                StartCoroutine(HitStun());
                StartCoroutine(HitImmune());

            }

            //StartCoroutine(HitStun());
            //StartCoroutine(HitImmune());
            //if (life <= 0)
            //{

            //    OnDeath.Invoke();
            //}
        }
        else
        {
            return;
        }





    }



    public void DealDamageDiscreet(int i)
    {
        if (!immortal)
            life -= i;
    }
    IEnumerator HitStun()
    {
        isHitStun = true;
        OnHit.Invoke();

        yield return new WaitForSeconds(hitStunSeconds);
        isHitStun = false;
        OnStunEnd.Invoke();
    }


    IEnumerator HitImmune()
    {

        isImmune = true;
        if (flashSprite)
            FlashSprite(sprite);
        yield return new WaitForSeconds(immunitySeconds);
        isImmune = false;

    }


    public class CollisionInfo
    {
        public Vector2 position;
        public int totalDmg;
        public float knockbackForce;





    }

    public void HealAdditive(int i)
    {
        life = Mathf.Clamp(life + i, 0, maxLife);
        OnHealed.Invoke();

    }


    public void UpgradeHealth(int i)
    {
        maxLife += i;
        life = maxLife;
        OnHealed.Invoke();
    }


    public void FlashSprite(SpriteRenderer s)
    {
        StartCoroutine(DoFlashSprite(s));
    }


    IEnumerator DoFlashSprite(SpriteRenderer r)
    {
        float t = 0;
        while (t <= immunitySeconds)
        {
            r.enabled = !r.enabled;
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        r.enabled = true;
        yield return null;
    }


}

