using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving = false;
    private Animator animator;
    private Transform targetChest;
    private Transform previousChest;
    Vector2 pos;
    NewPlayerController playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController= GetComponent<NewPlayerController>();
    }

    private void Update()
    {
        if (isMoving && targetChest != null)
        {
            Vector3 moveDirection = (targetChest.position - transform.position).normalized;
            CheckSpriteFlip();
        }
        
    }

    void CheckSpriteFlip()
    {
        if (pos.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (pos.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (targetChest != null)
        {

            if (collision.gameObject == targetChest.gameObject)
            {
                isMoving = false;
                animator.SetBool("isMoving", false);
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                FindObjectOfType<ChestSpawner>().DisplayAdvice(targetChest.gameObject);
                targetChest.GetComponent<Animator>().SetTrigger("open");
                playerController.ChestSelected = false;

            }
        }
        else
        {
            print("The target chest is null!");
        }
    }

    private void FixedUpdate()
    {
        if (isMoving && targetChest != null)
        {
            if (targetChest != previousChest)
            {
                float horizontalMovement = 0f;

                if (transform.position.x > targetChest.transform.position.x)
                {
                    horizontalMovement = -1f;
                }
                else if (transform.position.x < targetChest.transform.position.x)
                {
                    horizontalMovement = 1f;
                }

                Vector2 velocity = new Vector2(horizontalMovement * moveSpeed, 0f);
                GetComponent<Rigidbody2D>().velocity = velocity * Time.deltaTime;
            }
        }
    }


    public void MoveTowardsChest(Transform chestTransform)
    {
            pos = chestTransform.position;
            targetChest = chestTransform;
            isMoving = true;
            animator.SetBool("isMoving", true);
            animator.SetBool("isWaving", false);
    
    }
}
