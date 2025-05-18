using UnityEngine;

namespace Platformer.Mechanics
{
    public class NPCController : KinematicObject
    {
        /// <summary> ���H���ؼ� �p�G�ONull�h�N���S�ؼ� </summary>
        public PlayerController Player;
        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("NPCController is missing Animator component on " + gameObject.name, this);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            bool IsMove = false;
            if (Player != null)
            {
                int Index = Player.NPCs.IndexOf(this) + 1;
                float D = Vector3.Distance(transform.position, Player.transform.position);
                //�S���Ӫ�
                if (D > 0.5f * Index)
                {
                    float XD = Mathf.Abs(transform.position.x - Player.transform.position.x);
                    if (XD > 0.5f * Index)
                    {
                        //�ӻ��F �åB���a���a
                        if (D > 6f && Player.IsGrounded)
                        {
                            //����
                            transform.position = Player.transform.position;
                        }
                        else
                        {
                            //print(D);
                            //�����٬O�k��
                            if (transform.position.x > Player.transform.position.x)
                            {
                                targetVelocity.x = -2.8f;
                            }
                            else
                            {
                                targetVelocity.x = 2.8f;
                            }
                            //print(targetVelocity.x);
                            IsMove = true;
                        }
                    }
                }
            }
            if (!IsMove)
            {
                targetVelocity = Vector2.zero;
            }

            // Update Animator parameter
            if (animator != null)
            {
                animator.SetFloat("velocityX", Mathf.Abs(velocity.x));
            }
            //base.Update();
        }
    }
}