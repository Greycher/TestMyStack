using System;
using Code.View;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Presenter
{
    public class BlockDescriptionPresenter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private LayerMask blockLayerMask;
        [SerializeField] private BlockDescriptionView blockDescriptionView;
        [SerializeField] private StackPresenter stackPresenter;

        private void OnEnable()
        {
            stackPresenter.OnSelectedStackChangedEvent.AddListener(OnSelectedStackChanged);
        }

        private void OnDisable()
        {
            stackPresenter.OnSelectedStackChangedEvent.RemoveListener(OnSelectedStackChanged);
        }
        
        private void OnSelectedStackChanged()
        {
            blockDescriptionView.gameObject.SetActive(false);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            bool shouldBeEnabled = blockDescriptionView.gameObject.activeSelf;
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                shouldBeEnabled = false;
                var ray = Camera.main.ScreenPointToRay(eventData.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 50, blockLayerMask.value))
                {
                    var blockView = hit.collider.GetComponentInParent<BlockView>();
                    if (blockView)
                    {
                        if (stackPresenter.DoesBelongToSelectedStack(blockView, out int index))
                        {
                            var topic = stackPresenter.GetTopicAtSelectedStack(index);
                            blockDescriptionView.UpdateDescription(topic);
                            shouldBeEnabled = true;
                        }
                    }
                }
            }
            
            blockDescriptionView.gameObject.SetActive(shouldBeEnabled);
        }

        public void OnPointerDown(PointerEventData eventData) { }
    }
}