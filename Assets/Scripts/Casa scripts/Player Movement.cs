using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed = 3f;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb=GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        float moveX=Input.GetAxisRaw("Horizontal");
        float movey=Input.GetAxisRaw("Vertical");
        moveInput=new Vector2(moveX,movey);
    }
    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position+moveInput*speed*Time.fixedDeltaTime);
    }

}
