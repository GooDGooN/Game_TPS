using System;
using System.Collections;
using System.Linq;
using Unity.Entities.UniversalDelegates;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CharacterNamespace
{
    public class PlayerControl : CharacterProperty
    {
        #region RIFLE

        public PlayerRifleControl PlayerRifle { get => playerRifle; }
        [SerializeField] private PlayerRifleControl playerRifle;

        public GameObject HitScanBullet { get => hitScanBullet; }
        [SerializeField] private GameObject hitScanBullet;

        public Vector3 BulletHitPoint { get => bulletHitPoint; }
        private Vector3 bulletHitPoint;

        public float FireDelay = 0.0f;

        public float FireRate { get => fireRate; }
        private float fireRate = 0.4f;
        #endregion

        #region SPINE_FIELD
        [SerializeField] private GameObject playerHead;
        [SerializeField] private GameObject playerSpine;
        private Vector3 sightHitPoint;
        private Vector3 spineRotationSave;
        #endregion

        #region BODY_FIELD
        public GameObject PlayerBody { get => playerBody; }
        [SerializeField] private GameObject playerBody;

        private float animBlendPosX = 0.0f;
        private float animBlendPosXVelocity = 0.0f;
        private float animBlendPosY = 0.0f;
        private float animBlendPosYVelocity = 0.0f;
        private float animBlendPosSmoothTime = 0.1f;
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

        public Vector3 BottomCastBox { get => bottomCastBox; }
        private Vector3 bottomCastBox = new Vector3(0.2f, 0.01f, 0.2f);

        public float MoveDirDampSmooth { get => moveDirDampSmooth; set => moveDirDampSmooth = value; }
        private float moveDirDampSmooth = 0.1f;

        public float DefaultMoveSpeed { get => defaultMoveSpeed; }
        private float defaultMoveSpeed;

        #endregion

        #region STATE_FIELD
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

        #region CAMERA_FIELD
        [SerializeField] private PlayerCameraControl playerCameraControlScr;
        [SerializeField] private GameObject dummyCameraObj;
        [SerializeField] private GameObject cameraObj;
        private Vector3 cameraLookForward;

        #endregion

        #region STATUS_FIELD
        public bool IsStaminaRecharge { get => isStaminaRecharge; }
        private bool isStaminaRecharge = false;
        public float MaxStamina { get => maxStamina; }
        private float maxStamina;
        public float Stamina { get => stamina; }
        private float stamina;

        public float StaminaMultiply = 1.0f;
        #endregion

        private void Start()
        {
            stateController = new CharacterStateController(this);
            hitScanBullet = Instantiate(hitScanBullet, transform);
            stateController.ChangeState(CharacterState.Idle);
            stateController.ChangeState(CharacterUpperState.Normal);
            defaultMoveSpeed = moveSpeed = 200.0f;
            maxHealth = health = 100;
            maxStamina = stamina = 1.0f;
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
                Physics.SphereCast(myRigidbody.position, capsuleColliderRadius, Vector3.down, out var playerSphere, float.PositiveInfinity, GlobalVarStorage.SolidLayer);
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0.0f, myRigidbody.velocity.z);
                if (playerSphere.normal.y >= 0.8f)
                {
                    var pos = new Vector3(myRigidbody.position.x, playerSphere.point.y + capsuleColliderHeight, myRigidbody.position.z);
                    myRigidbody.MovePosition(pos);
                }
            }
            if (MyState != CharacterState.Dash)
            {
                TempMoveDirection = Vector3.zero;
                BlendPos = Vector2.zero;
                if (RightPressing ^ LeftPressing)
                {
                    var temp = PlayerBody.transform.right;
                    BlendPos += Vector2.right;
                    if (LeftPressing)
                    {
                        BlendPos += Vector2.left * 2;
                        temp = -temp;
                    }
                    TempMoveDirection += temp;
                }
                if (ForwardPressing ^ BackPressing)
                {
                    var temp = PlayerBody.transform.forward;
                    BlendPos += Vector2.up;
                    if (BackPressing)
                    {
                        BlendPos += Vector2.down * 2;
                        temp = -temp;
                    }
                    TempMoveDirection += temp;
                }
            }

            var dir = moveDirection * moveSpeed * Time.deltaTime;
            myRigidbody.velocity = new Vector3(dir.x, myRigidbody.velocity.y, dir.z);
        }

        private void GeneralUpdate()
        {

            #region CHECKKEYPRESS
            rightPressing = GameSystem.GetKey(KeyInputs.MoveRight);
            leftPressing = GameSystem.GetKey(KeyInputs.MoveLeft);
            forwardPressing = GameSystem.GetKey(KeyInputs.MoveFoward);
            backPressing = GameSystem.GetKey(KeyInputs.MoveBack);
            dashPressing = GameSystem.GetKey(KeyInputs.Dash);
            firePressing = GameSystem.GetKey(KeyInputs.Fire);
            zoomInPressing = GameSystem.GetKey(KeyInputs.ZoomIn);
            reloadPressed = GameSystem.GetKeyPressed(KeyInputs.Reload);
            jumpPressed = GameSystem.GetKeyPressed(KeyInputs.Jump);
            #endregion

            #region SMOOTHDAMP
            if (tempMoveDirection != Vector3.zero)
            {
                tempMoveDirection.Normalize();
                moveDirection = Vector3.SmoothDamp(moveDirection, tempMoveDirection, ref moveDirDampVelocity, moveDirDampSmooth);
                animBlendPosX = Mathf.SmoothDamp(animBlendPosX, blendPos.x, ref animBlendPosXVelocity, animBlendPosSmoothTime);
                animBlendPosY = Mathf.SmoothDamp(animBlendPosY, blendPos.y, ref animBlendPosYVelocity, animBlendPosSmoothTime);
            }
            else
            {
                if(myRigidbody.velocity.y == 0)
                {
                    moveDirection = Vector3.SmoothDamp(moveDirection, Vector3.zero, ref moveDirDampVelocity, moveDirDampSmooth * 0.5f);
                }
                animBlendPosX = Mathf.SmoothDamp(animBlendPosX, 0.0f, ref animBlendPosXVelocity, GameSystem.Instance.PlayerRotDampSmoothTimeValue);
                animBlendPosY = Mathf.SmoothDamp(animBlendPosY, 0.0f, ref animBlendPosYVelocity, GameSystem.Instance.PlayerRotDampSmoothTimeValue);
                animBlendPosX = Mathf.Approximately(animBlendPosX, 0.0f) ? 0.0f : animBlendPosX;
                animBlendPosY = Mathf.Approximately(animBlendPosY, 0.0f) ? 0.0f : animBlendPosY;
            }
            myAnimator.SetFloat("MotionX", animBlendPosX);
            myAnimator.SetFloat("MotionY", animBlendPosY);
            #endregion

            FireDelay = FireDelay > 0.0f ? FireDelay - Time.deltaTime : 0.0f;

            if (Input.GetKey(KeyCode.LeftBracket))
            {
                health -= 1;
            }
            if (Input.GetKey(KeyCode.RightBracket))
            {
                health += 1;
            }
            health = Mathf.Clamp(health, 0, maxHealth);
            var staminaChangeValue = myState != CharacterState.MidAir ? Time.deltaTime * 0.25f : 0.0f;
            stamina += myState == CharacterState.Dash ? -staminaChangeValue : staminaChangeValue * StaminaMultiply;
            if (stamina <= 0.0f)
            {
                isStaminaRecharge = true;
            }
            if (stamina >= 1.0f)
            {
                isStaminaRecharge = false;
            }
            stamina = Mathf.Clamp(stamina, 0.0f, maxStamina);

            //Debug.Log(stateController.LastState);
            // Debug.Log(MyState);
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
            var camTransform = dummyCameraObj.transform;
            cameraLookForward = dummyCameraObj.transform.forward;
            playerBody.transform.eulerAngles = new Vector3(0.0f, playerCameraControlScr.RotationYSave.y, 0.0f);
            if (myState != CharacterState.Dash)
            {
                playerSpine.transform.eulerAngles = Vector3.zero;
            }
            if(!Input.GetMouseButton(2))
            {
                sightHitPoint = cameraLookForward * 10.0f + camTransform.position;
            }
            spineRotationSave = Quaternion.LookRotation(sightHitPoint - playerSpine.transform.position, playerSpine.transform.up).eulerAngles;
            var spineAxis = Vector3.Cross(playerSpine.transform.up, playerBody.transform.forward);
            if (myState != CharacterState.Dash)
            {
                playerSpine.transform.eulerAngles = Vector3.up * (spineRotationSave.y + 47.5f);
                playerSpine.transform.Rotate(spineAxis, spineRotationSave.x, Space.World);
            }
        }

        private void BulletHitPointUpdate()
        {
            var rayHits = Physics.RaycastAll(cameraObj.transform.position, cameraObj.transform.forward, float.PositiveInfinity);
            RaycastHit seletedHit = new RaycastHit();
            if (rayHits.Length > 0)
            {
                seletedHit = rayHits[0];
                foreach (var hit in rayHits)
                {
                    if (seletedHit.distance > hit.distance && !hit.collider.isTrigger)
                    {
                        seletedHit = hit;
                    }
                }
            }
            var camLongestPoint = cameraObj.transform.position + (cameraObj.transform.forward * 100.0f);
            bulletHitPoint = seletedHit.collider != null ? seletedHit.point : camLongestPoint;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bulletHitPoint, Vector3.one * 0.5f);
            Gizmos.DrawSphere(sightHitPoint, 0.25f);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(playerSpine.transform.position, (playerBody.transform.forward * 100.0f) + playerSpine.transform.position);
            var spine = myAnimator.GetBoneTransform(HumanBodyBones.Spine);
            Gizmos.DrawLine(spine.position, spine.forward * 100.0f);
        }
    }
}