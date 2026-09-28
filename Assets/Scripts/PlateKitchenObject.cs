using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler OnContentsChanged;
    public event EventHandler<OnIngredientAddedEvnetArgs> OnIngredientAdded;
    public class OnIngredientAddedEvnetArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!CanAddIngredient(kitchenObjectSO))
        {
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEvnetArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
        
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
    public bool CanAddIngredient(KitchenObjectSO food)
    {
        return food != null && food.name != "Rice" && validKitchenObjectSOList.Contains(food)
            && !kitchenObjectSOList.Exists(item => item.name == food.name);
    }
    public void RemoveIngredients(List<KitchenObjectSO> foods)
    {
        foreach (var food in foods) kitchenObjectSOList.Remove(food);
        OnContentsChanged?.Invoke(this, EventArgs.Empty);
    }}
