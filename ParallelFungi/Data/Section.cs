using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParallelFungi.Substance;
using Rhino.Render.CustomRenderMeshes;


namespace ParallelFungi.Data
{
    class Section
    {
        //Properties : Geometry
        public Point3d start;      //starting point of section
        public Point3d end;      //end point of section : null=growing, else=end of growth
        public List<Point3d> subseq = new List<Point3d>();      //sequence of growth by time t
        public Vector3d path;     //growing vetor
        public List<int> branch = new List<int>();     //branch of section : null=growing, else=end
        public bool fin = false;     // 0:growing 1:end of growth

        //Properties : Growth Property
        public GrowthData GrowthData;

        public int Section_hash;      // Section code
        public int thickness;

        //construct

        /// <summary>
        /// Start of Section
        /// </summary>
        /// <param name="start"> 
        /// <param name="GrowthData"></param>
        /// <param name="Section_hash"></param>
        public Section(Point3d start, GrowthData GrowthData, int Section_hash)
        {
            this.start = new Point3d(start);
            path = new Vector3d(0, 0, 1) + Rand_vec();
            end = new Point3d(start + path);

            this.GrowthData = GrowthData;

            this.Section_hash = Section_hash;
        }


        //method
        /// <summary>
        /// Growth function of fungi.
        /// The Growth will stop if Section.fin == False
        /// </summary>
        public Point3d Grow()
        {
            if (fin == true)
            {
                Console.WriteLine("Branch {0} is adult. Nowhere to Grow", Section_hash);
            }
            else if (fin == false)
            {
                subseq.Add(end);
                end = subseq[subseq.Count - 1] + this.GrowthData.growth_rate * Unitize(path) + Rand_vec();
            }

            return this.end;
        }

        /// <summary>
        /// Finish Growth if Section.fin == False
        /// </summary>
        /// <param name="branch_new"></param>
        public void Graft(Section branch_new)
        {
            //if Section is not fully grown, set the section fully grown
            if (fin == false)
            {
                fin = true;
            }
            branch_new.path += Rand_vec();     //나중에 지우기 꼭 나중에 수정할 것
            branch.Add(branch_new.Section_hash);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Substances"></param>
        public void substance_update(List<ISubstance> Substances)
        {
            Vector3d attract_grad = new Vector3d(0, 0, 0);

            ///Travers Substance. Check type of Substance
            foreach (var substance in Substances)
            {
                Point3d attract_point = new Point3d();

                if (substance is AttractPoint attractPoint) 
                { 
                    attract_point = (Point3d)attractPoint.Substance;
                }
                else if (substance is AttractCurve attractCurve) 
                {
                    double t = new double();        //hyperparameter for location on curve
                    ((Curve) attractCurve.Substance).ClosestPoint(end, out t);
                    attract_point = ((Curve)attractCurve.Substance).PointAt(t);
                }
                else if (substance is AttractMesh attractMesh) 
                { 
                    ((Mesh) attractMesh.Substance).ClosestPoint(end, out attract_point, 0.0); 
                }

                double distance = end.DistanceTo(attract_point);

                attract_grad += attract_point - this.end;
            }

            path = path.Length * Unitize(path + Unitize(attract_grad));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subpoint"></param>
        public void NeighborSensing(RTree subSequence,List<Point3d> subSequenceList)
        {
            List<Point3d> SequenceInRadius = new List<Point3d>();

            EventHandler<RTreeEventArgs> searchCallback = (object sender, RTreeEventArgs args) =>
            {
                if (this.end.DistanceTo(subSequenceList[args.Id]) <= this.GrowthData.sensing_radius)
                {
                    SequenceInRadius.Add(subSequenceList[args.Id]);
                }
            };

            // RTree를 이용해 반경 r 내의 점 검색
            subSequence.Search(new Sphere(this.end, this.GrowthData.sensing_radius), searchCallback);

            Vector3d temp = new Vector3d(0,0,0);

            foreach (var Neighbor in SequenceInRadius)
            {
                temp += quad_decay(this.end, Neighbor) * (this.end - Neighbor);
            }
            path += temp;
        }

        public void Fusion()
        {
            
        }


        ///misc
        private Vector3d Unitize(Vector3d Vec)
        {
            return Vec / Vec.Length;
        }

        private Vector3d Rand_vec()
        {
            Random rand = new Random();
            Vector3d rand_vec;

            double rand_X = rand.NextDouble();
            double rand_Y = rand.NextDouble();
            double rand_Z = rand.NextDouble();

            rand_vec = new Vector3d(rand_X, rand_Y, rand_Z);

            return Unitize(rand_vec);
        }

        private double quad_decay(Point3d pt1, Point3d pt2)
        {
            double dis = pt1.DistanceTo(pt2);
            if (dis <= this.GrowthData.fusion_radius) { dis = this.GrowthData.fusion_radius; }
            return this.GrowthData.neighbor_sensing_sensitivity / (dis * dis);
        }
    }
}
