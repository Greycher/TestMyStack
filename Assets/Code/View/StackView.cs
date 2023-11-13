using System;
using System.Collections.Generic;
using System.Drawing;
using Code.Model;
using TMPro;
using UnityEngine;

namespace Code.View
{
    public class StackView : MonoBehaviour
    {
        [SerializeField] private BlockView glassBlockPrefab;
        [SerializeField] private BlockView woodenBlockPrefab;
        [SerializeField] private BlockView stoneBlockPrefab;
        [SerializeField] private TextMeshPro gradeNameLabel;
        [SerializeField] private float spacing = .3f;

        private List<BlockView> _blockViews;
        private Vector3 _blockSize;

        private void Awake()
        {
            //Assumed all block is same size
            _blockSize = glassBlockPrefab.MeshRenderer.bounds.size;
        }

        public void BuildStack(Grade grade)
        {
            var blocks = grade.Topics;
            _blockViews = new List<BlockView>(blocks.Length);
            for (int i = 0; i < blocks.Length; i++)
            {
                var topic = blocks[i];
                BlockView prefab;
                switch (topic.BlockType)
                {
                    case BlockType.Glass:
                        prefab = glassBlockPrefab;
                        break;
                    
                    case BlockType.Wood:
                        prefab = woodenBlockPrefab;
                        break;
                    
                    case BlockType.Stone:
                        prefab = stoneBlockPrefab;
                        break;

                    default:
                        throw new Exception();
                }
                
                var pose = GetPoseAt(i);
                _blockViews.Add(Instantiate(prefab, pose.position, pose.rotation, transform));
            }

            gradeNameLabel.text = grade.DisplayName;
        }

        public void TestTheStack()
        {
            foreach (var blockView in _blockViews)
            {
                if (blockView.BlockType == BlockType.Glass)
                {
                    blockView.gameObject.SetActive(false);
                }
                else
                {
                    blockView.Rigidbody.isKinematic = false;
                }
            }
        }

        public void ResetStack()
        {
            for (int i = 0; i < _blockViews.Count; i++)
            {
                var blockView = _blockViews[i];
                blockView.Rigidbody.isKinematic = true;
                var localPose = GetLocalPoseAt(i);
                blockView.transform.localPosition = localPose.position;
                blockView.transform.localRotation = localPose.rotation;
                blockView.gameObject.SetActive(true);
            }
        }
        
        private Pose GetLocalPoseAt(int i)
        {
            var x = (i % 3 - 1) * _blockSize.x + (i % 3 - 1) * spacing;
            var y = (i / 3) * _blockSize.y;
            float z;
            Quaternion localRot;
            if (i / 3 % 2 == 0)
            {
                z = x;
                x = 0;
                localRot = Quaternion.LookRotation(Vector3.right);
            }
            else
            {
                localRot = Quaternion.identity;
                z = 0;
            }
            var localPos = new Vector3(x, y, z);
            return new Pose(localPos, localRot);
        }

        private Pose GetPoseAt(int i)
        {
            var localPose = GetLocalPoseAt(i);
            var pos = transform.TransformPoint(localPose.position);
            var rot = transform.rotation * localPose.rotation;
            return new Pose(pos, rot);
        }

        public int FindBlockIndex(BlockView blockView)
        {
            return _blockViews.FindIndex(view => view.GetInstanceID() == blockView.GetInstanceID());
        }
    }
}