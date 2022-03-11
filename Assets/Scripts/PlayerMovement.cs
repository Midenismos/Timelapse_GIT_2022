using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody body = null;

    [SerializeField] private Transform groundCheck = null;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    public event Action<float> OnCharacterLanded;
    public bool isGrounded = true;
    private float fallenDistance = 0;
    private Vector3 bodyVelocity;

    private float lastInterval;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        bool oldIsGrounded = isGrounded;

        float delta = Time.unscaledDeltaTime;

        float timeNow = Time.realtimeSinceStartup;
        delta = timeNow - lastInterval;
        lastInterval = timeNow;

        //TOChange
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && !oldIsGrounded)
        {
            OnCharacterLanded?.Invoke(fallenDistance);
            fallenDistance = 0;
        } else if(!isGrounded && body.velocity.y < 0)
        {
            fallenDistance -= body.velocity.y * Time.unscaledDeltaTime;
        }
        if (isGrounded && body.velocity.y <-8)
        {
            FindObjectOfType<SoundManager>().RandomisePitch("Jump");
            FindObjectOfType<SoundManager>().Play("Jump", 0f);
        }

        //Simule la gravité
        if (!isGrounded)
        {
            //La vélocité Y est Clampé pour éviter que le joueur ne saute trop haut lors d'un changement de temps
            bodyVelocity.y = Mathf.Clamp(bodyVelocity.y - body.mass * delta, -100, 4);
        }
        else
        {
            bodyVelocity.y = Mathf.Clamp(bodyVelocity.y, 0, 100);
        }
    }

    private void FixedUpdate()
    {

        //Applique le déplacement au joueur
        body.velocity = bodyVelocity;
    }


    public void Move(Vector3 velocity)
    {
        //float verticalVelocity = body.velocity.y;
        //Règle la direction du déplacement du joueur
        bodyVelocity = new Vector3(velocity.x, bodyVelocity.y, velocity.z);

    }

    public void Jump(float impulsion)
    {
        if(isGrounded)
        {
            bodyVelocity.y += impulsion;
        }
    }

}
