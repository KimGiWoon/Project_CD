#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


public static class EnemyCSVImporter
{
    // CSV 파일 위치
    private const string CsvPath = "Assets/Resources/CSVData/Enemy/EnemyCSV_Data.csv";
    // Enemy SO 생성 위치
    private const string SoFolderPath = "Assets/05_DataSO/Enemy";

    [MenuItem("Tool/CartridgeDungeon/Import_Enemy_CSV")]
 
    public static void ImportEnemyCSV()
    {
        // CSV 에셋 가져오기
        TextAsset csvAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(CsvPath);

        if (csvAsset == null)
        {
            Debug.Log("Enemy의 SCV 파일을 찾을 수 없습니다.");
            return;
        }

        // SO가 생성될 폴더가 없으면 생성
        if (!AssetDatabase.IsValidFolder(SoFolderPath))
        {
            // 상위 폴더와 새폴더 이름 분해
            string parent = System.IO.Path.GetDirectoryName(SoFolderPath);
            string folderName = System.IO.Path.GetFileName(SoFolderPath);

            // 폴더 생성
            AssetDatabase.CreateFolder(parent, folderName);
            Debug.Log($"폴더 생성 : {SoFolderPath}");
        }

        // CSV 텍스트 행 단위 분리
        string[] dataLines = csvAsset.text.Split('\n');

        if(dataLines.Length <= 1)
        {
            Debug.Log("Enemy의 CSV 데이터가 없습니다.");
            return;
        }

        // CSV 텍스트 열 단위 분리
        for(int i = 1; i < dataLines.Length; i++)
        {
            string line = dataLines[i].Trim();

            // 빈 줄은 건너뛰기
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            // 쉼포 기준 분리
            string[] dataCols = line.Split(',');

            // CSV 데이터 파싱
            string id = dataCols[0].Trim();
            string name = dataCols[1].Trim();
            int maxHp = CSVParseUtil.ParseInt(dataCols[2].Trim());
            float moveSpeed = CSVParseUtil.ParseFloat(dataCols[3].Trim());
            int attackDamage = CSVParseUtil.ParseInt(dataCols[4].Trim());
            float attackCoolTime = CSVParseUtil.ParseFloat(dataCols[5].Trim());
            float attackRange = CSVParseUtil.ParseFloat(dataCols[6].Trim());
            float traceRange = CSVParseUtil.ParseFloat(dataCols[7].Trim());
            EnemyType type = CSVParseUtil.ParseEnum(dataCols[8], EnemyType.Melee);

            // SO 생성
            string soPath = $"{SoFolderPath}/{id}.asset";

            // 기존 SO가 있으면 갱신, 없으면 생성
            EnemyDataSO enemyData = AssetDatabase.LoadAssetAtPath<EnemyDataSO>(soPath);
            
            if (enemyData == null)
            {
                enemyData = ScriptableObject.CreateInstance<EnemyDataSO>();
                AssetDatabase.CreateAsset(enemyData, soPath);
                Debug.Log($"새로운 SO 생성 : {soPath}");
            }

            // CSV 값 적용
            enemyData.ApplyCSVdata(id, name, maxHp, moveSpeed, attackDamage, attackCoolTime, attackRange, traceRange, type);
        }

        // 에셋 저장 및 리프레시
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
#endif
