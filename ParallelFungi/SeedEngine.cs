using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace BinaryNature
{
    public class ParallelFungi : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public ParallelFungi()
          : base("ParallelFungi","ParallelFungi" ,
            "CUDA Accelerated Fungus growth Simulation",
            "BinaryNature", "Fungus")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("start", "S", "Starting Point of Fungus", GH_ParamAccess.item, new Point3d(0, 0, 0));      //0
            pManager.AddPointParameter("avoid_pt", "Av_p", "Avoiding Point List", GH_ParamAccess.list, null);        //1
            pManager.AddPointParameter("attract_pt", "At_p", "Attacting Point List", GH_ParamAccess.list, null);         //2
            pManager.AddNumberParameter("avoid_coef", "Ac_co", "Avoid Strength of Avoiding Point List", GH_ParamAccess.item, 10);       //3
            pManager.AddNumberParameter("attract_coef", "At_co", "Attract Strength of Attracting Point List", GH_ParamAccess.item, 10);            //4
            pManager.AddNumberParameter("neighbor_avoid_coef", "NAc_co", "Neighbor sensing avoid coef", GH_ParamAccess.item, 3);       //5
            pManager.AddIntegerParameter("growth_rate", "Gr_r", "Growing Lenght per Time Delta", GH_ParamAccess.item, 1);           //6
            pManager.AddNumberParameter("branch_rate", "Br_r", "Branch Possibility of Section", GH_ParamAccess.item, 0.1);          //7
            pManager.AddNumberParameter("quad_decay threshold", "q_th", "quad decay threshold", GH_ParamAccess.item);          //8
            pManager.AddBooleanParameter("reset", "reset", "Reset to Initialize Parameter", GH_ParamAccess.item, false);            //9
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Branch_pt", "B_pt", "End Point of Section", GH_ParamAccess.list);           //0
            pManager.AddPointParameter("subpoint", "sub_pt", "Subpoint of Section", GH_ParamAccess.list);           //1
            pManager.AddPointParameter("subline", "sub_ln", "Lines of Section", GH_ParamAccess.tree);            //2
            pManager.AddIntegerParameter("delta_t", "dt", "Time Pass", GH_ParamAccess.item);            //3
        }

        List<Section> fungus;
        int delta;
        int Section_num;
        int branch_num;

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region ///Set Input Parameter 

            Point3d start = new Point3d();
            List<Point3d> avoid_pt = new List<Point3d>();
            List<Point3d> attract_pt = new List<Point3d>();
            double avoid_coef = new double();
            double attract_coef = new double();
            double Neighbor_attract_coef = new double();
            int growth_rate = new int();
            double branch_rate = new double();
            double q_th = new double();
            bool reset = new bool();

            if (!DA.GetData(0, ref start)) { return; }
            if (!DA.GetDataList(1, avoid_pt)) { return; }
            if (!DA.GetDataList(2, attract_pt)) { return; }
            if (!DA.GetData(3, ref avoid_coef)) { return; }
            if (!DA.GetData(4, ref attract_coef)) { return; }
            if (!DA.GetData(5, ref attract_coef)) { return; }
            if (!DA.GetData(6, ref growth_rate)) { return; }
            if (!DA.GetData(7, ref branch_rate)) { return; }
            if (!DA.GetData(8, ref q_th)) { return; }
            if (!DA.GetData(9, ref reset)) { return; }
            #endregion

            Random branch_rand = new Random();

            if (reset == false)
            {
                fungus = new List<Section>();
                Section first = new Section(start, 0);      // starting point, hash of first fungi = 0
                fungus.Add(first);
                delta = 0;
                Section_num = 1;
                branch_num = 0;
            }

            List<Point3d> subpoint = new List<Point3d>();
            List<Point3d> branch_pt = new List<Point3d>();
            DataTree<Point3d> section_crv = new DataTree<Point3d>();

            for (int i = 0; i < fungus.Count; i++)
            {
                if (fungus[i].fin == false)
                {
                    if (branch_rand.NextDouble() < branch_rate)
                    {
                        Section branch1 = new Section(fungus[i].end, Section_num);
                        Section branch2 = new Section(fungus[i].end, Section_num + 1);

                        fungus[i].graft(branch1);
                        fungus[i].graft(branch2);

                        Section_num += 2;

                        fungus.Add(branch1);
                        fungus.Add(branch2);

                        branch_num++;
                    }

                    else
                    {
                        if (avoid_pt.Count != 0)
                        {
                            fungus[i].avoid_path_update(avoid_pt, avoid_coef, q_th);
                        }
                        if (attract_pt.Count != 0)
                        {
                            fungus[i].attract_path_update(attract_pt, attract_coef, q_th);
                        }
                    }
                    fungus[i].avoid_path_update(subpoint, Neighbor_attract_coef, q_th);
                    fungus[i].grow(growth_rate);
                }
            }

            for (int j = 0; j < fungus.Count; j++)
            {
                branch_pt.Add(fungus[j].end);
                if (fungus[j].subseq.Count != 0)
                {
                    List<Point3d> sec = new List<Point3d> ();
                    sec.Add(fungus[j].start);
                    sec.AddRange(fungus[j].subseq);
                    sec.Add(fungus[j].end);
                    section_crv.AddRange(sec, new Grasshopper.Kernel.Data.GH_Path(j));
                }
                for (int k = 0; k < fungus[j].subseq.Count; k++)
                {
                    subpoint.Add(fungus[j].subseq[k]);
                }
            }

            delta++;

            DA.SetDataList(0, branch_pt);
            DA.SetDataList(1, subpoint);
            DA.SetDataTree(2, section_crv);
            DA.SetData(3, delta);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
       /// protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c1824cd3-f4cf-4d7b-8822-5dac070a787b");
    }
}