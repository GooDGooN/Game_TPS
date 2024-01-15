
using UnityEngine;
namespace CharacterNamespace
{
    public class PlayerCameraControl : MonoBehaviour
    {
        [SerializeField] private GameObject springArmXObj;
        [SerializeField] private GameObject springArmYObj;
        [SerializeField] private GameObject cameraObj;
        [SerializeField] private GameObject playerHeadObj;
        [SerializeField] private LayerMask solidMask; 

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

        private void Start()
        {
            gs = GameSystem.Instance;
        }

        private void FixedUpdate() 
        {
            if (!Input.GetKey(KeyCode.LeftAlt) && Application.isFocused)
            {
                springArmXObj.transform.Rotate(-Input.GetAxis("Mouse Y") * gs.MouseSensitive, 0.0f, 0.0f);
                springArmYObj.transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X") * gs.MouseSensitive, 0.0f));
            }
        }

        private void Update()
        {
            CameraViewControl();
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
            /*camDeltadistValue += Input.mouseScrollDelta.y * 20.0f * Time.deltaTime;
            camDeltadistValue = Mathf.Clamp(camDeltadistValue, 0.0f, 2.0f);*/
            var player = GlobalVarStorage.Instance.PlayerScript;
            if (player.MyState != CharacterState.Dash && player.MyUpperState != CharacterUpperState.Reloading)
            {
                CamDeltadistValue = player.ZoomInPressing ? 1.5f : 0.0f;
            }
            else
            {
                CamDeltadistValue = 0.0f;
            }
            camDistance = camZoomRange.y + camDeltadistValue;
            if (Physics.Raycast(transform.position, -cameraObj.transform.forward, out RaycastHit rayhit, -camDistance, solidMask))
            {
                camDistance = -rayhit.distance < camZoomRange.x ? -rayhit.distance + camDeltadistValue : camZoomRange.x;
            }
            cameraObj.transform.localPosition = new Vector3(0.40f, 0.15f, camDistance);
            cameraObj.transform.localPosition -= GlobalVarStorage.Instance.PlayerScript.MyState == CharacterState.Dash ? Vector3.forward * 1.5f : Vector3.zero;
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