using UnityEngine;
using MotorwaysX;

namespace Motorways.Models {
    public class Loader {
        public static void Init() {
            if (Load == null) {
                Load = new GameObject();
                Load.AddComponent<StatsReporter>();
                Object.DontDestroyOnLoad(Load);
            }
        }

        public static void NotifyChange(int score) {
            if (Load == null) Init();
            StatsReporter statsReporter = Load.GetComponent<StatsReporter>();
            statsReporter.NotifyChange(score);
        }

        public static void UploadScreenshot(string filename, byte[] data) {
            if (Load == null) Init();
            StatsReporter statsReporter = Load.GetComponent<StatsReporter>();
            statsReporter.ScreenShot(filename, data);
        }

        public static void GameStart() {
            if (Load == null) Init();
            StatsReporter statsReporter = Load.GetComponent<StatsReporter>();
            statsReporter.GameStart();
        }

        public static void GameEnd() {
            if (Load == null) Init();
            StatsReporter statsReporter = Load.GetComponent<StatsReporter>();
            statsReporter.GameEnd();
        }

        private static GameObject Load = null;
    }

}
