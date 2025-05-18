using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the health component on an enemy has a hitpoint value of  0.
    /// </summary>
    /// <typeparam name="EnemyDeath"></typeparam>
    public class EnemyDeath : Simulation.Event<EnemyDeath>
    {
        public EnemyController enemy;

        public override void Execute()
        {
            if (enemy._audio && enemy.ouch)
                enemy._audio.PlayOneShot(enemy.ouch);

            Animator animator = enemy.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("death");
            }
        }
    }
}