using UnityEngine;

public class weaponSway : MonoBehaviour
{
    public MovementScript mover;

    [Header("Sway settings")]
    public float smooth;
    public float swayMultiplier;

    [Header("Bob settiings")]
    public float bobSpeed;
    public float bobAmount;
    public float backwardOffset = -0.1f;
    float newZPosition;
    float oldZPosition;

    private float originalYPosition;

    private void Start()
    {
        originalYPosition = transform.localPosition.y;
        newZPosition = transform.localPosition.z + backwardOffset;
    }

    void Update()
    {
        Sway();
        Bob();
    }

    void Sway()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right); ;
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }

    void Bob()
    {
        float movementFactor = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
       
        Vector3 targetPosition;

        if (mover != null && mover.isMoving)
        {
            
            targetPosition = new Vector3(transform.localPosition.x, originalYPosition + movementFactor, newZPosition /*transform.localPosition.z + backwardOffset*/);
        }
        else if (mover != null && !mover.isMoving)
        {
            targetPosition = new Vector3(transform.localPosition.x, originalYPosition, transform.localPosition.z);
            targetPosition.z = oldZPosition;
        }
        else
        {        
            return;
        }
    
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smooth * Time.deltaTime);
    }
}


