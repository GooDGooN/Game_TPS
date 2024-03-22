
using System.Linq.Expressions;
using UnityEngine;
namespace CharacterNamespace
{
    public class PlayerCameraControl : MonoBehaviour
    {
        [SerializeField] private GameObject springArmXObj;
        [SerializeField] private GameObject springArmYObj;
        [SerializeField] private GameObject dummyCameraObj;
        [SerializeField] private GameObject playerHeadObj;
        [SerializeField] private LayerMask solidMask;
        private GameObject conversationObj;

        private GameSystem gs;

        private float camDistance = -4.0f;
        public float CamDeltadistValue { get => camDeltadistValue; set => camDeltadistValue = value; }
        private float camDeltadistValue = 0.0f;
        private float camDeltadistSave = 0.0f;
        private Vector2 camAxisXRange = new Vector2(-55.0f, 55.0f);
        private Vector2 camFreeViewXRange = new Vector2(-15.0f, 180.0f);
        private Vector2 camZoomRange = new Vector2(-1.5f, -3.0f);
        private Vector3 rotationXSave = Vector3.zero;
        private Vector3 rotationYSave = Vector3.zero;
        public Vector3 RotationYSave { get => rotationYSave; }


        private bool isFocus = false;

        private void Start()
        {
        }

        private void FixedUpdate() 
        {
            if (!Input.GetKey(KeyCode.LeftAlt) && Application.isFocused && !isFocus)
            {
                var sensitive = ((GameSystem.MouseSensitive + 1.0f) * 4.0f);
                springArmXObj.transform.Rotate(-Input.GetAxis("Mouse Y") * sensitive, 0.0f, 0.0f);
                springArmYObj.transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X") * sensitive, 0.0f));
            }
        }

        private void Update()
        {
            var conversationTarget = UIDialogControl.Instance.CurrentConversationTarget;
            if (conversationTarget != null)
            {
                conversationObj = UIDialogControl.Instance.CurrentConversationTarget.gameObject;
            }
            else
            {
                conversationObj = null;
            }
            isFocus = conversationObj != null;
            if (isFocus) 
            {
                var playerTransform = PlayerControl.Instance.transform;
                var targetPos = new Vector3(playerTransform.position.x, conversationObj.transform.position.y, playerTransform.position.z);
                var lookNormal = (conversationObj.transform.position - targetPos).normalized;
                var targetCapsuleHeight = conversationTarget.MyCapsuleCollider.height;

                dummyCameraObj.transform.position = (conversationObj.transform.position - (lookNormal * 3.5f)) +  Vector3.up * (targetCapsuleHeight * 0.75f); // + Vector3.up * (targetCapsuleHeight + (heightDelta / 0.5f));
                dummyCameraObj.transform.LookAt(conversationObj.transform.position + Vector3.up * (targetCapsuleHeight * 0.75f));
            }
            else
            {
                dummyCameraObj.transform.localRotation = Quaternion.identity;
                dummyCameraObj.transform.localPosition = Vector3.zero;
                CameraViewControl();
            }
        }

        private void CameraViewControl()
        {
            var saXTransform = springArmXObj.transform;
            var saYTransform = springArmYObj.transform;
            transform.position = playerHeadObj.transform.position;

            #region CAMERA ROTATION
            if (!Input.GetKey(KeyCode.LeftAlt) && Application.isFocused)
            {
                //Cursor.visible = false;
                var xValue = 0.0f;
                if (Input.GetMouseButtonUp(2))
                {
                    saXTransform.localRotation = Quaternion.Euler(rotationXSave);
                    saYTransform.localRotation = Quaternion.Euler(rotationYSave);
                    camDeltadistValue = camDeltadistSave;
                }
                if (Input.GetMouseButton(2))
                {
                    xValue = CameraRotateLimitSet(camFreeViewXRange, saXTransform.transform.localEulerAngles.x);
                }
                else
                {
                    xValue = CameraRotateLimitSet(camAxisXRange, saXTransform.transform.localEulerAngles.x);
                    rotationXSave = saXTransform.localEulerAngles;
                    //rotationYSave = saYTransform.localEulerAngles;
                    rotationYSave = saYTransform.localEulerAngles;
                    camDeltadistSave = camDeltadistValue;
                }
                saXTransform.transform.localRotation = Quaternion.Euler(xValue, 0.0f, 0.0f);
            }
            else
            {
                Cursor.visible = true;
            }
            #endregion
            #region CAMERA ZOOMINOUT

            var player = PlayerControl.Instance;
            if (player.MyState != CharacterState.Dash && player.MyUpperState != CharacterUpperState.Reloading)
            {
                CamDeltadistValue = player.ZoomInPressing ? 1.5f : 0.0f;
            }
            else
            {
                CamDeltadistValue = 0.0f;
            }
            camDistance = camZoomRange.y + camDeltadistValue;
            if (Physics.Raycast(transform.position, -dummyCameraObj.transform.forward, out RaycastHit rayhit, -camDistance, solidMask))
            {
                camDistance = -rayhit.distance < camZoomRange.x ? -rayhit.distance + camDeltadistValue : camZoomRange.x;
            }
            dummyCameraObj.transform.localPosition -= player.MyState == CharacterState.Dash ? Vector3.forward * 1.5f : Vector3.zero;
            dummyCameraObj.transform.localPosition = new Vector3(0.60f, 0.15f, camDistance);
            #endregion
        }

        private float CameraRotateLimitSet(Vector2 range, float xValue)
        {
            if (xValue > range.y && xValue < 270.0f)
            {
                xValue = range.y;
            }
            else if (xValue < 360.0f + range.x && xValue > 180.0f)
            {
                xValue = 360.0f + range.x;
            }
            return xValue;
        }
    }
}