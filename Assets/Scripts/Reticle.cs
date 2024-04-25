using UnityEngine;

public class Reticle : MonoBehaviour
{
    private RectTransform reticle;
    private M16Handler m16Handler;
    private GlockHandler glockHandler;

    private bool wasShooting = false;

    public float maxSize;
    public float restingSize;
    public float speed;
    public Rigidbody player;

    private float currentSize;

    private void Start()
    {
        reticle = GetComponent<RectTransform>();
        m16Handler = GameObject.Find("m16Finished")?.GetComponent<M16Handler>();
        glockHandler = GameObject.Find("glock-18")?.GetComponent<GlockHandler>();
    }

    public void Update()
    {
        if ((m16Handler != null && (isMoving || m16Handler.IsMouseButtonDown)) ||
            (glockHandler != null && (isMoving || glockHandler.IsMouseButtonDown)))
        {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
            wasShooting = true;
        }
        else
        {
            if (wasShooting)
            {
                currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
                wasShooting = false;
            }
            else
            {
                currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
            }
        }

        reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }

    private bool isMoving
    {
        get
        {
            return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }
    }
}
