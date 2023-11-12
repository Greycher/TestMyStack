using UnityEngine;

namespace Code.View
{
    [RequireComponent(typeof(Rigidbody), typeof(MeshRenderer))]
    public class BlockFacade : MonoBehaviour
    {
        private Rigidbody _rigidBody;
        private MeshRenderer _meshRenderer;

        public Rigidbody Rigidbody
        {
            get
            {
                if (!_rigidBody)
                {
                    _rigidBody = GetComponent<Rigidbody>();
                }

                return _rigidBody;
            }
        }
        
        public MeshRenderer MeshRenderer
        {
            get
            {
                if (!_meshRenderer)
                {
                    _meshRenderer = GetComponent<MeshRenderer>();
                }

                return _meshRenderer;
            }
        }
    }
}