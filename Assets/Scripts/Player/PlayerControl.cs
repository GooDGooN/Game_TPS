using System;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static GlobalEnums;

namespace CharacterNamespace
{
    public class PlayerControl : CharacterProperty
    {
        #region SPINE_FIELD
        [SerializeField] private GameObject playerHead;
        [SerializeField] private GameObject playerSpine;
        [SerializeField] private GameObject cameraObj;
        [SerializeField] private GameObject rifleObj;
        private RaycastHit[] cameraCastHits = new RaycastHit[5];
        private Vector3 cameraLookForward;
        private Vector3 sightHitPoint;
        private Vector3 spineRotationSave;
        private float camRayDistanceSave = 100.0f;
        #endregion

        #region BODY_FIELD
        public GameObject PlayerBody { get => playerBody; }
        [SerializeField] private GameObject playerBody;

        private CapsuleCollider myCollider;

        public float ColliderHeight { get => colliderHeight; }
        private float colliderHeight;

        private float colliderRadius;
        [SerializeField] private PlayerCameraControl playerCameraControlScr;
        private Vector3 targetBodyAngle;
        #endregion

        #region MOVING_FIELD
        public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }
        private Vector3 moveDirection = Vector3.zero;

        public Vector3 TempMoveDirection { get => tempMoveDirection; set => tempMoveDirection = value; }
        private Vector3 tempMoveDirection = Vector3.zero;

        public Vector3 MoveDirDampVelocity { get => moveDirDampVelocity; set => moveDirDampVelocity = value; }
        private Vector3 moveDirDampVelocity = Vector3.zero;

        public Vector2 BlendPos { get => blendPos; set => blendPos = value; }
        private Vector2 blendPos = Vector2.zero;

        public float MoveDirDampSmooth { get => moveDirDampSmooth; }
        private float moveDirDampSmooth = 0.1f;

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
        private float moveSpeed = 200.0f;

        public float JumpPower { get => jumpPower; }
        protected float jumpPower = 40.0f;
        #endregion

        #region STATE_FIELD
        public CharacterState MyState { get => myState; set => myState = value; }
        private CharacterState myState = CharacterState.Idle;

        public CharacterUpperState MyUpperState { get => myUpperState; set => myUpperState = value; }
        private CharacterUpperState myUpperState = CharacterUpperState.Normal;

        private PlayerStateController stateController;
        public bool CheckFlying { get => checkFlying; }
        private bool checkFlying;

        public bool CheckJump { get => checkJump; set => checkJump = value; }
        private bool checkJump;
        #endregion

        #region INPUTKEY_FIELD
        public bool RightPressing { get => rightPressing; }
        private bool rightPressing = false;
        public bool LeftPressing { get => leftPressing; }
        private bool leftPressing = false;
        public bool ForwardPressing { get => forwardPressing; }
        private bool forwardPressing = false;
        public bool BackPressing { get => backPressing; }
        private bool backPressing = false;
        public bool DashPressing { get => dashPressing; }
        private bool dashPressing = false;
        public bool FirePressing { get => firePressing; }
        private bool firePressing = false;
        public bool ZoomInPressing { get => zoomInPressing; }
        private bool zoomInPressing = false;
        public bool JumpPressed { get => jumpPressed; }
        private bool jumpPressed = false;
        public bool ReloadPressed { get => reloadPressed; }
        private bool reloadPressed = false;


        #endregion

        #region CHARCTER_FIELD
        public float FireRate { get => fireRate; }
        private float fireRate = 3.0f;

        public Vector3 BulletHitPoint { get => bulletHitPoint; }
        private Vector3 bulletHitPoint;

        public LayerMask SolidLayer { get => solidLayer; }
        [SerializeField] private LayerMask solidLayer;

        public GameObject RifleMuzzle { get => rifleMuzzle; }
        [SerializeField] private GameObject rifleMuzzle;
        #endregion

        private void Awake()
        {
            myCollider = GetComponent<CapsuleCollider>();
            myRigidbody = GetComponent<Rigidbody>();
            stateController = new PlayerStateController(this);
            stateController.ChangeState(myState);
            stateController.ChangeUpperState(myUpperState);
            colliderHeight = myCollider.height * 0.5f;
            colliderRadius = myCollider.radius;
        }

        private void FixedUpdate()
        {
            stateController.CurrentState.StateFixedUpdate();
            stateController.CurrentUpperState.StateFixedUpdate();
            GeneralFixedUpdate();
        }

        private void Update()
        {
            TestInput();
            stateController.CurrentState.StateUpdate();
            stateController.CurrentUpperState.StateUpdate();
            GeneralUpdate();
        }

        private void LateUpdate()
        {
            GeneralLateUpdate();
        }

        private void GeneralFixedUpdate()
        {
            if(myState != CharacterState.MidAir)
            {
                Physics.SphereCast(myRigidbody.position, colliderRadius, Vector3.down, out var playerSphere, float.PositiveInfinity, solidLayer);
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0.0f, myRigidbody.velocity.z);
                if (playerSphere.normal.y >= 0.8f)
                {
                    var pos = new Vector3(myRigidbody.position.x, playerSphere.point.y + colliderHeight, myRigidbody.position.z);
                    myRigidbody.MovePosition(pos);
                }
            }
            var dir = moveDirection * moveSpeed * Time.deltaTime;
            myRigidbody.velocity = new Vector3(dir.x, myRigidbody.velocity.y, dir.z);
        }

        private void GeneralUpdate()
        {
            rightPressing = GameSystem.Instance.GetKey(KeyInputs.MoveRight);
            leftPressing = GameSystem.Instance.GetKey(KeyInputs.MoveLeft);
            forwardPressing = GameSystem.Instance.GetKey(KeyInputs.MoveFoward);
            backPressing = GameSystem.Instance.GetKey(KeyInputs.MoveBack);
            dashPressing = GameSystem.Instance.GetKey(KeyInputs.Dash);
            firePressing = GameSystem.Instance.GetKey(KeyInputs.Fire);
            zoomInPressing = GameSystem.Instance.GetKey(KeyInputs.ZoomIn);
            reloadPressed = GameSystem.Instance.GetKeyPressed(KeyInputs.Reload);
            jumpPressed = GameSystem.Instance.GetKeyPressed(KeyInputs.Jump);

            if (tempMoveDirection != Vector3.zero)
            {
                tempMoveDirection.Normalize();
                moveDirection = Vector3.SmoothDamp(moveDirection, tempMoveDirection, ref moveDirDampVelocity, moveDirDampSmooth);
                BlendPosX = Mathf.SmoothDamp(BlendPosX, blendPos.x, ref BlendPosXVelocity, BlendPosSmoothTime);
                BlendPosY = Mathf.SmoothDamp(BlendPosY, blendPos.y, ref BlendPosYVelocity, BlendPosSmoothTime);
            }
            else
            {
                if(myRigidbody.velocity.y == 0)
                {
                    moveDirection = Vector3.SmoothDamp(moveDirection, Vector3.zero, ref moveDirDampVelocity, moveDirDampSmooth * 0.5f);
                }
                BlendPosX = Mathf.SmoothDamp(BlendPosX, 0.0f, ref BlendPosXVelocity, GameSystem.Instance.PlayerRotDampSmoothTimeValue);
                BlendPosY = Mathf.SmoothDamp(BlendPosY, 0.0f, ref BlendPosYVelocity, GameSystem.Instance.PlayerRotDampSmoothTimeValue);
                BlendPosX = Mathf.Approximately(BlendPosX, 0.0f) ? 0.0f : BlendPosX;
                BlendPosY = Mathf.Approximately(BlendPosY, 0.0f) ? 0.0f : BlendPosY;
            }
            myanimator.SetFloat("MotionX", BlendPosX);
            myanimator.SetFloat("MotionY", BlendPosY);


            if(myState != CharacterState.Dash && myUpperState != CharacterUpperState.Reloading)
            {
                playerCameraControlScr.CamDeltadistValue = zoomInPressing ? 2.0f : 0.0f;
            }
            else
            {
                playerCameraControlScr.CamDeltadistValue = 0.0f;
            }

            //Debug.Log(MyState);
            //Debug.Log(MyUpperState);
        }

        private void GeneralLateUpdate()
        {
            CharacterAngleUpdate();
            BulletHitPointUpdate();
        }

        private void TestInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(0);
            }

        }

        private void CharacterAngleUpdate()
        {
            cameraLookForward = cameraObj.transform.forward;
            playerBody.transform.eulerAngles = new Vector3(0.0f, playerCameraControlScr.RotationYSave.y, 0.0f);

            var camTransform = cameraObj.transform;
            var raycount = Physics.RaycastNonAlloc(camTransform.position, cameraLookForward, cameraCastHits, 100.0f);
            if (!Input.GetMouseButton(2))
            {
                if (raycount > 0)
                {
                    for (int i = raycount; i >= 1; i--)
                    {
                        var angle = Vector3.SignedAngle(playerBody.transform.forward, (cameraCastHits[i - 1].point - playerBody.transform.position).normalized, playerBody.transform.right);
                        var isAtForward = Vector3.Dot(playerBody.transform.forward, (cameraCastHits[i - 1].point - playerBody.transform.position).normalized) > 0.0f;
                        if (angle > -20.0f && isAtForward)
                        {
                            camRayDistanceSave = cameraCastHits[i - 1].distance - cameraObj.transform.localPosition.z;
                            sightHitPoint = cameraCastHits[i - 1].point;
                            break;
                        }
                        if (i == raycount)
                        {
                            sightHitPoint = cameraLookForward * camRayDistanceSave + camTransform.position;
                        }
                    }
                }
                else
                {
                    sightHitPoint = cameraLookForward * camRayDistanceSave + camTransform.position;
                }
                spineRotationSave = Quaternion.LookRotation(sightHitPoint - playerSpine.transform.position).eulerAngles;
            }
            var spineAxis = Vector3.Cross(playerSpine.transform.up, playerBody.transform.forward);
            if(myState != CharacterState.Dash)
            {
                playerSpine.transform.eulerAngles = Vector3.zero;
                playerSpine.transform.eulerAngles = (Vector3.up * spineRotationSave.y) + (Vector3.up * 47.5f);
                playerSpine.transform.Rotate(spineAxis, spineRotationSave.x, Space.World);
            }
        }

        private void BulletHitPointUpdate()
        {
            Physics.Raycast(cameraObj.transform.position, cameraObj.transform.forward, out var bulletHit, float.PositiveInfinity);
            var camLongestPoint = cameraObj.transform.position + (cameraObj.transform.forward * 100.0f);
            bulletHit.point = bulletHit.point == Vector3.zero ? camLongestPoint : bulletHit.point;
            /*Physics.Raycast(cameraObj.transform.position, cameraObj.transform.forward, out var cameraHit, float.PositiveInfinity);
            var camLongestPoint = cameraObj.transform.position + (cameraObj.transform.forward * 100.0f);
            cameraHit.point = cameraHit.point == Vector3.zero ? camLongestPoint : cameraHit.point;

            Physics.Raycast(rifleMuzzle.transform.position, (cameraHit.point - rifleMuzzle.transform.position).normalized, out var bulletHit, float.PositiveInfinity);
            var bulletLongestPoint = rifleMuzzle.transform.position + ((cameraHit.point - rifleMuzzle.transform.position).normalized * 100.0f);
            bulletHit.point = bulletHit.point == Vector3.zero ? bulletLongestPoint : bulletHit.point;*/

            bulletHitPoint = bulletHit.point;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(sightHitPoint, 0.25f);
            Gizmos.DrawLine(rifleMuzzle.transform.position, sightHitPoint);
            Gizmos.DrawLine(playerSpine.transform.position, sightHitPoint);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(rifleMuzzle.transform.position, (rifleMuzzle.transform.forward * 100.0f)+ rifleMuzzle.transform.position);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerSpine.transform.position, (playerBody.transform.forward * 100.0f) + playerSpine.transform.position);
            var spine = myanimator.GetBoneTransform(HumanBodyBones.Spine);
            Gizmos.DrawLine(spine.position, spine.forward * 100.0f);
        }
    }
}