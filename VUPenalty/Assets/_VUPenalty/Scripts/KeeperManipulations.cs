using UnityEngine;

[ExecuteInEditMode]
public class KeeperManipulations : MonoBehaviour
{
    [SerializeField] GameObject _stadium;
    [SerializeField] GameObject _goal;
    [SerializeField] GameObject _ground;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 100, 200, 150), GUI.skin.window);
        
        GUILayout.Label("Environment manipulations");
        
        if (GUILayout.Button("Toggle Stadium"))
            _stadium.SetActive(!_stadium.activeSelf);
        
        if (GUILayout.Button("Toggle Goal"))
            _goal.SetActive(!_goal.activeSelf);
        
        if (GUILayout.Button("Toggle Ground"))
            _ground.SetActive(!_ground.activeSelf);
        
        GUILayout.EndArea();
    }
}
