using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    private ChangeScene sceneChanger;
    [SerializeField] private float moveSpeed;
   // [SerializeField] private bool playerCanMove = true;
    [SerializeField] private bool isMoving;
    [SerializeField] private LayerMask solidObjectsLayer;
    [SerializeField] private LayerMask grassLayer;
    private Animator playerAnimator;
    private Vector2 moveInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!isMoving)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    
        //remove diagonal movement
        if (moveInput.x != 0)
            moveInput.y = 0;
        
        if (moveInput != Vector2.zero)
        {
            playerAnimator.SetFloat("moveX", moveInput.x);
            playerAnimator.SetFloat("moveY", moveInput.y);
            var targetPos = transform.position;
            targetPos.x += moveInput.x;
            targetPos.y += moveInput.y;
            
            if (isWalkable(targetPos))
                StartCoroutine(Move(targetPos));
        }
        
        playerAnimator.SetBool("isMoving", isMoving);
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");
        // Vector3 direction = new Vector3(horizontal, vertical, 0);
        // if (playerCanMove)
        // {
        //     transform.Translate(direction * (moveSpeed * Time.deltaTime));
        // }
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;

        CheckForEncounters();
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if (Random.Range(1, 101) <= 10)
            {
                SceneManager.LoadScene("BattleScene");
            }
        }
    }

    private bool isWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }
}
