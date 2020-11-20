using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class CustomTerrain : MonoBehaviour
{
    public Vector2 RandomHeightRange = new Vector2(0, 0.1f);
    public Texture2D HeightMapImage;
    public Vector3 HeightMapScale = new Vector3(1, 1, 1);

    public bool ResetHeightMap;

    public float PerlinXScale = 0.01f;
    public float PerlinYScale = 0.01f;
    public int PerlinXOffset = 0;
    public int PerlinYOffset = 0;
    public int PerlinOctaves = 3;
    public float PerlinPersistance = 8;
    public float PerlinHeightScale = 0.09f;

    [System.Serializable]
    public class PerlinParameters
    {
        public float PerlinXScale = 0.01f;
        public float PerlinYScale = 0.01f;
        public int PerlinXOffset = 0;
        public int PerlinYOffset = 0;
        public int PerlinOctaves = 3;
        public float PerlinPersistance = 8;
        public float PerlinHeightScale = 0.09f;
        public bool Remove;
    }

    public Terrain Terrain;
    public TerrainData TerrainData;

    private float[,] GetHeightMap()
    {
        if (this.ResetHeightMap)
        {
            return new float[this.TerrainData.heightmapResolution, this.TerrainData.heightmapResolution];
        }
        else
        {
            return this.TerrainData.GetHeights(0, 0, this.TerrainData.heightmapResolution, this.TerrainData.heightmapResolution);
        }
    }

    public void Perlin()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < this.TerrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < this.TerrainData.heightmapResolution; y++)
            {
                //heightMap[x, y] = Mathf.PerlinNoise(
                //    (this.PerlinXOffset + x) * this.PerlinXScale,
                //    (this.PerlinYOffset + y) * this.PerlinYScale
                //    );
                heightMap[x, y] += Utils.FractalBrownianMotion(
                    (x + this.PerlinXOffset) * this.PerlinXScale,
                    (y + this.PerlinYOffset) * this.PerlinYScale,
                    this.PerlinOctaves,
                    this.PerlinPersistance
                    ) * this.PerlinHeightScale;
            }
        }

        this.TerrainData.SetHeights(0, 0, heightMap);
    }

    public void RandomTerrain()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < this.TerrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < this.TerrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += UnityEngine.Random.Range(this.RandomHeightRange.x, this.RandomHeightRange.y);
            }
        }

        this.TerrainData.SetHeights(0, 0, heightMap);
    }

    public void LoadTexture()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < this.TerrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < this.TerrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += (HeightMapImage.GetPixel(
                    (int)(x * this.HeightMapScale.x),
                    (int)(y * this.HeightMapScale.z)).grayscale)
                    * HeightMapScale.y;
            }
        }

        this.TerrainData.SetHeights(0, 0, heightMap);
    }

    public void ResetTerrain()
    {
        float[,] heightMap = GetHeightMap();

        for (int x = 0; x < this.TerrainData.heightmapResolution; x++)
        {
            for (int y = 0; y < this.TerrainData.heightmapResolution; y++)
            {
                heightMap[x, y] += 0;
            }
        }

        this.TerrainData.SetHeights(0, 0, heightMap);

    }

    private void OnEnable()
    {
        Debug.Log("Initializing Terrain Data");
        this.Terrain = this.GetComponent<Terrain>();
        this.TerrainData = Terrain.activeTerrain.terrainData;
    }

    void Awake()
    {
        var asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0];
        SerializedObject tagManager = new SerializedObject(asset);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        AddTag(tagsProp, "Terrain");
        AddTag(tagsProp, "Cloud");
        AddTag(tagsProp, "Shore");

        tagManager.ApplyModifiedProperties();

        this.gameObject.tag = "Terrain";
    }

    void AddTag(SerializedProperty tagsProp, string newTag)
    {
        bool found = false;

        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
            newTagProp.stringValue = newTag;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
