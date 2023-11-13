using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Presenter
{
    public class OrbitalCameraMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float sensitivity = 1f;
        [SerializeField] private float followLerpSpeed = 1.5f;
        [SerializeField] private float followMinSpeed = 4f;
        
        private const int NullPointerId = Int32.MinValue;
        
        private Transform _target;
        private Transform _fakeTarget;
        private int _pointerID = NullPointerId;
        private float _lastPosX;
        
        private void Awake()
        {
            _fakeTarget = new GameObject("Fake Camera Target").transform;
            ChangeTarget(virtualCamera.LookAt);
            virtualCamera.LookAt = _fakeTarget;
            virtualCamera.Follow = _fakeTarget;
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (!_target)
            {
                return;
            }
            
            var dist = Vector3.Distance(_fakeTarget.position, _target.position);
            var movement = Mathf.Max(followMinSpeed, dist * followLerpSpeed);
            _fakeTarget.position = Vector3.MoveTowards(_fakeTarget.position, _target.position, movement * Time.deltaTime);
        }

        public void ChangeTarget(Transform target)
        {
            _target = target;
        }
        
        public void TeleportToTarget()
        {
            if (!_target)
            {
                return;
            }
            
            _fakeTarget.position = _target.position;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_pointerID != NullPointerId || 
                eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            _pointerID = eventData.pointerId;
            _lastPosX = eventData.pressPosition.x;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_pointerID != eventData.pointerId)
            {
                return;
            }

            var posX = eventData.position.x;
            //Multiplied with 0.001f to keep sensitivity more relatable value
            var angle = (posX - _lastPosX) * Screen.dpi * sensitivity * 0.001f;
            _fakeTarget.transform.Rotate(Vector3.up, angle);
            _lastPosX = posX;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_pointerID != eventData.pointerId)
            {
                return;
            }
            
            _pointerID = NullPointerId;
        }
    }
}