#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class RhythmCsvImporter : EditorWindow
{
    TextAsset csvFile;
    readonly string apiKey = "AIzaSyBFU13dXMNDscj1XW4BTjqQP65m_-N9Cco";
    string address;
    string range;
    string sheetID;
    string assetName = "NewRhythm";
    bool readBpmFromCsv = true;
    float overrideBpm = 120f;
    string outputFolder = "Assets/Resources/ Rhythms";

    [MenuItem("Tools/Import CSV -> Rhythm")]
    static void OpenWindow()
    {
        GetWindow<RhythmCsvImporter>("Rhythm CSV Importer");
    }

    void OnGUI()
    {
        GUILayout.Label("CSV to Rhythm Importer");
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);
        sheetID = EditorGUILayout.TextField("Google Sheet ID", sheetID);
        range = EditorGUILayout.TextField("Google Sheet Range", range);
        assetName = EditorGUILayout.TextField("Asset Name", assetName);
        //readBpmFromCsv = EditorGUILayout.Toggle("Read BPM from CSV", readBpmFromCsv);
        overrideBpm = EditorGUILayout.FloatField("Override BPM", overrideBpm);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (GUILayout.Button("Import"))
        {
            if (address == null && range == null && sheetID == null)
            {
                EditorUtility.DisplayDialog("Error", "CSV 파일 주소를 지정하세요.", "OK");
                return;
            }
            try
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(LoadSheet());
            }
            catch (Exception ex)
            {
                Debug.LogError("Import failed: " + ex);
                EditorUtility.DisplayDialog("Error", "Import 실패: 콘솔 확인", "OK");
            }
        }
    }

    IEnumerator LoadSheet()
    {
        UnityWebRequest request = UnityWebRequest.Get(GetCSVAddress());
        yield return request.SendWebRequest();
        ImportCsv(ConvertJsonToCsv(request.downloadHandler.text));
    }
    
    string ConvertJsonToCsv(string json)
    {
        var root = JObject.Parse(json);
        var values = root["values"] as JArray;

        StringBuilder csvBuilder = new StringBuilder();

        foreach (var row in values)
        {
            var line = string.Join(",", row.Select(v => $"{v}"));
            csvBuilder.AppendLine(line);
        }

        return csvBuilder.ToString();
    }
    
    string GetCSVAddress()
    {
        return $"https://sheets.googleapis.com/v4/spreadsheets/{sheetID}/values/{range}?key={apiKey}";
    }
    
    void ImportCsv(string csvText)
    {
        var lines = csvText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        
        float bpm = overrideBpm;
        //var entries = new List<(string color, int index, string noteStr, bool isRest)>();
        var tracks = new List<(string trackColor, int index, int[] beats)>();

        // string[] -> string, int[] 형태로 변환
        foreach (var raw in lines)
        {
            var parts = raw.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.Length == 0) continue;

            string trackColor = parts[0];
            int index = int.Parse(parts[1]);
            int[] beats = parts.Skip(2).Select(int.Parse).ToArray();
            
            tracks.Add((trackColor, index, beats));
        }

        Track[] redTrack = new Track[2];
        Track[] blueTrack = new Track[2];

        for (int i = 0; i < redTrack.Length; i++)
            redTrack[i] = new Track();
        for (int i = 0; i < blueTrack.Length; i++)
            blueTrack[i] = new Track();
        
        foreach (var track in tracks)
        {
            List<RhythmNote> notes = new List<RhythmNote>();

            foreach (var note in track.beats)
            {
                bool isRest = note < 0 ? true : false;
                notes.Add(new RhythmNote((Note)(Mathf.Abs(note)), isRest));
            }

            switch (track.trackColor)
            {
                case "Red":
                    redTrack[track.index].RhythmNoteTrack = notes;
                    break;
                case "Blue":
                    blueTrack[track.index].RhythmNoteTrack = notes;
                    break;
            }
        }

        // Create asset
        var rhythm = ScriptableObject.CreateInstance<Rhythm>();
        rhythm.SetBpm(bpm);
        rhythm.SetRedTracks(redTrack);
        rhythm.SetBlueTracks(blueTrack);

        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string assetPath = Path.Combine(outputFolder, assetName + ".asset").Replace("\\", "/");
        AssetDatabase.CreateAsset(rhythm, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"Rhythm asset created at {assetPath}", "OK");
    }
}
#endif