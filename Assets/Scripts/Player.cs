using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IkitchenObjecParent
{
    public static Player Instance{ get; private set; }

    public event EventHandler OnPickedSomething;

    public event EventHandler<OnSelectCounterChangedEventArgs> OnSelectCounterChanged;
    public class OnSelectCounterChangedEventArgs : EventArgs
    {
        public BaseCounter baseCounter;
    }
    [SerializeField] private LayerMask counterLayerMask;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private KitchenObject kitchenObject;
    //[SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] public Transform kitchenObjectHoldPoint;

    private bool iswalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;

    public bool isColliderKitchenObject = false;
    public bool ispick = false;

    private void Awake()
    {
        //单例模式
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }



    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
        {
            return;
        }
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
            ispick = false;
        }
        else
        {
            ispick = true;
        }


    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying())
        {
            return;
        }
        if (selectedCounter != null)
        {
            //Debug.Log(selectedCounter);
            selectedCounter.Interact(this);

        }
    }

    private void Update()
    {
        HandleMove();
        HandleInteractions();
        //Debug.Log(isColliderKitchenObject);
    }

    public bool isWalking()
    {
        return iswalking;
    }

    //处理交互
    private void HandleInteractions()
    {
        Vector2 inputvector2 = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputvector2.x, 0f, inputvector2.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 1f;
        //有没有碰撞
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance,counterLayerMask))
        {
            //Debug.Log(raycastHit.transform.gameObject.name);
            //是否碰撞到柜台
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //柜台是否被选择
                if(baseCounter != selectedCounter)
                {
                    SetSelectCounter(baseCounter);
                }
            }
            else
            {

                SetSelectCounter(null);
            }
        }
        else
        {
            SetSelectCounter(null);
        }
    }

    //处理移动情况，即碰撞到物体
    private void HandleMove()
    {

        Vector2 inputvector2 = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputvector2.x, 0f, inputvector2.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .9f;
        float playerHeight = 6f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //该前进方向有障碍物
            //只可以在x轴移动的情况
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        //没有碰撞到物体
        if (canMove)
        {
            //角色的位置加上角色的位移
            transform.position += moveDir * moveDistance;
            if(Physics.Raycast(transform.position,transform.forward, out RaycastHit hit, 1f))
            {
                if (hit.collider.gameObject.tag == "KitchenObject")
                {
                    isColliderKitchenObject = true;
                }
                else
                {
                    isColliderKitchenObject = false;
                }
            }
            
        }

        //正在走路播放行走动画
        iswalking = moveDir != Vector3.zero;

        //随移动修改面向方向
        float rotatSpeed = 10f;
        //Vector3.Slerp平滑的过渡旋转角度，根据原方向、目的方向、游戏运行时间*旋转速度
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotatSpeed);
    }

    private void SetSelectCounter(BaseCounter baseCounter)
    {
        this.selectedCounter = baseCounter;
        OnSelectCounterChanged?.Invoke(this, new OnSelectCounterChangedEventArgs
        {
            baseCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HaskKitchenObject()
    {
        return this.kitchenObject != null;
    }
}
