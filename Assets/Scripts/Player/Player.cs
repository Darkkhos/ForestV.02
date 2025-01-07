using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public GameObject projectile;
    public Transform shootPoint;

    public PoolManager poolManager;

    public SOPlayerSetup soPlayerSetup;

    [Header("Animation Setup")]
    public SOFloat soJumpScaleX;
    public SOFloat soJumpScaleY;
    public SOFloat soJumpScaleTime;


    public Ease ease = Ease.OutBack;

    [Header("Animation Player")]
    public string boolWalk = "Walk";
    public Animator animator;

    public bool _isGround;
    public bool _isRunning;
    private float _currentSpeed;
    private int _playerDirection = 1;


    private void Start()
    {
        _isGround = true;
        _isRunning = false;
    }
    private void Update()
    {
        HandleJump();
        HandleMovements();
    }

    
    private void HandleMovements()
    {
        if (UnityEngine.Input.GetKey(KeyCode.LeftShift))
            _currentSpeed = soPlayerSetup.speedRun;
        else
            _currentSpeed = soPlayerSetup.speed;

        if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
            SpawnObject();
            


        #region Move X
        if (UnityEngine.Input.GetKey(KeyCode.A))
        {
            rb2D.linearVelocity = new Vector2(-_currentSpeed, rb2D.linearVelocity.y);
            animator.SetBool(boolWalk, true);
            rb2D.transform.localScale = new Vector3(-10, 10, 1);
            _playerDirection = -1;
        }
        else if (UnityEngine.Input.GetKey(KeyCode.D))
        {
            rb2D.linearVelocity = new Vector2(_currentSpeed, rb2D.linearVelocity.y);
            animator.SetBool(boolWalk, true);
            rb2D.transform.localScale = new Vector3(10, 10, 1);
            _playerDirection = 1;
        }
        else
        {
            animator.SetBool(boolWalk, false);
        }
        #endregion

        #region Move Y

        if (rb2D.linearVelocity.x > 0)
        {
            rb2D.linearVelocity -= soPlayerSetup.friction;
        }
        else if (rb2D.linearVelocity.x < 0)
        {
            rb2D.linearVelocity += soPlayerSetup.friction;
        }

        #endregion
    }


    private void HandleJump()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && _isGround)
        {           
            rb2D.linearVelocity = Vector2.up * soPlayerSetup.forceJump;
            rb2D.transform.localScale = new Vector3(11, 10, 1);
            if (tween != null) tween.Kill();

            HandleScaleJump();
        }
    }

    private void SpawnObject()
    {
        var obj = poolManager.GetPooledObject();

        obj.SetActive(true);
        obj.GetComponent<Projectile>().StartProjectile();
        obj.transform.SetParent(null);
        obj.transform.position = shootPoint.transform.position;
    }

    Tween tween;

    private void HandleScaleJump() 
    {
        rb2D.transform.DOScaleY(soJumpScaleY.value, soJumpScaleTime.value).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        tween = DOTween.To(ScaleXGetter, ScaleXSetter, soJumpScaleX.value, soJumpScaleTime.value).SetEase(ease);
    }

    private float ScaleXGetter()
    {
        return rb2D.transform.localScale.x;
    }

    private void ScaleXSetter(float value)
    {
        var s = rb2D.transform.localScale;
        s.x = value * _playerDirection;
        rb2D.transform.localScale = s;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }
}
