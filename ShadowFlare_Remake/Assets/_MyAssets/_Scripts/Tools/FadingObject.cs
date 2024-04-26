using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowFlareRemake.Tools {
    public class FadingObject : MonoBehaviour, IEquatable<FadingObject> {

        public List<Renderer> Renderers = new List<Renderer>();
        public Vector3 Position;
        public List<Material> Materials = new List<Material>();

        [HideInInspector]
        public float InitialAlpha;

        private bool _isFaded = false;

        private void Awake() {

            Position = transform.position;

            if(Renderers.Count == 0) {
                Renderers.AddRange(GetComponentsInChildren<Renderer>());
            }

            foreach(Renderer renderer in Renderers) {

                Materials.AddRange(renderer.materials);
            }

            InitialAlpha = Materials[0].color.a;
        }

        public bool Equals(FadingObject other) {
            return Position.Equals(other.Position);
        }

        public override int GetHashCode() {
            return Position.GetHashCode();
        }

        public bool GetIsFaded() {
            return _isFaded;
        }

        public void SetIsFaded(bool isFaded) {
            _isFaded = isFaded;
        }
    }
}

