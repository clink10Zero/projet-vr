using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;


public class continuousMovement : MonoBehaviour
{
    public float speed = 1;
    public XRNode inputSource;
    public XRNode inputSourceRotation;

    public float gravity = -9.81f;
    public LayerMask groundlayer;
    public float additionalHeight = 0.2f;

    private float fallingSpeed;
    public  XROrigin rig;
    private Vector2 inputAxis;
    private Vector2 inputAxisRotatation;

    private CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        InputDevice device2 = InputDevices.GetDeviceAtXRNode(inputSourceRotation);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        device2.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisRotatation);
    }

    void FixedUpdate()
    {
        CapsuleFollowHeadset();
        
        Quaternion headYaw = Quaternion.Euler(0, rig.Camera.gameObject.transform.eulerAngles.y, 0);
        Vector3 direction =  headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);

        //gravity
        bool isGrounded = CheckIfGrounded();
        if (!isGrounded)
            fallingSpeed += gravity * Time.fixedDeltaTime;
        else
            fallingSpeed = 0;
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);

        rig.gameObject.transform.rotation *= (Quaternion.Euler(0, 10 * (-inputAxisRotatation.y), 0));
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.gameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height/2 + character.skinWidth , capsuleCenter.z);
    }

    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        return Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundlayer);
    }
}
