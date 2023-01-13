using System;
using System.Collections;
using System.Collections.Generic;
using Argyle.UnclesToolkit.Base;
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
        public List<FrameObject> frameObjects = new List<FrameObject>();
        private SimpleStore _store;

        #region ==== Monobehavior ====------------------

        // Start is called before the first frame update
        void Start()
        {
            _store = new SimpleStore();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        #endregion -----------------/Monobehavior ====


        #region ==== CRUD ====------------------

        public void CreateFrame()
        {
            
        }

        public void InstantiateFrame()
        {
            
        }
        

        #endregion -----------------/CRUD ====


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