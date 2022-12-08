using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;

namespace OpenLatino.MapServer.Domain.Map.OpenMath
{
    /// <summary>
	/// Esta matriz se utiliza para realizar transformaciones afines y matriciales 
	/// en el espacio 3D.
	/// Consta de 4 filas por cuatro columnas y tiene optimizado los calculos correpondientes a 
	/// las transformaciones 2D.
	/// </summary>
    public class LtnMatrix
    {
        public class LtnMatrix3x3Solver
        {
            /// <summary>
            /// Matriz que representa la ecuacion
            /// </summary>
            double[][] matrix;
            /// <summary>
            /// Parte derecha de la ecuacion
            /// </summary>
            double[] der;
            /// <summary>
            /// Determinate
            /// </summary>
            double dm;

            /// <summary>
            /// Inicializa el sistema de ecuaciones
            /// </summary>
            /// <param name="_matrix">Matriz que representa las ecuaciones</param>
            /// <param name="_der">Parte derecha de las ecuaciones</param>
            public LtnMatrix3x3Solver(double[][] _matrix, double[] _der)
            {
                matrix = _matrix;
                der = _der;
                dm = CalcDeterminante(matrix);
                if (dm == 0) throw new Exception("No tiene solución.");
            }

            /// <summary>
            /// Metodo para calcular el determinate de una matriz
            /// </summary>
            /// <param name="m">Matriz a la que calcular el determinante</param>
            /// <returns>Valor del determinate</returns>
            /// 


            protected void SwapRows(int i, int j, ref double[][] m)
            {
                for (int k = 0; k < m[i].Length; k++)
                {
                    double temp = m[i][k];
                    m[i][k] = m[j][k];
                    m[j][k] = temp;
                }
                double tempq = auxder[i];
                auxder[i] = auxder[j];
                auxder[j] = tempq;
            }

            protected void Reordena(int i, ref double[][] m)
            {
                if (m[i][i] != 0) return;
                for (int j = i + 1; j < m.Length; j++)
                {
                    if (m[j][i] != 0)
                    {
                        SwapRows(i, j, ref m);
                    }
                }
            }

            protected void RestaFilas(int i, int j, ref double[][] m)
            {
                if ((m[i][i] == 0)) return;
                if ((m[j][i] == 0)) return;
                double scale = m[j][i] / m[i][i];
                for (int k = i; k < m.Length; k++)
                {
                    m[j][k] = m[j][k] - scale * m[i][k];
                }
                auxder[j] = auxder[j] - scale * auxder[i];
            }

            protected double[] DameDeterminante(double[][] m)
            {
                double[][] auxm = new double[m.Length][];
                for (int i = 0; i < m.Length; i++)
                {
                    auxm[i] = new double[m.Length];
                    for (int j = 0; j < m.Length; j++)
                    {
                        auxm[i][j] = m[i][j];
                    }
                }

                for (int i = 0; i < auxm.Length - 1; i++)
                {
                    Reordena(i, ref auxm);
                    if (auxm[i][i] != 0)
                    {
                        for (int j = i + 1; j < auxm.Length; j++)
                        {
                            if (auxm[j][i] != 0)
                            {
                                RestaFilas(i, j, ref auxm);
                            }
                        }
                    }
                }
                return Resuelve(auxm);
            }

            protected double[] Resuelve(double[][] m)
            {
                double[] solu = new double[m.Length];
                solu[m.Length - 1] = auxder[m.Length - 1] / m[m.Length - 1][m.Length - 1];

                for (int i = m.Length - 2; i > -1; i--)
                {
                    solu[i] = auxder[i];
                    for (int j = i + 1; j < m.Length; j++)
                    {
                        solu[i] = solu[i] - m[i][j] * solu[j];
                    }
                    solu[i] = solu[i] / m[i][i];
                }
                return solu;
            }

            public double CalcDeterminante(double[][] m)
            {
                double[][] mm = new double[3][];
                for (int i = 0; i < 3; i++)
                {
                    mm[i] = new double[3];
                    for (int j = 0; j < 3; j++)
                    {
                        mm[i][j] = m[i][j];
                    }
                }
                for (int i = 1; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        mm[i][j] = m[i][j] - m[0][j];
                    }
                }
                return (((double)mm[0][0] * (double)mm[1][1] * (double)mm[2][2]) + ((double)mm[0][1] * (double)mm[1][2] * (double)mm[2][0]) + ((double)mm[1][0] * (double)mm[0][2] * (double)mm[2][1]) -
                    ((double)mm[0][2] * (double)mm[1][1] * (double)mm[2][0]) - ((double)mm[0][1] * (double)mm[1][0] * (double)mm[2][2]) - ((double)mm[0][0] * (double)mm[1][2] * (double)mm[2][1]));
            }


            public double[] tsol2 = new double[3];
            double[] auxder;
            /// <summary>
            /// Metodo que resuelve el sistema de ecuaciones
            /// </summary>
            /// <returns>Arreglo con las soluciones</returns>
            public double[] Solve()
            {

                try
                {
                    double[][] m1 = new double[3][];
                    auxder = new double[3];
                    for (int i = 0; i < 3; i++)
                    {
                        auxder[i] = der[i];
                        m1[i] = new double[3];
                        for (int j = 0; j < 3; j++)
                        {
                            m1[i][j] = matrix[i][j];
                        }
                    }

                    return DameDeterminante(m1);
                }
                catch
                {
                }
                dm = CalcDeterminante(matrix);
                double[][] mAux = new double[3][];
                double[] d = new double[3];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        mAux[j] = new double[3];
                        for (int k = 0; k < 3; k++)
                            if (k == i) mAux[j][k] = der[j];
                            else mAux[j][k] = matrix[j][k];
                    }
                    d[i] = (double)(CalcDeterminante(mAux) / dm);
                }
                return d;
            }
        }

        internal class LtnMatrix4x4Solver
        {
            double[][] matrix;
            double[] der;

            public LtnMatrix4x4Solver(double[][] _matrix, double[] _der)
            {
                matrix = _matrix;
                der = _der;
            }


            public double[][] Menor(int index, double[][] amatrix)
            {
                double[][] result = new double[3][];
                int count = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (i != index)
                    {
                        result[count] = new double[3];
                        for (int j = 1; j < 4; j++)
                        {
                            result[count][j - 1] = amatrix[i][j];
                        }
                        count++;
                    }
                }
                return result;
            }


            public decimal CD(double[][] m)
            {
                double[][] mm = new double[3][];
                for (int i = 0; i < 3; i++)
                {
                    mm[i] = new double[3];
                    for (int j = 0; j < 3; j++)
                    {
                        mm[i][j] = m[i][j];
                    }
                }
                for (int i = 1; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        mm[i][j] = m[i][j] - m[0][j];
                    }
                }
                return (((decimal)mm[0][0] * (decimal)mm[1][1] * (decimal)mm[2][2]) + ((decimal)mm[0][1] * (decimal)mm[1][2] * (decimal)mm[2][0]) + ((decimal)mm[1][0] * (decimal)mm[0][2] * (decimal)mm[2][1]) -
                    ((decimal)mm[0][2] * (decimal)mm[1][1] * (decimal)mm[2][0]) - ((decimal)mm[0][1] * (decimal)mm[1][0] * (decimal)mm[2][2]) - ((decimal)mm[0][0] * (decimal)mm[1][2] * (decimal)mm[2][1]));
            }

            public decimal CalcDeterminante(double[][] amatrix)
            {
                decimal det = 0;
                for (int i = 0; i < 4; i++)
                {
                    if ((i == 0) || (i == 2))
                    {
                        double[][] men = Menor(i, amatrix);
                        det = det + (decimal)amatrix[i][0] * CD(men);
                    }
                    else
                    {
                        double[][] men = Menor(i, amatrix);
                        det = det - (decimal)amatrix[i][0] * CD(men);
                    }
                }
                return det;
            }

            protected void SwapRows(int i, int j, double[][] m)
            {
                for (int k = 0; k < m[i].Length; k++)
                {
                    double temp = m[i][k];
                    m[i][k] = m[j][k];
                    m[j][k] = temp;
                }
            }

            protected void Reordena(int i, double[][] m)
            {
                if (m[i][i] != 0) return;
                for (int j = i + 1; j < m.Length; j++)
                {
                    if (m[j][i] != 0)
                    {
                        SwapRows(i, j, m);
                    }
                }
            }

            protected void RestaFilas(int i, int j, double[][] m)
            {
                double scale = m[i][i] / m[j][i];
                for (int k = i; k < m.Length; k++)
                {
                    m[j][k] = m[j][k] - scale * m[i][k];
                }
            }

            protected double[] DameDeterminante(double[][] m)
            {
                double[][] auxm = new double[m.Length][];
                for (int i = 0; i < m.Length; i++)
                {
                    auxm[i] = new double[m.Length];
                    for (int j = 0; j < m.Length; j++)
                    {
                        auxm[i][j] = m[i][j];
                    }
                }

                for (int i = 0; i < auxm.Length - 1; i++)
                {
                    Reordena(i, auxm);
                    if (auxm[i][i] != 0)
                    {
                        for (int j = i + 1; j < auxm.Length; j++)
                        {
                            if (auxm[j][i] != 0)
                            {
                                RestaFilas(i, j, auxm);
                            }
                        }
                    }
                }
                return Resuelve(auxm);
            }

            protected double[] Resuelve(double[][] m)
            {
                double[] solu = new double[m.Length];
                solu[m.Length - 1] = this.der[m.Length - 1] / m[m.Length - 1][m.Length - 1];

                for (int i = m.Length - 2; i > -1; i--)
                {
                    solu[i] = der[i];
                    for (int j = i + 1; j < m.Length; j++)
                    {
                        solu[i] = solu[i] - m[i][j] * solu[j];
                    }
                    solu[i] = solu[i] / m[i][i];
                }
                return solu;
            }


            public double[] Solve()
            {

                try
                {
                    double[][] m1 = new double[4][];
                    for (int i = 0; i < 4; i++)
                    {
                        m1[i] = new double[4];
                        for (int j = 0; j < 4; j++)
                        {
                            m1[i][j] = matrix[i][j];
                        }
                    }
                    return DameDeterminante(m1);
                }
                catch
                {
                }
                decimal dm = CalcDeterminante(matrix);
                double[][] mAux = new double[4][];
                double[] d = new double[4];

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        mAux[j] = new double[4];
                        for (int k = 0; k < 4; k++)
                            if (k == i) mAux[j][k] = der[j];
                            else mAux[j][k] = matrix[j][k];
                    }
                    d[i] = (double)(CalcDeterminante(mAux) / dm);
                }
                return d;
            }
        }

        public double m00; public double m01; public double m02; public double m03;
        public double m10; public double m11; public double m12; public double m13;
        public double m20; public double m21; public double m22; public double m23;
        public double m30; public double m31; public double m32; public double m33;

        public LtnMatrix()
        {
            Reset();
        }

        /// <summary>
		/// Setea nuevamente la matriz identidad.
		/// </summary>
		public void Reset()
        {
            m00 = 1; m01 = 0; m02 = 0; m03 = 0;
            m10 = 0; m11 = 1; m12 = 0; m13 = 0;
            m20 = 0; m21 = 0; m22 = 1; m23 = 0;
            m30 = 0; m31 = 0; m32 = 0; m33 = 1;
        }

        public void SetERT(Point p1, Point p2, Point p3, Point pp1, Point pp2, Point pp3)
        {
            double[][] izq = new double[3][];
            double[] der = new double[3];
            izq[0] = new double[3];

            izq[0][0] = p1.X;
            izq[0][1] = p1.Y;
            izq[0][2] = 1;

            izq[1] = new double[3];
            izq[1][0] = p2.X;
            izq[1][1] = p2.Y;
            izq[1][2] = 1;

            izq[2] = new double[3];
            izq[2][0] = p3.X;
            izq[2][1] = p3.Y;
            izq[2][2] = 1;

            der[0] = pp1.X;
            der[1] = pp2.X;
            der[2] = pp3.X;

            LtnMatrix3x3Solver ms = new LtnMatrix3x3Solver(izq, der);
            double[] solvx;
            solvx = ms.Solve();
            //{
            //    solvx = ms.Solve();
            //}
            //catch (Exception)
            //{
            //    return LtnRESULT.LR_Error;
            //}

            der[0] = pp1.Y;
            der[1] = pp2.Y;
            der[2] = pp3.Y;
            LtnMatrix3x3Solver ms1 = new LtnMatrix3x3Solver(izq, der);
            double[] solvy;
            solvy = ms1.Solve();
            //try
            //{
            //    solvy = ms1.Solve();
            //}
            //catch (Exception)
            //{
            //    return LtnRESULT.LR_Error;
            //}

            this.Reset();
            m00 = solvx[0]; m01 = solvx[1]; m03 = solvx[2];
            m10 = solvy[0]; m11 = solvy[1]; m13 = solvy[2];
        }

        public virtual Point TransformPoint(Point ps)
        {
            Point r = new Point(ps.X, ps.Y, ps.Z, ps.M);
            double auxx = ps.X * m00 + ps.Y * m01 + m03;
            double auxy = ps.X * m10 + ps.Y * m11 + m13;

            r.X = auxx;
            r.Y = auxy;

            return r;
        }
    }
}
