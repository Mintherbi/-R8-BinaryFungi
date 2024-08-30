using System;
using System.Collections.Generic;
using ParallelFungi.Data;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace ParallelFungi.Behavior
{
    public class GrowthBehavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public GrowthBehavior()
          : base("GrowthBehavior", "GB",
              "Set Growth Property of Fungi",
              "BinaryNature", "BinaryFungi")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Growth_Rate", "GR", "Growing Length per time", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Branch_Rate", "BR", "Growing Length per time", GH_ParamAccess.item,0.1);
            pManager.AddNumberParameter("Neighbor_Sensing_Rate", "NR", "Growing Length per time", GH_ParamAccess.item,3);
            pManager.AddNumberParameter("Sensing_Radius", "SR", "Radius of Sensing boundary", GH_ParamAccess.item,5);
            pManager.AddNumberParameter("Fusion_Radius", "FR", "Radius of Fusion boundary", GH_ParamAccess.item, 1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Growth_Behavior", "GB", "Merged Data of Growth Behavior", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region ///Set Input Param

            int Growth_Rate = new int();
            double Branch_Rate = new double();
            double neighbor_sensing_sensitivity = new double();
            double sensing_radius = new double();
            double fusion_radius = new double();

            if (!DA.GetData(0, ref Growth_Rate)) { return; }
            if (!DA.GetData(1, ref Branch_Rate)) { return; }
            if (!DA.GetData(2, ref neighbor_sensing_sensitivity)) { return; }
            if (!DA.GetData(3, ref sensing_radius)) { return; }    
            if (!DA.GetData(4, ref fusion_radius)) { return; }
            #endregion

            GrowthData Growth = new GrowthData(Growth_Rate, Branch_Rate, neighbor_sensing_sensitivity, sensing_radius, fusion_radius);

            DA.SetData(0, Growth);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        /*
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }
        */
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("FB8592F0-FCB9-43C2-B9C0-13429036E15C"); }
        }
    }
}