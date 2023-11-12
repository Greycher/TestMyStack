using System;
using System.Drawing;
using Code.Model;
using UnityEngine;

namespace Code.View
{
    public class JengaView : MonoBehaviour
    {
        [SerializeField] private BlockFacade glassBlockPrefab;
        [SerializeField] private BlockFacade woodenBlockPrefab;
        [SerializeField] private BlockFacade stoneBlockPrefab;
        [SerializeField] private float spacing = .3f;

        private Rigidbody[] _blockBodies;
        private Vector3 _blockSize;

        private void Awake()
        {
            //Assumed all block is same size
            _blockSize = glassBlockPrefab.MeshRenderer.bounds.size;
        }

        public void BuildJenga(Grade grade)
        {
            var blocks = grade.Blocks;
            _blockBodies = new Rigidbody[blocks.Length];
            for (int i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];
                Rigidbody prefab;
                switch (block.BlockType)
                {
                    case BlockType.Glass:
                        prefab = glassBlockPrefab.Rigidbody;
                        break;
                    
                    case BlockType.Wood:
                        prefab = woodenBlockPrefab.Rigidbody;
                        break;
                    
                    case BlockType.Stone:
                        prefab = stoneBlockPrefab.Rigidbody;
                        break;

                    default:
                        throw new Exception();
                }

                var x = (i % 3 - 1) * _blockSize.x + (i % 3 - 1) * spacing;
                var y = (i / 3) * _blockSize.y;
                float z;
                Quaternion rotation;
                if (i / 3 % 2 == 0)
                {
                    z = x;
                    x = 0;
                    rotation = Quaternion.LookRotation(Vector3.right);
                }
                else
                {
                    rotation = Quaternion.identity;
                    z = 0;
                }
                var localPos = new Vector3(x, y, z);
                var pos = transform.TransformPoint(localPos);
                _blockBodies[i] = Instantiate(prefab, pos, rotation, transform);
            }

        }
    }
}