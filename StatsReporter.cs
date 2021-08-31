using UnityEngine;
using UnityEngine.Networking;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MotorwaysX {

    public class StatsReporter: MonoBehaviour {

        public void Start() {
            stats = new Dictionary<string, object>();
            stats["steam_id"] = SteamClient.SteamId.AccountId;
            stats["name"] = SteamClient.Name;
            stats["score"] = 0;
            stats["status"] = "initialized";
            StartCoroutine(CollectData());
        }

        public void NotifyChange(int score) {
            if ((string)stats["status"] != "started") return;
            stats["score"] = score;
            StartCoroutine(Report());
        }

        public void GameStart() {
            stats["score"] = 0;
            stats["status"] = "started";
            StartCoroutine(Report());
        }
        public void GameEnd() {
            stats["score"] = 0;
            stats["status"] = "gameover";
            StartCoroutine(Report());
        }

        public void ScreenShot(string filename, byte[] data) {
            StartCoroutine(UploadScreenshot(filename, data));
        }

        public void Update() {
            // Report();
        }

        private IEnumerator Report() {
            UnityWebRequest req = Post(API, Json.Serialize(stats));
            yield return req.SendWebRequest();
        }

        private IEnumerator UploadScreenshot(string filename, byte[] data) {
            UnityWebRequest req = Post(SCREENSHOT_API, data, "application/octet-stream");
            req.SetRequestHeader("X-Screenshot-Name", filename);

            yield return req.SendWebRequest();
        }

        private IEnumerator CollectData() {
            Dictionary<string, object> client_info = new Dictionary<string, object>();
            client_info["steam_name"] = SteamClient.Name;
            client_info["steam_id"] = SteamClient.SteamId.AccountId;
            client_info["build_id"] = SteamApps.BuildId;
            client_info["game_language"] = SteamApps.GameLanguage;
            client_info["app_owner"] = SteamApps.AppOwner.AccountId;
            UnityWebRequest req = Post(DATA_API, Json.Serialize(client_info));
            yield return req.SendWebRequest();
            StartCoroutine(Report());
        }

        private static UnityWebRequest Post(string url, byte[] body, string contentType = "application/json") {
            UnityWebRequest req = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandler uploader = new UploadHandlerRaw(body);

            req.useHttpContinue = false;
            req.uploadHandler = uploader;
            req.SetRequestHeader("X-Steam-Id", SteamClient.SteamId.AccountId.ToString());
            req.SetRequestHeader("X-Steam-Name", SteamClient.Name);
            uploader.contentType = contentType;

            return req;
        }
        private static UnityWebRequest Post(string url, string body, string contentType = "application/json") {
            return Post(url, Encoding.UTF8.GetBytes(body), contentType);
        }

        private static string API = "http://192.168.0.216:8080/api/report";
        private static string SCREENSHOT_API = "http://192.168.0.216:8080/api/screenshot";
        private static string DATA_API = "http://192.168.0.216:8080/api/report";
        private static Dictionary<string, object> stats;
    }
}