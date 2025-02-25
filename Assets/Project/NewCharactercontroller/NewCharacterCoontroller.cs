using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace character
{
    public class NewCharacterCoontroller : MonoBehaviour
    {   
        [Header("Main Settings")]
        [Tooltip("Jump Power")]
        [SerializeField]
        [Range(2,10)] private float jumpForce = 3.5f;
        [Tooltip("Velocity of character without sprint")]
        [SerializeField]
        [Range(2,10)] private float walkingSpeed = 5f;
        [Tooltip("Velocity of character with sprint")]
        [SerializeField]
        [Range(2,10)] private float runningSpeed = 7f;

        [Header("Camera Settings")]
        [Tooltip("Reference to virtual camera / Setup automatically")]
        [SerializeField]
        public CinemachineVirtualCamera virtualCamera;
        [Tooltip("Reference to physical camera / Added automatically")]
        Transform mainCamera;
        [Tooltip("Camera sensitivity value")]
        [SerializeField]
        [Range(2,50)]private float sensitivity = 8f;

        [Header("Footsteps")]
		[SerializeField]
		private AudioMixerGroup AudioMixerGroup=null;
		[SerializeField]
        private List<AudioClip> FootstepClips = new List<AudioClip>();
        private AudioSource AudioSource;

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
            if (canMove)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, mainCamera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKey(KeyCode.S))
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
        public void stopController(CinemachineVirtualCamera _virtualCamera)
        {
            _virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "";
            _virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "";
            _virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisValue = 0;
            _virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisValue = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void startController(CinemachineVirtualCamera _virtualCamera)
        {
            _virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "Mouse X";
            _virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "Mouse Y";
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
		private void PlaySound(AudioClip clip, float volume)
		{
			if (clip == null)
            {
                return;
            }

			if (this.AudioSource == null)
			{
				this.AudioSource = gameObject.AddComponent<AudioSource>();
				this.AudioSource.outputAudioMixerGroup = this.AudioMixerGroup;
				this.AudioSource.spatialBlend = 1f;
			}
			if (this.AudioSource != null)
			{
				this.AudioSource.PlayOneShot(clip, volume);
			}
		}
        private void FootSteps()
        {
            if (!this.isJumping && GetComponent<Rigidbody>().velocity.sqrMagnitude > 1.5f && this.FootstepClips.Count > 0)
			{
				AudioClip clip = this.FootstepClips[UnityEngine.Random.Range(0,this.FootstepClips.Count)];
				PlaySound(clip, 0.2f);
			}
        }
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isJumping = false;
            }
        }
        /*void FixedUpdate()
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
        }*/
    }







}


