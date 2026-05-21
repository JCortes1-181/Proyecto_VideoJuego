using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]private float speed = 3f;
    private Rigidbody2D playerRb;
    private Vector2 moveInput;
    private Animator playeranimator;
   
    void Start()
    {
        playerRb=GetComponent<Rigidbody2D>();
        playeranimator=GetComponent<Animator>();
    }

    
    void Update()
    {
        float moveX=Input.GetAxisRaw("Horizontal");
        float movey=Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, movey).normalized;

        playeranimator.SetFloat("Horizontal", moveX);
        playeranimator.SetFloat("Vertical", movey);
        playeranimator.SetFloat("speed", moveInput.sqrMagnitude);
    }
    private void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position+moveInput*speed*Time.fixedDeltaTime);
    }

}
