namespace ShadowFlareRemake.Interactables
{
    public interface Interactable
    {
        public InteractableType Type { get; }
        public void OnInteract();
    }
}
