using TestTask.Player;
using Spine;
using Spine.Unity;
using System.Collections;
using TestTask.UI;
using UnityEngine;
using TestTask.CameraSettings;


namespace TestTask.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] float enemySpeed;
        [SerializeField] float offsetY;
        [SerializeField] ParticleSystem deathParticles;
        [SerializeField] Transform particlePoint;
        [SerializeField] Transform point;
        Rigidbody2D rb;
        SkeletonAnimation skeletonAnimation;
        public Spine.AnimationState spineAnimationState;
        float currentEnemySpeed;
        PlayerController playerController;


        private void Start()
        {
            currentEnemySpeed = 0;
            playerController = FindObjectOfType<PlayerController>();
            rb = GetComponent<Rigidbody2D>();
            skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            spineAnimationState.Complete += CompleteAnimation;
            StartCoroutine(CheckInView());
        }

        private void CompleteAnimation(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == "win")
            {
                UIManager.uIManager.ActiveLoseCanvas();
            }

        }


        public void OnMouseDown()
        {
            if (GameManager.gameManager.levelFinished == true) { return; }
            playerController.Shoot(this);
            
        }


        IEnumerator CheckInView()
        {
            while (true)
            {
                if (CameraController.IsVisibleToCamera(point) == true)
                {
                    spineAnimationState.SetAnimation(0, "run", true);
                    currentEnemySpeed = enemySpeed;
                    break;
                }
                yield return null;
            }

        }


        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                GameManager.gameManager.levelFinished = true;
                spineAnimationState.SetAnimation(0, "angry", false);
                spineAnimationState.AddAnimation(0, "win", false, 0);
                currentEnemySpeed = 0f;
                playerController.Die();
                EnemiesIdle();
            }
        }

        private void EnemiesIdle()
        {
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            foreach (EnemyController enemy in enemies)
            {
                if (enemy != this)
                {
                    Destroy(enemy.gameObject);
  
                }
            }
        }

        public void Die()
        {
            ParticleSystem particles = Instantiate(deathParticles);
            particles.transform.position = particlePoint.position;
            particles.Play();
            Destroy(gameObject);
        }


        private void FixedUpdate()
        {
            rb.velocity = new Vector3(-currentEnemySpeed, 0, 0);
        }

    }
}
