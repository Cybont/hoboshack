using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RainRunner
{
    public class PlayerControl : MonoBehaviour
    {
        Rigidbody2D rb;
        GameObject camObj;
        Camera camera;
        private float regCamSize;

        #region Hp Variables
  
        private float currentHp;

        public float maxHp = 1;
        public float rainDamage = 0.01f;


        #endregion

        #region other variables
        private Vector2 movement;
        public float runSpeed = 15f;

        public float jumpForce = 50f;
        public int jumpsExtra = 1;
        private int jumpsCurrent;

        public float leapSpeedX = 50f;
        public float leapSpeedY = 30f;
        private bool leaping = false;
        public float leapTBegin = 0.3f;
        public float leapZoomAmount = 0.5f;
        public float leapZoomMax = 30f;
        private float leapT;
        public float leapTAmount = 0.05f;

        public float leapBeginTime = 0.5f;
        private Counter leapBeginTimer;

        bool grounded = false;
        public Transform groundCheck;
        float groundRadius = 0.1f;
        public LayerMask whatIsGround;


        #endregion


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            camObj = GameObject.Find("Main Camera");
            camera = camObj.GetComponent<Camera>();
            regCamSize = camera.orthographicSize;

            jumpsCurrent = jumpsExtra;
            currentHp = maxHp;
            leapT = leapTBegin;
            leapBeginTimer = new Counter(leapBeginTime);
        }

        #region HP stuff
    
        public void Damage(float damageAmount)
        {
            currentHp = currentHp - damageAmount;
        }
        public void Heal(float healAmount)
        {
            float healCal = healAmount + currentHp;
            if(healCal > maxHp) currentHp = maxHp;
            else currentHp = healCal;
        }

        void OnParticleCollision() 
        {
            Damage(rainDamage);
        }
        #endregion

        void Awake()
        {
       
        }

        void FixedUpdate()
        {
            #region Movement/Velocity

            grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
            
            #region Leaping
            if (leaping)
            {
                if(leapBeginTimer.Wait())
                {
                    float camSize = camera.orthographicSize;
                    if (camSize > regCamSize) camera.orthographicSize = camSize - leapZoomAmount;
                    else camera.orthographicSize = regCamSize;

                    jumpsCurrent = 0;
                    Debug.Log("Leaping");
                    float leapSpeed = Mathf.Lerp(runSpeed, leapSpeedX, leapT);
                    leapT = leapT + leapTAmount;
                    if (leapT > 1)
                    {
                        leapT = 1; //?

                        leaping = false;
                        leapT = leapTBegin;
                    }
                    else
                    {
                        leapT = leapT + leapTAmount;
                    }
                    movement = new Vector2(leapSpeed, leapSpeedY);
                    Debug.Log("LS " + leapSpeed);
                }
                else
                {
                    float camSize = camera.orthographicSize;
                    if (camSize < leapZoomMax) camera.orthographicSize = camSize + leapZoomAmount;
                    else camera.orthographicSize = leapZoomMax;
                    movement = new Vector2(0, 0);
                }

            }
            #endregion

            #region Running
            // Runs
            else
            {
                Debug.Log("Regular movement");
                movement = new Vector2(runSpeed, rb.velocity.y);
            }
            #endregion

            rb.velocity = movement;

            #endregion
        }
        void Update()
        {
            Jump();
            Leap();
        }

    #region PhysicsMechanics

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumpsCurrent > 0)
            {
                float jumpForceUsed;
                if (!grounded) jumpForceUsed = jumpForce * 0.8f;
                else jumpForceUsed = jumpForce;
                rb.velocity = new Vector2(0, jumpForceUsed);
                jumpsCurrent = jumpsCurrent - 1;

                Debug.Log("Force:" + jumpForceUsed);
            }
            if (grounded) jumpsCurrent = jumpsExtra;
        }

        private void Leap()
        {
            if (Input.GetKeyDown(KeyCode.A) && grounded)
            {
                leaping = true;
                leapBeginTimer.SetTimeStamp();
                Debug.Log("Leap");
            }
        }
        
    #endregion

    }
}
