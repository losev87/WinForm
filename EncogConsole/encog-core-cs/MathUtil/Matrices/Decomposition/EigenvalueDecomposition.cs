//
// Encog(tm) Core v3.3 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2014 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;
using Encog.Util;

namespace Encog.MathUtil.Matrices.Decomposition
{
    /// <summary>
    /// Eigenvalues and eigenvectors of a real matrix.
    /// 
    /// If A is symmetric, then A = V*D*V' where the eigenvalue matrix D is diagonal
    /// and the eigenvector matrix V is orthogonal. I.e. A =
    /// V.times(D.times(V.transpose())) and V.times(V.transpose()) equals the
    /// identity matrix.
    ///
    /// If A is not symmetric, then the eigenvalue matrix D is block diagonal with
    /// the real eigenvalues in 1-by-1 blocks and any complex eigenvalues, lambda +
    /// i*mu, in 2-by-2 blocks, [lambda, mu; -mu, lambda]. The columns of V represent
    /// the eigenvectors in the sense that A*V = V*D, i.e. A.times(V) equals
    /// V.times(D). The matrix V may be badly conditioned, or even singular, so the
    /// validity of the equation A = V*D*inverse(V) depends upon V.cond().
    /// 
    /// This file based on a class from the public domain JAMA package.
    /// http://math.nist.gov/javanumerics/jama/
    /// </summary>
    public class EigenvalueDecomposition
    {
        /// <summary>
        /// Arrays for internal storage of eigenvalues.
        /// </summary>
        private readonly double[] d;

        /// <summary>
        /// Arrays for internal storage of eigenvalues.
        /// </summary>
        private readonly double[] e;

        /// <summary>
        /// Array for internal storage of nonsymmetric Hessenberg form.
        /// </summary>
        private readonly double[][] h;

        /// <summary>
        /// Symmetry flag.
        /// </summary>
        private readonly bool issymmetric;

        /// <summary>
        /// Row and column dimension (square matrix).
        /// </summary>
        private readonly int n;

        /// <summary>
        /// Working storage for nonsymmetric algorithm.
        /// </summary>
        private readonly double[] ort;

        /// <summary>
        /// Array for internal storage of eigenvectors.
        /// </summary>
        private readonly double[][] v;

        /// <summary>
        /// Complex scalar division.
        /// </summary>
        private double cdivi;

        /// <summary>
        /// Complex scalar division.
        /// </summary>
        private double cdivr;

        /// <summary>
        /// Check for symmetry, then construct the eigenvalue decomposition
        /// </summary>
        /// <param name="matrix">Square matrix</param>
        public EigenvalueDecomposition(Matrix matrix)
        {
            double[][] a = matrix.Data;
            n = matrix.Cols;
            v = EngineArray.AllocateDouble2D(n, n);
            d = new double[n];
            e = new double[n];

            issymmetric = true;
            for (int j = 0; (j < n) & issymmetric; j++)
            {
                for (int i = 0; (i < n) & issymmetric; i++)
                {
                    issymmetric = (a[i][j] == a[j][i]);
                }
            }

            if (issymmetric)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        v[i][j] = a[i][j];
                    }
                }

                // Tridiagonalize.
                Tred2();

                // Diagonalize.
                Tql2();
            }
            else
            {
                h = EngineArray.AllocateDouble2D(n, n);
                ort = new double[n];

                for (int j = 0; j < n; j++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        h[i][j] = a[i][j];
                    }
                }

                // Reduce to Hessenberg form.
                Orthes();

                // Reduce Hessenberg to real Schur form.
                Hqr2();
            }
        }


        /// <summary>
        /// Return the eigenvector matrix.
        /// </summary>
        public Matrix V
        {
            get { return new Matrix(v); }
        }

        /// <summary>
        /// Return the real parts of the eigenvalues.
        /// </summary>
        public double[] RealEigenvalues
        {
            get { return d; }
        }

        /// <summary>
        /// Return the block diagonal eigenvalue matrix
        /// </summary>
        public Matrix D
        {
            get
            {
                var result = new Matrix(n, n);
                double[][] resultMatrix = result.Data;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        resultMatrix[i][j] = 0.0;
                    }
                    resultMatrix[i][i] = d[i];
                    if (e[i] > 0)
                    {
                        resultMatrix[i][i + 1] = e[i];
                    }
                    else if (e[i] < 0)
                    {
                        resultMatrix[i][i - 1] = e[i];
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Symmetric Householder reduction to tridiagonal form.
        /// </summary>
        private void Tred2()
        {
            // This is derived from the Algol procedures tred2 by
            // Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            // Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            // Fortran subroutine in EISPACK.

            for (int j = 0; j < n; j++)
            {
                d[j] = v[n - 1][j];
            }

            // Householder reduction to tridiagonal form.

            for (int i = n - 1; i > 0; i--)
            {
                // Scale to avoid under/overflow.

                double scale = 0.0;
                double h = 0.0;
                for (int k = 0; k < i; k++)
                {
                    scale = scale + Math.Abs(d[k]);
                }
                if (scale == 0.0)
                {
                    e[i] = d[i - 1];
                    for (int j = 0; j < i; j++)
                    {
                        d[j] = v[i - 1][j];
                        v[i][j] = 0.0;
                        v[j][i] = 0.0;
                    }
                }
                else
                {
                    // Generate Householder vector.

                    for (int k = 0; k < i; k++)
                    {
                        d[k] /= scale;
                        h += d[k]*d[k];
                    }
                    double f = d[i - 1];
                    double g = Math.Sqrt(h);
                    if (f > 0)
                    {
                        g = -g;
                    }
                    e[i] = scale*g;
                    h = h - f*g;
                    d[i - 1] = f - g;
                    for (int j = 0; j < i; j++)
                    {
                        e[j] = 0.0;
                    }

                    // Apply similarity transformation to remaining columns.

                    for (int j = 0; j < i; j++)
                    {
                        f = d[j];
                        v[j][i] = f;
                        g = e[j] + v[j][j]*f;
                        for (int k = j + 1; k <= i - 1; k++)
                        {
                            g += v[k][j]*d[k];
                            e[k] += v[k][j]*f;
                        }
                        e[j] = g;
                    }
                    f = 0.0;
                    for (int j = 0; j < i; j++)
                    {
                        e[j] /= h;
                        f += e[j]*d[j];
                    }
                    double hh = f/(h + h);
                    for (int j = 0; j < i; j++)
                    {
                        e[j] -= hh*d[j];
                    }
                    for (int j = 0; j < i; j++)
                    {
                        f = d[j];
                        g = e[j];
                        for (int k = j; k <= i - 1; k++)
                        {
                            v[k][j] -= (f*e[k] + g*d[k]);
                        }
                        d[j] = v[i - 1][j];
                        v[i][j] = 0.0;
                    }
                }
                d[i] = h;
            }

            // Accumulate transformations.

            for (int i = 0; i < n - 1; i++)
            {
                v[n - 1][i] = v[i][i];
                v[i][i] = 1.0;
                double h = d[i + 1];
                if (h != 0.0)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        d[k] = v[k][i + 1]/h;
                    }
                    for (int j = 0; j <= i; j++)
                    {
                        double g = 0.0;
                        for (int k = 0; k <= i; k++)
                        {
                            g += v[k][i + 1]*v[k][j];
                        }
                        for (int k = 0; k <= i; k++)
                        {
                            v[k][j] -= g*d[k];
                        }
                    }
                }
                for (int k = 0; k <= i; k++)
                {
                    v[k][i + 1] = 0.0;
                }
            }
            for (int j = 0; j < n; j++)
            {
                d[j] = v[n - 1][j];
                v[n - 1][j] = 0.0;
            }
            v[n - 1][n - 1] = 1.0;
            e[0] = 0.0;
        }

        /// <summary>
        /// Symmetric tridiagonal QL algorithm.
        /// </summary>
        private void Tql2()
        {
            // This is derived from the Algol procedures tql2, by
            // Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            // Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            // Fortran subroutine in EISPACK.

            for (int i = 1; i < n; i++)
            {
                e[i - 1] = e[i];
            }
            e[n - 1] = 0.0;

            double f = 0.0;
            double tst1 = 0.0;
            double eps = Math.Pow(2.0, -52.0);
            for (int l = 0; l < n; l++)
            {
                // Find small subdiagonal element

                tst1 = Math.Max(tst1, Math.Abs(d[l]) + Math.Abs(e[l]));
                int m = l;
                while (m < n)
                {
                    if (Math.Abs(e[m]) <= eps*tst1)
                    {
                        break;
                    }
                    m++;
                }

                // If m == l, d[l] is an eigenvalue,
                // otherwise, iterate.

                if (m > l)
                {
                    int iter = 0;
                    do
                    {
                        iter = iter + 1; // (Could check iteration count here.)

                        // Compute implicit shift

                        double g = d[l];
                        double p = (d[l + 1] - g)/(2.0*e[l]);
                        double r = EncogMath.Hypot(p, 1.0);
                        if (p < 0)
                        {
                            r = -r;
                        }
                        d[l] = e[l]/(p + r);
                        d[l + 1] = e[l]*(p + r);
                        double dl1 = d[l + 1];
                        double h = g - d[l];
                        for (int i = l + 2; i < n; i++)
                        {
                            d[i] -= h;
                        }
                        f = f + h;

                        // Implicit QL transformation.

                        p = d[m];
                        double c = 1.0;
                        double c2 = c;
                        double c3 = c;
                        double el1 = e[l + 1];
                        double s = 0.0;
                        double s2 = 0.0;
                        for (int i = m - 1; i >= l; i--)
                        {
                            c3 = c2;
                            c2 = c;
                            s2 = s;
                            g = c*e[i];
                            h = c*p;
                            r = EncogMath.Hypot(p, e[i]);
                            e[i + 1] = s*r;
                            s = e[i]/r;
                            c = p/r;
                            p = c*d[i] - s*g;
                            d[i + 1] = h + s*(c*g + s*d[i]);

                            // Accumulate transformation.

                            for (int k = 0; k < n; k++)
                            {
                                h = v[k][i + 1];
                                v[k][i + 1] = s*v[k][i] + c*h;
                                v[k][i] = c*v[k][i] - s*h;
                            }
                        }
                        p = -s*s2*c3*el1*e[l]/dl1;
                        e[l] = s*p;
                        d[l] = c*p;

                        // Check for convergence.
                    } while (Math.Abs(e[l]) > eps*tst1);
                }
                d[l] = d[l] + f;
                e[l] = 0.0;
            }

            // Sort eigenvalues and corresponding vectors.

            for (int i = 0; i < n - 1; i++)
            {
                int k = i;
                double p = d[i];
                for (int j = i + 1; j < n; j++)
                {
                    if (d[j] < p)
                    {
                        k = j;
                        p = d[j];
                    }
                }
                if (k != i)
                {
                    d[k] = d[i];
                    d[i] = p;
                    for (int j = 0; j < n; j++)
                    {
                        p = v[j][i];
                        v[j][i] = v[j][k];
                        v[j][k] = p;
                    }
                }
            }
        }

        /// <summary>
        /// This is derived from the Algol procedures orthes and ortran, by Martin
        /// and Wilkinson, Handbook for Auto. Comp., Vol.ii-Linear Algebra, and the
        /// corresponding Fortran subroutines in EISPACK.
        /// </summary>
        private void Orthes()
        {
            int low = 0;
            int high = n - 1;

            for (int m = low + 1; m <= high - 1; m++)
            {
                // Scale column.

                double scale = 0.0;
                for (int i = m; i <= high; i++)
                {
                    scale = scale + Math.Abs(h[i][m - 1]);
                }
                if (scale != 0.0)
                {
                    // Compute Householder transformation.

                    double lh = 0.0;
                    for (int i = high; i >= m; i--)
                    {
                        ort[i] = h[i][m - 1]/scale;
                        lh += ort[i]*ort[i];
                    }
                    double g = Math.Sqrt(lh);
                    if (ort[m] > 0)
                    {
                        g = -g;
                    }
                    lh = lh - ort[m]*g;
                    ort[m] = ort[m] - g;

                    // Apply Householder similarity transformation
                    // H = (I-u*u'/h)*H*(I-u*u')/h)

                    for (int j = m; j < n; j++)
                    {
                        double f = 0.0;
                        for (int i = high; i >= m; i--)
                        {
                            f += ort[i]*h[i][j];
                        }
                        f = f/lh;
                        for (int i = m; i <= high; i++)
                        {
                            h[i][j] -= f*ort[i];
                        }
                    }

                    for (int i = 0; i <= high; i++)
                    {
                        double f = 0.0;
                        for (int j = high; j >= m; j--)
                        {
                            f += ort[j]*h[i][j];
                        }
                        f = f/lh;
                        for (int j = m; j <= high; j++)
                        {
                            h[i][j] -= f*ort[j];
                        }
                    }
                    ort[m] = scale*ort[m];
                    h[m][m - 1] = scale*g;
                }
            }

            // Accumulate transformations (Algol's ortran).

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    v[i][j] = (i == j ? 1.0 : 0.0);
                }
            }

            for (int m = high - 1; m >= low + 1; m--)
            {
                if (h[m][m - 1] != 0.0)
                {
                    for (int i = m + 1; i <= high; i++)
                    {
                        ort[i] = h[i][m - 1];
                    }
                    for (int j = m; j <= high; j++)
                    {
                        double g = 0.0;
                        for (int i = m; i <= high; i++)
                        {
                            g += ort[i]*v[i][j];
                        }
                        // Double division avoids possible underflow
                        g = (g/ort[m])/h[m][m - 1];
                        for (int i = m; i <= high; i++)
                        {
                            v[i][j] += g*ort[i];
                        }
                    }
                }
            }
        }

        private void cdiv(double xr, double xi, double yr, double yi)
        {
            double r, d;
            if (Math.Abs(yr) > Math.Abs(yi))
            {
                r = yi/yr;
                d = yr + r*yi;
                cdivr = (xr + r*xi)/d;
                cdivi = (xi - r*xr)/d;
            }
            else
            {
                r = yr/yi;
                d = yi + r*yr;
                cdivr = (r*xr + xi)/d;
                cdivi = (r*xi - xr)/d;
            }
        }

        /// <summary>
        /// This is derived from the Algol procedure hqr2, by Martin and Wilkinson,
        /// Handbook for Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
        /// Fortran subroutine in EISPACK.
        /// </summary>
        private void Hqr2()
        {
            // Initialize
            int nn = this.n;
            int n = nn - 1;
            int low = 0;
            int high = nn - 1;
            double eps = Math.Pow(2.0, -52.0);
            double exshift = 0.0;
            double p = 0, q = 0, r = 0, s = 0, z = 0, t, w, x, y;

            // Store roots isolated by balanc and compute matrix norm

            double norm = 0.0;
            for (int i = 0; i < nn; i++)
            {
                if (i < low | i > high)
                {
                    d[i] = h[i][i];
                    e[i] = 0.0;
                }
                for (int j = Math.Max(i - 1, 0); j < nn; j++)
                {
                    norm = norm + Math.Abs(h[i][j]);
                }
            }

            // Outer loop over eigenvalue index

            int iter = 0;
            while (n >= low)
            {
                // Look for single small sub-diagonal element

                int l = n;
                while (l > low)
                {
                    s = Math.Abs(h[l - 1][l - 1]) + Math.Abs(h[l][l]);
                    if (s == 0.0)
                    {
                        s = norm;
                    }
                    if (Math.Abs(h[l][l - 1]) < eps*s)
                    {
                        break;
                    }
                    l--;
                }

                // Check for convergence
                // One root found

                if (l == n)
                {
                    h[n][n] = h[n][n] + exshift;
                    d[n] = h[n][n];
                    e[n] = 0.0;
                    n--;
                    iter = 0;

                    // Two roots found
                }
                else if (l == n - 1)
                {
                    w = h[n][n - 1]*h[n - 1][n];
                    p = (h[n - 1][n - 1] - h[n][n])/2.0;
                    q = p*p + w;
                    z = Math.Sqrt(Math.Abs(q));
                    h[n][n] = h[n][n] + exshift;
                    h[n - 1][n - 1] = h[n - 1][n - 1] + exshift;
                    x = h[n][n];

                    // Real pair

                    if (q >= 0)
                    {
                        if (p >= 0)
                        {
                            z = p + z;
                        }
                        else
                        {
                            z = p - z;
                        }
                        d[n - 1] = x + z;
                        d[n] = d[n - 1];
                        if (z != 0.0)
                        {
                            d[n] = x - w/z;
                        }
                        e[n - 1] = 0.0;
                        e[n] = 0.0;
                        x = h[n][n - 1];
                        s = Math.Abs(x) + Math.Abs(z);
                        p = x/s;
                        q = z/s;
                        r = Math.Sqrt(p*p + q*q);
                        p = p/r;
                        q = q/r;

                        // Row modification

                        for (int j = n - 1; j < nn; j++)
                        {
                            z = h[n - 1][j];
                            h[n - 1][j] = q*z + p*h[n][j];
                            h[n][j] = q*h[n][j] - p*z;
                        }

                        // Column modification

                        for (int i = 0; i <= n; i++)
                        {
                            z = h[i][n - 1];
                            h[i][n - 1] = q*z + p*h[i][n];
                            h[i][n] = q*h[i][n] - p*z;
                        }

                        // Accumulate transformations

                        for (int i = low; i <= high; i++)
                        {
                            z = v[i][n - 1];
                            v[i][n - 1] = q*z + p*v[i][n];
                            v[i][n] = q*v[i][n] - p*z;
                        }

                        // Complex pair
                    }
                    else
                    {
                        d[n - 1] = x + p;
                        d[n] = x + p;
                        e[n - 1] = z;
                        e[n] = -z;
                    }
                    n = n - 2;
                    iter = 0;

                    // No convergence yet
                }
                else
                {
                    // Form shift

                    x = h[n][n];
                    y = 0.0;
                    w = 0.0;
                    if (l < n)
                    {
                        y = h[n - 1][n - 1];
                        w = h[n][n - 1]*h[n - 1][n];
                    }

                    // Wilkinson's original ad hoc shift

                    if (iter == 10)
                    {
                        exshift += x;
                        for (int i = low; i <= n; i++)
                        {
                            h[i][i] -= x;
                        }
                        s = Math.Abs(h[n][n - 1]) + Math.Abs(h[n - 1][n - 2]);
                        x = y = 0.75*s;
                        w = -0.4375*s*s;
                    }

                    // MATLAB's new ad hoc shift

                    if (iter == 30)
                    {
                        s = (y - x)/2.0;
                        s = s*s + w;
                        if (s > 0)
                        {
                            s = Math.Sqrt(s);
                            if (y < x)
                            {
                                s = -s;
                            }
                            s = x - w/((y - x)/2.0 + s);
                            for (int i = low; i <= n; i++)
                            {
                                h[i][i] -= s;
                            }
                            exshift += s;
                            x = y = w = 0.964;
                        }
                    }

                    iter = iter + 1; // (Could check iteration count here.)

                    // Look for two consecutive small sub-diagonal elements

                    int m = n - 2;
                    while (m >= l)
                    {
                        z = h[m][m];
                        r = x - z;
                        s = y - z;
                        p = (r*s - w)/h[m + 1][m] + h[m][m + 1];
                        q = h[m + 1][m + 1] - z - r - s;
                        r = h[m + 2][m + 1];
                        s = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);
                        p = p/s;
                        q = q/s;
                        r = r/s;
                        if (m == l)
                        {
                            break;
                        }
                        if (Math.Abs(h[m][m - 1])*(Math.Abs(q) + Math.Abs(r)) < eps
                            *(Math.Abs(p)*(Math.Abs(h[m - 1][m - 1])
                                           + Math.Abs(z) + Math.Abs(h[m + 1][m + 1]))))
                        {
                            break;
                        }
                        m--;
                    }

                    for (int i = m + 2; i <= n; i++)
                    {
                        h[i][i - 2] = 0.0;
                        if (i > m + 2)
                        {
                            h[i][i - 3] = 0.0;
                        }
                    }

                    // Double QR step involving rows l:n and columns m:n

                    for (int k = m; k <= n - 1; k++)
                    {
                        bool notlast = (k != n - 1);
                        if (k != m)
                        {
                            p = h[k][k - 1];
                            q = h[k + 1][k - 1];
                            r = (notlast ? h[k + 2][k - 1] : 0.0);
                            x = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);
                            if (x != 0.0)
                            {
                                p = p/x;
                                q = q/x;
                                r = r/x;
                            }
                        }
                        if (x == 0.0)
                        {
                            break;
                        }
                        s = Math.Sqrt(p*p + q*q + r*r);
                        if (p < 0)
                        {
                            s = -s;
                        }
                        if (s != 0)
                        {
                            if (k != m)
                            {
                                h[k][k - 1] = -s*x;
                            }
                            else if (l != m)
                            {
                                h[k][k - 1] = -h[k][k - 1];
                            }
                            p = p + s;
                            x = p/s;
                            y = q/s;
                            z = r/s;
                            q = q/p;
                            r = r/p;

                            // Row modification

                            for (int j = k; j < nn; j++)
                            {
                                p = h[k][j] + q*h[k + 1][j];
                                if (notlast)
                                {
                                    p = p + r*h[k + 2][j];
                                    h[k + 2][j] = h[k + 2][j] - p*z;
                                }
                                h[k][j] = h[k][j] - p*x;
                                h[k + 1][j] = h[k + 1][j] - p*y;
                            }

                            // Column modification

                            for (int i = 0; i <= Math.Min(n, k + 3); i++)
                            {
                                p = x*h[i][k] + y*h[i][k + 1];
                                if (notlast)
                                {
                                    p = p + z*h[i][k + 2];
                                    h[i][k + 2] = h[i][k + 2] - p*r;
                                }
                                h[i][k] = h[i][k] - p;
                                h[i][k + 1] = h[i][k + 1] - p*q;
                            }

                            // Accumulate transformations

                            for (int i = low; i <= high; i++)
                            {
                                p = x*v[i][k] + y*v[i][k + 1];
                                if (notlast)
                                {
                                    p = p + z*v[i][k + 2];
                                    v[i][k + 2] = v[i][k + 2] - p*r;
                                }
                                v[i][k] = v[i][k] - p;
                                v[i][k + 1] = v[i][k + 1] - p*q;
                            }
                        } // (s != 0)
                    } // k loop
                } // check convergence
            } // while (n >= low)

            // Backsubstitute to find vectors of upper triangular form

            if (norm == 0.0)
            {
                return;
            }

            for (n = nn - 1; n >= 0; n--)
            {
                p = d[n];
                q = e[n];

                // Real vector

                if (q == 0)
                {
                    int l = n;
                    h[n][n] = 1.0;
                    for (int i = n - 1; i >= 0; i--)
                    {
                        w = h[i][i] - p;
                        r = 0.0;
                        for (int j = l; j <= n; j++)
                        {
                            r = r + h[i][j]*h[j][n];
                        }
                        if (e[i] < 0.0)
                        {
                            z = w;
                            s = r;
                        }
                        else
                        {
                            l = i;
                            if (e[i] == 0.0)
                            {
                                if (w != 0.0)
                                {
                                    h[i][n] = -r/w;
                                }
                                else
                                {
                                    h[i][n] = -r/(eps*norm);
                                }

                                // Solve real equations
                            }
                            else
                            {
                                x = h[i][i + 1];
                                y = h[i + 1][i];
                                q = (d[i] - p)*(d[i] - p) + e[i]*e[i];
                                t = (x*s - z*r)/q;
                                h[i][n] = t;
                                if (Math.Abs(x) > Math.Abs(z))
                                {
                                    h[i + 1][n] = (-r - w*t)/x;
                                }
                                else
                                {
                                    h[i + 1][n] = (-s - y*t)/z;
                                }
                            }

                            // Overflow control

                            t = Math.Abs(h[i][n]);
                            if ((eps*t)*t > 1)
                            {
                                for (int j = i; j <= n; j++)
                                {
                                    h[j][n] = h[j][n]/t;
                                }
                            }
                        }
                    }

                    // Complex vector
                }
                else if (q < 0)
                {
                    int l = n - 1;

                    // Last vector component imaginary so matrix is triangular

                    if (Math.Abs(h[n][n - 1]) > Math.Abs(h[n - 1][n]))
                    {
                        h[n - 1][n - 1] = q/h[n][n - 1];
                        h[n - 1][n] = -(h[n][n] - p)/h[n][n - 1];
                    }
                    else
                    {
                        cdiv(0.0, -h[n - 1][n], h[n - 1][n - 1] - p, q);
                        h[n - 1][n - 1] = cdivr;
                        h[n - 1][n] = cdivi;
                    }
                    h[n][n - 1] = 0.0;
                    h[n][n] = 1.0;
                    for (int i = n - 2; i >= 0; i--)
                    {
                        double ra, sa, vr, vi;
                        ra = 0.0;
                        sa = 0.0;
                        for (int j = l; j <= n; j++)
                        {
                            ra = ra + h[i][j]*h[j][n - 1];
                            sa = sa + h[i][j]*h[j][n];
                        }
                        w = h[i][i] - p;

                        if (e[i] < 0.0)
                        {
                            z = w;
                            r = ra;
                            s = sa;
                        }
                        else
                        {
                            l = i;
                            if (e[i] == 0)
                            {
                                cdiv(-ra, -sa, w, q);
                                h[i][n - 1] = cdivr;
                                h[i][n] = cdivi;
                            }
                            else
                            {
                                // Solve complex equations

                                x = h[i][i + 1];
                                y = h[i + 1][i];
                                vr = (d[i] - p)*(d[i] - p) + e[i]*e[i] - q*q;
                                vi = (d[i] - p)*2.0*q;
                                if (vr == 0.0 & vi == 0.0)
                                {
                                    vr = eps
                                         *norm
                                         *(Math.Abs(w) + Math.Abs(q)
                                           + Math.Abs(x) + Math.Abs(y) + Math
                                                                             .Abs(z));
                                }
                                cdiv(x*r - z*ra + q*sa, x*s - z*sa - q
                                                        *ra, vr, vi);
                                h[i][n - 1] = cdivr;
                                h[i][n] = cdivi;
                                if (Math.Abs(x) > (Math.Abs(z) + Math.Abs(q)))
                                {
                                    h[i + 1][n - 1] = (-ra - w*h[i][n - 1] + q
                                                       *h[i][n])
                                                      /x;
                                    h[i + 1][n] = (-sa - w*h[i][n] - q
                                                   *h[i][n - 1])
                                                  /x;
                                }
                                else
                                {
                                    cdiv(-r - y*h[i][n - 1], -s - y*h[i][n], z,
                                         q);
                                    h[i + 1][n - 1] = cdivr;
                                    h[i + 1][n] = cdivi;
                                }
                            }

                            // Overflow control

                            t = Math.Max(Math.Abs(h[i][n - 1]), Math.Abs(h[i][n]));
                            if ((eps*t)*t > 1)
                            {
                                for (int j = i; j <= n; j++)
                                {
                                    h[j][n - 1] = h[j][n - 1]/t;
                                    h[j][n] = h[j][n]/t;
                                }
                            }
                        }
                    }
                }
            }

            // Vectors of isolated roots

            for (int i = 0; i < nn; i++)
            {
                if (i < low | i > high)
                {
                    for (int j = i; j < nn; j++)
                    {
                        v[i][j] = h[i][j];
                    }
                }
            }

            // Back transformation to get eigenvectors of original matrix

            for (int j = nn - 1; j >= low; j--)
            {
                for (int i = low; i <= high; i++)
                {
                    z = 0.0;
                    for (int k = low; k <= Math.Min(j, high); k++)
                    {
                        z = z + v[i][k]*h[k][j];
                    }
                    v[i][j] = z;
                }
            }
        }

        /**
         * Return the imaginary parts of the eigenvalues.
         * 
         * @return imag(diag(D)).
         */

        public double[] getImagEigenvalues()
        {
            return e;
        }
    }
}
