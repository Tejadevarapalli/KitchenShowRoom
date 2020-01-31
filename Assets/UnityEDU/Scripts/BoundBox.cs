using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DimBoxes
{
    [ExecuteInEditMode]
    public class BoundBox : MonoBehaviour
    {

        public bool colliderBased = false;
        public bool permanent = false; //permanent//onMouseDown

        public Color lineColor = new Color(0f, 1f, 0.4f, 0.74f);

        private Bounds bound;
        private Vector3 boundOffset;
        [HideInInspector]
        public Bounds colliderBound;
        [HideInInspector]
        public Vector3 colliderBoundOffset;
        [HideInInspector]
        public Bounds meshBound;
        [HideInInspector]
        public Vector3 meshBoundOffset;

        public bool setupOnAwake = false;

        private Vector3[] corners;

        private Vector3[,] lines;

        private Quaternion quat;

        private Camera mcamera;

        private DimBoxes.DrawLines cameralines;

        private MeshFilter[] meshes;

        private Vector3 topFrontLeft;
        private Vector3 topFrontRight;
        private Vector3 topBackLeft;
        private Vector3 topBackRight;
        private Vector3 bottomFrontLeft;
        private Vector3 bottomFrontRight;
        private Vector3 bottomBackLeft;
        private Vector3 bottomBackRight;

        [HideInInspector]
        public Vector3 startingScale;
        private Vector3 previousScale;
        private Vector3 startingBoundSize;
        private Vector3 startingBoundCenterLocal;
        private Vector3 previousPosition;
        private Quaternion previousRotation;


        void Reset()
        {
            meshes = GetComponentsInChildren<MeshFilter>();
            calculateBounds();
            Start();
        }

        void Awake()
        {
            if (setupOnAwake)
            {
                meshes = GetComponentsInChildren<MeshFilter>();
                calculateBounds();
            }
        }

        void Start()
        {
            cameralines = FindObjectOfType(typeof(DimBoxes.DrawLines)) as DimBoxes.DrawLines;

            if (!cameralines)
            {
                Debug.LogError("DimBoxes: no camera with DimBoxes.DrawLines in the scene", gameObject);
                return;
            }

            mcamera = cameralines.GetComponent<Camera>();
            previousPosition = transform.position;
            previousRotation = transform.rotation;
            startingBoundSize = bound.size;
            startingScale = transform.localScale;
            previousScale = startingScale;
            startingBoundCenterLocal = transform.InverseTransformPoint(bound.center);
            init();
        }

        public void init()
        {
            setPoints();
            setLines();
        }

        void LateUpdate()
        {
            if (transform.localScale != previousScale)
            {
                scaleBounds();
                setPoints();
            }
            if (transform.position != previousPosition || transform.rotation != previousRotation || transform.localScale != previousScale)
            {
                setLines();
                previousRotation = transform.rotation;
                previousPosition = transform.position;
                previousScale = transform.localScale;
            }
            cameralines.setOutlines(lines, lineColor, new Vector3[0, 0]);
        }

        public void scaleBounds()
        {
            //bound.size = new Vector3(startingBoundSize.x * transform.localScale.x / startingScale.x, startingBoundSize.y * transform.localScale.y / startingScale.y, startingBoundSize.z * transform.localScale.z / startingScale.z);
            //bound.center = transform.TransformPoint(startingBoundCenterLocal);
        }

        void calculateBounds()
        {
            quat = transform.rotation;//object axis AABB

            BoxCollider coll = GetComponent<BoxCollider>();
            if (coll)
            {
                GameObject co = new GameObject("dummy");
                co.transform.position = transform.position;
                co.transform.localScale = transform.lossyScale;
                BoxCollider cobc = co.AddComponent<BoxCollider>();
                //quat = transform.rotation;
                cobc.center = coll.center;
                cobc.size = coll.size;
                colliderBound = cobc.bounds;
                DestroyImmediate(co);
                colliderBoundOffset = colliderBound.center - transform.position;
            }
            //return;

            /*isStatic = meshes[0].GetComponent<Renderer>().isPartOfStaticBatch;
            if (isStatic) quat = Quaternion.Euler(0f, 0f, 0f);//world axis
            
            if (isStatic)
            {
                meshBound = meshes[0].GetComponent<Renderer>().bounds;
                for (int i = 1; i < meshes.Length; i++)
                {
                    meshBound.Encapsulate(meshes[i].GetComponent<Renderer>().bounds);
                }
                return;
            }*/

            //if (Vector3.Magnitude(meshBound.size)>0.01f) return; //because of lenghty calculations - don't recalculate again
            meshBound = new Bounds();

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            for (int i = 0; i < meshes.Length; i++)
            {
                Mesh ms = meshes[i].sharedMesh;
                int vc = ms.vertexCount;
                for (int j = 0; j < vc; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        meshBound = new Bounds(meshes[i].transform.TransformPoint(ms.vertices[j]), Vector3.zero);
                    }
                    else
                    {
                        meshBound.Encapsulate(meshes[i].transform.TransformPoint(ms.vertices[j]));
                    }
                }
            }
            transform.rotation = quat;
            meshBoundOffset = meshBound.center - transform.position;
        }

        void setPoints()
        {

            if (colliderBased)
            {
                if (colliderBound == null)
                {
                    Debug.LogError("no collider - add collider to " + gameObject.name + " gameObject");
                    return;

                }
                bound = colliderBound;
                boundOffset = colliderBoundOffset;
            }

            else
            {
                bound = meshBound;
                boundOffset = meshBoundOffset;
            }

            bound.size = new Vector3(bound.size.x * transform.localScale.x / startingScale.x, bound.size.y * transform.localScale.y / startingScale.y, bound.size.z * transform.localScale.z / startingScale.z);
            boundOffset = new Vector3(boundOffset.x * transform.localScale.x / startingScale.x, boundOffset.y * transform.localScale.y / startingScale.y, boundOffset.z * transform.localScale.z / startingScale.z);


            topFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, 1));
            topFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, 1));
            topBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, 1, -1));
            topBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, 1, -1));
            bottomFrontRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, 1));
            bottomFrontLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, 1));
            bottomBackLeft = boundOffset + Vector3.Scale(bound.extents, new Vector3(-1, -1, -1));
            bottomBackRight = boundOffset + Vector3.Scale(bound.extents, new Vector3(1, -1, -1));

            corners = new Vector3[] { topFrontRight, topFrontLeft, topBackLeft, topBackRight, bottomFrontRight, bottomFrontLeft, bottomBackLeft, bottomBackRight };

        }

        void setLines()
        {

            Quaternion rot = transform.rotation;
            Vector3 pos = transform.position;

            List<Vector3[]> _lines = new List<Vector3[]>();
            //int linesCount = 12;

            Vector3[] _line;
            for (int i = 0; i < 4; i++)
            {
                //width
                _line = new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 1] + pos };
                _lines.Add(_line);
                //height
                _line = new Vector3[] { rot * corners[i] + pos, rot * corners[i + 4] + pos };
                _lines.Add(_line);
                //depth
                _line = new Vector3[] { rot * corners[2 * i] + pos, rot * corners[2 * i + 3 - 4 * (i % 2)] + pos };
                _lines.Add(_line);

            }
            lines = new Vector3[_lines.Count, 2];
            for (int j = 0; j < _lines.Count; j++)
            {
                lines[j, 0] = _lines[j][0];
                lines[j, 1] = _lines[j][1];
            }
        }

        void OnMouseDown()
        {
            if (permanent) return;
            enabled = !enabled;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (EditorApplication.isPlaying) return;
            init();
        }


#endif

        void OnDrawGizmos()
        {
            
            Gizmos.color = lineColor;
            for (int i = 0; i < lines.GetLength(0); i++)
            {
                Gizmos.DrawLine(lines[i, 0], lines[i, 1]);
            }
        }

    }
}
