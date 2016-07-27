using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class RacketMouseController : ARacketComponent
    {
        #region Inspector Properties
        public Collider selectionCollider = null;
        public Camera mainCamera = null;
        #endregion

        #region Properties
        protected bool _isSelected = false;
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
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Vector2 screenPos = new Vector2(UnityEngine.Input.mousePosition.x,
                                                UnityEngine.Input.mousePosition.y);
                RaycastHit hit = FindSelectable(mainCamera, screenPos);

                if (hit.collider == selectionCollider)
                {
                    _isSelected = true;
                }
            }
            else if (_isSelected && UnityEngine.Input.GetMouseButton(0))
            {
                UpdateDraggingMouse();
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _isSelected = false;
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                motor.TrySmash();
            }
        }

        void UpdateDraggingMouse()
        {
            RaycastHit hit = FindBoardHit(mainCamera, UnityEngine.Input.mousePosition);
            if(hit.collider != null)
            {
                float value = Mathf.InverseLerp(-RacketMotor.Settings.maxPositionX,
                                                RacketMotor.Settings.maxPositionX,
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