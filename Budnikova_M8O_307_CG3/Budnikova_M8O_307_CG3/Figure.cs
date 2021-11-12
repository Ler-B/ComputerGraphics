using System;
using System.Collections.Generic;
using System.Linq;
using Line2d = System.Collections.Generic.List<Budnikova_M8O_307_CG3.Vector2d>;

namespace Budnikova_M8O_307_CG3
{
    public class Figure
    {

        private readonly Vector4d _center,
                                  _xAxis,
                                  _yAxis,
                                  _zAxis;

        private readonly List<Polygon4Vec4d> _polygons = new();
        private readonly List<Line4d> _axis;

        public Figure(double r, double meridians, double parallels, double x = 0, double y = 0, double z = 0, double axisLen = 50)
        {
            //FGURE POINTS

            List<Vector4d> oneCirclePoints = new();
            List<List<Vector4d>> allPoints = new();

            for (double p = 0; p <= parallels; ++p)
            {
                double theta = Math.PI * p / parallels;
                for (double m = 1; m <= meridians; ++m)
                {
                    double phi = Math.PI * 2 * m / meridians;

                    double x1 = r * Math.Sin(theta) * Math.Cos(phi);
                    double y1 = r * Math.Sin(theta) * Math.Sin(phi);
                    double z1 = r * Math.Cos(theta);
                    double w1 = 1;
                    oneCirclePoints.Add(new Vector4d(x1, y1, z1, w1));
                }
                allPoints.Add(new List<Vector4d>(oneCirclePoints));
                oneCirclePoints.Clear();


                /*
                      meridians
                p     ------------------
                a    |      circle1     |
                r    |      circle2     |
                a    |      circle3     |
                l    |         .        |
                e    |         .        |
                l    |         .        |
                s    |      circle(n)   |
                      ------------------
                
                */
            }

            To_Polygons(allPoints);


            //AXIS
            _center = new Vector4d(x, y, z, 1);
            _xAxis = new Vector4d(x + axisLen, y, z, 1);
            _yAxis = new Vector4d(x, y - axisLen, z, 1);
            _zAxis = new Vector4d(x, y, z - axisLen, 1);

            _axis = Get_Axis();
        }

        private void To_Polygons(List<List<Vector4d>> points)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = 0; j < points[i].Count; ++j)
                {
                    if (i == points.Count - 1 && j == 0) break;
                    
                    int iInd = (i + 1) % points.Count;
                    int jInd = (j + 1) % points[i].Count;

                    Vector4d point1 = points[i][j];
                    Vector4d point2 = points[i][jInd];
                    Vector4d point3 = points[iInd][j];
                    Vector4d point4 = points[iInd][jInd];
                    
                    _polygons.Add(new Polygon4Vec4d(point1, point2, point4, point3));
                    
                }
                
            }
        }

        private List<Line4d> Get_Axis()
        {
            Line4d x = new(_center, _xAxis);
            Line4d y = new(_center, _yAxis);
            Line4d z = new(_center, _zAxis);

            return new List<Line4d> { x, y, z };
        }

        public void Rotation_XYZ(double angleX, double angleY, double angleZ)
        {
            foreach (var p in _polygons)
            {
                p.Modify(Matrix4d.Rot_XYZ(angleX, angleY, angleZ));
            }

            foreach (var p in _axis)
            {
                p.Modify(Matrix4d.Rot_XYZ(angleX, angleY, angleZ));
            }
        }
        
        public void Scale_XYZ(double scaleX, double scaleY, double scaleZ)
        {
            foreach (var p in _polygons)
            {
                p.Modify(Matrix4d.Scale_XYZ(scaleX, scaleY, scaleZ));
            }
        }

        public void Move_XYZ(double dx, double dy, double dz)
        {
            foreach (var p in _polygons)
            {
                p.Modify(Matrix4d.Move_XYZ(dx, dy, dz));
            }
        }

        public List<Line2d> Get_XYZ_Points_Without_Invisible_Lines(double dx = 0, double dy = 0)
        {
            List<Line2d> rezList = new();

            List<Polygon4Vec4d> zBuffPol = HideInvLine();

            foreach (var p in zBuffPol)
            {
                rezList.Add(Get_2d_line(p[0], p[1], dx, dy));
                rezList.Add(Get_2d_line(p[1], p[2], dx, dy));
                rezList.Add(Get_2d_line(p[2], p[3], dx, dy));
                rezList.Add(Get_2d_line(p[3], p[0], dx, dy));
            }
            return rezList;
        }

        public List<Polygon4Vec4d> HideInvLine()
        {
            List<Polygon4Vec4d> newPoints = new();
            
            foreach (var p in _polygons)
            {
                if (Vector4d.Angle(p.Norm, new Vector4d(0, 0, -1, 1)) < Math.PI / 2)
                {
                    newPoints.Add(new Polygon4Vec4d(p));
                }
            }

            return newPoints;
        }
        
        public List<Line2d> Get_XYZ_Points(double dx = 0, double dy = 0)
        {
            List<Line2d> rezList = new();
            foreach (var p in _polygons)
            {
                rezList.Add(Get_2d_line(p[0], p[1], dx, dy));
                rezList.Add(Get_2d_line(p[1], p[2], dx, dy));
                rezList.Add(Get_2d_line(p[2], p[3], dx, dy));
                rezList.Add(Get_2d_line(p[3], p[0], dx, dy));
            }
            return rezList;
        }

        static Line2d Get_2d_line(Vector4d p1, Vector4d p2, double dx = 0, double dy = 0)
        {
            double x1 = p1.X + dx;
            double y1 = p1.Y + dy;

            double x2 = p2.X + dx;
            double y2 = p2.Y + dy;
            return new Line2d(new Vector2d(x1, y1), new Vector2d(x2, y2));
        }

        private List<Line2d> Projection(double dx, double dy, Matrix4d m)
        {
            List<Line2d> rezList = new();

            List<Polygon4Vec4d> newPoints = new();

            foreach (var p in _polygons)
            {
                newPoints.Add(p.New_Modify(m));
            }

            foreach (var p in newPoints)
            {
                rezList.InsertRange(rezList.Count, p.To_Line_2d(dx, dy));
            }

            return rezList;
        }

        public List<Line2d> Get_XY_Points(double dx = 0, double dy = 0)
        {
            return Projection(dx, dy, Matrix4d.Proection_XY());
        }

        public List<Line2d> Get_XZ_Points(double dx = 0, double dy = 0)
        {
            return Projection(dx, dy, Matrix4d.Proection_XZ());
        }

        public List<Line2d> Get_YZ_Points(double dx = 0, double dy = 0)
        {
            return Projection(dx, dy, Matrix4d.Proection_YZ());
        }

        public List<Line2d> Get_XYZ_Axis(double dx = 0, double dy = 0)
        {
            List<Line2d> rezList = new();

            foreach (var p in _axis)
            {
                Line2d newLine = new(new Vector2d(p[0].X + dx, p[0].Y + dy), new Vector2d(p[1].X + dx, p[1].Y + dy));
                rezList.Add(newLine);
            }

            return rezList;
        }

        public List<Line2d> Get_XYZ_Normals(double dx = 0, double dy = 0)
        {
            List<Line2d> rezList = new();

            List<Polygon4Vec4d> newPolygons = HideInvLine();

            foreach (Polygon4Vec4d p in newPolygons)
            {
                Vector4d norm = p.Norm;

                double l2 = new[] {p[0].Abs(), p[1].Abs(), p[2].Abs(), p[3].Abs()}.Min() / 4;

                Vector4d p1 = (p[0] + p[1] + p[2] + p[3]) / 4;
                Vector4d p2 = p1 + norm * l2;

                Line2d newLine = new(new Vector2d(p1.X + dx, p1.Y + dy), new Vector2d(p2.X + dx, p2.Y + dy));
                rezList.Add(newLine);
            }
 
            return rezList;
        }
        public List<Polygon4Vec4d> Get_Polygons(double dx = 0, double dy = 0, double dz = 0)
        {
            List<Polygon4Vec4d> newPoligons = _polygons;

            foreach (Polygon4Vec4d p in newPoligons)
            {
                foreach (Vector4d v in p)
                {
                    v.X += dx;
                    v.Y += dy;
                    v.Z += dz;
                }
                
            }

            return newPoligons;
        }
        public List<Line2d> Get_Izometric(double dx = 0, double dy = 0)
        {
            List<Line2d> rezList = new();

            List<Polygon4Vec4d> iList = new();
            
            iList.AddRange(_polygons.Select(p => p.New_Modify(Matrix4d.Izometric())));

            foreach (var p in iList)
            {
                rezList = p.To_Line_2d(dx, dy);
            }

            return rezList;
        }

    }

}



