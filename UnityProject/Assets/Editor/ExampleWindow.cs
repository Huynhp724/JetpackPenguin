
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;

public class ExampleWindow : EditorWindow
{
    string path, jsonString;
    string animControllerString = "";
    string[] jsonStrings;
    int dropdown = 0;


    [MenuItem("Window/Anim Tool")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ExampleWindow>("Anim Tool");
    }


    private void OnGUI()
    {
        GUILayout.Label("Click Button once you have filled the two fields below.", EditorStyles.boldLabel); ;

        JsonDropdown();
        //objString = EditorGUILayout.TextField("Name of the object", objString);
        animControllerString = EditorGUILayout.TextField("Name of Animator Controller.", animControllerString);

        

        if (GUILayout.Button("Apply Anim Tool.")) {

            if (animControllerString != "")
            {
                ApplyAnimTool();
            }
            else {
                Debug.LogError("Fill the fields above");
            }
        }
    }

    void JsonDropdown() {
        List<string> names = new List<string>();
        
        string newString;
        string[] strings;
        DirectoryInfo info = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] allFIles = info.GetFiles("*.*");

        foreach (FileInfo file in allFIles) {
            if (file.Extension.EndsWith(".json")) {
                newString = file.Name;
                strings = newString.Split(char.Parse("."));
                names.Add(strings[0]);
            }
        }

        jsonStrings = new string[names.Count];
        for (int i = 0; i < names.Count; i++) {
            jsonStrings[i] = names[i];
        }


        dropdown = EditorGUILayout.Popup("Name of the object", dropdown, jsonStrings);


    }

    void ApplyAnimTool() {
        path = Application.streamingAssetsPath + "/" + jsonStrings[dropdown] + ".json";
        jsonString = File.ReadAllText(path);
        AnimationMaker info = JsonUtility.FromJson<AnimationMaker>(jsonString);
        string newInfo = JsonUtility.ToJson(info);
        info.SetObject(jsonStrings[dropdown], animControllerString);
        Debug.Log(newInfo);
    }
}

[System.Serializable]
public class AnimationMaker {
    public string Model_Name;
    public string[] Animation_Names;
    public int Times;

    AnimatorState[] states;
    Animator anim;

    Motion[] motions;
    string objName, controllerName;

    public void SetObject(string obj, string controlName) {
        objName = obj;
        controllerName = controlName;
        GameObject go = Object.Instantiate(Resources.Load<GameObject>(Model_Name));
        if (go.GetComponent<Animator>() == null)
        {
            go.AddComponent<Animator>();
        }
        //var dummyObject = Resources.Load<GameObject>(Model_Name);

        anim = go.GetComponent<Animator>();
        states = new AnimatorState[Animation_Names.Length];
        MakeAnimationStuff();
        Debug.Log(go);
    }

    void MakeAnimationStuff() {
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Animations/" + controllerName + ".controller");

        // add state machine
        var rootStateMachine = controller.layers[0].stateMachine;

        for (int i = 0; i < Animation_Names.Length; i++)
        {
            string[] stringArrary = Animation_Names[i].Split(char.Parse("_"));
            var state = rootStateMachine.AddState(stringArrary[0]);
            //var motion = GetRightMotions(Animation_Names[i]);
            Debug.Log(objName + Animation_Names[i]);
            var motion = Resources.Load<Motion>(objName + "@" + Animation_Names[i]);
            if (motion == null)
            {
                Debug.Log("it is null");
            }
            state.motion = motion;
            states[i] = state;
        }

        rootStateMachine.defaultState = states[0];
        Debug.Log(states.Length);

        for (int i = 0; i < states.Length; i++)
        {
            if (i == states.Length - 1)
            {
                var transition = states[i].AddTransition(states[0], true);
                transition.duration = 0;
                break;
            }

            var otherTransition = states[i].AddTransition(states[i + 1], true);
            otherTransition.duration = 0;
        }

        anim.runtimeAnimatorController = controller;
    }
}
