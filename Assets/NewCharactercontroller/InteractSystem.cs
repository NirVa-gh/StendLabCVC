using UnityEngine;
using Cinemachine;
using System.Collections;
using UIWidgets;

namespace character
{
    public class InteractSystem : MonoBehaviour
    {   
        [Header("Cameras setup")]
        [Tooltip("Reference to virtual main camera on character")]
        [SerializeField]
        CinemachineVirtualCamera _virtualMainCamera;
        [Tooltip("Reference to virtual toPC camera on PC")]
        [SerializeField]
        CinemachineVirtualCamera _virtualtoPCCamera;
        [Tooltip("Reference to virtual PC camera on PC")]
        [SerializeField]
        CinemachineVirtualCamera _virtualPCCamera;
        [Tooltip("Reference to virtual Lab camera on PC")]
        [SerializeField]
        CinemachineVirtualCamera _virtualLabCamera;
        [Tooltip("Reference to physical work camera on PC")]
        [SerializeField]
        private Camera _mainCamera;

        [Header("Settings")]
        [Tooltip("How much to targer")]
        [SerializeField]
        float _reachDistance;
        [Tooltip("Secondary key to interact")]
        [SerializeField]
        public GameObject controlPanel;
        private KeyCode _secondaryKey;
        private NewCharacterCoontroller _newCharacterCoontroller;
        private bool canToPlayer = false;
        private Animator _animatorScreen;
        [Range(0.1f,4)]public float timeEnterBlend = 2f;
        [Range(0.1f,4)]public float timeExitBlend = 1f;
        
        void Awake()
        {
            _newCharacterCoontroller = FindAnyObjectByType<NewCharacterCoontroller>();
            _animatorScreen = GameObject.Find("FadeEffect").GetComponent<Animator>();
        }
 
        void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(_secondaryKey)  )
            {
                if (Physics.Raycast(ray, out hit, _reachDistance))
                {
                    if (hit.collider.gameObject.GetComponent<KeyBoard>() != null)
                    {   
                        EffectPC();
                        StartCoroutine(ExecuteAfterTime(0.5f));
                    }
                }
            }
            if (Input.GetKey(KeyCode.Space) && canToPlayer)
            {   
                toPlayer();
                canToPlayer = false;
            }
        }
        void toLab()
        {   
            controlPanel.SetActive(true);
            _mainCamera.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = timeEnterBlend;
            canToPlayer = true;
            _virtualtoPCCamera.Priority = 0;
            _virtualMainCamera.Priority = 0 ;
            _virtualPCCamera.Priority = 0;
            _virtualLabCamera.Priority = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            controlPanel.GetComponent<ControlPanel>().Show();
        }
        void toPlayer()
        {
            _mainCamera.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = timeExitBlend;
            _virtualMainCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "Mouse X";
            _virtualMainCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "Mouse Y";
            _virtualMainCamera.Priority = 1 ;
            _virtualPCCamera.Priority = 0;
            _virtualLabCamera.Priority = 0;
            _virtualtoPCCamera.Priority = 0;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _newCharacterCoontroller.canMove = true;
        }
        void EffectPC()
        {
            _mainCamera.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = timeEnterBlend;
            _virtualtoPCCamera.Priority = 1;
            _virtualMainCamera.Priority = 0 ;
            _virtualPCCamera.Priority = 0;
            _virtualLabCamera.Priority = 0;
            _animatorScreen.SetTrigger("In");
            GameObject.Find("Click").GetComponent<AudioSource>().Play();

        }
        
        IEnumerator ExecuteAfterTime(float timeInSec)
        {
            yield return new WaitForSeconds(timeInSec);
            toLab(); 
        } 
        void toPC()
        {
            _mainCamera.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = timeEnterBlend;
            canToPlayer = true;
            _newCharacterCoontroller.canMove = false;
            _virtualMainCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisName = "";
            _virtualMainCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisName = "";
            _virtualMainCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_InputAxisValue = 0;
            _virtualMainCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_InputAxisValue = 0;
            _virtualMainCamera.Priority = 0 ;
            _virtualPCCamera.Priority = 1;
            _virtualLabCamera.Priority = 0;
             _virtualtoPCCamera.Priority = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }  
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PC"))
            {   
                toPC();
            }
        }
    }
}

