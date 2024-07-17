using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ParallelFungi.Data
{
    class Section
    {
        //Properties
        public Point3d start;      //starting point of section
        public Point3d end;      //end point of section : null=growing, else=end of growth
        public List<Point3d> subseq = new List<Point3d>();      //sequence of growth by time t
        public Vector3d path;     //growing vetor
        public List<int> branch = new List<int>();     //branch of section : null=growing, else=end
        public bool fin = false;     // 0:growing 1:end of growth
        public int Section_hash;      // Section code

        //construct
        public Section(Point3d start, int Section_hash)
        {
            this.start = new Point3d(start);
            path = new Vector3d(0, 0, 1) + Rand_vec();
            end = new Point3d(start + path);
            this.Section_hash = Section_hash;
        }


        //method
        public void grow(double growth_rate)
        {
            if (fin == true)
            {
                Console.WriteLine("Branch {0} is adult. Nowhere to Grow", Section_hash);
            }
            else if (fin == false)
            {
                subseq.Add(end);
                end = subseq[subseq.Count - 1] + growth_rate * Unitize(path) + Rand_vec();
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


        public void avoid_path_update(List<Point3d> avoid, double av_co, double q_th)
        {
            if (avoid.Count != 0)
            {
                Vector3d avoid_grad = new Vector3d(0, 0, 0);

                for (int i = 0; i < avoid.Count; i++)
                {
                    avoid_grad += Unitize(Vector3d.Subtract(new Vector3d(avoid[i]), new Vector3d(end))) * quad_decay(end, avoid[i], q_th, av_co);
                    //avoid_grad += Unitize(Vector3d.Subtract(new Vector3d(avoid[i]), new Vector3d(this.end))) * sqrt_decay(this.end, avoid[i], s_th) * i_factor;
                }

                path = path.Length * Unitize(path + Unitize(avoid_grad));
            }
        }

        public void attract_path_update(List<Point3d> attract, double at_co, double q_th)
        {
            Vector3d attract_grad = new Vector3d(0, 0, 0);

            for (int i = 0; i < attract.Count; i++)
            {
                attract_grad += Unitize(Vector3d.Subtract(new Vector3d(attract[i]), new Vector3d(end))) * quad_decay(end, attract[i], q_th, at_co);
                //attract_grad += Unitize(Vector3d.Subtract(new Vector3d(attract[i]), new Vector3d(this.end))) * sqrt_decay(this.end, attract[i], s_th) * i_factor;
            }

            path = path.Length * Unitize(path + Unitize(attract_grad));
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

        /*
        private double sqrt_decay(Point3d pt1,  Point3d pt2, int s_th) 
        {
            double dis = pt1.DistanceTo(pt2);
            if (dis <= s_th) { dis = (double) s_th; }
            return 1/Math.Sqrt(dis);
        }
        */
    }
}
