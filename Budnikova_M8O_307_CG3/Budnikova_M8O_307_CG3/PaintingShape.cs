using System;
using Gtk;
using Gdk;
using Cairo;
using System.Collections.Generic;

namespace Budnikova_M8O_307_CG3
{
    public class FlatShading
    {
        private List<Polygon4Vec4d> _polygons;
        
        private readonly Vector4d _lightSource;
        
        private readonly Vector4d _kA;  //kA
        private readonly Vector4d _iA; //iA

        private readonly Vector4d _iL; //iL
        private readonly Vector4d _kD;  //kD
        
        private readonly Vector4d _kS; //kS

        private readonly double _k;
        private readonly double _d;
        
        private const double P = 1;


        public FlatShading(Vector4d lightSource, Vector4d kA, Vector4d  
            iA, Vector4d iL, Vector4d kD, Vector4d kS, double k, double d)
        {
            _lightSource = lightSource;
            _kA = kA;
            _kD = kD;
            _iA = iA;
            _iL = iL;
            _kS = kS;
            _k = k;
            _d = d;
        }

        public void Paint(Context ct, Vector4d rgb, List<Polygon4Vec4d> pol, double dx, double dy)
        {
            _polygons = pol;
            _lightSource.X += dx;
            _lightSource.Y += dy;
            
            var ambient =  Vector4d.LiniarMultiply(_iA, _kA);
            var coeffDiffuse = Vector4d.LiniarMultiply(_iL,_kD);
            var coeffSpecular = Vector4d.LiniarMultiply(_iL, _kS);

            foreach (var p in _polygons)
            {
                // var p = new Polygon4Vec4d(pol);
                foreach (Vector4d v in p)
                {
                    v.X += dx;
                    v.Y += dy;
                }
                
                var l = _lightSource - p.Center;
                var norm = p.Norm;

                var s = new Vector4d(0, 0, 1, 0);
                var r = norm * (l * Math.Cos(Vector4d.Angle(norm, l) * 2)) - l;

                var cosLNorm = Vector4d.Scalar(l.Normalize(), norm);
                var cosRS = Math.Cos(Vector4d.Angle(r.Normalize(), s.Normalize()));
                
                
                var diffuse = cosLNorm >= 0 ? 
                    coeffDiffuse / (_d * l.Abs() + _k) : new Vector4d(0, 0, 0, 0);
                
                var specular = cosRS >= 0 && cosLNorm >= 0 ?
                    coeffSpecular * Math.Pow(cosRS, P) / (_d * l.Abs() + _k) : new Vector4d(0, 0, 0, 0);
                
                
                var intns = ambient + diffuse + specular;
                
                var color = Vector4d.LiniarMultiply(rgb ,intns);
                
                // Console.WriteLine($"r: {intns.X}   g: {intns.Y}   b: {intns.Z},   ");

                ct.SetSourceRGB(color.X, color.Y, color.Z);
                ct.MoveTo(p[0].X, p[0].Y);
                ct.LineTo(p[1].X, p[1].Y);
                ct.LineTo(p[2].X, p[2].Y);
                ct.LineTo(p[3].X, p[3].Y);
                ct.ClosePath();
                ct.Fill();
            }
            
            DrawMiniCircle(ct, _lightSource);
        }
        private static void DrawMiniCircle(Context c, Vector4d point)
        {
            c.SetSourceRGB(1, 1, 1);
            c.Arc(point.X, point.Y, 3, 0,  2 * Math.PI);
            c.Fill();
            c.Stroke();
        }
        
        private static void DrawLine2d(Context context, Vector4d point1, Vector4d point2)
        {
            context.SetSourceRGB(1, 1, 1);
            context.MoveTo(point1.X, point1.Y);
            context.LineTo(point2.X, point2.Y);
            context.Stroke();
        }
        
    }
}