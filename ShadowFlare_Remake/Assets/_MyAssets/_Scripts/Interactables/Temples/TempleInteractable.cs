using UnityEngine;

namespace ShadowFlareRemake.Interactables
{
    public class TempleInteractable : MonoBehaviour, Interactable
    {
        public InteractableType Type => InteractableType.Temple;

        [Header("References")]
        [SerializeField] private ParticleSystem _effect;

        private const string _playerTag = "Player";

        public void Interact()
        {

        }

        public void FinishInteraction()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(_playerTag))
            {
                _effect.Play();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(_playerTag))
            {
                _effect.Stop();
            }
        }
    }
}
