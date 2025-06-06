using UnityEngine;

[ExecuteInEditMode]
public class ManipulationGui : MonoBehaviour
{
    [SerializeField] GameObject _stadium;
    [SerializeField] GameObject _goal;
    [SerializeField] GameObject _ground;

    [SerializeField] Material _stadiumMaterial;
    [SerializeField] Material _greenMaterial;
    [SerializeField] MeshRenderer _goalRenderer;
    bool _materialToggle;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 100, 200, 150), GUI.skin.window);
        
        GUILayout.Label("Environment manipulations");
        
        if (GUILayout.Button("Change stadium material"))
        {
            _materialToggle = !_materialToggle;
            if (_materialToggle)
                _goalRenderer.material = _stadiumMaterial;
            else
                _goalRenderer.material = _greenMaterial;
        }
        
        if (GUILayout.Button("Toggle Stadium"))
            _stadium.SetActive(!_stadium.activeSelf);
        
        if (GUILayout.Button("Toggle Goal"))
            _goal.SetActive(!_goal.activeSelf);
        
        if (GUILayout.Button("Toggle Ground"))
            _ground.SetActive(!_ground.activeSelf);
        
        GUILayout.EndArea();
    }
}
