using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour
{
    public enum Direction { North, South, East, West }

    //public bool useUnscaledTime;

    public Vector2 InputVel;
    public Vector2 LastInputVel;
    public Rigidbody2D body;
    public float speed = 3;

    public Vector2 FacingDirection;
    public Direction faceDir = Direction.South;

    public bool isMoving;
    public bool isReallyMoving;

    Vector2 lastPosition;


    public bool freeze;

    public bool move = true;


    public bool lockDirection;

    public bool MoveType;

    /// <summary>
    /// If required pixel perfect movement
    /// </summary>
    public PixelPerfectCamera pixCam;


    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0;
        LastInputVel = Vector2.down;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputVel.sqrMagnitude > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        Vector2 dirVel = new Vector2(Mathf.Round(InputVel.x), Mathf.Round(InputVel.y));


        if (!lockDirection)
        {
            if (Mathf.Abs(dirVel.x) > 0 && dirVel.y == 0)
            {

                FacingDirection = new Vector2(Mathf.Sign(LastInputVel.x), 0);

            }
            else if (Mathf.Abs(dirVel.y) > 0 && dirVel.x == 0)
            {

                FacingDirection = new Vector2(0, Mathf.Sign(LastInputVel.y));

            }
            else if (Mathf.Abs(dirVel.x) > 0 && Mathf.Abs(dirVel.y) > 0)
            {
                if (faceDir == Direction.North && dirVel.y < 0)
                {
                    FacingDirection = new Vector2(0, Mathf.Sign(LastInputVel.y));

                }
                if (faceDir == Direction.South && dirVel.y > 0)
                {
                    FacingDirection = new Vector2(0, Mathf.Sign(LastInputVel.y));

                }

                if (faceDir == Direction.East && dirVel.x < 0)
                {
                    FacingDirection = new Vector2(0, Mathf.Sign(LastInputVel.y));


                }
                if (faceDir == Direction.West && dirVel.x > 0)
                {
                    FacingDirection = new Vector2(0, Mathf.Sign(LastInputVel.y));

                }



            }

            if (FacingDirection.normalized == Vector2.right)
            {
                faceDir = Direction.East;
                //sword.transform.eulerAngles = new Vector3(0, 0, -90);
            }
            else if (FacingDirection.normalized == Vector2.left)
            {
                faceDir = Direction.West;
                //sword.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (FacingDirection.normalized == Vector2.down)
            {
                faceDir = Direction.South;
                //sword.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (FacingDirection.normalized == Vector2.up)
            {
                faceDir = Direction.North;
                //sword.transform.eulerAngles = new Vector3(0, 0, 180);
            }
        }

    }



    private void FixedUpdate()
    {
        //if(!useUnscaledTime)
        //{
        Move();
        //}

    }


    private void LateUpdate()
    {


        if (MoveType)
        {
            if (body.velocity.sqrMagnitude > 0)
            {
                isReallyMoving = true;
            }
            else
            {
                isReallyMoving = false;
            }
        }
        else
        {
            Vector2 d = (lastPosition - (Vector2)transform.position);

            if (lastPosition == (Vector2)transform.position)
            {
                //Debug.Log("SAME PLACE");
                isReallyMoving = false;
                lastPosition = transform.position;
            }
            else
            {
                //Debug.Log("DIFF PLACE");
                isReallyMoving = true;
                lastPosition = transform.position;
            }
        }

    }



    public void Move()
    {
        float delta = Time.deltaTime;


        if (move)
        {
            if (MoveType)
            {
                body.velocity = InputVel * speed;
            }
            else
            {



                Vector2 nPos = (Vector2)transform.position + InputVel * speed * delta;



                //If Pixel Perfect Movement is required
                if (pixCam)
                {
                    nPos = pixCam.RoundToPixel(nPos);
                }



                body.MovePosition(nPos);


            }

        }
    }
    public void SetInputVel(Vector2 input)
    {
        if (!freeze)
        {
            InputVel = input;

            if (InputVel.sqrMagnitude > 0)
            {
                LastInputVel = input;
            }
        }
        else
        {
            InputVel = Vector2.zero;
        }

    }


    public void HaltInputAndMovement()
    {
        InputVel = Vector2.zero;
        move = false;
        body.velocity = Vector2.zero;
    }

    public void ResumeMovement()
    {
        move = true;
    }
}
