using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");

        Vector3 tempVect = new Vector3(xInput, zInput, 0);
        tempVect = tempVect.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.transform.position + tempVect);
    }
}
