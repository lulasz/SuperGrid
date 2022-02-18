using UnityEngine;
using UnityEditor;

public class SuperGrid : EditorWindow
{
    bool toolActive = true;
    bool snap;
    bool grid;
    bool select;

    int gridSize = 8;
    float lineSize = 0.64f;

    Color gridColor = Color.gray;
    Color selectColor = Color.yellow;

    [MenuItem("Tools/SuperGrid")]
    static void Init()
    {
        SuperGrid window = (SuperGrid)EditorWindow.GetWindow(typeof(SuperGrid));
        window.Show();
    }


    // Draw editor window
    void OnGUI()
    {
        GUILayout.Label("SuperGrid Settings", EditorStyles.boldLabel);

        toolActive = EditorGUILayout.BeginToggleGroup("Enable Tool", toolActive);
        snap = EditorGUILayout.Toggle("Snap", snap);
        grid = EditorGUILayout.Toggle("View Grid", grid);
        select = EditorGUILayout.Toggle("Highlight Selected", select);
        gridColor = EditorGUILayout.ColorField("Grid Color", gridColor);
        selectColor = EditorGUILayout.ColorField("Select Color", selectColor);
        EditorGUILayout.EndToggleGroup();
    }

    private void Update()
    {
        // Snap to grid
        if (Selection.activeTransform && snap && toolActive)
        {
            var currentPos = Selection.activeTransform.gameObject.transform.position;
            Selection.activeTransform.gameObject.transform.position = new Vector3(Mathf.Round(currentPos.x), Mathf.Round(currentPos.y), Mathf.Round(currentPos.z));
        }
    }


    // Window has been selected
    void OnFocus()
    {
        // Remove delegate listener if it has previously been assigned.
        SceneView.duringSceneGui -= this.OnSceneGUI;
        // Add the delegate.
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (Selection.activeTransform && toolActive)
        {
            Vector3 p = Selection.activeTransform.gameObject.transform.position;
            // Draw grid
            if (grid)
            {
                Handles.color = gridColor;
                Vector3 offset = new Vector3(-0.5f, 0f, -0.5f);
                for (var x = 0; x < gridSize + 1; x++)
                {
                    Vector3 start = (p) + offset + new Vector3(gridSize, 0f, x);
                    Vector3 end = (p) + offset + new Vector3(-gridSize + 1, 0f, x);
                    Handles.DrawLine(start, end, lineSize);
                }

                for (var x = -1; x < gridSize; x++)
                {
                    Vector3 start = (p) + offset + new Vector3(gridSize, 0f, -x);
                    Vector3 end = (p) + offset + new Vector3(-gridSize + 1, 0f, -x);
                    Handles.DrawLine(start, end, lineSize);
                }

                for (var y = 0; y < gridSize + 1; y++)
                {
                    Vector3 start = (p) + offset + new Vector3(y, 0f, gridSize);
                    Vector3 end = (p) + offset + new Vector3(y, 0f, -gridSize + 1);
                    Handles.DrawLine(start, end, lineSize);
                }

                for (var y = -1; y < gridSize; y++)
                {
                    Vector3 start = (p) + offset + new Vector3(-y, 0f, gridSize);
                    Vector3 end = (p) + offset + new Vector3(-y, 0f, -gridSize + 1);
                    Handles.DrawLine(start, end, lineSize);
                }
            }

            // Draw Sprite outline
            if (select)
            {
                Handles.color = selectColor;
                Handles.DrawLine(p + new Vector3(0.5f, 0f, 0f) + (-Vector3.forward / 2f), p + new Vector3(0.5f, 0f, 0f) + (Vector3.forward / 2f), lineSize * 6);
                Handles.DrawLine(p + new Vector3(-0.5f, 0f, 0f) + (-Vector3.forward / 2f), p + new Vector3(-0.5f, 0f, 0f) + (Vector3.forward / 2f), lineSize * 6);
                Handles.DrawLine(p + new Vector3(0f, 0f, 0.5f) + (Vector3.right / 2f), p + new Vector3(0f, 0f, 0.5f) + (-Vector3.right / 2f), lineSize * 6);
                Handles.DrawLine(p + new Vector3(0f, 0f, -0.5f) + (Vector3.right / 2f), p + new Vector3(0f, 0f, -0.5f) + (-Vector3.right / 2f), lineSize * 6);
            }
        }
    }
}