using Spine;
using Spine.Unity;
using TestTask.Enemy;
using TestTask.UI;
using UnityEngine;


namespace TestTask.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed = 2f;
        [SerializeField] ParticleSystem fire;
        Rigidbody2D rb;
        SkeletonAnimation skeletonAnimation;
        public Spine.AnimationState spineAnimationState;
        float startSpeed;
        EnemyController currentEnemy;



        private void Start()
        {
            rb = GetComponentInChildren<Rigidbody2D>();
            skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            spineAnimationState.Start += AnimationStart;
            spineAnimationState.Event += HandleEvent;
            startSpeed = speed;

        }


        private void Update()
        {
            if (GameManager.gameManager.levelFinished==true) { return; }
            ShootAndroid();
        }

        private void ShootAndroid()
        {
            if (Input.touchCount > 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction);
                if (raycastHit2D)
                {
                    EnemyController enemy = raycastHit2D.collider.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        Shoot(enemy);
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector3(speed, 0, 0);
        }

        public void Shoot(EnemyController enemy)
        {
            if (skeletonAnimation.AnimationState.GetCurrent(0).Animation != null && skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name != "shoot")
            {
                speed = 0f;
                currentEnemy = enemy;
                spineAnimationState.SetAnimation(0, "shoot", false);
                spineAnimationState.AddAnimation(0, "walk", true, 0);

            }
        }

        public void Die()
        {
            spineAnimationState.SetAnimation(0, "loose", false);
            speed = 0f;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "LevelComplete")
            {
                GameManager.gameManager.levelFinished = true;
                UIManager.uIManager.ActiveWinCanvas();
            }
        }


#region AnimationEvents

        private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == "shooter/fire")
            {
                fire.gameObject.SetActive(true);
                fire.Play();
                fire.GetComponentInChildren<AudioSource>().Play();
                currentEnemy.Die();
                currentEnemy = null;

            }
        }


        private void AnimationStart(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == "walk")
            {
                speed = startSpeed;
            }
        }
#endregion

    }
}
