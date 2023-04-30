using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    public Rigidbody rb;
    public float moveSpeed = 4.5f;
    public float timeToMoove = 0.5f;
    //public InputAction playerControls;
    PlayerKeymap playerKeymap;
    public Camera mainCamera;
    public float debugVectorDistance = 5f;
    public Vector3 debugVector;
    public Vector3 debugVector2;
    [SerializeField]private float angleDifference;
    private bool isIdle = true;
    float mooveForwardMaxAngle = 35;
    float mooveForwardMinAngle = -35;
    float mooveBackwardMaxAngle = 150;
    float mooveBackwardMinAngle = -150;
    float mooveLeftMaxAngle = -45;
    float mooveLeftMinAngle = -138;
    float mooveRightMaxAngle = 138;
    float mooveRightMinAngle = 45;
    Vector3 moveDirection = Vector3.zero;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerKeymap = new PlayerKeymap();
    }

    private void OnEnable()
    {
        playerKeymap.Enable();
    }

    private void OnDisable()
    {
        playerKeymap.Disable();
    }
   
    void Update()
    {
        rb.velocity = moveDirection * moveSpeed;

        Vector3 vectorToDebug2 = (debugVector2 - transform.position).normalized;

        angleDifference = Vector3.Angle(transform.forward, vectorToDebug2) * Mathf.Sign(Vector3.Cross(transform.forward, vectorToDebug2).y);

        moveDirection = playerKeymap.PlayerMovement.WASD.ReadValue<Vector3>();

        debugVector = transform.position + (transform.forward * debugVectorDistance);

        if (moveDirection != Vector3.zero)
        {
            debugVector2 = transform.position + moveDirection.normalized * debugVectorDistance;
            isIdle = false;
            animator.SetBool("idle", false);
        }
        else
        {
            debugVector2 = transform.position;
            isIdle = true;
            animator.SetBool("idle", true);
            animator.SetBool("mooveforward", false);
            animator.SetBool("mooveback", false);
            animator.SetBool("mooveleft", false);
            animator.SetBool("mooveright", false);
        }

        if (!isIdle)
        {
            if (rb.velocity != Vector3.zero)
            {
                StartCoroutine(WaitToMoove());

                if (angleDifference >= mooveForwardMinAngle && angleDifference <= mooveForwardMaxAngle)
                {

                    if (!animator.GetBool("mooveforward"))
                    {
                        animator.SetBool("mooveforward", true);
                        animator.SetBool("mooveback", false);
                        animator.SetBool("mooveleft", false);
                        animator.SetBool("mooveright", false);
                    }
                }

                if ((angleDifference >= mooveBackwardMaxAngle && angleDifference <= 180f) || (angleDifference <= mooveBackwardMinAngle && angleDifference > -180))
                {
                    if (!animator.GetBool("mooveback"))
                    {
                        animator.SetBool("mooveback", true);
                        animator.SetBool("mooveforward", false);
                        animator.SetBool("mooveleft", false);
                        animator.SetBool("mooveright", false);
                    }
                    moveDirection *= 0.7f; 
                }

                if (angleDifference >= mooveLeftMinAngle && angleDifference <= mooveLeftMaxAngle)
                {
                    if (!animator.GetBool("mooveleft"))
                    {
                        animator.SetBool("mooveleft", true);
                        animator.SetBool("mooveforward", false);
                        animator.SetBool("mooveback", false);
                        animator.SetBool("mooveright", false);
                    }
                }

                if (angleDifference >= mooveRightMinAngle && angleDifference <= mooveRightMaxAngle)
                {
                    if (!animator.GetBool("mooveright"))
                    {
                        animator.SetBool("mooveright", true);
                        animator.SetBool("mooveforward", false);
                        animator.SetBool("mooveback", false);
                        animator.SetBool("mooveleft", false);
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLenght;

        if (groundPlane.Raycast(cameraRay, out rayLenght))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    IEnumerator WaitToMoove()
    {
        yield return new WaitForSeconds(timeToMoove);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, debugVector - transform.position);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, debugVector2 - transform.position);
    }

}