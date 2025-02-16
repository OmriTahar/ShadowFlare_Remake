using UnityEngine;

namespace ShadowFlareRemake.Interactables.Warehouse
{
    public class WarehouseInteractable : MonoBehaviour, Interactable
    {
        public InteractableType Type => InteractableType.Warehouse;

        public void Interact()
        {
            print("Warehouse bitch!");
        }

        public void FinishInteraction()
        {
            print("Finished Warehouse bitch!");
        }
    }
}
