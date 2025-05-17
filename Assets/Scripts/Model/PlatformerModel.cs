using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Model
{
    /// <summary>
    /// The main model containing needed data to implement a platformer style
    /// game. This class should only contain data, and methods that operate
    /// on the data. It is initialised with data in the GameController class.
    /// </summary>
    [System.Serializable]
    public class PlatformerModel
    {
        /// <summary>
        /// The virtual camera in the scene.
        /// </summary>
        public Unity.Cinemachine.CinemachineCamera virtualCamera;

        /// <summary>
        /// The main component which controls the player sprite, controlled
        /// by the user.
        /// </summary>
        public PlayerController player;

        /// <summary>
        /// The spawn point in the scene.
        /// </summary>
        public Transform spawnPoint;

        /// <summary>
        /// A global jump modifier applied to all initial jump velocities.
        /// </summary>
        public float jumpModifier = 1.5f;

        /// <summary>
        /// A global jump modifier applied to slow down an active jump when
        /// the user releases the jump input.
        /// </summary>
        public float jumpDeceleration = 0.5f;

        /// <summary>
        /// 壁跳相關參數
        /// </summary>
        public float wallJumpForce = 100f;
        public float wallSlideSpeed = 0.3f;

        /// <summary>
        /// 推動相關參數
        /// </summary>
        public float pushForce = 5f;

        /// <summary>
        /// 踢擊相關參數
        /// </summary>
        public float kickForce = 8f;
        public float kickCooldown = 0.5f;

        /// <summary>
        /// 暈眩相關參數
        /// </summary>
        public float stunDuration = 2f;

        /// <summary>
        /// 救援系統相關參數
        /// </summary>
        public int maxRescueTargets = 5;
        public int rescuedCount = 0;

        /// <summary>
        /// 關卡相關參數
        /// </summary>
        public float timeLimit = 300f;
        public float timeRemaining = 300f;
        public int score = 0;
        public int defeatedMinorEnemies = 0;
        public int destroyedObjects = 0;

        /// <summary>
        /// 分數係數
        /// </summary>
        public int minorEnemyScore = 100;
        public int objectDestroyScore = 50;
        public int timeBonus = 10;
    }
}