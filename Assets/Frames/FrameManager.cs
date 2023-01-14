using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Argyle.UnclesToolkit.Base;
using Argyle.UnclesToolkit.SceneStuff;
using EasyButtons;
using UnityEngine;

namespace Frames
{
    /// <summary>
    /// Monobehavior component to gather, isntantiate, and create frames.
    /// Central control point from the AR app perspective.
    /// </summary>
    public class FrameManager : Manager<FrameManager>
    {
        public HashSet<FrameObject> frameObjects = new HashSet<FrameObject>();
        private SimpleStore _store;

        public GameObject GeoReferencePrefab;
        private GeoReferenceObject _currentGeoReference;

        public GameObject FramePrefab;
        [Tooltip("If no posematch is added to this gameobject, the Startfunction will supply it. ")]
        public PoseMatch _poseMatch;
        
        
        #region ==== Monobehavior ====------------------

        // Start is called before the first frame update
        void Start()
        {
            _store = new SimpleStore();
            if (!_poseMatch)
                _poseMatch = GetComponent<PoseMatch>();
            if (!_poseMatch)
                _poseMatch = GO.AddComponent<PoseMatch>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion -----------------/Monobehavior ====

        #region ==== Frame management ====------------------


        #region == Crud ==----


        public void CreateFrame()
        {
            if (!_currentGeoReference)
            {
                Debug.LogError($"No geo reference set");
                return;
            }

            Vector3 position = _currentGeoReference.TForm.InverseTransformPoint(Reference.MainCameraTransform.position);
            Vector3 rotation = (Quaternion.Inverse(Reference.MainCameraTransform.rotation) * TForm.rotation)
                .eulerAngles;

            FrameObject frameObject = Instantiate(FramePrefab, TForm).AddComponent<FrameObject>();
            frameObject.TForm.position = Reference.MainCameraTransform.position;
            frameObject.TForm.rotation = Reference.MainCameraTransform.rotation;
            
            
            
            frameObject.Data = new FrameData(
                _currentGeoReference.Data.Location.Translate(frameObject.TForm.localPosition), rotation);
            
            frameObjects.Add(frameObject);
        }

        public void InstantiateFrame(FrameData frameData)
        {
            if (!_currentGeoReference)
            {
                Debug.LogError($"No geo reference set");
                return;
            }


            FrameObject frameObject = Instantiate(FramePrefab, TForm).GetComponent<FrameObject>();
            frameObject.Data = frameData;

            frameObject.TForm.localPosition = _currentGeoReference.Data.Location.TranslationTo(frameData.geometry);
            frameObject.TForm.localEulerAngles = frameData.Rotation;
        }
        

        

        #endregion ----/Crud ==

        #region == IO ==----

        string filename(string id) => Path.Combine(folderName, $"Frame {id}.json");
        private const string folderName = "Frames";
        
        
        
        public void SaveFrame(FrameData frame)
        {
            if (frame.AssetId == null)
                frame.AssetId = Guid.NewGuid().ToString();
            _store.Store(frame, filename(frame.AssetId));
        }

        public void SaveFrames()
        {
            foreach (var frameObject in frameObjects)
                SaveFrame(frameObject.Data);
        }

        public void LoadFrame(string id)
        {
            FrameData data = _store.Retrieve<FrameData>(filename(id));
            InstantiateFrame(data);
        }

        public void LoadAllSavedFrames()
        {
            string directorypath = Path.Combine(_store.DirectoryPath(), folderName);
            foreach (var filePAth in Directory.GetFiles(directorypath))
            {
                LoadFrame(filePAth);
            }
        }
        
        #endregion ----/IO ==
        

        #endregion -----------------/Frame management ====


        #region ==== Georeference management ====------------------

        #region == Crud ==----

        public void BeginGeoReference()
        {
            Instantiate(GeoReferencePrefab);
        }
        public void ConfirmGeoReference()
        {
            _currentGeoReference = GetComponent<GeoReferenceObject>();
            _currentGeoReference.Data = new GeoReference(
                new GeoLocation(0,0,0));

            _poseMatch.matchTarget = _currentGeoReference.TForm;
            _poseMatch.runOnUpdate = true;
        }

        public void RemoveGeoReference()
        {
            _poseMatch.targetReference = null;
            _poseMatch.runOnUpdate = false;
            Destroy(_currentGeoReference.GO);
            _currentGeoReference = null;
        }

        #endregion ----/Crud ==

        #endregion -----------------/Georeference management ====
        
        
        
        
        
        
        
        #region ==== Test and Support ====------------------

        [Button]
        public void SerializeStuff()
        {
            if(!Application.isPlaying)
                return;
            
            FrameData frameDate = new FrameData(
                new GeoLocation(42.360262895981464, -71.08716788773268, 1642),
                Vector3.forward);
            
            _store.Store(frameDate, "exampleFrameData.json");
        }

        #endregion -----------------/Test and Support ====
        
    }
}