// DisableEnemyOnDeath.cs
using UnityEngine;
using Platformer.Mechanics; // Added to access EnemyController

public class DisableEnemyOnDeath : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the EnemyController and Collider2D components from the animator's GameObject
        EnemyController enemyController = animator.GetComponent<EnemyController>();
        Collider2D enemyCollider = animator.GetComponent<Collider2D>();
        Rigidbody2D enemyRigidbody = animator.GetComponent<Rigidbody2D>(); // Also disable Rigidbody

        // Disable the components
        if (enemyController != null)
        {
            enemyController.enabled = false;
        }
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
        if (enemyRigidbody != null) // Disable rigidbody simulation
        {
            enemyRigidbody.simulated = false;
        }

        // Optional: You can destroy the GameObject after a delay if needed
        // Destroy(animator.gameObject, 0.1f); // Example: Destroy after a small delay
    }

    // Optional: You can implement OnStateEnter, OnStateUpdate, etc. if needed
}
