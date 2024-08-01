using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using ParallelFungi.Substance;

namespace ParallelFungi.Behavior
{
    public class SubstanceProperty : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public SubstanceProperty()
          : base("Substance", "S",
              "Substance that can change environment",
              "BinaryNature", "BinaryFungi")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Substance", "S", "Location of Substance", GH_ParamAccess.item);
            pManager.AddNumberParameter("Force", "F", "Strength of Substance", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Treshold", "T", "Threshold of Distance", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Direction", "D", "True : Attact | False : Repel", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SubstanceProperty", "SB", "Property of substance", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region ///Set Input Paramter

            Object Substance = null;
            double force = 0.0;
            int threshold = 0;
            bool direction = true;

            if (!DA.GetData(0, ref Substance)) { return; }
            if (!DA.GetData(1, ref force)) { return; }
            if (!DA.GetData(2, ref threshold)) { return; }
            if (!DA.GetData(3, ref direction)) { return; }

            if (Substance is Point3d)
            {
                DA.SetData(0, AttractPoint(Substance, ))
            }
            else if (Substance is Curve)
            {
                // x가 Curve 타입이면 이 블록이 실행됩니다.
            }
            else if (Substance is Mesh)
            {
                // x가 Mesh 타입이면 이 블록이 실행됩니다.
            }
            else
            {
                // x가 Point3d, Curve, Mesh 어느 것도 아니면 이 블록이 실행됩니다.
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("DE751F48-3C16-4F2B-9ABD-7A544862BBA4"); }
        }
    }
}