using Cinemachine;
using System;
using UnityEngine;

namespace character
{
    public class NewCharacterCoontroller : MonoBehaviour
    {   
        [Header("Main Settings")]
        [Tooltip("Jump Power")]
        [SerializeField]
        private float jumpForce = 3.5f;
        [Tooltip("Velocity of character without sprint")]
        [SerializeField]
        private float walkingSpeed = 5f;
        [Tooltip("Velocity of character with sprint")]
        [SerializeField]
        private float runningSpeed = 7f;

        [Header("Camera Settings")]
        [Tooltip("Reference to virtual camera / Setup automatically")]
        [SerializeField]
        CinemachineVirtualCamera virtualCamera;
        [Tooltip("Reference to physical camera / Added automatically")]
        [SerializeField]
        Transform mainCamera;
        [Tooltip("Camera sensitivity value")]
        [SerializeField]
        [Range(2,50)]private float sensitivity = 8f;

        [Header("Status viewer")]
        public bool isJumping = false;
        public float currentSpeed;
        public bool Sprint;
        public bool canMove = true;
        public bool canJump = false;
        private Rigidbody _rigidbody;

        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _rigidbody = GetComponent<Rigidbody>();
            mainCamera = GameObject.Find("Main Camera").transform;
        }
        private void Update()
        {
            if (canMove)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                    {
                        currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
                    }
                    else
                    {
                        currentSpeed = Mathf.Lerp(currentSpeed, runningSpeed, Time.deltaTime * 3);
                        Sprint = true;
                    }
                }
                else
                {
                    currentSpeed = Mathf.Lerp(currentSpeed, walkingSpeed, Time.deltaTime * 3);
                    Sprint = false;        
                }
                if (canJump)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && !isJumping) 
                    {
                        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                        isJumping = true;
                    }
                }

            }
            
        }
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isJumping = false;
            }
        }
        void FixedUpdate()
        {
            if (canMove)
            {
                virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = sensitivity * 100;
                virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = sensitivity * 100;
                Vector3 camF = mainCamera.forward;
                Vector3 camR = mainCamera.right;
                camF.y = 0;
                camR.y = 0;
                Vector3 movingVector;
                movingVector = Vector3.ClampMagnitude(camF.normalized * Input.GetAxis("Vertical") * currentSpeed +
                camR.normalized * Input.GetAxis("Horizontal") * currentSpeed, currentSpeed);
                _rigidbody.velocity = new Vector3(movingVector.x, _rigidbody.velocity.y, movingVector.z);
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }



    }







}


