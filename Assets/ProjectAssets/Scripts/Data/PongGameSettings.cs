using UnityEngine;

namespace FF.Pong
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "PongGameSettings.asset", menuName = "Game Data/Pong Game Settings")]
    internal class PongGameSettings : ScriptableObject
    {
        #region Inspector Properties
        [Header("Match")]
        public int requiredPointsToWin = 5;

        [Header("Ball")]
        public float ballBaseVelocity = 12f;
        public float smashSpeedMultiplier = 1.5f;

        /*[Header("Racket")]
        public float lerpSpeed = 5f;
        public float moveSpeed = 1f;*/
        #endregion

        /*[MenuItem("Game Data/New PongGameSettings")]
        internal static void CreateAsset()
        {
            PongGameSettings pgs = CreateInstance<PongGameSettings>();
            AssetDatabase.CreateAsset(pgs, "Assets/PongGameSettings.asset");
            AssetDatabase.SaveAssets();
        }*/
    }
}
