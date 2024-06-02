using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public class GeometryNames
    {
        const string shortMoment = "M";
        const string shortLongForce = "N";
        const string shortShearForce = "Q";
        const string fullMoment = "Moment M";
        const string fullLongForce = "Force N";
        const string fullShearForce = "Force Q";
        const string curvature = "K";
        const string strain = "Eps_";
        public string FstAxisName => "x";
        public string SndAxisName => "y";
        public string TrdAxisName => "z";
        public string MomFstName => shortMoment + FstAxisName;
        public string MomSndName => shortMoment + SndAxisName;
        public string LongForceName => shortLongForce + TrdAxisName;
        public string FullMomFstName => fullMoment + FstAxisName;
        public string FullMomSndName => fullMoment + SndAxisName;
        public string FullLongForceName => fullLongForce + TrdAxisName;
        public string CurvFstName => curvature + FstAxisName;
        public string CurvSndName => curvature + SndAxisName;
        public string StrainTrdName => strain + TrdAxisName;
    }
}
