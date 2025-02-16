using UnityEngine;

namespace ShadowFlareRemake.Interactables.Warehouse
{
    public class WarehouseInteractable : MonoBehaviour, Interactable
    {
        public InteractableType Type => InteractableType.Warehouse;

        public void OnInteract()
        {
            print("Warehouse bitch!");
        }
    }
}
