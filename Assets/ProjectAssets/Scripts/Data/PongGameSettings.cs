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
        public BallSettings ballSettings = null;

        [Header("Racket")]
        public RacketSettings racketSettings = null;


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
