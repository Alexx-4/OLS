using System;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;

namespace TileGenerator.Domain.Entities
{
    public class LtnMatrix
    {
        public double m00; public double m01; public double m02; public double m03;
        public double m10; public double m11; public double m12; public double m13;
        public double m20; public double m21; public double m22; public double m23;
        public double m30; public double m31; public double m32; public double m33;

        public bool modified = true;
        private double angx, angy, angz;
        private double psx, psy, psz;

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

        protected void DameDeterminante(double[][] m)
        {
            double[][] auxm = new double[m.Length][];
            for (int i = 0; i < m.Length; i++)
            {
                for (int j = 0; j < m.Length; j++)
                {
                    auxm[i][j] = m[i][j];
                }
            }

            for (int i = 0; i < m.Length - 1; i++)
            {
                Reordena(i, auxm);
                if (m[i][i] != 0)
                {
                    for (int j = i + 1; j < m.Length; j++)
                    {
                        if (m[j][i] != 0)
                        {
                            RestaFilas(i, j, m);
                        }
                    }
                }
            }
        }

        private double det
        {
            get
            {
                if ((angy != 0) || (angx != 0))
                {
                    double value;
                    value =
                        m03 * m12 * m21 * m30 - m02 * m13 * m21 * m30 - m03 * m11 * m22 * m30 + m01 * m13 * m22 * m30 +
                        m02 * m11 * m23 * m30 - m01 * m12 * m23 * m30 - m03 * m12 * m20 * m31 + m02 * m13 * m20 * m31 +
                        m03 * m10 * m22 * m31 - m00 * m13 * m22 * m31 - m02 * m10 * m23 * m31 + m00 * m12 * m23 * m31 +
                        m03 * m11 * m20 * m32 - m01 * m13 * m20 * m32 - m03 * m10 * m21 * m32 + m00 * m13 * m21 * m32 +
                        m01 * m10 * m23 * m32 - m00 * m11 * m23 * m32 - m02 * m11 * m20 * m33 + m01 * m12 * m20 * m33 +
                        m02 * m10 * m21 * m33 - m00 * m12 * m21 * m33 - m01 * m10 * m22 * m33 + m00 * m11 * m22 * m33;
                    return value;
                }
                else
                {
                    return (m00 * m11 - m10 * m01);
                }

            }
        }


        /// <summary>
        /// Constructor de la matriz.
        /// </summary>
        /// <param name="_m">Recibe un arreglo de 16 parametros en forma de lista por filas de arriba hacia abajo.</param>
        public LtnMatrix(double[] _m)
        {
            m00 = _m[0]; m01 = _m[1]; m02 = _m[2]; m03 = _m[3];
            m10 = _m[4]; m11 = _m[5]; m12 = _m[6]; m13 = _m[7];
            m20 = _m[8]; m21 = _m[9]; m22 = _m[10]; m23 = _m[11];
            m30 = _m[12]; m31 = _m[13]; m32 = _m[14]; m33 = _m[15];
            //OffSetX = (m03 / m00);
            //OffSetY = (m13 / m11);
            this.psx = oldsx;
            this.psy = oldsy;
            this.psz = oldsz;
           
        }

        /// <summary>
        /// Constructor que construye la matriz Identidad.
        /// </summary>
        public LtnMatrix()
        {
            Reset();
        }

        private int NRaised = 0;
        private double[] BackupElements;
      

        /// <summary>
        /// Setea nuevamente la matriz identidad.
        /// </summary>
        public void Reset()
        {
           

            m00 = 1; m01 = 0; m02 = 0; m03 = 0;
            m10 = 0; m11 = 1; m12 = 0; m13 = 0;
            m20 = 0; m21 = 0; m22 = 1; m23 = 0;
            m30 = 0; m31 = 0; m32 = 0; m33 = 1;
            this.angx = 0;
            this.angy = 0;
            this.angz = 0;
            this.modified = false;
            this.psx = 1;
            this.psy = 1;
            this.psz = 1;
           
        }

        int fp, i;
        int NVertex = 0;
        int X, Y, Xold, Yold;
        int Xnext = 0, Ynext = 0;
        int Xdif, Ydif;

        /// <summary>
        /// Metodo que aplica la matriz actual a un LtnPoint3D sin tener en cuenta la componente Z.
        /// </summary>
        /// <param name="p">Punto a transformar.</param>
        /// <returns>Devuelve un nuevo punto tranformado.</returns>
        public virtual Point TransformPoint(Point p)
        {
            Point ps = p;
            long auxx;
            long auxy;
            //long auxz;
            auxx = (long)Math.Round(((double)(ps.X * m00) + (double)(ps.Y * m01) + /*(double)(ps.z * m02)*/+m03));
            auxy = (long)Math.Round(((double)(ps.X * m10) + (double)(ps.Y * m11) + /*(double)(ps.z * m12)*/+m13));
            //auxz = (long)((double)(ps.x * m20) + (double)(ps.y * m21)+ (double)(ps.z * m22)+ m23);

            ps.X = auxx;
            ps.Y = auxy;
            //ps.z = auxz;
            return ps;
        }


        /// <summary>
        /// Verdadero si la matriz es invertible.
        /// </summary>
        public bool IsInvertible
        {
            get
            {
                return (det != 0);
            }
        }

        /// <summary>
        /// Devuelve el determinante.
        /// </summary>
        /// <returns></returns>
        public double Determinante()
        {
            return det;
        }

        /// <summary>
        /// invierte de ser posible la matriz actual.
        /// </summary>
        public void Invert()
        {

            if (this.IsInvertible)
            {
                if ((this.angy != 0) || (angx != 0))
                {
                    double deti = Determinante();
                    double t00 = m12 * m23 * m31 - m13 * m22 * m31 + m13 * m21 * m32 - m11 * m23 * m32 - m12 * m21 * m33 + m11 * m22 * m33;
                    double t01 = m03 * m22 * m31 - m02 * m23 * m31 - m03 * m21 * m32 + m01 * m23 * m32 + m02 * m21 * m33 - m01 * m22 * m33;
                    double t02 = m02 * m13 * m31 - m03 * m12 * m31 + m03 * m11 * m32 - m01 * m13 * m32 - m02 * m11 * m33 + m01 * m12 * m33;
                    double t03 = m03 * m12 * m21 - m02 * m13 * m21 - m03 * m11 * m22 + m01 * m13 * m22 + m02 * m11 * m23 - m01 * m12 * m23;
                    double t10 = m13 * m22 * m30 - m12 * m23 * m30 - m13 * m20 * m32 + m10 * m23 * m32 + m12 * m20 * m33 - m10 * m22 * m33;
                    double t11 = m02 * m23 * m30 - m03 * m22 * m30 + m03 * m20 * m32 - m00 * m23 * m32 - m02 * m20 * m33 + m00 * m22 * m33;
                    double t12 = m03 * m12 * m30 - m02 * m13 * m30 - m03 * m10 * m32 + m00 * m13 * m32 + m02 * m10 * m33 - m00 * m12 * m33;
                    double t13 = m02 * m13 * m20 - m03 * m12 * m20 + m03 * m10 * m22 - m00 * m13 * m22 - m02 * m10 * m23 + m00 * m12 * m23;
                    double t20 = m11 * m23 * m30 - m13 * m21 * m30 + m13 * m20 * m31 - m10 * m23 * m31 - m11 * m20 * m33 + m10 * m21 * m33;
                    double t21 = m03 * m21 * m30 - m01 * m23 * m30 - m03 * m20 * m31 + m00 * m23 * m31 + m01 * m20 * m33 - m00 * m21 * m33;
                    double t22 = m01 * m13 * m30 - m03 * m11 * m30 + m03 * m10 * m31 - m00 * m13 * m31 - m01 * m10 * m33 + m00 * m11 * m33;
                    double t23 = m03 * m11 * m20 - m01 * m13 * m20 - m03 * m10 * m21 + m00 * m13 * m21 + m01 * m10 * m23 - m00 * m11 * m23;
                    double t30 = m12 * m21 * m30 - m11 * m22 * m30 - m12 * m20 * m31 + m10 * m22 * m31 + m11 * m20 * m32 - m10 * m21 * m32;
                    double t31 = m01 * m22 * m30 - m02 * m21 * m30 + m02 * m20 * m31 - m00 * m22 * m31 - m01 * m20 * m32 + m00 * m21 * m32;
                    double t32 = m02 * m11 * m30 - m01 * m12 * m30 - m02 * m10 * m31 + m00 * m12 * m31 + m01 * m10 * m32 - m00 * m11 * m32;
                    double t33 = m01 * m12 * m20 - m02 * m11 * m20 + m02 * m10 * m21 - m00 * m12 * m21 - m01 * m10 * m22 + m00 * m11 * m22;
                    m00 = t00 / deti;
                    m01 = t01 / deti;
                    m02 = t02 / deti;
                    m03 = t03 / deti;
                    m10 = t10 / deti;
                    m11 = t11 / deti;
                    m12 = t12 / deti;
                    m13 = t13 / deti;
                    m20 = t20 / deti;
                    m21 = t21 / deti;
                    m22 = t22 / deti;
                    m23 = t23 / deti;
                    m30 = t30 / deti;
                    m31 = t31 / deti;
                    m32 = t32 / deti;
                    m33 = t33 / deti;

                    modified = true;
                }
                else
                {
                    double deti = det;
                    double t00 = m11 / deti;
                    double t10 = -m10 / deti;
                    double t01 = -m01 / deti;
                    double t11 = m00 / deti;
                    double t03 = (m01 * m13 - m03 * m11) / deti;
                    double t13 = (m03 * m10 - m00 * m13) / deti;
                    m00 = t00;
                    m10 = t10;
                    m01 = t01;
                    m11 = t11;
                    m03 = t03;
                    m13 = t13;
                }
            }
           
        }

        internal double oldsx = 1;
        internal double oldsy = 1;
        internal double oldsz = 1;

        /// <summary>
        /// Setea los valores de una matriz con una rotacion y un escalado especificado a partir de la matriz identidad.
        /// </summary>
        /// <param name="sx">Escala por la X.</param>
        /// <param name="sy">Escala por la Y.</param>
        /// <param name="angle">Valor del angulo de rotacion.</param>
        /// <param name="center">Centro de rotacion.</param>
        /// <param name="center_image">Centro en coordenadas del visor.</param>
        //public void SetERT(double sx, double sy, double angle, LtnPoint3D center, LtnPoint3D center_image)
        //{
        //    RaiseBeforeChangeEvent();

        //    Reset();
        //    SetERT3D(sx, sy, 1, 0, 0, angle, center, center_image);
        //    OffSetX = (m03 / m00);
        //    OffSetY = (m13 / m11);

        //    RaiseAfterChangeEvent();
        //}

        public void SetERT(double sx, double sy, double angle, double centerx, double centery, double centerz, double center_imagex, double center_imagey, double center_imagez)
        {
            Reset();
            SetERT3D(sx, sy, 1, 0, 0, angle, centerx, centery, centerz, center_imagex, center_imagey, center_imagez);
            //OffSetX = (m03 / m00);
            //OffSetY = (m13 / m11);
        }

        /// <summary>
		/// Setea los valores de una matriz con una rotacion y un escalado especificado a partir de la matriz identidad.
		/// </summary>
		/// <param name="sx">Escala por la X.</param>
		/// <param name="sy">Escala por la Y.</param>
		/// <param name="sz">Escala por la Z.</param>
		/// <param name="_x">Angulo en X:</param>
		/// <param name="_y">Angulo en Y:</param>
		/// <param name="_z">Angulo en Z:</param>
		/// <param name="center">Centro de la pantalla en coordenadas mundo.</param>
		/// <param name="centerimg">Centro en coordenadas pantalla.</param>
		//public void SetERT3D(double sx, double sy, double sz, double _x, double _y, double _z, LtnPoint3D center, LtnPoint3D centerimg)
  //      {
  //          double senx, cosx, seny, cosy, senz, cosz, a1, a2, a3;

  //          RaiseBeforeChangeEvent();

  //          Reset();
  //          senx = Math.Sin(_x); cosx = Math.Cos(_x);
  //          seny = Math.Sin(_y); cosy = Math.Cos(_y);
  //          senz = Math.Sin(_z); cosz = Math.Cos(_z);

  //          m00 = cosy * cosz - senx * seny * senz;
  //          m01 = cosy * senz + senx * seny * cosz;
  //          m02 = -cosx * seny;

  //          m10 = -cosx * senz;
  //          m11 = cosx * cosz;
  //          m12 = senx;

  //          m20 = seny * cosz + senx * cosy * senz;
  //          m21 = seny * senz - senx * cosy * cosz;
  //          m22 = cosx * cosy;

  //          double temp;
  //          temp = m01;
  //          m01 = m10;
  //          m10 = temp;

  //          temp = m20;
  //          m20 = m02;
  //          m02 = temp;

  //          temp = m21;
  //          m21 = m12;
  //          m12 = temp;

  //          a1 = center.x;
  //          a2 = center.y;
  //          a3 = center.z;

  //          m32 = 0.0;

  //          m00 *= sx;
  //          m01 *= sx;
  //          m02 *= sx;
  //          m10 *= sy;
  //          m11 *= sy;
  //          m12 *= sy;
  //          m20 *= sz;
  //          m21 *= sz;
  //          m22 *= sz;

  //          m03 = -a1 * m00 - a2 * m01 - a3 * m02 + centerimg.x;
  //          m13 = -a1 * m10 - a2 * m11 - a3 * m12 + centerimg.y;
  //          m23 = -a1 * m20 - a2 * m21 - a3 * m22 + centerimg.z;
  //          m33 = 1.0;

  //          modified = false;
  //          psx = sx; psy = sy; psz = sz;
  //          angx = _x;
  //          angy = _y;
  //          angz = _z;
            

  //          RaiseAfterChangeEvent();

  //      }

        public void SetERT3D(double sx, double sy, double sz, double _x, double _y, double _z, double centerx, double centery, double centerz, double centerimgx, double centerimgy, double centerimgz)
        {
            double senx, cosx, seny, cosy, senz, cosz, a1, a2, a3;

           

            Reset();
            senx = Math.Sin(_x); cosx = Math.Cos(_x);
            seny = Math.Sin(_y); cosy = Math.Cos(_y);
            senz = Math.Sin(_z); cosz = Math.Cos(_z);

            m00 = cosy * cosz - senx * seny * senz;
            m01 = cosy * senz + senx * seny * cosz;
            m02 = -cosx * seny;

            m10 = -cosx * senz;
            m11 = cosx * cosz;
            m12 = senx;

            m20 = seny * cosz + senx * cosy * senz;
            m21 = seny * senz - senx * cosy * cosz;
            m22 = cosx * cosy;

            double temp;
            temp = m01;
            m01 = m10;
            m10 = temp;

            temp = m20;
            m20 = m02;
            m02 = temp;

            temp = m21;
            m21 = m12;
            m12 = temp;

            a1 = centerx;
            a2 = centery;
            a3 = centerz;

            m32 = 0.0;

            m00 *= sx;
            m01 *= sx;
            m02 *= sx;
            m10 *= sy;
            m11 *= sy;
            m12 *= sy;
            m20 *= sz;
            m21 *= sz;
            m22 *= sz;

            m03 = -a1 * m00 - a2 * m01 - a3 * m02 + centerimgx;
            m13 = -a1 * m10 - a2 * m11 - a3 * m12 + centerimgy;
            m23 = -a1 * m20 - a2 * m21 - a3 * m22 + centerimgz;
            m33 = 1.0;

            modified = false;
            psx = sx; psy = sy; psz = sz;
            angx = _x;
            angy = _y;
            angz = _z;
           

        }











    }
}
