using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace E4.Utility
{
    // DataManager 를 사용하기 위한 인터페이스
    public interface ISavable<out T> where T : class
    {
        string FileName => typeof(T) + ".json";
        T Data { get; } // 저장할 데이터 클래스
        bool IsDirty { get; set; } // true 일 경우에만 데이터 저장
    }
    
    // TODO GPGS 및 FireBase 데이터 관리 기능 추가
    /// <summary>
    /// 데이터 저장 및 불러오기를 담당하는 클래스
    /// </summary>
    public class DataManager
    {
        /* Static */
        // 기본 데이터 저장 경로
        static readonly string DefaultDataPath = Application.persistentDataPath;
        static Queue<Action> Requests = new Queue<Action>();

        /* 데이터 저장 */
        public static bool RequestSaveData<T>(ISavable<T> target) where T : class => RequestSaveData(target, DefaultDataPath);
        public static bool RequestSaveData<T>(ISavable<T> target, string filePath) where T : class
        {
            // 더티 플래그 확인 및 설정
            if (target is null) return false;

            if (target.IsDirty)
            {
#if UNITY_EDITOR
                Debug.Log("<color=yellow>Request For Saving Data Is Already Exist : " + target + "</color>");
#endif
                return false;
            }
            target.IsDirty = true;

#if UNITY_EDITOR
            Debug.Log("<color=yellow>Receive Request For Saving Data : " + target + "</color>");
#endif
            
            // 요청 목록에 작업 추가
            Requests.Enqueue(() =>
            {
                SaveData(target, filePath);
                target.IsDirty = false;
            });
            return true;
        }
        public static void HandleRequests()
        {
            // 요청이 존재하는 경우에만 실행
            if (Requests.Count == 0) return;
            
#if UNITY_EDITOR
            Debug.Log("<color=red>Start Handling All Requests</color>");
#endif
            
            // 요청 실행
            while (Requests.Count != 0)
            {
                var request = Requests.Dequeue();
                request.Invoke();
            }
        }
        
        static void SaveData<T>(ISavable<T> target) where T : class => SaveData(target, DefaultDataPath);

        static void SaveData<T>(ISavable<T> target, string filePath) where T : class
        {
            // 유효성 검사
            if (target is null || !target.IsDirty) return;
            
            // JSON 데이터 저장
            var path = Path.Combine(filePath, target.FileName);

            var json = JsonUtility.ToJson(target.Data, true);
            File.WriteAllText(path, json);

#if UNITY_EDITOR
            Debug.Log("<color=green>Save Data At " + path + "</color>");
#endif
        }

        /* 데이터 불러오기 */
        public static T LoadData<T>(ISavable<T> target) where T : class => LoadData<T>(target, DefaultDataPath);

        public static T LoadData<T>(ISavable<T> target, string filePath) where T : class
        {
            // 유효성 검사
            if (target is null) return null;

            // 저장된 데이터 확인
            var path = Path.Combine(filePath, target.FileName);
            if (!File.Exists(path)) return null;

#if UNITY_EDITOR
            Debug.Log("<color=green>Load Data From " + path + "</color>");
#endif

            // 데이터 불러오기
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }

        /* 데이터 삭제 */
        public static void DeleteData(string fileName) => DeleteData(fileName, DefaultDataPath);

        public static void DeleteData(string fileName, string filePath)
        {
            // 저장된 데이터 확인
            var path = Path.Combine(filePath, fileName);
            if (!File.Exists(path))
            {
#if UNITY_EDITOR
                Debug.Log("<color=green>No Data At " + path + "</color>");
#endif
                return;
            }

            // 데이터 삭제
            File.Delete(path);

#if UNITY_EDITOR
            Debug.Log("<color=green>Delete Data At " + path + "</color>");
#endif
        }

        public static void DeleteAllData()
        {
            // 저장된 데이터 확인
            if (!Directory.Exists(DefaultDataPath)) return;

            var files = Directory.GetFiles(DefaultDataPath);

            // 데이터 삭제
            foreach (var file in files)
            {
                File.Delete(Path.Combine(DefaultDataPath, file));
            }

#if UNITY_EDITOR
            Debug.Log("<color=green>All Data Deleted At " + DefaultDataPath + "</color>");
#endif
        }
    }
}
