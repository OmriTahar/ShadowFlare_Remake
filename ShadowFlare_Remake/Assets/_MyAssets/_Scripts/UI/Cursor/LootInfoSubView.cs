using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor
{
    public class LootInfoSubView : MonoBehaviour
    {
        [Header("References")]
        public RectTransform _lootInfoRect; 

        private Camera mainCamera;    // Reference to the main camera
        private Vector2 screenSize;     // Stores the screen dimensions

        private void Start()
        {
            mainCamera = Camera.main;   // Get the main camera
            screenSize = new Vector2(Screen.width, Screen.height);
        }

        private void Update()
        {
            Vector2 mousePosition = Input.mousePosition;

            // Calculate desired image position based on cursor and screen size
            float desiredWidth = screenSize.x / 5f;
            float desiredHeight = desiredWidth * _lootInfoRect.sizeDelta.y / _lootInfoRect.sizeDelta.x; // Maintain aspect ratio

            Vector2 desiredPosition = mousePosition;

            // Clamp desired position to stay within screen bounds
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, desiredWidth / 2f, screenSize.x - desiredWidth / 2f);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, desiredHeight / 2f, screenSize.y - desiredHeight / 2f);

            // Update image position with anchor point adjustment
            _lootInfoRect.anchoredPosition = mainCamera.ScreenToWorldPoint(desiredPosition) - _lootInfoRect.parent.transform.position;
        }
    }
}
