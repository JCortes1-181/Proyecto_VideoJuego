using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
   [SerializeField] private float speed = 5f;

   /* [SerializeField] private Sprite leftShip;
    [SerializeField] private Sprite rightShip;
    [SerializeField] private Sprite centerShip;
*/

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

   private Vector2 movement;

    private void Awake()
    {
        rb2d=GetComponent<Rigidbody2D>();
        sr=GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
      movement.x=Input.GetAxis("Horizontal"); 
      
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position+movement*speed*Time.fixedDeltaTime);
    }
}
