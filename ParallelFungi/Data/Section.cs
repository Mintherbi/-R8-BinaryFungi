using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParallelFungi.Substance;


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
        public Section(Point3d start, GrowthData GrowthData, int Section_hash)
        {
            this.start = new Point3d(start);
            path = new Vector3d(0, 0, 1) + Rand_vec();
            end = new Point3d(start + path);

            this.GrowthData = GrowthData;

            this.Section_hash = Section_hash;
        }


        //method
        public void grow()
        {
            if (fin == true)
            {
                Console.WriteLine("Branch {0} is adult. Nowhere to Grow", Section_hash);
            }
            else if (fin == false)
            {
                subseq.Add(end);
                end = subseq[subseq.Count - 1] + this.GrowthData.growth_rate * Unitize(path) + Rand_vec();
                /*
                this.subseq.Add(this.subseq[this.subseq.Count - 1] + (growth_rate * Unitize(path)) + Rand_vec());
                this.end = this.subseq[this.subseq.Count - 1];
                */
            }
        }

        public void graft(Section branch_new)
        {
            //if Section is not fully grown, set the section fully grown
            if (fin == false)
            {
                fin = true;
            }
            branch_new.path += Rand_vec();     //나중에 지우기 꼭 나중에 수정할 것
            branch.Add(branch_new.Section_hash);
        }

        public void substance_update(List<ISubstance> Substances)
        {
            Vector3d attract_grad = new Vector3d(0, 0, 0);

            foreach (var substance in Substances)
            {
                Point3d attract_point = new Point3d();

                if (substance is AttractPoint attractPoint) { attract_point = (Point3d)attractPoint.Substance; }
                else if (substance is AttractCurve attractCurve) { ((Curve) attractCurve.Substance).ClosestPoint(end); }
                else if (substance is AttractMesh attractMesh) { ((Mesh) attractMesh.Substance).ClosestPoint(end); }

                double distance = end.DistanceTo(attract_point);

                if (distance <= q_th)
                {
                    Vector3d direction = Vector3d.Subtract(new Vector3d(attract_point), new Vector3d(end));
                    direction.Unitize();

                    double force = at_co * quad_decay(end, attract_point, q_th, at_co); // 여기에 적절한 force 계산식을 사용하세요
                    attract_grad += direction * force;
                }
                else
                {
                    Vector3d direction = Vector3d.Subtract(new Vector3d(attract_point), new Vector3d(end));
                    direction.Unitize();

                    double force = Math.Min(at_co, at_co * quad_decay(end, attract_point, q_th, at_co)); // 최대 force 값을 설정
                    attract_grad += direction * force;
                }
            }

            path = path.Length * Unitize(path + Unitize(attract_grad));
        }

        public void NeighborSensing(List<Point3d> subpoint)
        {

        }


        //misc
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

        private double quad_decay(Point3d pt1, Point3d pt2, double q_th, double coef)
        {
            double dis = pt1.DistanceTo(pt2);
            if (dis <= q_th) { dis = (double)q_th; }
            return coef / (dis * dis);
        }
    }
}
