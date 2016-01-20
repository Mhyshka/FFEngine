using UnityEngine;
using System.Collections;
using System;

namespace FF.Pong
{
    internal class RacketTouchController : ARacketComponent
    {
        #region Inspector Properties
        public Collider selectionCollider = null;
        public Camera mainCamera = null;
        #endregion

        #region Properties
        protected bool _isSelected = false;
        protected int _touchId;
        #endregion

        #region Init & Destroy
        internal override void Activate()
        {
        }

        internal override void TearDown()
        {
        }
        #endregion

        void Update()
        {
            
            if (UnityEngine.Input.touchCount > 0)
            {
                foreach (Touch each in UnityEngine.Input.touches)
                {
                    if (!_isSelected && each.phase == TouchPhase.Began)
                    {
                        Vector2 screenPos = UnityEngine.Input.touches[0].position;
                        RaycastHit hit = FindSelectable(mainCamera, screenPos);

                        if (hit.collider == selectionCollider)
                        {
                            _isSelected = true;
                            _touchId = each.fingerId;
                        }
                    }
                    else if (each.fingerId == _touchId && (each.phase == TouchPhase.Canceled || each.phase == TouchPhase.Ended))
                    {
                        _isSelected = false;
                    }
                    else if (each.fingerId != _touchId && each.phase == TouchPhase.Ended)
                    {
                        motor.TrySmash();
                    }
                }

                if (_isSelected)
                    UpdateDraggingTouch();
            }

            
        }

        void UpdateDraggingTouch()
        {
            RaycastHit hit = FindBoardHit(mainCamera, UnityEngine.Input.GetTouch(_touchId).position);
            if(hit.collider != null)
            {
                float value = Mathf.InverseLerp(-motor.maxPositionX, 
                                                motor.maxPositionX,
                                                hit.point.x);
                motor.TargetRatio = value;
            }
            
        }

        RaycastHit FindBoardHit(Camera a_cam, Vector2 a_screenPos)
        {
            Ray ray = a_cam.ScreenPointToRay(a_screenPos);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Board"));
            return hit;
        }

        RaycastHit FindSelectable(Camera a_cam, Vector2 a_screenPos)
        {
            Ray ray = a_cam.ScreenPointToRay(a_screenPos);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("Selection"));
            return hit;
        }
    }
}