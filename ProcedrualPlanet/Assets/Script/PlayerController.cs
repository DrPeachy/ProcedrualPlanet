using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int moveSpeed = 500;
    public int descendSpeed = 50;
    public float lookSpeedX = 1;
    public float lookSpeedY = 1;
    
    public Transform camTrans;
    public GameObject viewCam,mainCam;
    private Vector3 pos;
    float xRotation;
    float yRotation;
    Rigidbody _rigidbody;
    private bool locked = true;
    private bool _enabled = false;

    void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        lookSpeedX *= .60f;
        lookSpeedY *= .60f;
#endif
        _rigidbody = GetComponent<Rigidbody>();
        pos = transform.position;
        _enabled = false;
    }


    void Update()
    {
        SwapCam();
        if(_enabled){
            if(locked){
                yRotation += Input.GetAxis("Mouse X") * lookSpeedX;
                xRotation -= Input.GetAxis("Mouse Y") * lookSpeedY; 
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                camTrans.localEulerAngles = new Vector3(xRotation, 0, 0);
                transform.eulerAngles = new Vector3(0, yRotation, 0);
            }
            
            Vector3 moveDir = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxis("Horizontal"); 
            moveDir *= moveSpeed;
            
            if (Input.GetKey("space"))
            {
                moveDir.y += descendSpeed;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDir.y -= descendSpeed;
            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                Cursor.lockState = CursorLockMode.None;
                locked = false;
            }
            if(Input.GetKeyUp(KeyCode.LeftAlt)){
                Cursor.lockState = CursorLockMode.Locked;
                locked = true;
            }
            _rigidbody.velocity = moveDir;
        }
    }

    public void SwapCam(){
        if (Input.GetKeyDown(KeyCode.C)) {
            viewCam.SetActive(!_enabled);
            mainCam.SetActive(_enabled);
            _enabled = !_enabled;
            if(!_enabled){
                Cursor.lockState = CursorLockMode.None;
            } else{
                Cursor.lockState = CursorLockMode.Locked;
            }
        } 
    }
}