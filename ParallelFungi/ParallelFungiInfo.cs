using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ParallelFungi
{
    public class ParallelFungiInfo : GH_AssemblyInfo
    {
        public override string Name => "ParallelFungi";

        //Return a 24x24 pixel bitmap to represent this GHA library.
       // public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("321ed77b-17a5-4a45-9d97-b1703797c8f7");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}