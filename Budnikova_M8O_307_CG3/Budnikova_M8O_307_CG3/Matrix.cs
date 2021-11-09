using System;

namespace Budnikova_M8O_307_CG3
{
    public class Angle
    {
        internal static Angle Instance { get; } = new Angle();
        public static double Sin(double angle)
        {
            return Math.Sin(angle * Math.PI / 180);
        }
        public static double Cos(double angle)
        {
            return Math.Cos(angle * Math.PI / 180);
        }
        public static double Tng(double angle)
        {
            return Math.Tan(angle * Math.PI / 180);
        }
        public static double Ctg(double angle)
        {
            return 1 / Math.Tan(angle * Math.PI / 180);
        }

    }


    public class Vector2d
    {
        private readonly double[] _vector2d;

        public double X
        {
            get => _vector2d[0];
            set => _vector2d[0] = value;
        }

        public double Y
        {
            get => _vector2d[1];
            set => _vector2d[1] = value;
        }

        public Vector2d(double x, double y)
        {
            _vector2d = new[] {x, y};
        }

        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            return new Vector2d(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2d operator /(Vector2d a, double b)
        {
            return new Vector2d(a.X / b, a.Y / b);
        }
        public static Vector2d operator +(Vector2d a, double b)
        {
            return new Vector2d(a.X + b, a.Y + b);
        }
        public static Vector2d operator *(Vector2d a, double b)
        {
            return new Vector2d(a.X * b, a.Y * b);
        }
    }

    public class Vector4d
    {
        private readonly double[] _vector4d;

        public Vector4d(double x, double y, double z, double w)
        {
            _vector4d = new[] { x, y, z, w};
        }

        public Vector4d(Vector4d v)
        {
            _vector4d = new[] { v.X, v.Y, v.Z, v.W };
        }


        public double this[int i]
        {
            get => _vector4d[i];
            set => _vector4d[i] = value;
        }

        public double X
        {
            get => _vector4d[0];
            set => _vector4d[0] = value;
        }

        public double Y
        {
            get => _vector4d[1];
            set => _vector4d[1] = value;
        }
        public double Z
        {
            get => _vector4d[2];
            set => _vector4d[2] = value;
        }
        public double W
        {
            get => _vector4d[3];
            set => _vector4d[3] = value;
        }


        public static Vector4d operator +(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.X + b.X,
                                a.Y + b.Y,
                                a.Z + b.Z,
                                a.W);
        }
        public static Vector4d operator +(Vector4d a, double b)
        {
            return new Vector4d(a.X + b,
                                a.Y + b,
                                a.Z + b,
                                a.W);
        }
        public static Vector4d operator -(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.X - b.X,
                                a.Y - b.Y,
                                a.Z - b.Z,
                                a.W - b.W);
        }
        public static Vector4d operator /(Vector4d a, double b)
        {
            return new Vector4d(a.X / b,
                                a.Y / b,
                                a.Z / b,
                                a.W);
        }
        public static Vector4d operator *(Vector4d a, Vector4d b)
        {
            return new Vector4d(a.Y * b.Z - a.Z * b.Y,
                                a.Z * b.X - a.X * b.Z,
                                a.X * b.Y - a.Y * b.X,
                                0);
        }
        public static Vector4d operator *(Vector4d a, double b)
        {
            return new Vector4d(a.X * b,
                                a.Y * b,
                                a.Z * b,
                                0);
        }
        public static double Scalar(Vector4d a, Vector4d b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        public double Abs()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        public double Sum_Coordinates()
        {
            return Y;
        }
        
    }

    public class Matrix4d
    {
        private readonly double[,] _matrix;

        private double this[int i, int j]
        {
            get => _matrix[i, j];
            set => _matrix[i, j] = value;
        }

        private Matrix4d(double[,] val)
        {
            _matrix = new double[4, 4];
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    _matrix[i, j] = val[i, j];
                }
            }
        }

        public static Matrix4d operator *(Matrix4d m1, Matrix4d m2)
        {
            Matrix4d m = new(new double[,] { { 0, 0, 0, 0 },
                                                { 0, 0, 0, 0 },
                                                { 0, 0, 0, 0 },
                                                { 0, 0, 0, 0 } });

            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    m[i, j] = m1[i, 0] * m2[0, j] + m1[i, 1] * m2[1, j] + m1[i, 2] * m2[2, j] + m1[i, 3] * m2[3, j];
                }
            }


            return m;
        }

        public static Vector4d operator *(Matrix4d m, Vector4d a)
        {
            Vector4d vec = new(0, 0, 0, 0);

            for (int i = 0; i < 4; ++i)
            {
                vec[i] = m[i, 0] * a.X + m[i, 1] * a.Y + m[i, 2] * a.Z + m[i, 3] * a.W;
            }


            return vec;
        }

        private static Matrix4d Rot_X(double angle)
        {
            return new Matrix4d(new[,] { {1, 0, 0, 0},
                                                   {0, Angle.Cos(angle), -Angle.Sin(angle), 0},
                                                   {0, Angle.Sin(angle), Angle.Cos(angle), 0},
                                                   {0, 0, 0, 1}});
        }

        private static Matrix4d Rot_Y(double angle)
        {
            return new Matrix4d(new[,] { {Angle.Cos(angle), 0, Angle.Sin(angle), 0},
                                                   {0, 1, 0, 0},
                                                   {-Angle.Sin(angle), 0, Angle.Cos(angle), 0},
                                                   {0, 0, 0, 1}});
        }

        private static Matrix4d Rot_Z(double angle)
        {
            return new Matrix4d(new[,] { {Angle.Cos(angle), -Angle.Sin(angle), 0, 0},
                                                   {Angle.Sin(angle), Angle.Cos(angle), 0, 0},
                                                   {0, 0, 1, 0},
                                                   {0, 0, 0, 1}});
        }


        public static Matrix4d Rot_XYZ(double angleX, double angleY, double angleZ)
        {
            return Rot_X(angleX) * Rot_Y(angleY) * Rot_Z(angleZ);
        }

        public static Matrix4d Scale_XYZ(double scaleX, double scaleY, double scaleZ)
        {
            return new Matrix4d(new[,] { {scaleX, 0, 0, 0},
                                                   {0, scaleY, 0, 0},
                                                   {0, 0, scaleZ, 0},
                                                   {0, 0, 0, 1}});
        }

        public static Matrix4d Move_XYZ(double dx, double dy, double dz)
        {
            return new Matrix4d(new[,] { {1, 0, 0, dx},
                                                   {0, 1, 0, dy},
                                                   {0, 0, 1, dz},
                                                   {0, 0, 0, 1}});
        }

        public static Matrix4d Proection_XY()
        {
            return new Matrix4d(new double[,] { {1, 0, 0, 0},
                                                   {0, 1, 0, 0},
                                                   {0, 0, 0, 0},
                                                   {0, 0, 0, 1}});
        }
        public static Matrix4d Proection_XZ()
        {
            return new Matrix4d(new double[,] { {1, 0, 0, 0},
                                                   {0, 0, 1, 0},
                                                   {0, 0, 0, 0},
                                                   {0, 0, 0, 1}});
        }
        public static Matrix4d Proection_YZ()
        {
            return new Matrix4d(new double[,] { {0, 0, 1, 0},
                                                   {0, 1, 0, 0},
                                                   {0, 0, 0, 0},
                                                   {0, 0, 0, 1}});
        }

        public static Matrix4d Izometric(double left = -1, double right = 1, double bottom = -1, double top = 1, double near = 1, double far = 8)
        {
            return new Matrix4d(new[,] { {2 * near / (right - left), 0, (right + left) / (right - left), 0},
                                                   {0, 2 * near / (top - bottom), (top + bottom) / (top - bottom), 0},
                                                   {0, 0, -(far + near) / (far - near), -2 * near * far / (far - near)},
                                                   {0, 0, 0, 1}});
        }
    }
}