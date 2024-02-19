using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace CharacterNamespace
{
    public class PlayerControl : CharacterProperty
    {
        public static PlayerControl Instance { get; private set; }

        #region RIFLE
        public PlayerRifleControl PlayerRifle { get => playerRifle; }
        [SerializeField] private PlayerRifleControl playerRifle;

        public Vector3 BulletHitPoint { get => bulletHitPoint; }
        private Vector3 bulletHitPoint;

        private Vector3 bulletHitPointDelta;

        public float AttackDelay = 0.0f;

        /*public float FireRate { get => fireRate; }
        private float fireRate = 0.4f;*/
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

        public float ColliderDelta { get => colliderDelta; }
        private float colliderDelta = 0.05f;

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

        public bool IsMovePressed { get => isMovePressed; }
        private bool isMovePressed = false;

        public bool IsFreeViewClick { get => isFreeViewClick; }
        private bool isFreeViewClick = false;
        public bool IsFreeViewClicked { get => isFreeViewClicked; }
        private bool isFreeViewClicked = false;


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

        public float StaminaRechargeMultiplier = 1.0f;

        public bool ReloadComplete { get => reloadComplete; set => reloadComplete = value; }
        private bool reloadComplete = false;

        public float ReloadMotionSpeedMultiplier { get => reloadMotionSpeedMultiplier; }
        private float reloadMotionSpeedMultiplier = 1.0f;

        public bool IsFocus { get => isFocus; }
        private bool isFocus = false;

        public float MoveSpeedMutiplier { get => moveSpeedMutiplier; }
        private float moveSpeedMutiplier = 1.0f;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            var insts = FindObjectsOfType<PlayerControl>();
            if (insts.Length > 1)
            {
                foreach (var inst in insts)
                {
                    if (inst != Instance)
                    {
                        Destroy(inst);
                    }
                }
            }
        }

        private void Start()
        {
            stateController.ChangeState(CharacterState.Idle);
            stateController.ChangeState(CharacterUpperState.Normal);

            //testset
            atkDamage = 3;
            defaultMoveSpeed = moveSpeed = 200.0f * moveSpeedMutiplier;
            maxHealth = health = 100;
            maxStamina = stamina = 1.0f;
            atkSpeed = 0.4f;
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
            if (myState != CharacterState.MidAir)
            {
                Physics.SphereCast(myRigidbody.position, capsuleColliderRadius, Vector3.down, out var playerSphere, float.PositiveInfinity, Constants.SolidLayer);
                myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0.0f, myRigidbody.velocity.z);
                if (playerSphere.normal.y >= 0.6f)
                {
                    var pos = new Vector3(myRigidbody.position.x, playerSphere.point.y + capsuleColliderHeight, myRigidbody.position.z);
                    myRigidbody.MovePosition(pos);
                }
            }
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

            var dir = moveDirection * moveSpeed * Time.deltaTime;
            myRigidbody.velocity = new Vector3(dir.x, myRigidbody.velocity.y, dir.z);
        }

        private void GeneralUpdate()
        {
            #region CHECKKEYPRESS
            isFocus = UIConversationControl.Instance.InConversation;
            rightPressing = isFocus ? false : GameSystem.GetKey(KeyInputs.MoveRight);
            leftPressing = isFocus ? false : GameSystem.GetKey(KeyInputs.MoveLeft);
            forwardPressing = isFocus ? false : GameSystem.GetKey(KeyInputs.MoveFoward);
            backPressing = isFocus ? false : GameSystem.GetKey(KeyInputs.MoveBack);
            dashPressing = isFocus ? false : GameSystem.GetKey(KeyInputs.Dash);
            firePressing = isFocus ? false : GameSystem.GetKey(KeyInputs.Fire);
            zoomInPressing = isFocus ? false : GameSystem.GetKey(KeyInputs.ZoomIn);
            reloadPressed = isFocus ? false : GameSystem.GetKeyPressed(KeyInputs.Reload);
            jumpPressed = isFocus ? false : GameSystem.GetKeyPressed(KeyInputs.Jump);
            isMovePressed = rightPressing || leftPressing || forwardPressing || backPressing;
            isFreeViewClick = isFocus ? false : GameSystem.GetKey(KeyInputs.FreeView);
            isFreeViewClicked = isFocus ? false : GameSystem.GetKeyPressed(KeyInputs.FreeView);

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

            AttackDelay = AttackDelay > 0.0f ? AttackDelay - Time.deltaTime : 0.0f;

            #region STAMINA
            var staminaChangeValue = myState != CharacterState.MidAir ? Time.deltaTime * 0.25f : 0.0f;
            if (stamina <= 0.0f)
            {
                isStaminaRecharge = true;
            }
            if (stamina >= 1.0f)
            {
                isStaminaRecharge = false;
            }
            stamina += myState == CharacterState.Dash ? -staminaChangeValue : staminaChangeValue * StaminaRechargeMultiplier;
            if (jumpPressed && !IsStaminaRecharge && myState != CharacterState.MidAir)
            {
                stamina -= 1.0f / 5.0f;
            }
            stamina = Mathf.Clamp(stamina, 0.0f, maxStamina);
            #endregion
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

            #region HEALTHTEST
            if (Input.GetKey(KeyCode.LeftBracket))
            {
                health -= 1;
            }
            if (Input.GetKey(KeyCode.RightBracket))
            {
                health += 1;
            }
            health = Mathf.Clamp(health, 0, maxHealth);
            #endregion
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
            if(!isFreeViewClick && !isFocus)
            {
                sightHitPoint = cameraLookForward * 10.0f + camTransform.position;
                spineRotationSave = Quaternion.LookRotation(sightHitPoint - playerSpine.transform.position, playerSpine.transform.up).eulerAngles;
            }
            var spineAxis = Vector3.Cross(playerSpine.transform.up, playerBody.transform.forward);
            if (myState != CharacterState.Dash)
            {
                playerSpine.transform.eulerAngles = Vector3.up * (spineRotationSave.y + 52.5f);
                playerSpine.transform.Rotate(spineAxis, spineRotationSave.x, Space.World);
            }
        }

        private void BulletHitPointUpdate()
        {
            if (isFreeViewClick)
            {
                if (isFreeViewClicked)
                {
                    bulletHitPointDelta = transform.position;
                }
                bulletHitPoint += transform.position - bulletHitPointDelta;
                bulletHitPointDelta = transform.position;
            }
            else
            {
                var rayHits = Physics.RaycastAll(cameraObj.transform.position + cameraObj.transform.forward, cameraObj.transform.forward, float.PositiveInfinity);
                RaycastHit selectedHit = new RaycastHit();
                if (rayHits.Length > 0)
                {
                    selectedHit = rayHits[0];
                    foreach (var hit in rayHits)
                    {
                        if (selectedHit.distance > hit.distance && !hit.collider.isTrigger)
                        {
                            selectedHit = hit;
                        }
                    }
                }
                var camLongestPoint = cameraObj.transform.position + (cameraObj.transform.forward * 100.0f);
                bulletHitPoint = selectedHit.collider != null ? selectedHit.point : camLongestPoint;
            }
        }

        public override void GetBuff(ItemType type)
        {
            switch (type)
            {
                case ItemType.Damage:
                    atkDamage += Random.Range(3, 7);
                    break;
                case ItemType.AttackSpeed:
                    atkSpeed -= 0.03f;
                    Mathf.Clamp(atkSpeed, 0.1f, 0.4f);
                    break;
                case ItemType.Health:
                    var value = Random.Range(10, 20);
                    maxHealth += value;
                    health += value;
                    break;
                case ItemType.Heal:
                    health += Mathf.CeilToInt(health * 0.15f);
                    Mathf.Clamp(health, 0, maxHealth);
                    break;
                case ItemType.Magazine:
                    playerRifle.MaxImumMagazineCapacity += 3;
                    Mathf.Clamp(playerRifle.MaxImumMagazineCapacity, 20, 50);
                    break;
                case ItemType.Reload:
                    reloadMotionSpeedMultiplier += 0.1f;
                    myAnimator.SetFloat("ReloadMotionSpeedMultiplier", reloadMotionSpeedMultiplier);
                    Mathf.Clamp(reloadMotionSpeedMultiplier, 1.0f, 2.0f);
                    break;
                case ItemType.Stamina:
                    StaminaRechargeMultiplier += 0.1f;
                    Mathf.Clamp(StaminaRechargeMultiplier, 1.0f, 2.0f);
                    break;
                case ItemType.MoveSpeed:
                    moveSpeedMutiplier += 0.1f;
                    defaultMoveSpeed = moveSpeed = 200.0f * moveSpeedMutiplier;
                    Mathf.Clamp(moveSpeedMutiplier, 1.0f, 2.0f);
                    break;
            }
        }
    }
}