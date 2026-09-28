using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO KitchenObjectSO;

    private IkitchenObjecParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return KitchenObjectSO;
    }

    public void SetKitchenObjectParent(IkitchenObjecParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null )
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        kitchenObjectParent.SetKitchenObject(this);

        //变化的是物体的父级
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IkitchenObjecParent GetkitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public void DestorySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SwpanKitchenObject(KitchenObjectSO kitchenObjectSO,IkitchenObjecParent kitchenObjecParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjecParent);
        return kitchenObject;
    }

}
