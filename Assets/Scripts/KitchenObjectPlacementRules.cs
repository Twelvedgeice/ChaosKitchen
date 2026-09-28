public static class KitchenObjectPlacementRules
{
    public static bool MustBePlacedOnPlateOrInPot(KitchenObjectSO kitchenObjectSO)
    {
        if (kitchenObjectSO == null) return false;
        return kitchenObjectSO.name == "RiceCooked" ||
               kitchenObjectSO.name == "TomatoSliced" ||
               kitchenObjectSO.name == "MeatSliced";
    }

    public static bool CanCombineWithCookedRice(KitchenObjectSO kitchenObjectSO)
    {
        if (kitchenObjectSO == null) return false;
        return kitchenObjectSO.name == "TomatoSliced" ||
               kitchenObjectSO.name == "MeatSliced" ||
               kitchenObjectSO.name == "CheeseBlock";
    }
}
