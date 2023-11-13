using Code.Model;
using UnityEngine;

namespace Code.View
{
    [RequireComponent(typeof(Rigidbody))]
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private BlockType blockType;

        public Rigidbody Rigidbody => rigidBody;
        public MeshRenderer MeshRenderer => meshRenderer;
        public BlockType BlockType => blockType;
    }
}