using UnityEngine;
using UnityEditor;

public class SceneViewCameraTest : EditorWindow
{
    [MenuItem("Window/SceneViewCameraTest")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        SceneViewCameraTest window = (SceneViewCameraTest)EditorWindow.GetWindow(typeof(SceneViewCameraTest));
    }

    void OnGUI()
    {
        //EditorGUILayout.TextField("SceneViewCameraAngle", "" + SceneView.lastActiveSceneView.rotation);
        EditorGUILayout.TextField("SceneViewCameraPosition", "" + SceneView.lastActiveSceneView.camera.transform.position);
        EditorGUILayout.TextField("SceneViewCameraRotation", "" + SceneView.lastActiveSceneView.camera.transform.eulerAngles);
    }
}